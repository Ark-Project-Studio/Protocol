using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using fNbt;
using Protocol.Minecraft;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(MetadataInts metadata)
		{
			if (metadata == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint)metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				MetadataInt slot = metadata[i] as MetadataInt;
				if (slot != null)
				{
					WriteUnsignedVarInt((uint)slot.Value);
				}
			}
		}

		public MetadataInts ReadMetadataInts()
		{
			MetadataInts metadata = new MetadataInts();
			uint count = ReadUnsignedVarInt();

			for (byte i = 0; i < count; i++)
			{
				metadata[i] = new MetadataInt((int)ReadUnsignedVarInt());
			}

			return metadata;
		}

		public void Write(List<CreativeItemEntry> itemStacks)
		{
			WriteUnsignedVarInt((uint)itemStacks.Count);

			var netId = 0;
			foreach (var item in itemStacks)
			{
				item.Item.RuntimeId = 0;
				WriteUnsignedVarInt((uint)netId);
				Write(item.Item,false);
				WriteUnsignedVarInt(item.GroupIndex);
				netId++;
			}
		}

		public List<CreativeItemEntry> ReadCreativeItemStacks()
		{
			var metadata = new List<CreativeItemEntry>();

			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				var networkId = ReadUnsignedVarInt();
				Item item = ReadItem(false);
				item.NetworkId = (int)networkId;
				uint groupIndex = ReadUnsignedVarInt();
				metadata.Add(new CreativeItemEntry(groupIndex, item));
			}

			return metadata;
		}

		public void Write(List<creativeGroup> groups)
		{
			WriteUnsignedVarInt((uint)groups.Count);

			foreach (var group in groups)
			{
				Write(group.Category);
				Write(group.Name);
				Write(group.Icon,false);
			}
		}

		public List<creativeGroup> ReadCreativeGroups()
		{
			var group = new List<creativeGroup>();

			var groupCount = ReadUnsignedVarInt();
			for (int i = 0; i < groupCount; i++)
			{
				int category = ReadInt();
				string name = ReadString();
				Item item = ReadItem(false);
				group.Add(new creativeGroup(category, name, item));
			}

			return group;
		}

		public void Write(ItemStacks itemStacks)
		{
			if (itemStacks == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint)itemStacks.Count);
			for (int i = 0; i < itemStacks.Count; i++)
			{
				Write(itemStacks[i],true);
			}
		}

		public ItemStacks ReadItemStacks()
		{
			var metadata = new ItemStacks();

			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				int networkId = 0;
				if (this is McbeCreativeContent) networkId = ReadVarInt();
				Item item = ReadItem(true);
				item.NetworkId = networkId;
				metadata.Add(item);
			}

			return metadata;
		}
		public void WriteItems(ItemStacks itemStacks)
		{
			if (itemStacks == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint)itemStacks.Count);
			for (int i = 0; i < itemStacks.Count; i++)
			{
				Write(itemStacks[i], false);
			}
		}
		private ItemStacks ReadItems()
		{
			var items = new ItemStacks();

			var count = ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				items.Add(ReadItem(false));
			}

			return items;
		}

		private const int ShieldId = 355;

		public void Write(Item stack,bool withnet = true)
		{
			
			WriteSignedVarInt(stack.Id);

			Write((ushort)stack.Count);
			WriteUnsignedVarInt((uint)stack.Metadata);
			if (withnet)
			{
				if (stack.NetworkId != -1)
				{
					Write(true);
					WriteVarInt(stack.NetworkId);
				}
				else
				{
					Write(false);
				}
			}
		

			WriteSignedVarInt(stack.RuntimeId);

			Write("none");
		}

		public Item ReadItem(bool withnet = true)
		{
			int id = ReadSignedVarInt();
			if (id == 0)
			{
				return new ItemAir();
			}

			short count = (short)ReadUshort();
			var metadata = ReadUnsignedVarInt();


			Item stack = new ItemAir();
			stack.Metadata = (short)metadata;
			stack.Id = (short)id;
			if(withnet){
				var readBool = ReadBool();
				if (readBool)
				{
					stack.NetworkId = ReadVarInt();
				}
			}


			stack.RuntimeId = ReadVarInt();

			var readString = ReadString();
			return stack;
		}


		public static byte[] GetNbtData(NbtCompound nbtCompound, bool useVarInt = true)
		{
			nbtCompound.Name = string.Empty;
			var file = new NbtFile(nbtCompound);
			file.BigEndian = false;
			file.UseVarInt = useVarInt;

			return file.SaveToBuffer(NbtCompression.None);
		}

		public void Write(MetadataDictionary metadata)
		{
			if (metadata != null)
			{
				metadata.WriteTo(_writer);
			}
		}

		public MetadataDictionary ReadMetadataDictionary()
		{
			var reader = new BinaryReader(_reader);
			var dictionary = MetadataDictionary.FromStream(reader);

			return dictionary;
		}

		public void WriteRecipeIngredient(Item stack)
		{
			if (stack == null || stack.Id == 0)
			{
				Write(false);
				WriteVarInt(0);
				return;
			}

			Write(true);
			var translated = new TranslatedItem(0, 0);
			if (translated.Id != stack.Id)
			{
				Write((short)translated.Id);
				Write(translated.Meta);
			}
			else
			{
				Write(stack.Id);
				Write(stack.Metadata);
			}

			WriteSignedVarInt(stack.Count);
		}

		public Item ReadRecipeData()
		{
			short type = ReadByte();

			if (type == 1)
			{
				short id = ReadShort();
				short meta = ReadShort();
				short count = (short)ReadSignedVarInt();

				return ItemFactory.GetItem(id, meta, count);
			}
			else if (type == 2)
			{
				string expression = ReadString();
				int version = ReadByte();
				short count = (short)ReadSignedVarInt();

				return ItemFactory.GetItem(ItemFactory.GetItemIdByName(expression));
			}
			else if (type == 3)
			{
				string sId = ReadString();
				short count = (short)ReadSignedVarInt();

				return ItemFactory.GetItem(sId, 0, count);
			}
			else if (type == 4)
			{
				string sId = ReadString();
				short meta = ReadShort();

				return new ItemAir();
			}
			else if (type == 5)
			{
				string stri = ReadString();

				ItemFactory.GetItem(ItemFactory.GetItemIdByName(stri));
			}

			short coun = (short)ReadSignedVarInt();

			return new ItemAir();
		}

		public Item ReadShapedRecipeIngredient()
		{
			short type = ReadByte();
			if (type == -1)
			{
			}

			return new ItemAir();
		}

		public void Write(Itemstates itemstates)
		{
			if (itemstates == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint)itemstates.Count);
			foreach (var itemstate in itemstates)
			{
				Write(itemstate.Name);
				Write(itemstate.Id);
				Write(itemstate.ComponentBased);
				WriteVarInt(itemstate.Version);
				Nbt nbt = new Nbt
				{
					NbtFile = new NbtFile
					{
						BigEndian = false,
						UseVarInt = true,
						RootTag = new NbtCompound("")
					}
				};
				if (itemstate.Components.Count() > 0)
				{
					using (MemoryStream stream = new MemoryStream(itemstate.Components))
					{
						NbtFile file = new NbtFile();
						file.LoadFromStream(stream, NbtCompression.None);
						var componentNbt = new NbtCompound("")
						{
							file.RootTag as NbtCompound
						};
						nbt.NbtFile.RootTag = componentNbt;
					}
				}

				Write(nbt);
			}
		}

		public Itemstates ReadItemstates()
		{
			var result = new Itemstates();
			uint count = ReadUnsignedVarInt();
			for (int runtimeId = 0; runtimeId < count; runtimeId++)
			{
				var name = ReadString();
				var legacyId = ReadShort();
				var component = ReadBool();
				var version = ReadVarInt();
				var components = ReadNbt();

				byte[] componentValue = new byte[0];

				if (components.NbtFile.RootTag["components"] != null)
				{
					using (MemoryStream stream = new MemoryStream())
					{
						NbtFile file = new NbtFile(components.NbtFile.RootTag["components"] as NbtCompound);
						file.SaveToStream(stream, NbtCompression.None);
						componentValue = stream.ToArray();
					}
				}

				result.Add(new Itemstate
				{
					Id = legacyId,
					Name = name,
					ComponentBased = component,
					Version = version,
					Components = componentValue
				});
			}

			return result;
		}
	}
}
