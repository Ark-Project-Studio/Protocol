namespace Protocol.Network.MinecraftPacket;
public class McbePhotoTransfer : Packet
{
    public string fileName;
    public string imageData;
    public string bookId;
    public byte type;
    public byte sourceType;
    public long ownerId;
    public string newPhotoName;
    public McbePhotoTransfer()
    {
        Id = 0x63;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(fileName);
        Write(imageData);
        Write(bookId);
        Write(type);
        Write(sourceType);
        Write(ownerId);
        Write(newPhotoName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        fileName = ReadString();
        imageData = ReadString();
        bookId = ReadString();
        type = ReadByte();
        sourceType = ReadByte();
        ownerId = ReadLong();
        newPhotoName = ReadString();
    }
}
