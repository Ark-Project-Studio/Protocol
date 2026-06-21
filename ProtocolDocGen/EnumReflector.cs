using System.Reflection;
using Protocol.Codec.Packets;
using Protocol.Network;

namespace ProtocolDocGen;

/// <summary>
/// 通过反射从已编译的 Protocol.dll 中提取所有公开枚举定义。
/// 解决了纯源码解析无法准确获取嵌套类型、Flags 属性等元数据的问题。
/// </summary>
public class EnumReflector
{
    /// <summary>
    /// 扫描 Protocol 程序集中命名空间为 Protocol.Minecraft.* 和 Protocol.Network.MinecraftPacket 的所有公开枚举。
    /// </summary>
    public List<EnumDef> ExtractAll()
    {
        var results = new List<EnumDef>();

        // 用 McbeLogin 所在的程序集作为入口
        var asm = typeof(McbeLogin).Assembly;
        // 也用 Packet 所在程序集（同一个）
        var allTypes = asm.GetExportedTypes();

        foreach (var type in allTypes)
        {
            if (!type.IsEnum) continue;

            // 只关注 Protocol.Minecraft.* 和 Protocol.Network.* 命名空间
            var ns = type.Namespace ?? "";
            if (!ns.StartsWith("Protocol.Minecraft", StringComparison.Ordinal) &&
                !ns.StartsWith("Protocol.Network", StringComparison.Ordinal))
                continue;

            var def = ExtractEnum(type);
            if (def != null)
                results.Add(def);
        }

        return results;
    }

    private static EnumDef? ExtractEnum(Type type)
    {
        var underlying = Enum.GetUnderlyingType(type);
        var isFlags = type.GetCustomAttribute<FlagsAttribute>() != null;

        var members = new List<EnumMemberDef>();
        var names = Enum.GetNames(type);
        var values = Enum.GetValues(type);
        var underlyingValues = values.Cast<object>().Select(v => Convert.ChangeType(v, underlying)).ToArray();

        for (int i = 0; i < names.Length; i++)
        {
            members.Add(new EnumMemberDef
            {
                Name = names[i],
                Value = underlyingValues[i]?.ToString() ?? "0"
            });
        }

        return new EnumDef
        {
            Name = type.Name,
            FullName = GetFullName(type),
            Namespace = type.Namespace ?? "",
            UnderlyingType = underlying.FullName ?? underlying.Name,
            IsFlags = isFlags,
            IsPublic = type.IsPublic,
            Members = members
        };
    }

    /// <summary>
    /// 获取完整类型名：嵌套类型用 + 连接 (如 McbeInventoryTransaction+TriggerType)
    /// </summary>
    private static string GetFullName(Type type)
    {
        if (type.DeclaringType == null)
            return type.FullName ?? type.Name;

        // 递归构建嵌套类型名
        var stack = new Stack<string>();
        var current = type;
        while (current != null)
        {
            stack.Push(current.Name);
            current = current.DeclaringType;
        }

        var ns = type.Namespace;
        return ns != null ? $"{ns}.{string.Join("+", stack)}" : string.Join("+", stack);
    }
}

public class EnumDef
{
    public string Name { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Namespace { get; set; } = "";
    public string UnderlyingType { get; set; } = "";
    public bool IsFlags { get; set; }
    public bool IsPublic { get; set; }
    public List<EnumMemberDef> Members { get; set; } = [];
}

public class EnumMemberDef
{
    public string Name { get; set; } = "";
    public string Value { get; set; } = "";
}
