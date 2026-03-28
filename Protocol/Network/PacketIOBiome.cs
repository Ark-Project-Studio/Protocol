using System;
using System.Collections.Generic;
using System.Linq;
using Protocol.Minecraft;
using Protocol.Utils;

namespace Protocol.Network
{
	public partial class Packet
	{
		
		public void Write(BiomeDefinition value)
		{
			Write(value.NameIndex);
			Write(value.BiomeID);
			Write(value.Temperature);
			Write(value.Downfall);
			Write(value.FoliageSnow);
			Write(value.Depth);
			Write(value.Scale);
			Write(value.MapWaterColour);
			Write(value.Rain);

			Write(value.Tags.HasValue);
			if (value.Tags.HasValue && value.Tags.Value != null)
			{
				WriteSliceVarint32Length(value.Tags.Value.ToArray(), Write,false);
			}

			Write(value.ChunkGeneration.HasValue);
			if (value.ChunkGeneration.HasValue)
			{
				Write(value.ChunkGeneration.Value);
			}
		}

		public void Write(BiomeChunkGeneration value)
		{
			Write(value.Climate.HasValue);
			if (value.Climate.HasValue)
			{
				Write(value.Climate.Value);
			}

			Write(value.ConsolidatedFeatures.HasValue);
			if (value.ConsolidatedFeatures.HasValue && value.ConsolidatedFeatures.Value != null)
			{
				WriteSliceVarint32Length(value.ConsolidatedFeatures.Value.ToArray(), Write);
			}

			Write(value.MountainParameters.HasValue);
			if (value.MountainParameters.HasValue)
			{
				Write(value.MountainParameters.Value);
			}

			Write(value.SurfaceMaterialAdjustments.HasValue);
			if (value.SurfaceMaterialAdjustments.HasValue && value.SurfaceMaterialAdjustments.Value != null)
			{
				WriteSliceVarint32Length(value.SurfaceMaterialAdjustments.Value.ToArray(), Write);
			}

			Write(value.SurfaceMaterials.HasValue);
			if (value.SurfaceMaterials.HasValue)
			{
				Write(value.SurfaceMaterials.Value);
			}

			Write(value.HasDefaultOverworldSurface);
			Write(value.HasSwampSurface);
			Write(value.HasFrozenOceanSurface);
			Write(value.HasEndSurface);

			Write(value.MesaSurface.HasValue);
			if (value.MesaSurface.HasValue)
			{
				Write(value.MesaSurface.Value);
			}

			Write(value.CappedSurface.HasValue);
			if (value.CappedSurface.HasValue)
			{
				Write(value.CappedSurface.Value);
			}

			Write(value.OverworldRules.HasValue);
			if (value.OverworldRules.HasValue)
			{
				Write(value.OverworldRules.Value);
			}

			Write(value.MultiNoiseRules.HasValue);
			if (value.MultiNoiseRules.HasValue)
			{
				Write(value.MultiNoiseRules.Value);
			}

			Write(value.LegacyRules.HasValue);
			if (value.LegacyRules.HasValue && value.LegacyRules.Value != null)
			{
				WriteSliceVarint32Length(value.LegacyRules.Value.ToArray(), Write);
			}

			Write(value.ReplacementsData.HasValue);
			if (value.ReplacementsData.HasValue && value.ReplacementsData.Value != null)
			{
				WriteSliceVarint32Length(value.ReplacementsData.Value.ToArray(), Write);
			}

			Write(value.VillageType.HasValue);
			if (value.VillageType.HasValue)
			{
				Write(value.VillageType.Value);
			}
		}

		public void Write(BiomeClimate value)
		{
			Write(value.Temperature);
			Write(value.Downfall);
			Write(value.SnowAccumulationMin);
			Write(value.SnowAccumulationMax);
		}

		public void Write(BiomeConsolidatedFeature value)
		{
			Write(value.Scatter);
			Write(value.Feature);
			Write(value.Identifier);
			Write(value.Pass);
			Write(value.CanUseInternal);
		}

