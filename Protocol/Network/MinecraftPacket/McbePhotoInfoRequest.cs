namespace Protocol.Network.MinecraftPacket;
public class McbePhotoInfoRequest : Packet
{
    public McbePhotoInfoRequest()
    {
        Id = 173;
        IsMcbe = true;
    }

    public long PhotoID { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(PhotoID);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        PhotoID = ReadSignedVarLong();
    }
}
