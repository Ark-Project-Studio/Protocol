using Protocol.Network;
using Protocol.Utils;

namespace Protocol.Codec.Packets;
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
        var pixels = pixellist?.mapData ?? new List<pixelsData>();
        Write((uint)pixels.Count);
        foreach (var pixel in pixels)
        {
            Write(pixel.pixel);
            Write(pixel.index);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        mapId = ReadSignedVarLong();
        pixellist = ReadPixelList();
    }
}