		public void Write(BiomeScatterParameter value)
		{
			if (value.Coordinates != null)
			{
				WriteSliceVarint32Length(value.Coordinates.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			WriteSignedVarInt(value.EvaluationOrder);
			WriteSignedVarInt(value.ChancePercentType);
			Write(value.ChancePercent);
			Write(value.ChanceNumerator);
			Write(value.ChanceDenominator);
			WriteSignedVarInt(value.IterationsType);
			Write(value.Iterations);
		}

		public void Write(BiomeCoordinate value)
		{
			WriteSignedVarInt(value.MinValueType);
			Write(value.MinValue);
			WriteSignedVarInt(value.MaxValueType);
			Write(value.MaxValue);
			Write(value.GridOffset);
			Write(value.GridStepSize);
			WriteSignedVarInt(value.Distribution);
		}

		public void Write(BiomeMountainParameters value)
		{
			Write(value.SteepBlock);
			Write(value.NorthSlopes);
			Write(value.SouthSlopes);
			Write(value.WestSlopes);
			Write(value.EastSlopes);
			Write(value.TopSlideEnabled);
		}

		public void Write(BiomeElementData value)
		{
			Write(value.NoiseFrequencyScale);
			Write(value.NoiseLowerBound);
			Write(value.NoiseUpperBound);
			WriteSignedVarInt(value.HeightMinType);
			Write(value.HeightMin);
			WriteSignedVarInt(value.HeightMaxType);
			Write(value.HeightMax);
			Write(value.AdjustedMaterials);
		}

		public void Write(BiomeSurfaceMaterial value)
		{
			Write(value.TopBlock);
			Write(value.MidBlock);
			Write(value.SeaFloorBlock);
			Write(value.FoundationBlock);
			Write(value.SeaBlock);
			Write(value.SeaFloorDepth);
		}

		public void Write(BiomeMesaSurface value)
		{
			Write(value.ClayMaterial);
			Write(value.HardClayMaterial);
			Write(value.BrycePillars);
			Write(value.HasForest);
		}

		public void Write(BiomeCappedSurface value)
		{
			if (value.FloorBlocks != null)
			{
				WriteSliceVarint32Length(value.FloorBlocks.ToArray(), Write,false);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			if (value.CeilingBlocks != null)
			{
				WriteSliceVarint32Length(value.CeilingBlocks.ToArray(), Write,false);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			Write(value.SeaBlock.HasValue);
			if (value.SeaBlock.HasValue)
			{
				Write(value.SeaBlock.Value);
			}

			Write(value.FoundationBlock.HasValue);
			if (value.FoundationBlock.HasValue)
			{
				Write(value.FoundationBlock.Value);
			}

			Write(value.BeachBlock.HasValue);
			if (value.BeachBlock.HasValue)
			{
				Write(value.BeachBlock.Value);
			}
		}

		public void Write(BiomeOverworldRules value)
		{
			if (value.HillsTransformations != null)
			{
				WriteSliceVarint32Length(value.HillsTransformations.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			if (value.MutateTransformations != null)
			{
				WriteSliceVarint32Length(value.MutateTransformations.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			if (value.RiverTransformations != null)
			{
				WriteSliceVarint32Length(value.RiverTransformations.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			if (value.ShoreTransformations != null)
			{
				WriteSliceVarint32Length(value.ShoreTransformations.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			if (value.PreHillsEdgeTransformations != null)
			{
				WriteSliceVarint32Length(value.PreHillsEdgeTransformations.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			if (value.PostShoreEdgeTransformations != null)
			{
				WriteSliceVarint32Length(value.PostShoreEdgeTransformations.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			if (value.ClimateTransformations != null)
			{
				WriteSliceVarint32Length(value.ClimateTransformations.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}
		}

		public void Write(BiomeMultiNoiseRules value)
		{
			Write(value.Temperature);
			Write(value.Humidity);
			Write(value.Altitude);
			Write(value.Weirdness);
			Write(value.Weight);
		}

		public void Write(BiomeConditionalTransformation value)
		{
			if (value.WeightedBiomes != null)
			{
				WriteSliceVarint32Length(value.WeightedBiomes.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			Write(value.ConditionJSON);
			Write(value.MinPassingNeighbours);
		}

		public void Write(BiomeWeight value)
		{
			Write(value.Biome);
			Write(value.Weight);
		}

		public void Write(BiomeTemperatureWeight value)
		{
			WriteSignedVarInt(value.Temperature);
			Write(value.Weight);
		}

		public void Write(BiomeReplacementData value)
		{
			Write(value.Biome);
			Write(value.Dimension);

			if (value.TargetBiomes != null)
			{
				WriteSliceVarint32Length(value.TargetBiomes.ToArray(), Write,false);
			}
			else
			{
				WriteSignedVarInt(0);
			}

			Write(value.Amount);
			Write(value.NoiseFrequencyScale);
			Write(value.ReplacementIndex);
		}
		public BiomeDefinition ReadBiomeDefinition()
		{
			var value = new BiomeDefinition
			{
				NameIndex = ReadShort(),
				BiomeID = ReadShort(),
				Temperature = ReadFloat(),
				Downfall = ReadFloat(),
				FoliageSnow = ReadFloat(),
				Depth = ReadFloat(),
				Scale = ReadFloat(),
				MapWaterColour = ReadInt(),
				Rain = ReadBool()
			};

			if (ReadBool())
			{
				var tagsArray = ReadSlice(ReadUshort,false);
				value.Tags = new Optional<System.Collections.Generic.List<ushort>>(new System.Collections.Generic.List<ushort>(tagsArray));
			}

			if (ReadBool())
			{
				value.ChunkGeneration = new Optional<BiomeChunkGeneration>(ReadBiomeChunkGeneration());
			}

			return value;
		}

		public BiomeChunkGeneration ReadBiomeChunkGeneration()
		{
			var value = new BiomeChunkGeneration();

			if (ReadBool())
			{
				value.Climate = new Optional<BiomeClimate>(ReadBiomeClimate());
			}

			if (ReadBool())
			{
				var featuresArray = ReadSlice(ReadBiomeConsolidatedFeature);
				value.ConsolidatedFeatures = new Optional<System.Collections.Generic.List<BiomeConsolidatedFeature>>(new System.Collections.Generic.List<BiomeConsolidatedFeature>(featuresArray));
			}

			if (ReadBool())
			{
				value.MountainParameters = new Optional<BiomeMountainParameters>(ReadBiomeMountainParameters());
			}

			if (ReadBool())
			{
				var adjustmentsArray = ReadSlice(ReadBiomeElementData);
				value.SurfaceMaterialAdjustments = new Optional<System.Collections.Generic.List<BiomeElementData>>(new System.Collections.Generic.List<BiomeElementData>(adjustmentsArray));
			}

			if (ReadBool())
			{
				value.SurfaceMaterials = new Optional<BiomeSurfaceMaterial>(ReadBiomeSurfaceMaterial());
			}

			value.HasDefaultOverworldSurface = ReadBool();
			value.HasSwampSurface = ReadBool();
			value.HasFrozenOceanSurface = ReadBool();
			value.HasEndSurface = ReadBool();

			if (ReadBool())
			{
				value.MesaSurface = new Optional<BiomeMesaSurface>(ReadBiomeMesaSurface());
			}

			if (ReadBool())
			{
				value.CappedSurface = new Optional<BiomeCappedSurface>(ReadBiomeCappedSurface());
			}

			if (ReadBool())
			{
				value.OverworldRules = new Optional<BiomeOverworldRules>(ReadBiomeOverworldRules());
			}

			if (ReadBool())
			{
				value.MultiNoiseRules = new Optional<BiomeMultiNoiseRules>(ReadBiomeMultiNoiseRules());
			}

			if (ReadBool())
			{
				var legacyArray = ReadSlice(ReadBiomeConditionalTransformation);
				value.LegacyRules = new Optional<System.Collections.Generic.List<BiomeConditionalTransformation>>(new System.Collections.Generic.List<BiomeConditionalTransformation>(legacyArray));
			}

			if (ReadBool())
			{
				var replacementsArray = ReadSlice(ReadBiomeReplacementData);
				value.ReplacementsData = new Optional<System.Collections.Generic.List<BiomeReplacementData>>(new System.Collections.Generic.List<BiomeReplacementData>(replacementsArray));
			}

			if (ReadBool())
			{
				value.VillageType = new Optional<byte>(ReadByte());
			}

			return value;
		}

		public BiomeClimate ReadBiomeClimate()
		{
			return new BiomeClimate
			{
				Temperature = ReadFloat(),
				Downfall = ReadFloat(),
				SnowAccumulationMin = ReadFloat(),
				SnowAccumulationMax = ReadFloat()
			};
		}

		public BiomeConsolidatedFeature ReadBiomeConsolidatedFeature()
		{
			return new BiomeConsolidatedFeature
			{
				Scatter = ReadBiomeScatterParameter(),
				Feature = ReadShort(),
				Identifier = ReadShort(),
				Pass = ReadShort(),
				CanUseInternal = ReadBool()
			};
		}

		public BiomeScatterParameter ReadBiomeScatterParameter()
		{
			return new BiomeScatterParameter
			{
				Coordinates = new System.Collections.Generic.List<BiomeCoordinate>(ReadSlice(ReadBiomeCoordinate)),
				EvaluationOrder = ReadSignedVarInt(),
				ChancePercentType = ReadSignedVarInt(),
				ChancePercent = ReadShort(),
				ChanceNumerator = ReadInt(),
				ChanceDenominator = ReadInt(),
				IterationsType = ReadSignedVarInt(),
				Iterations = ReadShort()
			};
		}

		public BiomeCoordinate ReadBiomeCoordinate()
		{
			return new BiomeCoordinate
			{
				MinValueType = ReadSignedVarInt(),
				MinValue = ReadShort(),
				MaxValueType = ReadSignedVarInt(),
				MaxValue = ReadShort(),
				GridOffset = ReadUint(),
				GridStepSize = ReadUint(),
				Distribution = ReadSignedVarInt()
			};
		}

		public BiomeMountainParameters ReadBiomeMountainParameters()
		{
			return new BiomeMountainParameters
			{
				SteepBlock = ReadInt(),
				NorthSlopes = ReadBool(),
				SouthSlopes = ReadBool(),
				WestSlopes = ReadBool(),
				EastSlopes = ReadBool(),
				TopSlideEnabled = ReadBool()
			};
		}

		public BiomeElementData ReadBiomeElementData()
		{
			return new BiomeElementData
			{
				NoiseFrequencyScale = ReadFloat(),
				NoiseLowerBound = ReadFloat(),
				NoiseUpperBound = ReadFloat(),
				HeightMinType = ReadSignedVarInt(),
				HeightMin = ReadShort(),
				HeightMaxType = ReadSignedVarInt(),
				HeightMax = ReadShort(),
				AdjustedMaterials = ReadBiomeSurfaceMaterial()
			};
		}

		public BiomeSurfaceMaterial ReadBiomeSurfaceMaterial()
		{
			return new BiomeSurfaceMaterial
			{
				TopBlock = ReadInt(),
				MidBlock = ReadInt(),
				SeaFloorBlock = ReadInt(),
				FoundationBlock = ReadInt(),
				SeaBlock = ReadInt(),
				SeaFloorDepth = ReadInt()
			};
		}

		public BiomeMesaSurface ReadBiomeMesaSurface()
		{
			return new BiomeMesaSurface
			{
				ClayMaterial = ReadUint(),
				HardClayMaterial = ReadUint(),
				BrycePillars = ReadBool(),
				HasForest = ReadBool()
			};
		}

		public BiomeCappedSurface ReadBiomeCappedSurface()
		{
			var value = new BiomeCappedSurface
			{
				FloorBlocks = new System.Collections.Generic.List<int>(ReadSlice(ReadInt,false)),
				CeilingBlocks = new System.Collections.Generic.List<int>(ReadSlice(ReadInt,false))
			};

			if (ReadBool())
			{
				value.SeaBlock = new Optional<uint>(ReadUint());
			}

			if (ReadBool())
			{
				value.FoundationBlock = new Optional<uint>(ReadUint());
			}

			if (ReadBool())
			{
				value.BeachBlock = new Optional<uint>(ReadUint());
			}

			return value;
		}

		public BiomeOverworldRules ReadBiomeOverworldRules()
		{
			return new BiomeOverworldRules
			{
				HillsTransformations = new System.Collections.Generic.List<BiomeWeight>(ReadSlice(ReadBiomeWeight)),
				MutateTransformations = new System.Collections.Generic.List<BiomeWeight>(ReadSlice(ReadBiomeWeight)),
				RiverTransformations = new System.Collections.Generic.List<BiomeWeight>(ReadSlice(ReadBiomeWeight)),
				ShoreTransformations = new System.Collections.Generic.List<BiomeWeight>(ReadSlice(ReadBiomeWeight)),
				PreHillsEdgeTransformations = new System.Collections.Generic.List<BiomeConditionalTransformation>(ReadSlice(ReadBiomeConditionalTransformation)),
				PostShoreEdgeTransformations = new System.Collections.Generic.List<BiomeConditionalTransformation>(ReadSlice(ReadBiomeConditionalTransformation)),
				ClimateTransformations = new System.Collections.Generic.List<BiomeTemperatureWeight>(ReadSlice(ReadBiomeTemperatureWeight))
			};
		}

		public BiomeMultiNoiseRules ReadBiomeMultiNoiseRules()
		{
			return new BiomeMultiNoiseRules
			{
				Temperature = ReadFloat(),
				Humidity = ReadFloat(),
				Altitude = ReadFloat(),
				Weirdness = ReadFloat(),
				Weight = ReadFloat()
			};
		}

		public BiomeConditionalTransformation ReadBiomeConditionalTransformation()
		{
			return new BiomeConditionalTransformation
			{
				WeightedBiomes = new System.Collections.Generic.List<BiomeWeight>(ReadSlice(ReadBiomeWeight)),
				ConditionJSON = ReadShort(),
				MinPassingNeighbours = ReadUint()
			};
		}

		public BiomeWeight ReadBiomeWeight()
		{
			return new BiomeWeight
			{
				Biome = ReadShort(),
				Weight = ReadUint()
			};
		}

		public BiomeTemperatureWeight ReadBiomeTemperatureWeight()
		{
			return new BiomeTemperatureWeight
			{
				Temperature = ReadSignedVarInt(),
				Weight = ReadUint()
			};
		}

		public BiomeReplacementData ReadBiomeReplacementData()
		{
			return new BiomeReplacementData
			{
				Biome = ReadShort(),
				Dimension = ReadShort(),
				TargetBiomes = new System.Collections.Generic.List<short>(ReadSlice(ReadShort,false)),
				Amount = ReadFloat(),
				NoiseFrequencyScale = ReadFloat(),
				ReplacementIndex = ReadUint()
			};
		}
	}
	
}
