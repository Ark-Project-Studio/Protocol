namespace Protocol.Network.MinecraftPacket;
public class McbeLecternUpdate : Packet
{
    public McbeLecternUpdate()
    {
        Id = 0x7d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
    }
}
