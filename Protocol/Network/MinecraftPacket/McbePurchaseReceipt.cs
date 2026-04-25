namespace Protocol.Network.MinecraftPacket;
public class McbePurchaseReceipt : Packet
{
    public string[] purchaseReceipts;

    public McbePurchaseReceipt()
    {
        Id = 0x5c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSlice(purchaseReceipts ?? System.Array.Empty<string>(), Write);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        purchaseReceipts = ReadSlice(ReadString);
    }
}
