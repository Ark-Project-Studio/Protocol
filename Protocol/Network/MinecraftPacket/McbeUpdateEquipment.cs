using Protocol.Minecraft;
using Protocol.Minecraft.NBT;

namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateEquipment : Packet
{
    public long entityId;
    public Nbt namedtag;
    public int size;
    public byte windowId;
    public byte windowType;
    public McbeUpdateEquipment()
    {
        Id = 0x51;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(windowId);
        Write(windowType);
        WriteSignedVarInt(size);
        WriteSignedVarLong(entityId);
        Write(namedtag);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        windowId = ReadByte();
        windowType = ReadByte();
        size = ReadSignedVarInt();
        entityId = ReadSignedVarLong();
        namedtag = ReadNbt();
    }
}
