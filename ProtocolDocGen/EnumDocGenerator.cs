using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProtocolDocGen;

/// <summary>
/// 将反射提取的枚举定义生成 protocol-enums.json 和更新的 unmatch.json。
/// </summary>
public class EnumDocGenerator
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    /// <summary>
    /// 生成 protocol-enums.json（所有 C# 枚举的完整文档，合并旧 C++ 文档的 Source 信息）。
    /// </summary>
    public void GenerateEnumDoc(List<EnumDef> enums, string outputDir)
    {
        Directory.CreateDirectory(outputDir);

        // 先读旧文档，提取 Source 映射
        var oldDocPath = Path.Combine(outputDir, "protocol-enums.json");
        Dictionary<string, string>? oldSources = null;
        if (File.Exists(oldDocPath))
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(File.ReadAllText(oldDocPath));
                oldSources = jsonDoc.RootElement.GetProperty("Enums")
                    .EnumerateArray()
                    .Where(e => e.TryGetProperty("Source", out var s) && s.GetString()?.Length > 0)
                    .ToDictionary(
                        e => e.GetProperty("FullName").GetString() ?? "",
                        e => e.GetProperty("Source").GetString() ?? "",
                        StringComparer.OrdinalIgnoreCase);
                Console.WriteLine($"[INFO] 从旧文档恢复了 {oldSources.Count} 个 C++ Source 映射。");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[WARN] 无法解析旧 protocol-enums.json: {ex.Message}");
            }
        }

        // 按命名空间和完整名称排序
        var sorted = enums.OrderBy(e => e.FullName).ToList();

        var doc = new
        {
            DocCount = 104, // 保持 C++ 文档层的参考计数
            Count = sorted.Count,
            Enums = sorted.Select(e =>
            {
                string? source = null;
                oldSources?.TryGetValue(e.FullName, out source);
                return new
                {
                    e.Name,
                    e.FullName,
                    e.Namespace,
                    e.UnderlyingType,
                    DocUnderlyingType = MapToDocUnderlyingType(e.UnderlyingType),
                    e.IsFlags,
                    e.IsPublic,
                    Source = source ?? "",
                    Values = e.Members.Select(m => new { m.Name, m.Value }).ToList()
                };
            }).ToList()
        };

        var path = Path.Combine(outputDir, "protocol-enums.json");
        var json = JsonSerializer.Serialize(doc, JsonOptions);
        File.WriteAllText(path, json);
        Console.WriteLine($"[INFO] 枚举文档: {path} ({sorted.Count} 个枚举)");
    }

    /// <summary>
    /// 生成新的 unmatch.json。与旧的 protocol-enums.json（C++ 文档层）做对比。
    /// 应在 GenerateEnumDoc 之前调用。
    /// </summary>
    public void GenerateUnmatch(List<EnumDef> csEnums, string docDir)
    {
        var oldDocPath = Path.Combine(docDir, "protocol-enums.json");
        HashSet<string>? docFullNames = null;

        if (File.Exists(oldDocPath))
        {
            try
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(oldDocPath));
                docFullNames = new HashSet<string>(
                    doc.RootElement.GetProperty("Enums")
                        .EnumerateArray()
                        .Select(e => e.GetProperty("FullName").GetString() ?? ""),
                    StringComparer.OrdinalIgnoreCase);
                Console.WriteLine($"[INFO] 旧协议枚举文档中有 {docFullNames.Count} 个枚举定义。");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[WARN] 无法解析旧 protocol-enums.json: {ex.Message}");
            }
        }

        // 找出 C# 中有但文档中没有的枚举
        var unmatched = new List<object>();
        foreach (var e in csEnums)
        {
            var existsInDoc = docFullNames != null && docFullNames.Contains(e.FullName);
            if (!existsInDoc)
            {
                unmatched.Add(new
                {
                    Enum = new
                    {
                        e.FullName,
                        e.Namespace,
                        e.Name,
                        e.UnderlyingType,
                        e.IsFlags,
                        e.IsPublic,
                        e.Members
                    },
                    Reason = "EnumNameNotFound",
                    DocEnum = (object?)null,
                    UnmatchedMembers = e.Members.Select(m => new { m.Name, m.Value }).ToList()
                });
            }
        }

        var result = new
        {
            Count = unmatched.Count,
            Enums = unmatched
        };

        var path = Path.Combine(docDir, "unmatch.json");
        var json = JsonSerializer.Serialize(result, JsonOptions);
        File.WriteAllText(path, json);
        Console.WriteLine($"[INFO] 未匹配枚举: {path} ({unmatched.Count} 个)");
    }

    /// <summary>
    /// 将 .NET 类型名映射为 C++ 文档层的类型名。
    /// </summary>
    private static string MapToDocUnderlyingType(string dotnetType)
    {
        return dotnetType switch
        {
            "System.Byte" => "std::uint8_t",
            "System.SByte" => "std::int8_t",
            "System.Int16" => "std::int16_t",
            "System.UInt16" => "std::uint16_t",
            "System.Int32" => "std::int32_t",
            "System.UInt32" => "std::uint32_t",
            "System.Int64" => "std::int64_t",
            "System.UInt64" => "std::uint64_t",
            "System.Single" => "float",
            "System.Double" => "double",
            _ => "std::int32_t"
        };
    }
}
