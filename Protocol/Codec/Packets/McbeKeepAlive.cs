using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeKeepAlive : Packet
{
    public McbeKeepAlive()
    {
        Id = 0;
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
