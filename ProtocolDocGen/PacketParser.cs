using System.Text.RegularExpressions;

namespace ProtocolDocGen;

/// <summary>
/// 解析 MinecraftPacket/*.cs 中的 Packet 类定义，提取编解码字段信息。
/// </summary>
public partial class PacketParser
{
    private readonly string _packetDir;

    public PacketParser(string packetDir)
    {
        _packetDir = packetDir;
    }

    /// <summary>
    /// 解析所有数据包文件，返回 Packet 元数据列表。
    /// </summary>
    public List<PacketMeta> ParseAll()
    {
        var results = new List<PacketMeta>();
        if (!Directory.Exists(_packetDir))
        {
            Console.Error.WriteLine($"[WARN] 数据包目录不存在: {_packetDir}");
            return results;
        }

        var files = Directory.GetFiles(_packetDir, "*.cs", SearchOption.TopDirectoryOnly);
        foreach (var file in files)
        {
            var meta = ParseFile(file);
            if (meta != null)
                results.Add(meta);
        }

        return results;
    }

    /// <summary>
    /// 解析单个 .cs 文件。
    /// </summary>
    private PacketMeta? ParseFile(string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        // 跳过帮助类和未知数据包
        if (fileName is "UnknownPacket")
            return null;

        var code = File.ReadAllText(filePath);

        // 检查是否有 Packet 子类
        if (!code.Contains(": Packet") && !code.Contains(": Packet\n"))
            return null;

        var meta = new PacketMeta
        {
            ClassName = fileName,
            SourceFile = Path.GetRelativePath(_packetDir, filePath)
        };

        // ── 提取 Packet ID ──
        var idMatch = Regex.Match(code, @"\bId\s*=\s*(0x[0-9a-fA-F]+|\d+)");
        if (idMatch.Success)
        {
            var idStr = idMatch.Groups[1].Value;
            meta.PacketIdHex = idStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? idStr : $"0x{int.Parse(idStr):X2}";
            meta.PacketId = Convert.ToInt32(idStr, idStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? 16 : 10);
        }

        // 检查 IsMcbe
        meta.IsMcbe = code.Contains("IsMcbe = true");

        // ── 提取枚举定义 (同一文件中的) ──
        meta.LocalEnums = ParseLocalEnums(code);

        // ── 解析 DecodePacket 方法 ──
        meta.Fields = ParseDecodeMethod(code);

        // ── 提取命名空间 ──
        var nsMatch = Regex.Match(code, @"namespace\s+([\w.]+)");
        if (nsMatch.Success)
            meta.Namespace = nsMatch.Groups[1].Value;

        return meta;
    }

    /// <summary>
    /// 解析同一文件中定义的枚举。
    /// </summary>
    private List<EnumMeta> ParseLocalEnums(string code)
    {
        var enums = new List<EnumMeta>();
        // 匹配 public/internal enum Name : Type { ... }
        var matches = Regex.Matches(code,
            @"(?:public|internal|private)\s+enum\s+(\w+)\s*(?::\s*(\w+))?\s*\{([^}]*)\}",
            RegexOptions.Singleline);

        foreach (Match m in matches)
        {
            var name = m.Groups[1].Value;
            var underlying = m.Groups[2].Success ? m.Groups[2].Value : "int";
            var body = m.Groups[3].Value;

            var values = new List<(string Name, string Value)>();
            var lines = body.Split(',', StringSplitOptions.RemoveEmptyEntries);
            int nextVal = 0;
            foreach (var line in lines)
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("//")) continue;

                var eqIdx = trimmed.IndexOf('=');
                if (eqIdx >= 0)
                {
                    var valName = trimmed[..eqIdx].Trim();
                    var valStr = trimmed[(eqIdx + 1)..].Trim();
                    // 移除行注释
                    var commentIdx = valStr.IndexOf("//", StringComparison.Ordinal);
                    if (commentIdx >= 0) valStr = valStr[..commentIdx].Trim();

                    values.Add((valName, valStr));
                    // 尝试解析为数值以继续自动计数
                    if (int.TryParse(valStr, System.Globalization.NumberStyles.Integer, null, out var parsed))
                        nextVal = parsed + 1;
                    else if (valStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                             int.TryParse(valStr[2..], System.Globalization.NumberStyles.HexNumber, null, out var hexParsed))
                        nextVal = hexParsed + 1;
                }
                else
                {
                    values.Add((trimmed, nextVal.ToString()));
                    nextVal++;
                }
            }

