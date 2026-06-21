using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeAddEntityDeprecated : Packet
{
    public McbeAddEntityDeprecated()
    {
        Id = 127;
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
