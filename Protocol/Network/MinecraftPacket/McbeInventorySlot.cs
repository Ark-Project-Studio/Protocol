using Protocol.Minecraft.Inventory.Item;
using Protocol.Minecraft.Inventory.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeInventorySlot : Packet
{
    public Optional<FullContainerName> ContainerName = new();
    public uint inventoryId;
    public NetworkItemStackDescriptor item;
    public uint slot;
    public Optional<NetworkItemStackDescriptor> storageItem = new Optional<NetworkItemStackDescriptor>();
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
        if (ContainerName.HasValue)
        {
	        Write(ContainerName.HasValue);
	        Write(ContainerName.Value);
        }
        else
        {
	        Write(false);
        }
		if (storageItem.HasValue)
		{
			Write(storageItem.HasValue);
			WriteCereal(storageItem.Value);
		}
		else
		{
			Write(false);
		}
        WriteCereal(item);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        inventoryId = ReadUnsignedVarInt();
        slot = ReadUnsignedVarInt();
       
        if (ReadBool())
        {
	        ContainerName = new Optional<FullContainerName>(readFullContainerName());
        }

        if (ReadBool())
        {
	        storageItem = new Optional<NetworkItemStackDescriptor>(ReadCerealNetworkItemStackDescriptor());
        }
        item = ReadCerealNetworkItemStackDescriptor();
    }
}
