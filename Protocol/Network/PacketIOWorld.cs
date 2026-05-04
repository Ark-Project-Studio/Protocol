using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fNbt;
using Protocol.Minecraft;
using Protocol.Minecraft.Inventory.Item;
using Protocol.Minecraft.Inventory.Recipe;
using Protocol.Minecraft.Level;
using Protocol.Minecraft.Level.Block;
using Protocol.Minecraft.Level.Chunk;
using Protocol.Minecraft.Level.Map;
using Protocol.Network.MinecraftPacket;
using Protocol.Utils;
using static Protocol.Network.MinecraftPacket.McbeStructureBlockUpdate;
using Console = System.Console;

namespace Protocol.Network
{
	public partial class Packet
	{
		private IEnumerable<IBlockState> GetBlockStates(NbtTag tag)
		{
			switch (tag.TagType)
			{
				case NbtTagType.List:
				{
					foreach (var state in GetBlockStatesFromList((NbtList)tag))
						yield return state;
				}
					break;

				case NbtTagType.Compound:
				{
					foreach (var state in GetBlockStatesFromCompound((NbtCompound)tag))
						yield return state;
				}
					break;

				default:
				{
					if (TryGetStateFromTag(tag, out var state))
						yield return state;
				}
					break;
			}
		}

		private IEnumerable<IBlockState> GetBlockStatesFromCompound(NbtCompound list)
		{
			if (list.TryGet("states", out NbtTag states))
			{
				foreach (var state in GetBlockStates(states))
				{
					yield return state;
				}
			}
		}


		private IEnumerable<IBlockState> GetBlockStatesFromList(NbtList list)
		{
			foreach (NbtTag tag in list)
			{
				if (TryGetStateFromTag(tag, out var state))
				{
					yield return state;
				}
				else
				{
					foreach (var s in GetBlockStates(tag))
					{
						yield return s;
					}
				}
			}
		}

		private bool TryGetStateFromTag(NbtTag tag, out IBlockState state)
		{
			switch (tag.TagType)
			{
				case NbtTagType.Byte:
					state = new BlockStateByte()
					{
						Name = tag.Name,
						Value = tag.ByteValue
					};
					return true;

				case NbtTagType.Int:
					state = new BlockStateInt()
					{
						Name = tag.Name,
						Value = tag.IntValue
					};
					return true;

				case NbtTagType.String:
					state = new BlockStateString()
					{
						Name = tag.Name,
						Value = tag.StringValue
					};
					return true;
			}

			state = null;

			return false;
		}

		public BlockPalette ReadBlockPalette()
		{
			var result = new BlockPalette();
			var count = ReadUnsignedVarInt();

			for (int runtimeId = 0; runtimeId < count; runtimeId++)
			{
				var record = new BlockStateContainer();
				record.Id = record.RuntimeId = runtimeId;
				record.Name = ReadString();
				record.States = new List<IBlockState>();

				var nbt = ReadNbt(_reader);
				var rootTag = nbt.NbtFile.RootTag;

				foreach (var state in GetBlockStates(rootTag))
				{
					record.States.Add(state);
				}
			}

			return result;
		}

		public void Write(BlockPalette palette)
		{
			if (palette == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint)palette.Count);
			foreach (BlockStateContainer record in palette.Values)
			{
				Write(record.Name);
				Write(record.StatesCacheNbt);
			}
		}

		public void Write(DimensionData data)
		{
			Write(data.Identifier);
			WriteSignedVarInt(data.MaxHeight);
			WriteSignedVarInt(data.MinHeight);
			WriteSignedVarInt(data.Generator);
			WriteSignedVarInt(data.Dimension);
		}

		public DimensionData ReadDimensionData()
		{
			DimensionData data = new DimensionData();
			data.Identifier = ReadString();
			data.MaxHeight = ReadSignedVarInt();
			data.MinHeight = ReadSignedVarInt();
			data.Generator = ReadSignedVarInt();
			data.Dimension = ReadSignedVarInt();
			return data;
		}

		public void Write(DimensionDefinitions definitions)
		{
			WriteUnsignedVarInt((uint)definitions.Count);

			foreach (var def in definitions)
			{
				Write(def.Value);
			}
		}

