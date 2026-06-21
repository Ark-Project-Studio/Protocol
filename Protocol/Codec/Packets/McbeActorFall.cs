using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeActorFall : Packet
{
    public McbeActorFall()
    {
        Id = 37;
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
