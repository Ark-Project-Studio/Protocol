using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public class McbeMapInfoRequest : Packet
{
    public long mapId;
    public pixelList pixellist;
    public McbeMapInfoRequest()
    {
        Id = 0x44;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(mapId);
        WriteUnsignedVarInt(0);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        mapId = ReadSignedVarLong();
        pixellist = ReadPixelList();
    }
}
