namespace Protocol.Network.MinecraftPacket;

public class McbeServerStoreInfo : Packet
{
    public McbeServerStoreInfo()
    {
        Id = 346;
        IsMcbe = true;
    }

    public string StoreId { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(StoreId);
        Write(StoreName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        StoreId = ReadString();
        StoreName = ReadString();
    }
}
