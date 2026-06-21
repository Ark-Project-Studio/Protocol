using Protocol.Minecraft.Inventory.Item;
using Protocol.Minecraft.Inventory.Transaction;
using Protocol.Network;

namespace Protocol.Codec.Packets;
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
        WriteSlice(Contents, WriteCereal);
        Write(ContainerName);
        WriteCereal(StorageItem);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        InventoryId = ReadUnsignedVarInt();
        Contents = ReadSlice(ReadCerealNetworkItemStackDescriptor);
        ContainerName = readFullContainerName();
        StorageItem = ReadCerealNetworkItemStackDescriptor();
    }
}
