using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeItemFrameDropItem : Packet
{
    public McbeItemFrameDropItem()
    {
        Id = 71;
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
