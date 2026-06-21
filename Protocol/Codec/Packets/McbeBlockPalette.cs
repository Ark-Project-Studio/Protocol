using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeBlockPalette : Packet
{
    public McbeBlockPalette()
    {
        Id = 116;
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
