namespace Protocol.Network.MinecraftPacket;

public class McbeServerPresenceInfo : Packet
{
    public McbeServerPresenceInfo()
    {
        Id = 347;
        IsMcbe = true;
    }

    public Optional<string> ExperienceId { get; set; } = new();
    public Optional<string> WorldName { get; set; } = new();

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(ExperienceId.HasValue);
        if (ExperienceId.HasValue)
            Write(ExperienceId.Value);
        Write(WorldName.HasValue);
        if (WorldName.HasValue)
            Write(WorldName.Value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        if (ReadBool())
            ExperienceId = new Optional<string>(ReadString());
        if (ReadBool())
            WorldName = new Optional<string>(ReadString());
    }
}
