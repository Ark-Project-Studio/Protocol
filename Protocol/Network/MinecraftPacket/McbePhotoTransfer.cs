namespace Protocol.Network.MinecraftPacket;
public class McbePhotoTransfer : Packet
{
    public string fileName;
    public string imageData;
    public string unknown2;
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
        Write(unknown2);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        fileName = ReadString();
        imageData = ReadString();
        unknown2 = ReadString();
    }
}
