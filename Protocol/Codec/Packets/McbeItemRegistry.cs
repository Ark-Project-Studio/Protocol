using Protocol.Minecraft;
using Protocol.Minecraft.NBT;
using Protocol.Network;

namespace Protocol.Codec.Packets;

public class ItemEntry
{
	public string Name { get; set; }


	public short RuntimeID { get; set; }


	public bool ComponentBased { get; set; }


	public int Version { get; set; }


	public Nbt Data { get; set; }
}

public class McbeItemRegistry : Packet
{
	public McbeItemRegistry()
	{
		Id = 162;
		IsMcbe = true;
	}


	public List<ItemEntry> Items { get; set; } = new();


	protected override void EncodePacket()
	{
		base.EncodePacket();
		WriteUnsignedVarInt((uint)Items.Count);
		foreach (var item in Items)
		{
			Write(item.Name);
			Write(item.RuntimeID);
			Write(item.ComponentBased);
			WriteVarInt(item.Version);
			Write(item.Data);
		}
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		Items.Clear();
		var count = ReadUnsignedVarInt();
		Items.Capacity = (int)count;

		for (var i = 0; i < count; i++)
		{
			var item = new ItemEntry();
			item.Name = ReadString();
			item.RuntimeID = ReadShort();
			item.ComponentBased = ReadBool();
			item.Version = ReadVarInt();
			item.Data = ReadNbt();
			Items.Add(item);
		}
	}
}
