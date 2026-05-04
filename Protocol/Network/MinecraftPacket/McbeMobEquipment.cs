using System.Collections;
using Protocol.Minecraft.Inventory.Item;

namespace Protocol.Network.MinecraftPacket;

public class McbeMobEquipment : Packet
{
	/// <summary>
	/// Runtime ID of the mob or player whose equipment is changing.
	/// </summary>
	public ulong runtimeEntityId;

	/// <summary>
	/// The item stack being equipped. Empty item indicates no item.
	/// </summary>
	public NetworkItemStackDescriptor item;

	/// <summary>
	/// The inventory slot index the item occupies within the container.
	/// </summary>
	public byte slot;

	/// <summary>
	/// The hotbar slot that is currently selected by the player.
	/// Matches slot for non-player mobs.
	/// </summary>
	public byte selectedSlot;

	/// <summary>
	/// Identifies which container the item belongs to, e.g. inventory or offhand.
	/// </summary>
	public byte containerId;

	public McbeMobEquipment()
	{
		Id = 0x1f;
		IsMcbe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		WriteUnsignedVarLong(runtimeEntityId);
		WriteItemStack(item);
		Write(slot);
		Write(selectedSlot);
		Write(containerId);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		runtimeEntityId = ReadUnsignedVarLong();
		item = ReadItemStack();
		slot = ReadByte();
		selectedSlot = ReadByte();
		containerId = ReadByte();
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
