namespace Protocol.Network.MinecraftPacket;
public class McbePurchaseReceipt : Packet
{
    public McbePurchaseReceipt()
    {
        Id = 0x5c;
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
