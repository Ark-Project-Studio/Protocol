using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeSetMovementAuthority : Packet
{
    public McbeSetMovementAuthority()
    {
        Id = 319;
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
