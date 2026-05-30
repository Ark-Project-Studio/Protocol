using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ProtocolDocGen;

/// <summary>
/// 将 PacketMeta 生成为 JSON Schema 文档。
/// 通过 TypeResolver 反射解析自定义类型，生成 $defs + $ref。
/// </summary>
public class SchemaGenerator
{
    private readonly string _outputDir;
    private readonly TypeResolver _typeResolver;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public SchemaGenerator(string outputDir, TypeResolver typeResolver)
    {
        _outputDir = outputDir;
        _typeResolver = typeResolver;
    }

    /// <summary>
    /// 为所有 Packet 生成 JSON Schema 文件，并生成索引。
    /// </summary>
    public void GenerateAll(List<PacketMeta> packets)
    {
        if (packets.Count == 0)
        {
            Console.WriteLine("[INFO] 没有数据包需要生成。");
            return;
        }

        Directory.CreateDirectory(_outputDir);
        var schemasDir = Path.Combine(_outputDir, "schemas");
        Directory.CreateDirectory(schemasDir);

        Console.WriteLine($"[INFO] 生成 {packets.Count} 个数据包的 JSON Schema...");

        foreach (var packet in packets)
        {
            var schema = BuildSchema(packet);
            var json = JsonSerializer.Serialize(schema, JsonOptions);

            var outPath = Path.Combine(schemasDir, $"{packet.ClassName}.json");
            File.WriteAllText(outPath, json);
        }

        // 生成索引文件
        GenerateIndex(packets, schemasDir);

        Console.WriteLine($"[DONE] 文件输出到: {schemasDir}");
    }

    private JsonObject BuildSchema(PacketMeta packet)
    {
        var schema = new JsonObject
        {
            ["$schema"] = "https://json-schema.org/draft/2020-12/schema",
            ["$id"] = $"https://ark.protocol/mcbe/packets/{packet.ClassName}.json",
            ["title"] = packet.ClassName,
            ["description"] = $"Minecraft Bedrock Edition Packet {packet.PacketIdHex} ({packet.PacketId})",
            ["type"] = "object"
        };

        // 额外的 Packet 元数据
        schema["x-packetId"] = packet.PacketId;
        schema["x-packetIdHex"] = packet.PacketIdHex;
        schema["x-isMcbe"] = packet.IsMcbe;
        schema["x-sourceFile"] = packet.SourceFile;
        if (packet.Namespace != null)
            schema["x-namespace"] = packet.Namespace;

        // ── 属性 ──
        var properties = new JsonObject();
        var required = new JsonArray();

        foreach (var field in packet.Fields)
        {
            var prop = BuildProperty(field, packet);

            // 有条件的字段标记条件说明
            if (field.Condition != null)
            {
                var existingDesc = prop.ContainsKey("description") ? prop["description"]?.GetValue<string>() ?? "" : "";
                prop["description"] = $"[条件: {field.Condition}] {existingDesc}".Trim();
            }

            // 仅非条件、非临时字段加入 required
            if (field.Condition == null && !field.IsTransient)
            {
                required.Add(field.Name);
            }

            properties[field.Name] = prop;
        }

        schema["properties"] = properties;
        if (required.Count > 0)
            schema["required"] = required;

        // ── $defs: 收集字段引用的所有自定义类型 ──
        var resolvedInfos = packet.Fields
            .Select(f => f.ResolvedType)
            .Where(r => r != null)
            .Cast<ResolvedTypeInfo>();

        var defs = _typeResolver.CollectDefs(resolvedInfos);
        if (defs.Count > 0)
        {
            var defsObj = new JsonObject();
            foreach (var (key, typeDef) in defs)
            {
                defsObj[key] = BuildTypeDefSchema(typeDef);
            }
            schema["$defs"] = defsObj;
        }

        // ── 本地枚举定义（源码解析的，保留兼容）──
        if (packet.LocalEnums.Count > 0)
        {
            var xEnums = new JsonObject();
            foreach (var e in packet.LocalEnums)
            {
                xEnums[e.Name] = JsonValue.Create(new
                {
                    underlyingType = e.UnderlyingType,
                    values = e.Values.ToDictionary(v => v.Name, v => v.Value)
                });
            }
            schema["x-localEnums"] = xEnums;
        }

        return schema;
    }

