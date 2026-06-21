using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeRemoveEntityDeprecated : Packet
{
    public McbeRemoveEntityDeprecated()
    {
        Id = 128;
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
