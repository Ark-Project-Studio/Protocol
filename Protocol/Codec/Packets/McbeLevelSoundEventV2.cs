using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeLevelSoundEventV2 : Packet
{
    public McbeLevelSoundEventV2()
    {
        Id = 120;
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
