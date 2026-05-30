using System.Collections;
using System.Numerics;
using System.Reflection;
using Protocol.Network;

namespace ProtocolDocGen;

/// <summary>
/// 通过反射解析 Packet 类的所有 Read* 方法的返回类型，
/// 并递归提取自定义类型的属性结构，生成 JSON Schema $defs。
/// </summary>
public class TypeResolver
{
    private readonly Dictionary<string, ResolvedTypeInfo> _methodMap = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<Type, TypeDef> _typeDefs = [];
    private readonly HashSet<Type> _visited = [];
    private readonly HashSet<Type> _enumTypes = [];

    // ── 原始类型映射 ──
    private static readonly Dictionary<Type, (string Type, string? Format, string Desc)> PrimitiveMap = new()
    {
        [typeof(byte)]   = ("integer", "uint8",  "无符号字节 (0-255)"),
        [typeof(sbyte)]  = ("integer", "int8",   "有符号字节 (-128-127)"),
        [typeof(bool)]   = ("boolean", null,     "布尔值"),
        [typeof(short)]  = ("integer", "int16",  "有符号 16 位整数"),
        [typeof(ushort)] = ("integer", "uint16", "无符号 16 位整数"),
        [typeof(int)]    = ("integer", "int32",  "有符号 32 位整数"),
        [typeof(uint)]   = ("integer", "uint32", "无符号 32 位整数"),
        [typeof(long)]   = ("integer", "int64",  "有符号 64 位整数"),
        [typeof(ulong)]  = ("integer", "uint64", "无符号 64 位整数"),
        [typeof(float)]  = ("number",  "float",  "32 位浮点数"),
        [typeof(double)] = ("number",  "double", "64 位浮点数"),
        [typeof(string)] = ("string",  null,     "字符串 (长度前缀)"),
        [typeof(byte[])] = ("array",   "byte-array", "字节数组 (长度前缀)"),
    };

    // ── 特殊类型名 → 唯一名称（用于 $defs key）──
    private static readonly Dictionary<string, string> SpecialTypeNames = new(StringComparer.Ordinal)
    {
        ["Vector2"] = "Vector2f",
        ["Vector3"] = "Vector3f",
        ["Vector4"] = "Vector4f",
    };

    /// <summary>
    /// 初始化：扫描 Packet 类的所有 Read* 方法。
    /// </summary>
    public void Initialize()
    {
        var packetType = typeof(Packet);

        foreach (var method in packetType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            if (!method.Name.StartsWith("Read", StringComparison.Ordinal)) continue;
            if (method.GetParameters().Length > 2) continue; // 跳过需要复杂参数的方法

            var returnType = method.ReturnType;
            var resolved = ResolveReturnType(method.Name, returnType);
            _methodMap[method.Name] = resolved;
        }

        // ReadSlice 是泛型方法，返回类型取决于 lambda 参数，反射解析为 List<>，
        // 因此跳过反射，交给 ReadMethodMapper 的 fallback 处理。
        _methodMap.Remove("ReadSlice");
    }

    /// <summary>
    /// 根据 Read 方法名获取解析后的类型信息。
    /// </summary>
    public ResolvedTypeInfo? GetReadMethodInfo(string methodName)
    {
        if (_methodMap.TryGetValue(methodName, out var info))
            return info;
        return null;
    }

    /// <summary>
    /// 获取所有已解析的类型定义 ($defs)。
    /// </summary>
    public IReadOnlyDictionary<Type, TypeDef> TypeDefs => _typeDefs;

    /// <summary>
    /// 获取所有已知的枚举类型。
    /// </summary>
    public IReadOnlyCollection<Type> EnumTypes => _enumTypes;

