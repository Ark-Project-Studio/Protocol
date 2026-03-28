using Protocol.Minecraft;
using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeInventoryContent : Packet
{
    public FullContainerName ContainerName = new();
    public ItemStacks input;
    public uint inventoryId;
    public Item storageItem;
    public McbeInventoryContent()
    {
        Id = 0x31;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(inventoryId);
        Write(input);
        Write(ContainerName);
        Write(storageItem);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        inventoryId = ReadUnsignedVarInt();
        input = ReadItemStacks();
        ContainerName = readFullContainerName();
        storageItem = ReadItem();
    }
}
