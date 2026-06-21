using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeLevelSoundEventV1 : Packet
{
    public McbeLevelSoundEventV1()
    {
        Id = 24;
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
