namespace ProtocolDocGen;

/// <summary>
/// 将 Packet 的 Read* 方法映射为 JSON Schema 类型。
/// 委托 TypeResolver 通过反射获取实际返回类型。
/// 仅对 wire-format（VarInt/VarLong 等无法从 C# 类型推断的）做方法名覆盖。
/// </summary>
public static class ReadMethodMapper
{
    private static TypeResolver? _resolver;

    /// <summary>
    /// Wire-format 覆盖：某些 Read* 方法的 C# 返回类型无法反映线格式（如 VarInt 返回 int）。
    /// </summary>
    private static readonly Dictionary<string, (string Format, string Desc)> WireFormatOverrides = new(StringComparer.OrdinalIgnoreCase)
    {
        // ── VarInt / VarLong (C# 返回 int/long，线格式不同) ──
        ["ReadVarInt"]           = ("varint",           "有符号 VarInt"),
        ["ReadSignedVarInt"]     = ("signed-varint",    "有符号 VarInt (ZigZag)"),
        ["ReadUnsignedVarInt"]   = ("unsigned-varint",  "无符号 VarInt"),
        ["ReadLength"]           = ("unsigned-varint",  "长度 (无符号 VarInt)"),
        ["ReadVarLong"]          = ("varlong",          "有符号 VarLong"),
        ["ReadSignedVarLong"]    = ("signed-varlong",   "有符号 VarLong (ZigZag)"),
        ["ReadUnsignedVarLong"]  = ("unsigned-varlong", "无符号 VarLong"),

        // ── 大端 (C# 返回 short/int，线格式是 BE) ──
        ["ReadShortBe"]          = ("int16-be",  "有符号 16 位整数 (大端)"),
        ["ReadIntBe"]            = ("int32-be",  "有符号 32 位整数 (大端)"),
        ["ReadLong"]             = ("int64-be",  "有符号 64 位整数 (大端)"),  // Write/Read 都用 ReverseEndianness

        // ── 特殊 ──
        ["ReadLittle"]           = ("int24",     "24 位有符号整数 (小端)"),   // Int24 是自定义 value type
    };

    /// <summary>
    /// 设置 TypeResolver 实例（需在 Initialize 后调用）。
    /// </summary>
    public static void SetResolver(TypeResolver resolver)
    {
        _resolver = resolver;
    }

    /// <summary>
    /// 尝试将 Read 方法名映射为 JSON Schema 类型信息。
    /// 优先使用 TypeResolver 反射，叠加 wire-format 覆盖。
    /// </summary>
    public static ResolvedTypeInfo? Map(string readMethodName)
    {
        // 优先使用反射解析
        if (_resolver != null)
        {
            var info = _resolver.GetReadMethodInfo(readMethodName);
            if (info != null)
            {
                // 叠加 wire-format 覆盖
                if (WireFormatOverrides.TryGetValue(readMethodName, out var wireOverride))
                {
                    return new ResolvedTypeInfo
                    {
                        SchemaType = info.SchemaType,
                        SchemaFormat = wireOverride.Format,
                        Description = wireOverride.Desc,
                        IsComplex = info.IsComplex,
                        DefKey = info.DefKey,
                        RefPath = info.RefPath,
                        ArrayElementDefKey = info.ArrayElementDefKey,
                        ArrayElementRef = info.ArrayElementRef,
                        EnumType = info.EnumType
                    };
                }
                return info;
            }
        }

        // 回退：启发式命名（ReadSlice 等复杂泛型方法）
        if (readMethodName.StartsWith("Read", StringComparison.Ordinal))
        {
            return new ResolvedTypeInfo
            {
                SchemaType = "object",
                SchemaFormat = ToKebabCase(readMethodName["Read".Length..]),
                Description = $"自定义: {readMethodName}",
                IsComplex = false
            };
        }

        return null;
    }

    private static string ToKebabCase(string name)
    {
        return string.Concat(name.Select((c, i) =>
            i > 0 && char.IsUpper(c) ? "-" + char.ToLowerInvariant(c) : char.ToLowerInvariant(c).ToString()));
    }
}

/// <summary>
/// JSON Schema 类型描述（保留兼容旧代码）。
/// </summary>
public class JsonSchemaType
{
    public string Type { get; init; } = "object";
    public string? Format { get; init; }
    public string Description { get; init; } = "";
}
