using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Network.MinecraftPacket
{
	public enum AttributeLayerPayloadType
	{
		UpdateLayers = 0,
		UpdateSettings = 1,
		UpdateEnvironment = 2,
		RemoveEnvironment = 3
	}

	public enum AttributeDataType
	{
		Bool = 0,
		Float = 1,
		Colour = 2
	}

	public enum AttributeBoolOperation
	{
		Override = 0,
		AlphaBlend = 1,
		And = 2,
		Nand = 3,
		Or = 4,
		Nor = 5,
		Xor = 6,
		Xnor = 7
	}

	public enum AttributeFloatOperation
	{
		Override = 0,
		AlphaBlend = 1,
		Add = 2,
		Subtract = 3,
		Multiply = 4,
		Minimum = 5,
		Maximum = 6
	}

	public enum AttributeColourOperation
	{
		Override = 0,
		AlphaBlend = 1,
		Add = 2,
		Subtract = 3,
		Multiply = 4
	}

	public enum AttributeLayerWeightType
	{
		Float = 0,
		String = 1
	}
	public struct AttributeLayerSettings
	{
		public int Priority { get; set; }
		public uint WeightType { get; set; }
		public float FloatWeight { get; set; }
		public string StringWeight { get; set; }
		public bool Enabled { get; set; }
		public bool TransitionsPaused { get; set; }
	}

	public struct EnvironmentAttributeData
	{
		public string AttributeName { get; set; }
		public Optional<AttributeData> FromAttribute { get; set; }
		public AttributeData Attribute { get; set; }
		public Optional<AttributeData> ToAttribute { get; set; }
		public uint CurrentTransitionTicks { get; set; }
		public uint TotalTransitionTicks { get; set; }
		public int EaseType { get; set; }
	}

	public struct AttributeData
	{
		public uint Type { get; set; }
		public bool BoolValue { get; set; }
		public Optional<int> BoolOperation { get; set; }
		public float FloatValue { get; set; }
		public Optional<int> FloatOperation { get; set; }
		public Optional<float> FloatConstraintMin { get; set; }
		public Optional<float> FloatConstraintMax { get; set; }
		public int ColourValue { get; set; }
		public Optional<int> ColourOperation { get; set; }
	}
	public struct AttributeLayerData
	{
		public string Name { get; set; }
		public int DimensionID { get; set; }
		public AttributeLayerSettings Settings { get; set; }
		public System.Collections.Generic.List<EnvironmentAttributeData> EnvironmentAttributes { get; set; }
	}

	public class McbeClientBoundAttributeLayerSync : Packet
	{
		public uint PayloadType { get; set; }
		public System.Collections.Generic.List<AttributeLayerData> Layers { get; set; }
		public string LayerName { get; set; }
		public int DimensionID { get; set; }
		public AttributeLayerSettings Settings { get; set; }
		public System.Collections.Generic.List<EnvironmentAttributeData> EnvironmentAttributes { get; set; }
		public System.Collections.Generic.List<string> RemoveAttributeNames { get; set; }
		public McbeClientBoundAttributeLayerSync()
		{
			Id = 345;
			IsMcbe = true;
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			PayloadType = ReadUnsignedVarInt();
			switch ((AttributeLayerPayloadType)PayloadType)
			{
				case AttributeLayerPayloadType.UpdateLayers:
					Layers = ReadSlice(() => ReadAttributeLayerData()).ToList();
					break;
				case AttributeLayerPayloadType.UpdateSettings:
					LayerName = ReadString();
					DimensionID = ReadVarInt();
					Settings = ReadAttributeLayerSettings();
					break;
				case AttributeLayerPayloadType.UpdateEnvironment:
					LayerName = ReadString();
					DimensionID = ReadVarInt();
					EnvironmentAttributes = ReadSlice(() => ReadEnvironmentAttributeData()).ToList();
					break;
				case AttributeLayerPayloadType.RemoveEnvironment:
					LayerName = ReadString();
					DimensionID = ReadVarInt();
					RemoveAttributeNames = ReadSlice(() => ReadString()).ToList();
					break;
				default:
					// Unknown enum option
					break;
			}
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			WriteUnsignedVarInt(PayloadType);
			switch ((AttributeLayerPayloadType)PayloadType)
			{
				case AttributeLayerPayloadType.UpdateLayers:
					WriteSlice(Layers.ToArray(), Write);
					break;
				case AttributeLayerPayloadType.UpdateSettings:
					Write(LayerName);
					WriteVarInt(DimensionID);
					Write(Settings);
					break;
				case AttributeLayerPayloadType.UpdateEnvironment:
					Write(LayerName);
					WriteVarInt(DimensionID);
					WriteSlice(EnvironmentAttributes.ToArray(), Write);
					break;
				case AttributeLayerPayloadType.RemoveEnvironment:
					Write(LayerName);
					WriteVarInt(DimensionID);
					WriteSlice(RemoveAttributeNames.ToArray(), Write);
					break;
				default:
					// Unknown enum option
					break;
			}
		}
	}
}
