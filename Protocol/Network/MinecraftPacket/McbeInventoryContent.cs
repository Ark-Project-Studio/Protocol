using Protocol.Minecraft.Inventory.Item;
using Protocol.Minecraft.Inventory.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeInventoryContent : Packet
{
    public FullContainerName ContainerName = new();
    public NetworkItemStackDescriptor[] Contents = [];
    public uint InventoryId;
    public NetworkItemStackDescriptor StorageItem = NetworkItemStackDescriptor.Empty;
    public McbeInventoryContent()
    {
        Id = 0x31;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(InventoryId);
        WriteSlice(Contents, Write);
        Write(ContainerName);
        Write(StorageItem);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        InventoryId = ReadUnsignedVarInt();
        Contents = ReadSlice(ReadNetworkItemStackDescriptor);
        ContainerName = readFullContainerName();
        StorageItem = ReadNetworkItemStackDescriptor();
    }
}
