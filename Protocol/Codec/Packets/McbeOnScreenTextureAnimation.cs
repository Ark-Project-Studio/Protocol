using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeOnScreenTextureAnimation : Packet
{
    public int effectId;
    public McbeOnScreenTextureAnimation()
    {
        Id = 0x82;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(effectId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        effectId = ReadInt();
    }
}
