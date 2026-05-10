using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using fNbt;
using Protocol.Minecraft;
using Protocol.Minecraft.Actor.Player;
using Protocol.Minecraft.Inventory.Item;
using Protocol.Minecraft.Metadata;
using Protocol.Minecraft.NBT;
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

		public void Write(List<CreativeContentWriteEntry> entries)
		{
			WriteUnsignedVarInt((uint)entries.Count);

			foreach (var entry in entries)
			{
				WriteUnsignedVarInt(entry.NetId);
				Write(entry.Item);
				WriteUnsignedVarInt(entry.GroupIndex);
			}
		}

		public List<CreativeContentWriteEntry> ReadCreativeContentWriteEntries()
		{
			var entries = new List<CreativeContentWriteEntry>();

			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				uint networkId = ReadUnsignedVarInt();
				NetworkItemInstanceDescriptor item = ReadNetworkItemInstanceDescriptor();
				uint groupIndex = ReadUnsignedVarInt();
				entries.Add(new CreativeContentWriteEntry(networkId, item, groupIndex));
			}

			return entries;
		}

		public void Write(List<CreativeContentGroup> groups)
		{
			WriteUnsignedVarInt((uint)groups.Count);

			foreach (var group in groups)
			{
				Write(group.Category);
				Write(group.Name);
				Write(group.Icon);
			}
		}

		public List<CreativeContentGroup> ReadCreativeContentGroups()
		{
			var group = new List<CreativeContentGroup>();

			var groupCount = ReadUnsignedVarInt();
			for (int i = 0; i < groupCount; i++)
			{
				int category = ReadInt();
				string name = ReadString();
				NetworkItemInstanceDescriptor item = ReadNetworkItemInstanceDescriptor();
				group.Add(new CreativeContentGroup(category, name, item));
			}

			return group;
		}

		public void Write(NetworkItemStackDescriptor[] itemStacks)
		{
			WriteSlice(itemStacks ?? [], Write);
		}

		public NetworkItemStackDescriptor[] ReadNetworkItemStackDescriptors()
		{
			return ReadSlice(ReadNetworkItemStackDescriptor);
		}
		public void Write(NetworkItemInstanceDescriptor[] itemStacks)
		{
			WriteSlice(itemStacks ?? [], Write);
		}

		public NetworkItemInstanceDescriptor[] ReadNetworkItemInstanceDescriptors()
		{
			return ReadSlice(ReadNetworkItemInstanceDescriptor);
		}

		private const int ShieldId = 355;

		public void Write(NetworkItemStackDescriptor stack)
		{
			if (stack == null || stack.Id == 0)
			{
				WriteSignedVarInt(0);
				return;
			}

			WriteSignedVarInt(stack.Id);

			Write(stack.StackSize);
			WriteUnsignedVarInt(stack.Aux);
			if (stack.NetId.HasValue)
			{
				Write(true);
				WriteUnsignedVarInt((uint)stack.NetId.Value);
			}
			else
			{
				Write(false);
			}

			WriteUnsignedVarInt((uint)stack.BlockRuntimeId);

			Write(stack.UserData ?? string.Empty);
		}

		public void Write(NetworkItemInstanceDescriptor stack)
		{
			if (stack == null || stack.Id == 0)
			{
				WriteSignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint)stack.Id);
			Write(stack.StackSize);
			WriteUnsignedVarInt(stack.Aux);
			WriteUnsignedVarInt((uint)stack.BlockRuntimeId);
			Write(stack.UserData ?? string.Empty);
		}

		public NetworkItemStackDescriptor ReadNetworkItemStackDescriptor()
		{
		int id = ReadSignedVarInt();
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
		stack.BlockRuntimeId =ReadUnsignedVarInt();

		stack.UserData = ReadString();
			return stack;
		}

		public NetworkItemInstanceDescriptor ReadNetworkItemInstanceDescriptor()
		{
		
			int id = ReadSignedVarInt();
			var stack = new NetworkItemInstanceDescriptor { Id = id };
			if (id == 0)
			{
				return stack;
			}

			stack.StackSize = ReadUshort();
			stack.Aux = ReadUnsignedVarInt();
			stack.BlockRuntimeId = ReadSignedVarInt();

			uint len = ReadUnsignedVarInt();
			ReadBytes((int)len);
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

		public void Write(RecipeIngredient ingredient)
		{
			ingredient ??= new RecipeIngredient();
			Write((byte)ingredient.DescriptorType);
			switch (ingredient.DescriptorType)
			{
				case RecipeIngredientDescriptorType.InternalItem:
					Write(ingredient.Id);
					if (ingredient.Id != 0)
					{
						Write(ingredient.Aux);
					}
					break;
				case RecipeIngredientDescriptorType.Molang:
					Write(ingredient.Name);
					Write(ingredient.MolangVersion);
					break;
				case RecipeIngredientDescriptorType.ItemTag:
				case RecipeIngredientDescriptorType.ComplexAlias:
					Write(ingredient.Name);
					break;
				case RecipeIngredientDescriptorType.Deferred:
					Write(ingredient.Name);
					Write((ushort)ingredient.Aux);
					break;
			}

			WriteSignedVarInt(ingredient.StackSize);
		}

		public RecipeIngredient ReadRecipeIngredient()
		{
			var ingredient = new RecipeIngredient { DescriptorType = (RecipeIngredientDescriptorType)ReadByte() };
			switch (ingredient.DescriptorType)
			{
				case RecipeIngredientDescriptorType.InternalItem:
					ingredient.Id = ReadShort();
					if (ingredient.Id != 0)
					{
						ingredient.Aux = ReadShort();
					}
					break;
				case RecipeIngredientDescriptorType.Molang:
					ingredient.Name = ReadString();
					ingredient.MolangVersion = ReadByte();
					break;
				case RecipeIngredientDescriptorType.ItemTag:
				case RecipeIngredientDescriptorType.ComplexAlias:
					ingredient.Name = ReadString();
					break;
				case RecipeIngredientDescriptorType.Deferred:
					ingredient.Name = ReadString();
					ingredient.Aux = (short)ReadUshort();
					break;
			}

			ingredient.StackSize = ReadSignedVarInt();
			return ingredient;
		}

		public void Write(Itemstate[] itemstates)
		{
			WriteSlice(itemstates ?? [], itemstate =>
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
			});
		}

		public Itemstate[] ReadItemstates()
		{
			return ReadSlice(() =>
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

				return new Itemstate
				{
					Id = legacyId,
					Name = name,
					ComponentBased = component,
					Version = version,
					Components = componentValue
				};
			});
		}
		public void Write(UseItemTransactionData data)
		{
			WriteSignedVarInt(data.LegacyRequestID);
			WriteSignedVarInt(data.LegacySetItemSlots?.Length ?? 0);
			if (data.LegacySetItemSlots != null)
				foreach (var slot in data.LegacySetItemSlots)
					Write(slot);
			WriteSignedVarInt(data.Actions?.Length ?? 0);
			if (data.Actions != null)
				foreach (var action in data.Actions)
					Write(action);
			WriteUnsignedVarInt(data.ActionType);
			WriteUnsignedVarInt(data.TriggerType);
			Write(data.BlockPosition);
			WriteSignedVarInt(data.BlockFace);
			WriteSignedVarInt(data.HotBarSlot);
			Write(data.HeldItem);
			Write(data.Position);
			Write(data.ClickedPosition);
			WriteUnsignedVarInt(data.BlockRuntimeID);
			WriteUnsignedVarInt(data.ClientPrediction);
			Write(data.ClientCooldownState);
		}

		public UseItemTransactionData ReadUseItemTransactionData()
		{
			var legacyRequestID = ReadSignedVarInt();
			var legacySlotsCount = ReadSignedVarInt();
			var legacySlots = new LegacySetItemSlot[legacySlotsCount];
			for (var i = 0; i < legacySlotsCount; i++)
				legacySlots[i] = ReadLegacySetItemSlot();
			var actionsCount = ReadSignedVarInt();
			var actions = new InventoryAction[actionsCount];
			for (var i = 0; i < actionsCount; i++)
				actions[i] = ReadInventoryAction();
			var actionType = ReadUnsignedVarInt();
			var triggerType = ReadUnsignedVarInt();
			var blockPosition = ReadBlockCoordinates();
			var blockFace = ReadSignedVarInt();
			var hotBarSlot = ReadSignedVarInt();
			var heldItem = ReadNetworkItemStackDescriptor();
			var position = ReadVector3();
			var clickedPosition = ReadVector3();
			var blockRuntimeID = ReadUnsignedVarInt();
			var clientPrediction = ReadUnsignedVarInt();
			var clientcooldownstate = ReadByte();
			return new UseItemTransactionData(legacyRequestID, legacySlots, actions, actionType, triggerType, blockPosition, blockFace, hotBarSlot, heldItem, position, clickedPosition, blockRuntimeID, clientPrediction, clientcooldownstate);
		}

		public void Write(LegacySetItemSlot slot)
		{
			Write(slot.ContainerID);
			WriteUnsignedVarInt((uint)(slot.Slots?.Length ?? 0));
			if (slot.Slots != null)
				foreach (var s in slot.Slots)
					Write(s);
		}

		public LegacySetItemSlot ReadLegacySetItemSlot()
		{
			var containerID = ReadByte();
			var slotsCount = ReadUnsignedVarInt();
			var slots = new byte[slotsCount];
			for (var i = 0; i < slotsCount; i++)
				slots[i] = ReadByte();
			return new LegacySetItemSlot(containerID, slots);
		}

		public void Write(InventoryAction action)
		{
			WriteUnsignedVarInt(action.SourceType);
			WriteSignedVarInt(action.WindowID);
			WriteUnsignedVarInt(action.SourceFlags);
			WriteUnsignedVarInt(action.InventorySlot);
			Write(action.OldItem);
			Write(action.NewItem);
		}

		public InventoryAction ReadInventoryAction()
		{
			var sourceType = ReadUnsignedVarInt();
			var windowID = ReadSignedVarInt();
			var sourceFlags = ReadUnsignedVarInt();
			var inventorySlot = ReadUnsignedVarInt();
			var oldItem = ReadNetworkItemStackDescriptor();
			var newItem = ReadNetworkItemStackDescriptor();
			return new InventoryAction(sourceType, windowID, sourceFlags, inventorySlot, oldItem, newItem);
		}

		public void Write(PlayerBlockAction action)
		{
			WriteSignedVarInt(action.Action);
			Write(action.BlockPos);
			WriteSignedVarInt(action.Face);
		}

		public PlayerBlockAction ReadPlayerBlockAction()
		{
			var action = ReadSignedVarInt();
			var blockPos = ReadBlockCoordinates();
			var face = ReadSignedVarInt();
			return new PlayerBlockAction(action, blockPos, face);
		}
	}
}
