namespace Protocol.Network.MinecraftPacket;

public class McbeServerPresenceInfo : Packet
{
    public McbeServerPresenceInfo()
    {
        Id = 347;
        IsMcbe = true;
    }

    public string ExperienceName { get; set; } = string.Empty;
    public string WorldName { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(ExperienceName);
        Write(WorldName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ExperienceName = ReadString();
        WorldName = ReadString();
    }
}
