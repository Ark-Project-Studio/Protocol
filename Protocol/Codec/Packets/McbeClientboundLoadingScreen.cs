using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeClientboundLoadingScreen : Packet
{
    public McbeClientboundLoadingScreen()
    {
        Id = 311;
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
