using Protocol.Minecraft;
using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeInventoryContent : Packet
{
    public FullContainerName ContainerName = new();
    public NetworkItemStackDescriptor[] input;
    public uint inventoryId;
    public NetworkItemStackDescriptor storageItem;
    public McbeInventoryContent()
    {
        Id = 0x31;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(inventoryId);
        WriteSlice(input, Write);
        Write(ContainerName);
        Write(storageItem);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        inventoryId = ReadUnsignedVarInt();
        input = ReadSlice(ReadNetworkItemStackDescriptor);
        ContainerName = readFullContainerName();
        storageItem = ReadNetworkItemStackDescriptor();
    }
}