    private JsonObject BuildProperty(FieldMeta field, PacketMeta packet)
    {
        var resolved = field.ResolvedType;

        // ── 如果有 $ref（自定义复杂类型）──
        if (resolved?.RefPath != null)
        {
            var prop = new JsonObject
            {
                ["$ref"] = resolved.RefPath,
                ["x-readMethod"] = field.ReadMethod
            };
            if (field.ReadArgs?.Length > 0)
                prop["x-readArgs"] = field.ReadArgs;
            return prop;
        }

        // ── 数组（元素为复杂类型）──
        if (resolved?.SchemaType == "array" && resolved.ArrayElementRef != null)
        {
            var prop = new JsonObject
            {
                ["type"] = "array",
                ["items"] = new JsonObject { ["$ref"] = resolved.ArrayElementRef },
                ["x-readMethod"] = field.ReadMethod
            };
            if (field.ReadArgs?.Length > 0)
                prop["x-readArgs"] = field.ReadArgs;
            if (field.Description?.Length > 0 && !field.Description.StartsWith("[条件:"))
                prop["description"] = field.Description;
            return prop;
        }

        // ── 普通属性 ──
        var propObj = new JsonObject
        {
            ["type"] = resolved?.SchemaType ?? field.SchemaType,
            ["x-readMethod"] = field.ReadMethod
        };

        // Format 处理
        var format = resolved?.SchemaFormat ?? field.SchemaFormat;
        if (format != null)
        {
            if (format.Contains("int") || format.Contains("varint") || format.Contains("varlong"))
            {
                propObj["type"] = "integer";
                propObj["x-format"] = format;
            }
            else if (format is "float" or "double")
            {
                propObj["type"] = "number";
                propObj["x-format"] = format;
            }
            else if (format is "uuid")
            {
                propObj["type"] = "string";
                propObj["format"] = "uuid";
            }
            else if (format == "byte-array")
            {
                propObj["type"] = "array";
                propObj["x-format"] = format;
            }
            else
            {
                propObj["x-format"] = format;
            }
        }

        // 描述
        var desc = resolved?.Description ?? field.Description;
        if (!string.IsNullOrEmpty(desc) && !desc.StartsWith("[条件:"))
            propObj["description"] = desc;

        // ReadArgs
        if (field.ReadArgs?.Length > 0)
            propObj["x-readArgs"] = field.ReadArgs;

        // ── 枚举 oneOf（优先反射，回退源码解析）──
        Type? enumType = resolved?.EnumType;
        if (enumType != null)
        {
            BuildEnumOneOf(propObj, enumType);
        }
        else if (field.CastType != null)
        {
            // 回退：源码中的 CastType
            var enumDef = packet.LocalEnums.FirstOrDefault(e =>
                string.Equals(e.Name, field.CastType, StringComparison.OrdinalIgnoreCase));
            if (enumDef != null)
            {
                propObj["x-enum"] = field.CastType;
                propObj["oneOf"] = BuildLocalEnumOneOf(enumDef);
            }
        }

        return propObj;
    }

    /// <summary>
    /// 将 TypeDef 转换为 JSON Schema 对象。
    /// </summary>
    private JsonObject BuildTypeDefSchema(TypeDef typeDef)
    {
        var obj = new JsonObject
        {
            ["type"] = "object",
            ["description"] = typeDef.Description
        };

        var props = new JsonObject();
        var req = new JsonArray();

        foreach (var (name, p) in typeDef.Properties)
        {
            var propObj = BuildTypeDefProperty(p);

            // 非可选属性加入 required
            if (!p.IsOptional)
                req.Add(name);

            props[name] = propObj;
        }

        obj["properties"] = props;
        if (req.Count > 0)
            obj["required"] = req;

        return obj;
    }

