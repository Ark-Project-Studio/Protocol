using Protocol.Network;
using Protocol.Utils;

namespace Protocol.Codec.Packets;

public class PixelList
{
	public List<PixelsData> mapData = new();
}

public class PixelsData
{
	public ushort index;
	public uint pixel;
}
public class McbeMapInfoRequest : Packet
{
    public long mapId;
    public PixelList pixellist;
    public McbeMapInfoRequest()
    {
        Id = 0x44;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(mapId);
        var pixels = pixellist?.mapData ?? new List<PixelsData>();
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
