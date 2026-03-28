namespace Protocol.Network.MinecraftPacket;
public class McbeGuiDataPickItem : Packet
{
    public McbeGuiDataPickItem()
    {
        Id = 0x36;
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
