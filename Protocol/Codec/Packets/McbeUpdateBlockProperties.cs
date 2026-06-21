using Protocol.Minecraft;
using Protocol.Minecraft.NBT;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeUpdateBlockProperties : Packet
{
    public Nbt namedtag;
    public McbeUpdateBlockProperties()
    {
        Id = 0x86;
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