    private ResolvedTypeInfo ResolveReturnType(string methodName, Type returnType)
    {
        // 如果是 void（如 ReadInventoryTransactionPacket）
        if (returnType == typeof(void))
        {
            return new ResolvedTypeInfo
            {
                SchemaType = "object",
                SchemaFormat = ToKebab(methodName["Read".Length..]),
                Description = $"内联解码: {methodName}",
                IsComplex = false
            };
        }

        // 原始类型
        if (PrimitiveMap.TryGetValue(returnType, out var prim))
        {
            return new ResolvedTypeInfo
            {
                SchemaType = prim.Type,
                SchemaFormat = prim.Format,
                Description = prim.Desc,
                IsComplex = false
            };
        }

        // 数组 T[]
        if (returnType.IsArray)
        {
            var elemType = returnType.GetElementType()!;
            var elemInfo = ResolveTypeRecursive(elemType);
            return new ResolvedTypeInfo
            {
                SchemaType = "array",
                SchemaFormat = null,
                Description = $"{elemType.Name}[] 数组",
                IsComplex = true,
                ArrayElementDefKey = elemInfo.DefKey,
                ArrayElementRef = elemInfo.RefPath
            };
        }

        // List<T>
        if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elemType = returnType.GetGenericArguments()[0];
            var elemInfo = ResolveTypeRecursive(elemType);
            return new ResolvedTypeInfo
            {
                SchemaType = "array",
                SchemaFormat = null,
                Description = $"List<{elemType.Name}>",
                IsComplex = true,
                ArrayElementDefKey = elemInfo.DefKey,
                ArrayElementRef = elemInfo.RefPath
            };
        }

        // 枚举
        if (returnType.IsEnum)
        {
            _enumTypes.Add(returnType);
            return new ResolvedTypeInfo
            {
                SchemaType = "integer",
                SchemaFormat = null,
                Description = $"枚举 {returnType.Name}",
                IsComplex = false,
                EnumType = returnType
            };
        }

        // Optional<T> — 解包为 T
        if (returnType.IsGenericType && returnType.Name.StartsWith("Optional"))
        {
            var innerType = returnType.GetGenericArguments()[0];
            if (PrimitiveMap.ContainsKey(innerType))
            {
                var p = PrimitiveMap[innerType];
                return new ResolvedTypeInfo
                {
                    SchemaType = p.Type,
                    SchemaFormat = p.Format,
                    Description = $"{p.Desc} (可选)",
                    IsComplex = false
                };
            }
            // 复杂类型的 Optional<T> 等同于 T
            return ResolveReturnType(methodName, innerType);
        }

