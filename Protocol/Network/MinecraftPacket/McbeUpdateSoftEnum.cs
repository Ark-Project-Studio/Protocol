namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateSoftEnum : Packet
{
    public McbeUpdateSoftEnum()
    {
        Id = 0x72;
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
