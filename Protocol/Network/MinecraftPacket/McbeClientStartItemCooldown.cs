namespace Protocol.Network.MinecraftPacket;
public class McbeClientStartItemCooldown : Packet
{
    public McbeClientStartItemCooldown()
    {
        Id = 176;
        IsMcbe = true;
    }

    public string Category { get; set; } = string.Empty;
    public int Duration { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Category);
        WriteSignedVarInt(Duration);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Category = ReadString();
        Duration = ReadSignedVarInt();
    }
}