    private JsonObject BuildTypeDefProperty(TypeDefProperty p)
    {
        // $ref
        if (p.RefPath != null)
        {
            return new JsonObject
            {
                ["$ref"] = p.RefPath,
                ["description"] = p.Description ?? ""
            };
        }

        // 数组
        if (p.SchemaType == "array" && p.ItemsRefPath != null)
        {
            return new JsonObject
            {
                ["type"] = "array",
                ["items"] = new JsonObject { ["$ref"] = p.ItemsRefPath },
                ["description"] = p.Description ?? ""
            };
        }

        var prop = new JsonObject
        {
            ["type"] = p.SchemaType ?? "object"
        };

        if (p.SchemaFormat != null)
        {
            if (p.SchemaFormat.Contains("int"))
            {
                prop["type"] = "integer";
                prop["x-format"] = p.SchemaFormat;
            }
            else if (p.SchemaFormat is "float" or "double")
            {
                prop["type"] = "number";
                prop["x-format"] = p.SchemaFormat;
            }
            else if (p.SchemaFormat == "byte-array")
            {
                prop["type"] = "array";
                prop["x-format"] = "byte-array";
            }
            else
            {
                prop["x-format"] = p.SchemaFormat;
            }
        }

        if (p.Description?.Length > 0)
            prop["description"] = p.Description;

        // 枚举 oneOf
        if (p.EnumType != null)
        {
            BuildEnumOneOf(prop, p.EnumType);
        }

        return prop;
    }

    /// <summary>
    /// 通过反射获取枚举值并生成 oneOf。
    /// </summary>
    private static void BuildEnumOneOf(JsonObject prop, Type enumType)
    {
        var names = Enum.GetNames(enumType);
        var values = Enum.GetValues(enumType);
        var underlying = Enum.GetUnderlyingType(enumType);

        prop["type"] = "integer";
        prop["x-enum"] = enumType.Name;

        var oneOf = new JsonArray();
        for (int i = 0; i < names.Length; i++)
        {
            var val = Convert.ChangeType(values.GetValue(i)!, underlying);
            oneOf.Add(new JsonObject
            {
                ["const"] = JsonValue.Create(val),
                ["title"] = names[i]
            });
        }
        prop["oneOf"] = oneOf;
    }

    /// <summary>
    /// 从源码解析的枚举生成 oneOf。
    /// </summary>
    private static JsonArray BuildLocalEnumOneOf(EnumMeta enumDef)
    {
        var arr = new JsonArray();
        foreach (var v in enumDef.Values)
        {
            var constVal = int.TryParse(v.Value, out var n)
                ? JsonValue.Create(n)
                : JsonValue.Create(v.Value);
            arr.Add(new JsonObject
            {
                ["const"] = constVal,
                ["title"] = v.Name
            });
        }
        return arr;
    }

    /// <summary>
    /// 生成 packets.json 索引文件。
    /// </summary>
    private void GenerateIndex(List<PacketMeta> packets, string schemasDir)
    {
        var index = new JsonObject();

        foreach (var packet in packets.OrderBy(p => p.PacketId))
        {
            var key = $"{packet.PacketIdHex} ({packet.PacketId})";
            var entry = new JsonObject
            {
                ["class"] = packet.ClassName,
                ["namespace"] = packet.Namespace ?? "",
                ["schemaFile"] = $"schemas/{packet.ClassName}.json",
                ["fieldCount"] = packet.Fields.Count
            };

            if (packet.LocalEnums.Count > 0)
            {
                entry["localEnums"] = new JsonArray(
                    packet.LocalEnums.Select(e => JsonValue.Create(e.Name)!).ToArray()
                );
            }

            index[key] = entry;
        }

        var indexPath = Path.Combine(_outputDir, "packets.json");
        var json = JsonSerializer.Serialize(index, JsonOptions);
        File.WriteAllText(indexPath, json);
        Console.WriteLine($"[INFO] 索引文件: {indexPath}");

        // 同时生成一个 overview markdown
        GenerateMarkdownOverview(packets, _outputDir);
    }

    private void GenerateMarkdownOverview(List<PacketMeta> packets, string outputDir)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("# Minecraft Bedrock Protocol — Packet Index");
        sb.AppendLine();
        sb.AppendLine($"共 **{packets.Count}** 个数据包。");
        sb.AppendLine();
        sb.AppendLine("| ID (Hex) | ID (Dec) | Packet Class | Fields | Schema |");
        sb.AppendLine("|----------|----------|-------------|--------|--------|");

        foreach (var p in packets.OrderBy(p => p.PacketId))
        {
            var schemaLink = $"[📄](schemas/{p.ClassName}.json)";
            sb.AppendLine($"| {p.PacketIdHex} | {p.PacketId} | `{p.ClassName}` | {p.Fields.Count} | {schemaLink} |");
        }

        var path = Path.Combine(outputDir, "README.md");
        File.WriteAllText(path, sb.ToString());
        Console.WriteLine($"[INFO] Markdown 概览: {path}");
    }
}
