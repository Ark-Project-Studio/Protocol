using Protocol.Minecraft;
using Protocol.Minecraft.Level.Block;
using Protocol.Minecraft.NBT;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeBlockEntityData : Packet
{
    public BlockCoordinates coordinates;
    public Nbt namedtag;
    public McbeBlockEntityData()
    {
        Id = 0x38;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(coordinates);
        Write(namedtag);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        coordinates = ReadBlockCoordinates();
        namedtag = ReadNbt();
    }
}
