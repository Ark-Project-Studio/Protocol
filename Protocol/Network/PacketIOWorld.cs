using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fNbt;
using Protocol.Minecraft;
using Protocol.Minecraft.World.Map;
using Protocol.Network.MinecraftPacket;
using Protocol.Utils;
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
			WriteVarInt(data.MaxHeight);
			WriteVarInt(data.MinHeight);
			WriteVarInt(data.Generator);
			WriteVarInt(data.Dimension);
		}

		public DimensionData ReadDimensionData()
		{
			DimensionData data = new DimensionData();
			data.Identifier = ReadString();
			data.MaxHeight = ReadVarInt();
			data.MinHeight = ReadVarInt();
			data.Generator = ReadVarInt();
			data.Dimension = ReadVarInt();
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

		const byte Shapeless = 0;
		const byte Shaped = 1;
		const byte Furnace = 2;
		const byte FurnaceData = 3;
		const byte Multi = 4;
		const byte ShulkerBox = 5;
		const byte ShapelessChemistry = 6;
		const byte ShapedChemistry = 7;
		const byte SmithingTransform = 8;
		const byte SmithingTrim = 9;

		public void Write(Recipes recipes)
		{
			WriteUnsignedVarInt((uint)recipes.Count);
			int UniqueId = 1;
			foreach (Recipe recipe in recipes)
			{
				switch (recipe)
				{
					case ShapelessRecipe shapelessRecipe:
					{
						WriteSignedVarInt(Shapeless);

						var rec = shapelessRecipe;
						var uuid = new UUID(Guid.NewGuid().ToString());
						Write($"{uuid}");
						WriteVarInt(rec.Input.Count);
						foreach (Item stack in rec.Input)
						{
							WriteRecipeIngredient(stack);
						}

						WriteVarInt(rec.Result.Count);
						foreach (Item item in rec.Result)
						{
							item.RuntimeId = 0;
							Write(item,false);
						}

						Write(rec.Id);
						Write(rec.Block);
						WriteSignedVarInt(0);
						Write((byte)1);
						WriteVarInt(UniqueId);

						break;
					}
					case ShapedRecipe shapedRecipe:
					{
						WriteSignedVarInt(Shaped);

						var rec = shapedRecipe;
						var uuid = new UUID(Guid.NewGuid().ToString());
						Write($"{uuid}");
						WriteSignedVarInt(rec.Width);
						WriteSignedVarInt(rec.Height);
						for (int w = 0; w < rec.Width; w++)
						{
							for (int h = 0; h < rec.Height; h++)
							{
								WriteRecipeIngredient(rec.Input[(h * rec.Width) + w]);
							}
						}

						WriteVarInt(rec.Result.Count);
						foreach (Item item in rec.Result)
						{
							item.RuntimeId = 0;
							Write(item,false);
						}

						Write(rec.Id);
						Write(rec.Block);
						WriteUnsignedVarInt(0);
						Write(true);
						Write((byte)1);
						WriteVarInt(UniqueId);

						break;
					}
					case SmeltingRecipe smeltingRecipe:
					{
						var rec = smeltingRecipe;
						if (rec.Input.Metadata == 0)
						{
							WriteSignedVarInt(Furnace);
							WriteSignedVarInt(rec.Input.Id);
							Write(rec.Result,false);
							Write(rec.Block);
						}
						else
						{
							WriteSignedVarInt(FurnaceData);
							WriteSignedVarInt(rec.Input.Id);
							WriteSignedVarInt(rec.Input.Metadata);
							Write(rec.Result,false);
							Write(rec.Block);
						}

						break;
					}
					case MultiRecipe multiRecipe:
					{
						WriteSignedVarInt(Multi);
						Write(recipe.Id);
						WriteVarInt(UniqueId);
						break;
					}
				}

				UniqueId++;
			}
		}

		public Recipes ReadRecipes()
		{
			var recipes = new Recipes();

			int count = (int)ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				int recipeType = ReadSignedVarInt();


				if (recipeType < 0)
				{
					Console.WriteLine("Read void recipe");
					break;
				}

				switch (recipeType)
				{
					case Shapeless:
					case ShulkerBox:
					{
						var recipe = new ShapelessRecipe();
						ReadString();
						int ingrediensCount = ReadVarInt();
						for (int j = 0; j < ingrediensCount; j++)
						{
							recipe.Input.Add(ReadRecipeData());
						}

						int resultCount = ReadVarInt();
						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem(false));
						}

						recipe.Id = ReadUUID();
						recipe.Block = ReadString();
						ReadSignedVarInt();
						var unlockReq = ReadByte();
						if (unlockReq == 0)
						{
							var ingredientCount = ReadVarInt();
							for (int a = 0; a < ingredientCount; a++)
							{
								ReadRecipeData();
							}
						}

						recipe.UniqueId = ReadVarInt();


						break;
					}
					case Shaped:
					{
						var uniqueid = ReadString();

						int width = ReadSignedVarInt();
						int height = ReadSignedVarInt();

						var recipe = new ShapedRecipe(width, height);
						if (width > 3 || height > 3)
							throw new Exception("Wrong number of ingredience. Width=" + width + ", height=" + height);
						for (int w = 0; w < width; w++)
						{
							for (int h = 0; h < height; h++)
							{
								recipe.Input[(h * width) + w] = ReadRecipeData();
							}
						}

						int resultCount = ReadVarInt();

						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem(false));
						}

						recipe.Id = ReadUUID();

						recipe.Block = ReadString();
						ReadUnsignedVarInt();
						var symetric = ReadBool();
						var unlockReq = ReadByte();
						if (unlockReq == 0)
						{
							var ingredientCount = ReadVarInt();
							for (int a = 0; a < ingredientCount; a++)
							{
								ReadRecipeData();
							}
						}

						recipe.UniqueId = ReadVarInt();
						recipes.Add(recipe);

						break;
					}
					case Furnace:
					{
						var recipe = new SmeltingRecipe();
						short id = (short)ReadSignedVarInt();

						Item result = ReadItem(false);
						recipe.Block = ReadString();
						recipe.Input = ItemFactory.GetItem(id, 0);
						recipe.Result = result;


						break;
					}
					case FurnaceData:
					{
						var recipe = new SmeltingRecipe();
						short id = (short)ReadSignedVarInt();
						short meta = (short)ReadSignedVarInt();
						Item result = ReadItem(false);
						recipe.Block = ReadString();
						recipe.Input = ItemFactory.GetItem(id, meta);
						recipe.Result = result;


						break;
					}
					case Multi:
					{
						var recipe = new MultiRecipe();
						recipe.Id = ReadUUID();
						recipe.UniqueId = ReadVarInt();

						break;
					}
					case ShapelessChemistry:
					{
						var recipe = new ShapelessRecipe();
						ReadString();
						int ingrediensCount = ReadVarInt();
						for (int j = 0; j < ingrediensCount; j++)
						{
							recipe.Input.Add(ReadRecipeData());
						}

						int resultCount = ReadVarInt();
						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem(false));
						}

						recipe.Id = ReadUUID();
						recipe.Block = ReadString();
						ReadSignedVarInt();
						recipe.UniqueId = ReadVarInt();


						break;
					}
					case ShapedChemistry:
					{
						ReadString();
						int width = ReadSignedVarInt();
						int height = ReadSignedVarInt();
						var recipe = new ShapedRecipe(width, height);
						if (width > 3 || height > 3)
							throw new Exception("Wrong number of ingredience. Width=" + width + ", height=" + height);
						for (int w = 0; w < width; w++)
						{
							for (int h = 0; h < height; h++)
							{
								recipe.Input[(h * width) + w] = ReadRecipeData();
							}
						}

						int resultCount = ReadVarInt();
						for (int j = 0; j < resultCount; j++)
						{
							recipe.Result.Add(ReadItem(false));
						}

						recipe.Id = ReadUUID();
						recipe.Block = ReadString();
						ReadSignedVarInt();
						recipe.UniqueId = ReadVarInt();

						break;
					}
					case SmithingTrim:
					{
						var recipe = new SmithingTrimRecipe();
						recipe.RecipeId = ReadString();
						recipe.Template = ReadRecipeData();
						recipe.Input = ReadRecipeData();
						recipe.Addition = ReadRecipeData();
						recipe.Block = ReadString();
						recipe.UniqueId = ReadVarInt();


						break;
					}
					case SmithingTransform:
					{
						var recipe = new SmithingTransformRecipe();
						recipe.RecipeId = ReadString();
						recipe.Template = ReadRecipeData();
						recipe.Input = ReadRecipeData();
						recipe.Addition = ReadRecipeData();
						recipe.Output = ReadItem(false);
						recipe.Block = ReadString();
						recipe.UniqueId = ReadVarInt();


						break;
					}
					default:
						Console.WriteLine($"Read unknown recipe type: {recipeType}");

						break;
				}
			}


			return recipes;
		}

		public void Write(PotionContainerChangeRecipe[] recipes)
		{
			WriteSignedVarInt(0);
		}

		public PotionContainerChangeRecipe[] ReadPotionContainerChangeRecipes()
		{
			int count = (int)ReadUnsignedVarInt();
			var recipes = new PotionContainerChangeRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var recipe = new PotionContainerChangeRecipe();
				recipe.Input = ReadVarInt();
				recipe.Ingredient = ReadVarInt();
				recipe.Output = ReadVarInt();

				recipes[i] = recipe;
			}

			return recipes;
		}

		public void Write(MaterialReducerRecipe[] reducerRecipes)
		{
			WriteUnsignedVarInt((uint)reducerRecipes.Length);

			for (int i = 0; i < reducerRecipes.Length; i++)
			{
				var recipe = reducerRecipes[i];
				WriteVarInt((recipe.Input << 16) | recipe.InputMeta);
				WriteUnsignedVarInt((uint)recipe.Output.Length);

				foreach (var output in recipe.Output)
				{
					WriteVarInt(output.ItemId);
					WriteVarInt(output.ItemCount);
				}
			}
		}

		public MaterialReducerRecipe[] ReadMaterialReducerRecipes()
		{
			int count = (int)ReadUnsignedVarInt();
			var recipes = new MaterialReducerRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var inputIdAndMeta = ReadVarInt();
				var inputId = inputIdAndMeta >> 16;
				var inputMeta = inputIdAndMeta & 0x7fff;

				var outputCount = (int)ReadUnsignedVarInt();
				MaterialReducerRecipe.MaterialReducerRecipeOutput[] outputs =
					new MaterialReducerRecipe.MaterialReducerRecipeOutput[outputCount];

				for (int o = 0; o < outputs.Length; o++)
				{
					var itemId = ReadVarInt();
					var itemCount = ReadVarInt();

					outputs[o] = new MaterialReducerRecipe.MaterialReducerRecipeOutput(itemId, itemCount);
				}

				var recipe = new MaterialReducerRecipe(inputId, inputMeta, outputs);

				recipes[i] = recipe;
			}

			return recipes;
		}

		public void Write(PotionTypeRecipe[] recipes)
		{
			WriteSignedVarInt(0);
		}

		public PotionTypeRecipe[] ReadPotionTypeRecipes()
		{
			int count = (int)ReadUnsignedVarInt();
			var recipes = new PotionTypeRecipe[count];
			for (int i = 0; i < recipes.Length; i++)
			{
				var recipe = new PotionTypeRecipe();
				recipe.Input = ReadVarInt();
				recipe.InputMeta = ReadVarInt();
				recipe.Ingredient = ReadVarInt();
				recipe.IngredientMeta = ReadVarInt();
				recipe.Output = ReadVarInt();
				recipe.OutputMeta = ReadVarInt();

				recipes[i] = recipe;
			}

			return recipes;
		}
	}
}
