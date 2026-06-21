using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeCompressedBiomeDefinitionList : Packet
{
    public McbeCompressedBiomeDefinitionList()
    {
        Id = 301;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
    }
}
