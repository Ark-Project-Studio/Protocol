using Protocol.Minecraft.Inventory.Item;
using Protocol.Minecraft.Inventory.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeInventorySlot : Packet
{
    public FullContainerName ContainerName = new();
    public byte inventoryId;
    public NetworkItemStackDescriptor item;
    public uint slot;
    public NetworkItemStackDescriptor storageItem;
    public McbeInventorySlot()
    {
        Id = 0x32;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(inventoryId);
        WriteUnsignedVarInt(slot);
        Write(ContainerName);
        Write(storageItem);
        Write(item);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        inventoryId = ReadByte();
        slot = ReadUnsignedVarInt();
        ContainerName = readFullContainerName();
        storageItem = ReadNetworkItemStackDescriptor();
        item = ReadNetworkItemStackDescriptor();
    }
}