        // 自定义复杂类型
        var resolved = ResolveTypeRecursive(returnType);
        return new ResolvedTypeInfo
        {
            SchemaType = "object",
            SchemaFormat = null,
            Description = resolved.Description ?? $"自定义: {returnType.Name}",
            IsComplex = true,
            DefKey = resolved.DefKey,
            RefPath = resolved.RefPath
        };
    }

    /// <summary>
    /// 递归解析一个 CLR 类型，生成 TypeDef 并返回引用路径。
    /// </summary>
    private (string? DefKey, string? RefPath, string? Description) ResolveTypeRecursive(Type type)
    {
        // 剥离 Nullable<T>
        var underlying = Nullable.GetUnderlyingType(type);
        if (underlying != null)
            return ResolveTypeRecursive(underlying);

        // 原始类型不算复杂类型
        if (PrimitiveMap.ContainsKey(type))
        {
            return (null, null, PrimitiveMap[type].Desc);
        }

        // 字符串
        if (type == typeof(string))
        {
            return (null, null, "字符串");
        }

        // Vector2/Vector3 (System.Numerics)
        if (type == typeof(Vector2))
        {
            EnsureDef(typeof(Vector2), () => BuildVector2Def());
            return ("Vector2f", "#/$defs/Vector2f", "二维向量");
        }
        if (type == typeof(Vector3))
        {
            EnsureDef(typeof(Vector3), () => BuildVector3Def());
            return ("Vector3f", "#/$defs/Vector3f", "三维向量");
        }

        // 枚举 → 不生成 def，由 SchemaGenerator 处理 oneOf
        if (type.IsEnum)
        {
            _enumTypes.Add(type);
            return (type.Name, null, $"枚举 {type.Name}");
        }

        // 数组 T[]
        if (type.IsArray)
        {
            var elem = ResolveTypeRecursive(type.GetElementType()!);
            return (null, null, $"{type.GetElementType()?.Name}[]");
        }

        // List<T> — 由外层 ResolveReturnType 处理
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elem = ResolveTypeRecursive(type.GetGenericArguments()[0]);
            return (null, null, $"List<{type.GetGenericArguments()[0].Name}>");
        }

        // Optional<T> — 视为 T
        if (type.IsGenericType && type.Name.StartsWith("Optional"))
        {
            var inner = type.GetGenericArguments()[0];
            return ResolveTypeRecursive(inner);
        }

        // 其他泛型 → 生成 def
        var defKey = GetDefKey(type);
        if (!_typeDefs.ContainsKey(type))
        {
            BuildDef(type, defKey);
        }

        return (defKey, $"#/$defs/{defKey}", type.Name);
    }

    private void EnsureDef(Type type, Func<TypeDef> builder)
    {
        if (!_typeDefs.ContainsKey(type))
        {
            _typeDefs[type] = builder();
        }
    }

    private TypeDef BuildVector2Def()
    {
        var props = new Dictionary<string, TypeDefProperty>
        {
            ["X"] = new() { SchemaType = "number", SchemaFormat = "float", Description = "X 分量" },
            ["Y"] = new() { SchemaType = "number", SchemaFormat = "float", Description = "Y 分量" }
        };
        return new TypeDef { DefKey = "Vector2f", Properties = props, Description = "二维向量" };
    }

    private TypeDef BuildVector3Def()
    {
        var props = new Dictionary<string, TypeDefProperty>
        {
            ["X"] = new() { SchemaType = "number", SchemaFormat = "float", Description = "X 分量" },
            ["Y"] = new() { SchemaType = "number", SchemaFormat = "float", Description = "Y 分量" },
            ["Z"] = new() { SchemaType = "number", SchemaFormat = "float", Description = "Z 分量" }
        };
        return new TypeDef { DefKey = "Vector3f", Properties = props, Description = "三维向量" };
    }

    private void BuildDef(Type type, string defKey)
    {
        if (_visited.Contains(type)) return; // 防止循环
        _visited.Add(type);

        var props = new Dictionary<string, TypeDefProperty>();
        var typeDef = new TypeDef { DefKey = defKey, Properties = props, Description = type.Name };

        // 先登记，再填充（允许自引用）
        _typeDefs[type] = typeDef;

        // 遍历公开属性和字段
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (prop.GetIndexParameters().Length > 0) continue; // 跳过索引器
            var propInfo = ResolveMemberType(prop.PropertyType, prop.Name);
            props[prop.Name] = propInfo;
        }

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (field.IsStatic) continue;
            var fieldInfo = ResolveMemberType(field.FieldType, field.Name);
            if (!props.ContainsKey(field.Name))
                props[field.Name] = fieldInfo;
        }

        _visited.Remove(type);
    }

    private TypeDefProperty ResolveMemberType(Type memberType, string memberName)
    {
        // 原始类型
        if (PrimitiveMap.TryGetValue(memberType, out var prim))
        {
            return new TypeDefProperty
            {
                SchemaType = prim.Type,
                SchemaFormat = prim.Format,
                Description = prim.Desc
            };
        }

        // 字符串
        if (memberType == typeof(string))
        {
            return new TypeDefProperty { SchemaType = "string", Description = "字符串" };
        }

        // byte[] → array
        if (memberType == typeof(byte[]))
        {
            return new TypeDefProperty { SchemaType = "array", SchemaFormat = "byte-array", Description = "字节数组" };
        }

        // Vector2/Vector3
        if (memberType == typeof(Vector2))
        {
            EnsureDef(typeof(Vector2), () => BuildVector2Def());
            return new TypeDefProperty { RefPath = "#/$defs/Vector2f", Description = "二维向量" };
        }
        if (memberType == typeof(Vector3))
        {
            EnsureDef(typeof(Vector3), () => BuildVector3Def());
            return new TypeDefProperty { RefPath = "#/$defs/Vector3f", Description = "三维向量" };
        }

        // 枚举
        if (memberType.IsEnum)
        {
            _enumTypes.Add(memberType);
            return new TypeDefProperty
            {
                SchemaType = "integer",
                Description = $"枚举 {memberType.Name}",
                EnumType = memberType
            };
        }

        // 剥离 Nullable<T>
        var underlying = Nullable.GetUnderlyingType(memberType);
        if (underlying != null)
        {
            var inner = ResolveTypeRecursive(underlying);
            return new TypeDefProperty
            {
                RefPath = inner.RefPath,
                Description = $"{inner.Description} (可空)"
            };
        }

        // Optional<T>
        if (memberType.IsGenericType && memberType.Name.StartsWith("Optional"))
        {
            var inner = ResolveTypeRecursive(memberType.GetGenericArguments()[0]);
            return new TypeDefProperty
            {
                // 如果是原始类型，直接返回原始类型 schema（Optional 只是可空包装）
                SchemaType = inner.RefPath == null ? ResolvePrimitiveSchemaType(memberType.GetGenericArguments()[0]) : null,
                SchemaFormat = inner.RefPath == null ? ResolvePrimitiveSchemaFormat(memberType.GetGenericArguments()[0]) : null,
                RefPath = inner.RefPath,
                Description = inner.Description,
                IsOptional = true
            };
        }

        // 数组 T[]
        if (memberType.IsArray)
        {
            var elem = ResolveTypeRecursive(memberType.GetElementType()!);
            return new TypeDefProperty
            {
                SchemaType = "array",
                Description = $"{memberType.GetElementType()?.Name}[]",
                ItemsRefPath = elem.RefPath
            };
        }

        // List<T>
        if (memberType.IsGenericType && memberType.GetGenericTypeDefinition() == typeof(List<>))
        {
            var elem = ResolveTypeRecursive(memberType.GetGenericArguments()[0]);
            return new TypeDefProperty
            {
                SchemaType = "array",
                Description = $"List<{memberType.GetGenericArguments()[0].Name}>",
                ItemsRefPath = elem.RefPath
            };
        }

        // 嵌套的复杂类型（在 Protocol.Minecraft 命名空间内）
        if (memberType.Namespace?.StartsWith("Protocol", StringComparison.Ordinal) == true ||
            memberType.Namespace?.StartsWith("fNbt", StringComparison.Ordinal) == true)
        {
            var resolved = ResolveTypeRecursive(memberType);
            return new TypeDefProperty
            {
                RefPath = resolved.RefPath,
                Description = resolved.Description
            };
        }

        // 系统类型 / 其他外部类型 → 视为 object
        return new TypeDefProperty
        {
            SchemaType = "object",
            Description = memberType.FullName ?? memberType.Name
        };
    }

    /// <summary>
    /// 收集一个 Packet 中所有字段引用的 $defs（去重、去循环）。
    /// </summary>
    public Dictionary<string, TypeDef> CollectDefs(IEnumerable<ResolvedTypeInfo> fieldInfos)
    {
        var result = new Dictionary<string, TypeDef>();
        var visited = new HashSet<Type>();

        foreach (var info in fieldInfos)
        {
            if (info.DefKey != null && _typeDefs.TryGetValue(GetTypeByDefKey(info.DefKey), out var def))
            {
                CollectDefRecursive(def, result, visited);
            }
            if (info.ArrayElementDefKey != null && _typeDefs.TryGetValue(GetTypeByDefKey(info.ArrayElementDefKey), out var elemDef))
            {
                CollectDefRecursive(elemDef, result, visited);
            }
        }

        return result;
    }

    private void CollectDefRecursive(TypeDef def, Dictionary<string, TypeDef> result, HashSet<Type> visited)
    {
        var type = GetTypeByDefKey(def.DefKey);
        if (type == null || !visited.Add(type)) return;

        result[def.DefKey] = def;

        foreach (var prop in def.Properties.Values)
        {
            if (TryGetDefKeyFromRef(prop.RefPath, out var refKey))
            {
                var refType = GetTypeByDefKey(refKey);
                if (refType != null && _typeDefs.TryGetValue(refType, out var refDef))
                {
                    CollectDefRecursive(refDef, result, visited);
                }
            }
            if (TryGetDefKeyFromRef(prop.ItemsRefPath, out var itemRefKey))
            {
                var itemType = GetTypeByDefKey(itemRefKey);
                if (itemType != null && _typeDefs.TryGetValue(itemType, out var itemDef))
                {
                    CollectDefRecursive(itemDef, result, visited);
                }
            }
        }
    }

    private static string GetDefKey(Type type)
    {
        var name = type.Name;
        // 嵌套类型用 _
        if (type.DeclaringType != null)
            name = $"{type.DeclaringType.Name}_{type.Name}";

        // 泛型类型
        if (type.IsGenericType)
        {
            var genericName = name[..name.IndexOf('`')];
            var args = string.Join("_", type.GetGenericArguments().Select(GetDefKey));
            name = $"{genericName}_{args}";
        }

        return SpecialTypeNames.GetValueOrDefault(name, name);
    }

    private Type? GetTypeByDefKey(string defKey)
    {
        return _typeDefs.Keys.FirstOrDefault(t => GetDefKey(t) == defKey);
    }

    private static bool TryGetDefKeyFromRef(string? refPath, out string defKey)
    {
        defKey = "";
        if (refPath == null || !refPath.StartsWith("#/$defs/")) return false;
        defKey = refPath["#/$defs/".Length..];
        return true;
    }

    private static string ToKebab(string name)
    {
        return string.Concat(name.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "-" + char.ToLowerInvariant(c) : char.ToLowerInvariant(c).ToString()));
    }

    /// <summary>
    /// 获取原始类型的 SchemaType。
    /// </summary>
    private static string? ResolvePrimitiveSchemaType(Type type)
    {
        return PrimitiveMap.TryGetValue(type, out var p) ? p.Type : null;
    }

    /// <summary>
    /// 获取原始类型的 SchemaFormat。
    /// </summary>
    private static string? ResolvePrimitiveSchemaFormat(Type type)
    {
        return PrimitiveMap.TryGetValue(type, out var p) ? p.Format : null;
    }
}

