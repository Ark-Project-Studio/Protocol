using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeMobEquipment : Packet
{
    public Item item;
    public ulong runtimeEntityId;
    public byte selectedSlot;
    public byte slot;
    public byte windowsId;
    public McbeMobEquipment()
    {
        Id = 0x1f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(item);
        Write(slot);
        Write(selectedSlot);
        Write(windowsId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        item = ReadItem();
        slot = ReadByte();
        selectedSlot = ReadByte();
        windowsId = ReadByte();
    }
}
