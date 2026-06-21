using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeClientToServerHandshake : Packet
{
    public McbeClientToServerHandshake()
    {
        Id = 0x04;
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
