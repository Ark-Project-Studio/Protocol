using Protocol.Minecraft;
using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeInventoryContent : Packet
{
    public FullContainerName ContainerName = new();
    public Item[] input;
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
        WriteSlice(input,Write,true);
        Write(ContainerName);
        Write(storageItem,true);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        inventoryId = ReadUnsignedVarInt();
        input = ReadSlice(ReadItem,true);
        ContainerName = readFullContainerName();
        storageItem = ReadItem();
    }
}
