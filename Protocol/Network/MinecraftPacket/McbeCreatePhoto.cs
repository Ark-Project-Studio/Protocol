namespace Protocol.Network.MinecraftPacket;
public class McbeCreatePhoto : Packet
{
    public McbeCreatePhoto()
    {
        Id = 171;
        IsMcbe = true;
    }

    public long EntityUniqueID { get; set; }
    public string PhotoName { get; set; } = string.Empty;
    public string ItemName { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(EntityUniqueID);
        Write(PhotoName);
        Write(ItemName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        EntityUniqueID = ReadLong();
        PhotoName = ReadString();
        ItemName = ReadString();
    }
}
