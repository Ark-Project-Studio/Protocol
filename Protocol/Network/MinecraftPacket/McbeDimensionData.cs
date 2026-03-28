using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeDimensionData : Packet
{
    public DimensionDefinitions definitions;
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
