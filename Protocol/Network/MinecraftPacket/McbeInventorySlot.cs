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
        WriteItemStack(storageItem);
        WriteItemStack(item);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        inventoryId = ReadByte();
        slot = ReadUnsignedVarInt();
        ContainerName = readFullContainerName();
        storageItem = ReadItemStack();
        item = ReadItemStack();
    }
	private NetworkItemStackDescriptor ReadItemStack()
	{
		int id = ReadShort();
		var stack = new NetworkItemStackDescriptor { Id = id };
		if (id == 0)
		{
			return stack;
		}

		stack.StackSize = ReadUshort();
		stack.Aux = ReadUnsignedVarInt();
		var hasNetId = ReadBool();
		if (hasNetId)
		{
			stack.NetId = new Optional<int>((int)ReadUnsignedVarInt());
		}

		stack.BlockRuntimeId = (int)ReadUnsignedVarInt();

		stack.UserData = ReadString();
		return stack;
	}

	private void WriteItemStack(NetworkItemStackDescriptor stack)
	{
		if (stack == null || stack.Id == 0)
		{
			Write((short)0);
			return;
		}

		Write((short)stack.Id);

		Write(stack.StackSize);
		WriteUnsignedVarInt(stack.Aux);
		if (stack.NetId.HasValue)
		{
			Write(true);
			WriteSignedVarInt(stack.NetId.Value);
		}
		else
		{
			Write(false);
		}

		WriteSignedVarInt(stack.BlockRuntimeId);

		Write(stack.UserData ?? string.Empty);
	}
}