            enums.Add(new EnumMeta
            {
                Name = name,
                UnderlyingType = underlying,
                Values = values.Select(v => new EnumValueMeta { Name = v.Name, Value = v.Value }).ToList()
            });
        }

        return enums;
    }

    /// <summary>
    /// 解析 DecodePacket() 方法体，提取字段读取序列。
    /// 支持: 简单赋值、var 声明、索引赋值、for/foreach 循环。
    /// </summary>
    private List<FieldMeta> ParseDecodeMethod(string code)
    {
        var fields = new List<FieldMeta>();

        var methodBody = ExtractMethodBody(code, "DecodePacket");
        if (methodBody == null) return fields;

        var lines = methodBody.Split('\n');

        int depth = 0;
        var conditionStack = new Stack<string>();
        var loopArrayMap = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (string.IsNullOrEmpty(line) || line.StartsWith("//")) continue;
            if (line.Contains("base.DecodePacket()")) continue;

            var stripped = line.TrimStart();

            // ── 块进入 ──
            if (stripped.StartsWith("if ") || stripped.StartsWith("if("))
            { depth++; conditionStack.Push(ExtractCondition(stripped)); continue; }
            if (stripped.StartsWith("else if"))
            { if (conditionStack.Count > 0) conditionStack.Pop(); conditionStack.Push(ExtractCondition(stripped["else ".Length..])); continue; }
            if (stripped.StartsWith("else"))
            { if (conditionStack.Count > 0) conditionStack.Pop(); conditionStack.Push("!(...)"); continue; }
            if (stripped.StartsWith("switch "))
            { depth++; conditionStack.Push("switch(...)"); continue; }
            if (stripped.StartsWith("for ") || stripped.StartsWith("for(") || stripped.StartsWith("foreach "))
            { depth++; continue; }

            // ── 跳过 ──
            if (stripped == "{") continue;
            if (stripped.StartsWith("case ") || stripped.StartsWith("default:")) continue;

            // ── 块退出 ──
            if (stripped == "}" || stripped.StartsWith("}"))
            { if (depth > 0) { depth--; if (conditionStack.Count > 0) conditionStack.Pop(); } continue; }

            string? condition = conditionStack.Count > 0 ? string.Join(" && ", conditionStack.Reverse()) : null;

            // ── 模式 1: var name = ReadMethod(); ──
            var varMatch = VarAssignRegex().Match(line);
            if (varMatch.Success)
            {
                var localName = varMatch.Groups[1].Value;
                var readMethod = varMatch.Groups[2].Value;
                var args = varMatch.Groups[3].Success ? varMatch.Groups[3].Value : "";
                fields.Add(CreateField(localName, readMethod, args, null, condition, isTransient: true));
                continue;
            }

            // ── 模式 2: arrayName = new SomeType[count]; ──
            var newArrayMatch = NewArrayRegex().Match(line);
            if (newArrayMatch.Success)
            {
                loopArrayMap[newArrayMatch.Groups[1].Value] = newArrayMatch.Groups[2].Value;
                continue;
            }

            // ── 模式 3: arrayName[index] = ReadMethod(); ──
            var indexedMatch = IndexedAssignRegex().Match(line);
            if (indexedMatch.Success)
            {
                var arrayName = indexedMatch.Groups[1].Value;
                var readMethod = indexedMatch.Groups[2].Value;
                var args = indexedMatch.Groups[3].Success ? indexedMatch.Groups[3].Value : "";

                ResolvedTypeInfo? resolved = ReadMethodMapper.Map(readMethod);
                if (loopArrayMap.TryGetValue(arrayName, out var elemTypeName))
                {
                    resolved = MakeArrayResolved(resolved, elemTypeName);
                }

                var existing = fields.FirstOrDefault(f => f.Name == arrayName);
                if (existing != null) { existing.ResolvedType ??= resolved; }
                else { fields.Add(CreateField(arrayName, readMethod, args, null, condition, isTransient: false, overrideResolved: resolved)); }
                continue;
            }

            // ── 模式 4: 简单赋值 ──
            var assignMatch = AssignmentRegex().Match(line);
            if (!assignMatch.Success) continue;

            var fieldName = assignMatch.Groups[1].Value;
            var cast = assignMatch.Groups[2].Success ? assignMatch.Groups[2].Value : null;
            var rm = assignMatch.Groups[3].Value;
            var rmArgs = assignMatch.Groups[4].Success ? assignMatch.Groups[4].Value : "";

            if (fieldName == "var")
                continue;

            fields.Add(CreateField(fieldName, rm, rmArgs, cast, condition, isTransient: false));
        }

        return fields;
    }

    private FieldMeta CreateField(string name, string readMethod, string args, string? cast,
        string? condition, bool isTransient = false, ResolvedTypeInfo? overrideResolved = null)
    {
        var resolved = overrideResolved ?? ReadMethodMapper.Map(readMethod);
        return new FieldMeta
        {
            Name = name,
            ReadMethod = readMethod,
            ReadArgs = args,
            CastType = cast,
            SchemaType = resolved?.SchemaType ?? "object",
            SchemaFormat = resolved?.SchemaFormat,
            Description = (isTransient ? "[临时变量] " : "") + (resolved?.Description ?? $"调用 {readMethod}"),
            Condition = condition,
            ResolvedType = resolved,
            IsTransient = isTransient
        };
    }

    private static ResolvedTypeInfo MakeArrayResolved(ResolvedTypeInfo? elementResolved, string elemTypeName)
    {
        return new ResolvedTypeInfo
        {
            SchemaType = "array",
            SchemaFormat = null,
            Description = elementResolved?.Description ?? $"{elemTypeName}[]",
            IsComplex = true,
            ArrayElementDefKey = elementResolved?.DefKey,
            ArrayElementRef = elementResolved?.RefPath,
            EnumType = elementResolved?.EnumType
        };
    }

    // ── 正则表达式 ──

    [GeneratedRegex(@"^\s*(\w[\w.]*(?:\.\w+)*)\s*=\s*(?:\((\w+(?:\.\w+)*)\))?\s*(\w+)\s*\(([^;]*)\)\s*(?:\.\w+\s*\([^)]*\))?\s*;?\s*$")]
    private static partial Regex AssignmentRegex();

    [GeneratedRegex(@"^\s*var\s+(\w+)\s*=\s*(\w+)\s*\(([^;]*)\)\s*;?\s*$")]
    private static partial Regex VarAssignRegex();

    [GeneratedRegex(@"^\s*(\w+)\s*\[[^\]]+\]\s*=\s*(\w+)\s*\(([^;]*)\)\s*;?\s*$")]
    private static partial Regex IndexedAssignRegex();

    [GeneratedRegex(@"^\s*(\w+)\s*=\s*new\s+(\w+(?:\.\w+)*)\s*\[")]
    private static partial Regex NewArrayRegex();

    /// <summary>
    /// 通过括号计数从源代码中提取指定方法的函数体（不含方法签名和外部大括号）。
    /// </summary>
    private static string? ExtractMethodBody(string code, string methodName)
    {
        var pattern = $@"(?:protected|public|private|internal)?\s*(?:override\s+)?void\s+{methodName}\s*\([^)]*\)";
        var match = Regex.Match(code, pattern);
        if (!match.Success) return null;

        var startIdx = match.Index + match.Length;
        var braceIdx = code.IndexOf('{', startIdx);
        if (braceIdx < 0) return null;

        int braceCount = 1;
        int i = braceIdx + 1;
        while (i < code.Length && braceCount > 0)
        {
            if (code[i] == '{') braceCount++;
            else if (code[i] == '}') braceCount--;
            i++;
        }
        if (braceCount != 0) return null;

        var bodyStart = braceIdx + 1;
        var bodyEnd = i - 2;
        if (bodyEnd < bodyStart) return "";
        return code[bodyStart..(bodyEnd + 1)];
    }

    private static string ExtractCondition(string line)
    {
        var match = Regex.Match(line, @"\((.*)\)");
        return match.Success ? match.Groups[1].Value.Trim() : "?";
    }
}

public class PacketMeta
{
    public string ClassName { get; set; } = "";
    public string? Namespace { get; set; }
    public string SourceFile { get; set; } = "";
    public int PacketId { get; set; }
    public string PacketIdHex { get; set; } = "";
    public bool IsMcbe { get; set; }
    public List<FieldMeta> Fields { get; set; } = [];
    public List<EnumMeta> LocalEnums { get; set; } = [];
}

public class FieldMeta
{
    public string Name { get; set; } = "";
    public string ReadMethod { get; set; } = "";
    public string ReadArgs { get; set; } = "";
    public string? CastType { get; set; }
    public string SchemaType { get; set; } = "object";
    public string? SchemaFormat { get; set; }
    public string Description { get; set; } = "";
    public string? Condition { get; set; }
    public bool IsTransient { get; set; }

    /// <summary>反射解析后的类型信息（由 TypeResolver 提供）。</summary>
    public ResolvedTypeInfo? ResolvedType { get; set; }
}

public class EnumMeta
{
    public string Name { get; set; } = "";
    public string UnderlyingType { get; set; } = "int";
    public List<EnumValueMeta> Values { get; set; } = [];
}

public class EnumValueMeta
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
}