		public DimensionDefinitions ReadDimensionDefinitions()
		{
			DimensionDefinitions definitions = new DimensionDefinitions();

			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				var data = ReadDimensionData();

				definitions.TryAdd(data.Identifier, data);
			}

			return definitions;
		}

		public void Write(UpdateSubChunkBlocksPacketEntry entry)
		{
			Write(entry.Coordinates);
			WriteUnsignedVarInt(entry.BlockRuntimeId);
			WriteUnsignedVarInt(entry.Flags);
			WriteUnsignedVarLong((ulong)entry.SyncedUpdatedEntityUniqueId);
			WriteUnsignedVarInt(entry.SyncedUpdateType);
		}

		public UpdateSubChunkBlocksPacketEntry ReadUpdateSubChunkBlocksPacketEntry()
		{
			var entry = new UpdateSubChunkBlocksPacketEntry();
			entry.Coordinates = ReadBlockCoordinates();
			entry.BlockRuntimeId = ReadUnsignedVarInt();
			entry.Flags = ReadUnsignedVarInt();
			entry.SyncedUpdatedEntityUniqueId = (long)ReadUnsignedVarLong();
			entry.SyncedUpdateType = ReadUnsignedVarInt();

			return entry;
		}

		public void Write(UpdateSubChunkBlocksPacketEntry[] entries)
		{
			WriteUnsignedVarInt((uint)entries.Length);
			foreach (var entry in entries)
				Write(entry);
		}

		public UpdateSubChunkBlocksPacketEntry[] ReadUpdateSubChunkBlocksPacketEntrys()
		{
			var count = ReadUnsignedVarInt();
			UpdateSubChunkBlocksPacketEntry[] entries = new UpdateSubChunkBlocksPacketEntry[(int)count];

			for (int i = 0; i < entries.Length; i++)
			{
				entries[i] = ReadUpdateSubChunkBlocksPacketEntry();
			}

			return entries;
		}

		public void Write(HeightMapData data)
		{
			if (data == null)
			{
				Write((byte)SubChunkPacketHeightMapType.NoData);

				return;
			}

			if (data.IsAllTooHigh)
			{
				Write((byte)SubChunkPacketHeightMapType.AllTooHigh);

				return;
			}

			if (data.IsAllTooLow)
			{
				Write((byte)SubChunkPacketHeightMapType.AllTooLow);

				return;
			}

			Write((byte)SubChunkPacketHeightMapType.HasData);

			for (int i = 0; i < data.Heights.Length; i++)
			{
				Write((byte)data.Heights[i]);
			}
		}

		public HeightMapData ReadHeightMapData()
		{
			SubChunkPacketHeightMapType type = (SubChunkPacketHeightMapType)ReadByte();

			if (type != SubChunkPacketHeightMapType.HasData)
				return null;

			short[] heights = new short[256];

			for (int i = 0; i < heights.Length; i++)
			{
				heights[i] = (short)ReadByte();
			}

			return new HeightMapData(heights);
		}

		const int MapUpdateFlagTexture = 0x02;
		const int MapUpdateFlagDecoration = 0x04;
		const int MapUpdateFlagInitialisation = 0x08;

		// Write methods
		public void Write(MapTrackedObject value)
		{
			Write(value.Type);

			switch (value.Type)
			{
				case (int)MapObjectType.Entity:
					WriteSignedVarLong(value.EntityUniqueID);
					break;
				case (int)MapObjectType.Block:
					Write(value.BlockPosition);
					break;
				default:
					throw new ArgumentException($"Unknown map tracked object type: {value.Type}");
			}
		}

		public void Write(MapDecoration value)
		{
			Write(value.Type);
			Write(value.Rotation);
			Write(value.X);
			Write(value.Y);
			Write(value.Label);
			WriteVarRGBA(value.Colour);
		}

		public void Write(PixelRequest value)
		{
			WriteRGBA(value.Colour);
			Write(value.Index);
		}// Read methods
		public MapTrackedObject ReadMapTrackedObject()
		{
			var value = new MapTrackedObject
			{
				Type = ReadInt()
			};

			switch (value.Type)
			{
				case (int)MapObjectType.Entity:
					value.EntityUniqueID = ReadSignedVarLong();
					break;
				case (int)MapObjectType.Block:
					value.BlockPosition = ReadBlockCoordinates();
					break;
				default:
					throw new FormatException($"Unknown map tracked object type: {value.Type}");
			}

			return value;
		}

		public MapDecoration ReadMapDecoration()
		{
			return new MapDecoration
			{
				Type = ReadByte(),
				Rotation = ReadByte(),
				X = ReadByte(),
				Y = ReadByte(),
				Label = ReadString(),
				Colour = ReadVarRGBA()
			};
		}

		public PixelRequest ReadPixelRequest()
		{
			return new PixelRequest
			{
				Colour = ReadRGBA(),
				Index = ReadUshort()
			};
		}

		public pixelList ReadPixelList()
		{
			pixelList mapData = new pixelList();

			var listSize = ReadInt();
			for (int i = 0; i < listSize; i++)
			{
				mapData.mapData.Add(new pixelsData { pixel = ReadUnsignedVarInt(), index = ReadShort() });
			}

			return mapData;
		}

		public void Write(CraftingDataEntry[] entries)
		{
			WriteSlice(entries ?? [], Write);
		}

		public CraftingDataEntry[] ReadCraftingDataEntries()
		{
			return ReadSlice(ReadCraftingDataEntry);
		}

		public void Write(CraftingDataEntry entry)
		{
			WriteSignedVarInt((int)entry.Type);
			switch (entry.Type)
			{
				case CraftingDataEntryType.Shapeless:
					WriteShapelessRecipe((ShapelessRecipe)entry.Recipe);
					break;
				case CraftingDataEntryType.Shaped:
					WriteShapedRecipe((ShapedRecipe)entry.Recipe);
					break;
				case CraftingDataEntryType.Furnace:
					break;
				case CraftingDataEntryType.FurnaceAux:
					break;
				case CraftingDataEntryType.Multi:
					WriteMultiRecipe((MultiRecipe)entry.Recipe);
					break;
				case CraftingDataEntryType.UserDataShapeless:
					WriteUserDataShapelessRecipe((UserDataShapelessRecipe)entry.Recipe);
					break;
				case CraftingDataEntryType.ShapelessChemistry:
					WriteShapelessChemistryRecipe((ShapelessChemistryRecipe)entry.Recipe);
					break;
				case CraftingDataEntryType.ShapedChemistry:
					WriteShapedRecipe(((ShapedChemistryRecipe)entry.Recipe).ChemistryRecipe);
					break;
				case CraftingDataEntryType.SmithingTransform:
					WriteSmithingTransformRecipe((SmithingTransformRecipe)entry.Recipe);
					break;
				case CraftingDataEntryType.SmithingTrim:
					WriteSmithingTrimRecipe((SmithingTrimRecipe)entry.Recipe);
					break;
			}
		}

		public CraftingDataEntry ReadCraftingDataEntry()
		{
			var type = (CraftingDataEntryType)ReadSignedVarInt();
			Recipe recipe = type switch
			{
				CraftingDataEntryType.Shapeless => ReadShapelessRecipe(),
				CraftingDataEntryType.Shaped => ReadShapedRecipe(),
				CraftingDataEntryType.Furnace => new FurnaceRecipe(),
				CraftingDataEntryType.FurnaceAux => new FurnaceAuxRecipe(),
				CraftingDataEntryType.Multi => ReadMultiRecipe(),
				CraftingDataEntryType.UserDataShapeless => ReadUserDataShapelessRecipe(),
				CraftingDataEntryType.ShapelessChemistry => ReadShapelessChemistryRecipe(),
				CraftingDataEntryType.ShapedChemistry => new ShapedChemistryRecipe { ChemistryRecipe = ReadShapedRecipe() },
				CraftingDataEntryType.SmithingTransform => ReadSmithingTransformRecipe(),
				CraftingDataEntryType.SmithingTrim => ReadSmithingTrimRecipe(),
				_ => throw new InvalidDataException($"Unknown crafting data entry type: {type}")
			};

			return new CraftingDataEntry { Type = type, Recipe = recipe };
		}

		private void WriteUnlockingRequirement(RecipeUnlockingRequirement requirement)
		{
			requirement ??= new RecipeUnlockingRequirement();
			Write((byte)requirement.UnlockingContext);
			if (requirement.UnlockingContext == RecipeUnlockingContext.None)
			{
				WriteUnsignedVarInt((uint)requirement.UnlockingIngredients.Count);
				foreach (var ingredient in requirement.UnlockingIngredients)
				{
					Write(ingredient);
				}
			}
		}

		private RecipeUnlockingRequirement ReadUnlockingRequirement()
		{
			var requirement = new RecipeUnlockingRequirement { UnlockingContext = (RecipeUnlockingContext)ReadByte() };
			if (requirement.UnlockingContext == RecipeUnlockingContext.None)
			{
				var count = ReadUnsignedVarInt();
				for (int i = 0; i < count; i++)
				{
					requirement.UnlockingIngredients.Add(ReadRecipeIngredient());
				}
			}

			return requirement;
		}

		private void WriteRecipeIngredients(List<RecipeIngredient> ingredients)
		{
			WriteUnsignedVarInt((uint)(ingredients?.Count ?? 0));
			if (ingredients == null) return;
			foreach (var ingredient in ingredients) Write(ingredient);
		}

		private List<RecipeIngredient> ReadRecipeIngredients()
		{
			var ingredients = new List<RecipeIngredient>();
			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++) ingredients.Add(ReadRecipeIngredient());
			return ingredients;
		}

		private void WriteShapelessRecipe(ShapelessRecipe recipe)
		{
			Write(recipe.RecipeUniqueId);
			WriteRecipeIngredients(recipe.IngredientList);
			Write(recipe.ProductionList);
			Write(recipe.RecipeId);
			Write(recipe.RecipeTag);
			WriteSignedVarInt(recipe.Priority);
			WriteUnlockingRequirement(recipe.UnlockingRequirement);
			WriteUnsignedVarInt(recipe.NetId);
		}

		private ShapelessRecipe ReadShapelessRecipe()
		{
			return new ShapelessRecipe
			{
				RecipeUniqueId = ReadString(),
				IngredientList = ReadRecipeIngredients(),
				ProductionList = ReadNetworkItemInstanceDescriptors(),
				RecipeId = ReadUUID(),
				RecipeTag = ReadString(),
				Priority = ReadSignedVarInt(),
				UnlockingRequirement = ReadUnlockingRequirement(),
				NetId = ReadUnsignedVarInt()
			};
		}

		private void WriteShapedRecipe(ShapedRecipe recipe)
		{
			Write(recipe.RecipeUniqueId);
			WriteSignedVarInt(recipe.GridWidth);
			WriteSignedVarInt(recipe.GridHeight);
			foreach (var ingredient in recipe.IngredientList) Write(ingredient);
			Write(recipe.ProductionList);
			Write(recipe.RecipeId);
			Write(recipe.RecipeTag);
			WriteSignedVarInt(recipe.Priority);
			Write(recipe.AssumeSymmetry);
			WriteUnlockingRequirement(recipe.UnlockingRequirement);
			WriteUnsignedVarInt(recipe.NetId);
		}

		private ShapedRecipe ReadShapedRecipe()
		{
			var recipe = new ShapedRecipe
			{
				RecipeUniqueId = ReadString(),
				GridWidth = ReadSignedVarInt(),
				GridHeight = ReadSignedVarInt()
			};

			var ingredientCount = recipe.GridWidth * recipe.GridHeight;
			for (int i = 0; i < ingredientCount; i++) recipe.IngredientList.Add(ReadRecipeIngredient());
			recipe.ProductionList = ReadNetworkItemInstanceDescriptors();
			recipe.RecipeId = ReadUUID();
			recipe.RecipeTag = ReadString();
			recipe.Priority = ReadSignedVarInt();
			recipe.AssumeSymmetry = ReadBool();
			recipe.UnlockingRequirement = ReadUnlockingRequirement();
			recipe.NetId = ReadUnsignedVarInt();
			return recipe;
		}

		private void WriteMultiRecipe(MultiRecipe recipe)
		{
			Write(recipe.MultiRecipeId);
			WriteUnsignedVarInt(recipe.NetId);
		}

		private MultiRecipe ReadMultiRecipe()
		{
			return new MultiRecipe { MultiRecipeId = ReadUUID(), NetId = ReadUnsignedVarInt() };
		}

		private void WriteUserDataShapelessRecipe(UserDataShapelessRecipe recipe)
		{
			Write(recipe.RecipeUniqueId);
			WriteRecipeIngredients(recipe.IngredientList);
			Write(recipe.ProductionList);
			Write(recipe.RecipeId);
			Write(recipe.RecipeTag);
			WriteSignedVarInt(recipe.Priority);
			WriteUnlockingRequirement(recipe.UnlockingRequirement);
			WriteUnsignedVarInt(recipe.NetId);
		}

		private UserDataShapelessRecipe ReadUserDataShapelessRecipe()
		{
			return new UserDataShapelessRecipe
			{
				RecipeUniqueId = ReadString(),
				IngredientList = ReadRecipeIngredients(),
				ProductionList = ReadNetworkItemInstanceDescriptors(),
				RecipeId = ReadUUID(),
				RecipeTag = ReadString(),
				Priority = ReadSignedVarInt(),
				UnlockingRequirement = ReadUnlockingRequirement(),
				NetId = ReadUnsignedVarInt()
			};
		}

		private void WriteShapelessChemistryRecipe(ShapelessChemistryRecipe recipe)
		{
			Write(recipe.RecipeUniqueId);
			WriteRecipeIngredients(recipe.IngredientList);
			Write(recipe.ProductionList);
			Write(recipe.RecipeId);
			Write(recipe.RecipeTag);
			WriteSignedVarInt(recipe.Priority);
			WriteUnsignedVarInt(recipe.NetId);
		}

		private ShapelessChemistryRecipe ReadShapelessChemistryRecipe()
		{
			return new ShapelessChemistryRecipe
			{
				RecipeUniqueId = ReadString(),
				IngredientList = ReadRecipeIngredients(),
				ProductionList = ReadNetworkItemInstanceDescriptors(),
				RecipeId = ReadUUID(),
				RecipeTag = ReadString(),
				Priority = ReadSignedVarInt(),
				NetId = ReadUnsignedVarInt()
			};
		}

		private void WriteSmithingTransformRecipe(SmithingTransformRecipe recipe)
		{
			Write(recipe.RecipeUniqueId);
			Write(recipe.Template);
			Write(recipe.Base);
			Write(recipe.Addition);
			Write(recipe.Result);
			Write(recipe.RecipeTag);
			WriteUnsignedVarInt(recipe.NetId);
		}

		private SmithingTransformRecipe ReadSmithingTransformRecipe()
		{
			return new SmithingTransformRecipe
			{
				RecipeUniqueId = ReadString(),
				Template = ReadRecipeIngredient(),
				Base = ReadRecipeIngredient(),
				Addition = ReadRecipeIngredient(),
				Result = ReadNetworkItemInstanceDescriptor(),
				RecipeTag = ReadString(),
				NetId = ReadUnsignedVarInt()
			};
		}

		private void WriteSmithingTrimRecipe(SmithingTrimRecipe recipe)
		{
			Write(recipe.RecipeUniqueId);
			Write(recipe.Template);
			Write(recipe.Base);
			Write(recipe.Addition);
			Write(recipe.RecipeTag);
			WriteUnsignedVarInt(recipe.NetId);
		}

		private SmithingTrimRecipe ReadSmithingTrimRecipe()
		{
			return new SmithingTrimRecipe
			{
				RecipeUniqueId = ReadString(),
				Template = ReadRecipeIngredient(),
				Base = ReadRecipeIngredient(),
				Addition = ReadRecipeIngredient(),
				RecipeTag = ReadString(),
				NetId = ReadUnsignedVarInt()
			};
		}

		public void Write(List<PotionMixDataEntry> recipes)
		{
			WriteUnsignedVarInt((uint)(recipes?.Count ?? 0));
			if (recipes == null) return;
			foreach (var recipe in recipes)
			{
				WriteSignedVarInt(recipe.FromPotionId);
				WriteSignedVarInt(recipe.FromPotionAux);
				WriteSignedVarInt(recipe.ReagentItemId);
				WriteSignedVarInt(recipe.ReagentItemAux);
				WriteSignedVarInt(recipe.ToPotionId);
				WriteSignedVarInt(recipe.ToPotionAux);
			}
		}

		public List<PotionMixDataEntry> ReadPotionMixDataEntries()
		{
			var entries = new List<PotionMixDataEntry>();
			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				entries.Add(new PotionMixDataEntry
				{
					FromPotionId = ReadSignedVarInt(),
					FromPotionAux = ReadSignedVarInt(),
					ReagentItemId = ReadSignedVarInt(),
					ReagentItemAux = ReadSignedVarInt(),
					ToPotionId = ReadSignedVarInt(),
					ToPotionAux = ReadSignedVarInt()
				});
			}

			return entries;
		}

		public void Write(List<ContainerMixDataEntry> recipes)
		{
			WriteUnsignedVarInt((uint)(recipes?.Count ?? 0));
			if (recipes == null) return;
			foreach (var recipe in recipes)
			{
				WriteSignedVarInt(recipe.InputItemId);
				WriteSignedVarInt(recipe.ReagentItemId);
				WriteSignedVarInt(recipe.ToItemId);
			}
		}

		public List<ContainerMixDataEntry> ReadContainerMixDataEntries()
		{
			var entries = new List<ContainerMixDataEntry>();
			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				entries.Add(new ContainerMixDataEntry
				{
					InputItemId = ReadSignedVarInt(),
					ReagentItemId = ReadSignedVarInt(),
					ToItemId = ReadSignedVarInt()
				});
			}

			return entries;
		}

		public void Write(List<MaterialReducerDataEntry> reducerRecipes)
		{
			WriteUnsignedVarInt((uint)(reducerRecipes?.Count ?? 0));
			if (reducerRecipes == null) return;
			foreach (var recipe in reducerRecipes)
			{
				WriteSignedVarInt(recipe.Input);
				WriteUnsignedVarInt((uint)recipe.Items.Count);
				foreach (var item in recipe.Items)
				{
					WriteSignedVarInt(item.ItemId);
					WriteSignedVarInt(item.ItemCount);
				}
			}
		}

		public List<MaterialReducerDataEntry> ReadMaterialReducerDataEntries()
		{
			var entries = new List<MaterialReducerDataEntry>();
			var count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				var entry = new MaterialReducerDataEntry { Input = ReadSignedVarInt() };
				var itemCount = ReadUnsignedVarInt();
				for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
				{
					entry.Items.Add(new MaterialReducerItemInfo { ItemId = ReadSignedVarInt(), ItemCount = ReadSignedVarInt() });
				}

				entries.Add(entry);
			}

			return entries;
		}
		public void Write(StructureEditorData value)
		{
			Write(value.StructureName);
			Write(value.FilteredStructureName);
			Write(value.DataField);
			Write(value.IncludePlayer);
			Write(value.ShowBoundingBox);
			WriteSignedVarInt(value.StructureBlockType);
			Write(value.StructureSettings);
			WriteSignedVarInt(value.RedstoneSaveMode);
		}

		public StructureEditorData ReadStructureEditorData()
		{
			return new StructureEditorData
			{
				StructureName = ReadString(),
				FilteredStructureName = ReadString(),
				DataField = ReadString(),
				IncludePlayer = ReadBool(),
				ShowBoundingBox = ReadBool(),
				StructureBlockType = ReadSignedVarInt(),
				StructureSettings = ReadStructureSettings(),
				RedstoneSaveMode = ReadSignedVarInt()
			};
		}

		public void Write(StructureSettings value)
		{
			Write(value.PaletteName);
			Write(value.IgnoreEntities);
			Write(value.IgnoreBlocks);
			Write(value.AllowNonTickingPlayerAndTickingAreaChunks);
			Write(value.Size);
			Write(value.Offset);
			WriteSignedVarLong(value.LastEditPlayer);
			Write(value.Rotation);
			Write(value.Mirror);
			Write(value.AnimationMode);
			Write(value.AnimationSeconds);
			Write(value.IntegrityValue);
			Write(value.IntegritySeed);
			Write(value.RotationPivot);
		}

		public StructureSettings ReadStructureSettings()
		{
			return new StructureSettings
			{
				PaletteName = ReadString(),
				IgnoreEntities = ReadBool(),
				IgnoreBlocks = ReadBool(),
				AllowNonTickingPlayerAndTickingAreaChunks = ReadBool(),
				Size = ReadBlockCoordinates(),
				Offset = ReadBlockCoordinates(),
				LastEditPlayer = ReadSignedVarLong(),
				Rotation = ReadByte(),
				Mirror = ReadByte(),
				AnimationMode = ReadByte(),
				AnimationSeconds = ReadFloat(),
				IntegrityValue = ReadFloat(),
				IntegritySeed = ReadUint(),
				RotationPivot = ReadVector3()
			};
		}
	}
}
