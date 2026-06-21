using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbePassengerJump : Packet
{
    public McbePassengerJump()
    {
        Id = 20;
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
