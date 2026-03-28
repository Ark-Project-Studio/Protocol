using Protocol.Minecraft;
using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeInventorySlot : Packet
{
    public FullContainerName ContainerName = new();
    public uint inventoryId;
    public Item item;
    public uint slot;
    public Item storageItem;
    public McbeInventorySlot()
    {
        Id = 0x32;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(inventoryId);
        WriteUnsignedVarInt(slot);
        Write(ContainerName);
        Write(storageItem);
        Write(item);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        inventoryId = ReadUnsignedVarInt();
        slot = ReadUnsignedVarInt();
        ContainerName = readFullContainerName();
        storageItem = ReadItem();
        item = ReadItem();
    }
}
