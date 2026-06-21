using Protocol.Minecraft;
using Protocol.Minecraft.NBT;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeAvailableEntityIdentifiers : Packet
{
    public Nbt namedtag;
    public McbeAvailableEntityIdentifiers()
    {
        Id = 0x77;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(namedtag);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        namedtag = ReadNbt();
    }
}
