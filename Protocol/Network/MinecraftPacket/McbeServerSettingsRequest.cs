namespace Protocol.Network.MinecraftPacket;
public class McbeServerSettingsRequest : Packet
{
    public McbeServerSettingsRequest()
    {
        Id = 0x66;
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