/// <summary>
/// Read 方法对应的解析结果。
/// </summary>
public class ResolvedTypeInfo
{
    public string SchemaType { get; init; } = "object";
    public string? SchemaFormat { get; init; }
    public string Description { get; init; } = "";
    public bool IsComplex { get; init; }

    /// <summary>$defs key（如 "AbilityLayer"）</summary>
    public string? DefKey { get; init; }

    /// <summary>$ref 路径（如 "#/$defs/AbilityLayer"）</summary>
    public string? RefPath { get; init; }

    /// <summary>数组元素类型的 def key</summary>
    public string? ArrayElementDefKey { get; init; }

    /// <summary>数组元素类型的 $ref</summary>
    public string? ArrayElementRef { get; init; }

    /// <summary>如果是枚举，指向其类型</summary>
    public Type? EnumType { get; init; }
}

/// <summary>
/// 一个复杂类型的 JSON Schema 定义。
/// </summary>
public class TypeDef
{
    public string DefKey { get; set; } = "";
    public string Description { get; set; } = "";
    public Dictionary<string, TypeDefProperty> Properties { get; set; } = [];
}

/// <summary>
/// 类型定义中的一个属性。
/// </summary>
public class TypeDefProperty
{
    public string? SchemaType { get; set; }
    public string? SchemaFormat { get; set; }
    public string? RefPath { get; set; }
    public string? ItemsRefPath { get; set; }
    public string? Description { get; set; }
    public bool IsOptional { get; set; }
    public Type? EnumType { get; set; }
}
