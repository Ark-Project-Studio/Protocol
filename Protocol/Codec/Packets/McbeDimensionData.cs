using Protocol.Minecraft;
using Protocol.Minecraft.Level;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeDimensionData : Packet
{
    public DimensionData[] definitions;
    public McbeDimensionData()
    {
        Id = 0xb4;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(definitions);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        definitions = ReadDimensionDefinitions();
    }
}
