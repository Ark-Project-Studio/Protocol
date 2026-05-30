using ProtocolDocGen;

// ═══════════════════════════════════════════════════════════
//  ProtocolDocGen — Minecraft Bedrock Protocol JSON Schema + Enum 文档生成器
//  引用 Protocol.csproj，源码解析 + 反射提取，生成完整的协议文档。
// ═══════════════════════════════════════════════════════════

var solutionRoot = FindSolutionRoot();
if (solutionRoot == null)
{
    Console.Error.WriteLine("[ERROR] 找不到解决方案根目录 (Potocol.slnx)。");
    return 1;
}

var packetDir = Path.Combine(solutionRoot, "Protocol", "Network", "MinecraftPacket");
var schemaOutputDir = Path.Combine(solutionRoot, "doc", "protocol-schema");

Console.WriteLine("╔══════════════════════════════════════════════╗");
Console.WriteLine("║   Minecraft Protocol Doc Generator           ║");
Console.WriteLine("╚══════════════════════════════════════════════╝");
Console.WriteLine($"[INFO] 解决方案目录: {solutionRoot}");
Console.WriteLine($"[INFO] 数据包源码目录: {packetDir}");
Console.WriteLine($"[INFO] Schema 输出目录: {schemaOutputDir}");
Console.WriteLine();

// ═══ Phase 1: 源码解析 → JSON Schema ═══
Console.WriteLine("─── Phase 1: Packet Schema 生成 ───");

// 初始化 TypeResolver（反射扫描 Packet.Read* 方法）
var typeResolver = new TypeResolver();
typeResolver.Initialize();
ReadMethodMapper.SetResolver(typeResolver);
Console.WriteLine($"[INFO] TypeResolver 已初始化。");

var parser = new PacketParser(packetDir);
var packets = parser.ParseAll();
Console.WriteLine($"[INFO] 解析到 {packets.Count} 个数据包定义。");

var schemaGen = new SchemaGenerator(schemaOutputDir, typeResolver);
schemaGen.GenerateAll(packets);

var totalFields = packets.Sum(p => p.Fields.Count);
var withEnums = packets.Count(p => p.LocalEnums.Count > 0);
Console.WriteLine($"[STAT] 总字段数: {totalFields}");
Console.WriteLine($"[STAT] 包含本地枚举的数据包: {withEnums}");

// ═══ Phase 2: 反射 → 枚举文档 ═══
Console.WriteLine();
Console.WriteLine("─── Phase 2: 枚举文档生成 (反射) ───");
var enumReflector = new EnumReflector();
var allEnums = enumReflector.ExtractAll();
Console.WriteLine($"[INFO] 反射提取到 {allEnums.Count} 个公开枚举。");

var allEnumDocDir = Path.Combine(solutionRoot, "doc");
var enumDocGen = new EnumDocGenerator();

// 先对比旧文档，生成 unmatch.json（必须在 GenerateEnumDoc 之前）
enumDocGen.GenerateUnmatch(allEnums, allEnumDocDir);

// 再生成完整的 protocol-enums.json（合并旧的 Source 信息）
enumDocGen.GenerateEnumDoc(allEnums, allEnumDocDir);

Console.WriteLine();
Console.WriteLine("[DONE] 全部文档生成完成！");
return 0;


/// <summary>
/// 向上查找包含 Potocol.slnx 的目录。
/// </summary>
static string? FindSolutionRoot()
{
    var dir = AppContext.BaseDirectory;
    while (dir != null)
    {
        if (File.Exists(Path.Combine(dir, "Potocol.slnx")) ||
            File.Exists(Path.Combine(dir, "Protocol.sln")))
            return dir;

        var parent = Path.GetDirectoryName(dir);
        if (parent == dir) break;
        dir = parent;
    }
    return null;
}
