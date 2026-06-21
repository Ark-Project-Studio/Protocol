using Protocol.Network;

namespace Protocol.Codec.Level;

public class PresenceConfiguration
{
    public Optional<string> ExperienceName { get; set; } = new();
    public Optional<string> WorldName { get; set; } = new();
    public string RichPresenceId { get; set; } = string.Empty;

    public void Write(Packet packet)
    {
        packet.Write(ExperienceName.HasValue);
        if (ExperienceName.HasValue)
            packet.Write(ExperienceName.Value);
        packet.Write(WorldName.HasValue);
        if (WorldName.HasValue)
            packet.Write(WorldName.Value);
        packet.Write(RichPresenceId);
    }

    public void Read(Packet packet)
    {
        if (packet.ReadBool())
            ExperienceName = new Optional<string>(packet.ReadString());
        if (packet.ReadBool())
            WorldName = new Optional<string>(packet.ReadString());
        RichPresenceId = packet.ReadString();
    }
}
