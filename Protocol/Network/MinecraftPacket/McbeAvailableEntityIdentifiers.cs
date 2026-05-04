using Protocol.Minecraft;
using Protocol.Minecraft.NBT;

namespace Protocol.Network.MinecraftPacket;
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
