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
			WriteItemStack(storageItem.Value);
		}
		else
		{
			Write(false);
		}
        WriteItemStack(item);
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
	        storageItem = new Optional<NetworkItemStackDescriptor>(ReadItemStack());
        }
        item = ReadItemStack();
    }
	private NetworkItemStackDescriptor ReadItemStack()
	{
		int id = ReadShort();
		var stack = new NetworkItemStackDescriptor { Id = id };
	
		stack.StackSize = ReadUshort();
		stack.Aux = ReadUnsignedVarInt();
		var hasNetId = ReadBool();
		if (hasNetId)
		{
			var u = ReadUnsignedVarInt();
			switch (u)
			{
				case 0 :
					case 1:
					case 2:
						stack.NetId = new Optional<int>(ReadSignedVarInt());
					break;
			}
		}

		stack.BlockRuntimeId = ReadUnsignedVarInt();
		stack.UserData = ReadString();
		return stack;
	}

	private void WriteItemStack(NetworkItemStackDescriptor stack)
	{
		Write((short)stack.Id);

		Write(stack.StackSize);
		WriteUnsignedVarInt(stack.Aux);
		if (stack.NetId.HasValue)
		{
			Write(true);
			
			WriteUnsignedVarInt(0);
			WriteSignedVarInt(stack.NetId.Value);
		}
		else
		{
			Write(false);
		}

		WriteUnsignedVarInt(stack.BlockRuntimeId);

		Write(stack.UserData ?? string.Empty);
	}
}
