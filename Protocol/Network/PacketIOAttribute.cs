using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft.Camera;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(AttributeLayerSettings value)
		{
			Write(value.Priority);
			WriteUnsignedVarInt(value.WeightType);

			switch ((AttributeLayerWeightType)value.WeightType)
			{
				case AttributeLayerWeightType.Float:
					Write(value.FloatWeight);
					break;
				case AttributeLayerWeightType.String:
					Write(value.StringWeight);
					break;
				default:
					throw new ArgumentException($"Unknown attribute layer weight type: {value.WeightType}");
			}

			Write(value.Enabled);
			Write(value.TransitionsPaused);
		}

		public AttributeLayerSettings ReadAttributeLayerSettings()
		{
			var settings = new AttributeLayerSettings
			{
				Priority = ReadInt(),
				WeightType = ReadUnsignedVarInt()
			};

			switch ((AttributeLayerWeightType)settings.WeightType)
			{
				case AttributeLayerWeightType.Float:
					settings.FloatWeight = ReadFloat();
					break;
				case AttributeLayerWeightType.String:
					settings.StringWeight = ReadString();
					break;
				default:
					throw new FormatException($"Unknown attribute layer weight type: {settings.WeightType}");
			}

			settings.Enabled = ReadBool();
			settings.TransitionsPaused = ReadBool();

			return settings;
		}

		public void Write(AttributeData value)
		{
			WriteUnsignedVarInt(value.Type);

			switch ((AttributeDataType)value.Type)
			{
				case AttributeDataType.Bool:
					Write(value.BoolValue);
					Write(value.BoolOperation.HasValue);
					if (value.BoolOperation.HasValue)
					{
						Write(value.BoolOperation.Value);
					}
					break;
				case AttributeDataType.Float:
					Write(value.FloatValue);
					Write(value.FloatOperation.HasValue);
					if (value.FloatOperation.HasValue)
					{
						Write(value.FloatOperation.Value);
					}
					Write(value.FloatConstraintMin.HasValue);
					if (value.FloatConstraintMin.HasValue)
					{
						Write(value.FloatConstraintMin.Value);
					}
					Write(value.FloatConstraintMax.HasValue);
					if (value.FloatConstraintMax.HasValue)
					{
						Write(value.FloatConstraintMax.Value);
					}
					break;
				case AttributeDataType.Colour:
					Write(value.ColourValue);
					Write(value.ColourOperation.HasValue);
					if (value.ColourOperation.HasValue)
					{
						Write(value.ColourOperation.Value);
					}
					break;
				default:
					throw new ArgumentException($"Unknown attribute data type: {value.Type}");
			}
		}

		public AttributeData ReadAttributeData()
		{
			var data = new AttributeData
			{
				Type = ReadUnsignedVarInt()
			};

			switch ((AttributeDataType)data.Type)
			{
				case AttributeDataType.Bool:
					data.BoolValue = ReadBool();
					if (ReadBool())
					{
						data.BoolOperation = new Optional<int>(ReadInt());
					}
					break;
				case AttributeDataType.Float:
					data.FloatValue = ReadFloat();
					if (ReadBool())
					{
						data.FloatOperation = new Optional<int>(ReadInt());
					}
					if (ReadBool())
					{
						data.FloatConstraintMin = new Optional<float>(ReadFloat());
					}
					if (ReadBool())
					{
						data.FloatConstraintMax = new Optional<float>(ReadFloat());
					}
					break;
				case AttributeDataType.Colour:
					data.ColourValue = ReadInt();
					if (ReadBool())
					{
						data.ColourOperation = new Optional<int>(ReadInt());
					}
					break;
				default:
					throw new FormatException($"Unknown attribute data type: {data.Type}");
			}

			return data;
		}

		public void Write(EnvironmentAttributeData value)
		{
			// EaseType to string conversion
			string easingType = EasingType.EasingTypeToString(value.EaseType);

			Write(value.AttributeName);

			Write(value.FromAttribute.HasValue);
			if (value.FromAttribute.HasValue)
			{
				Write(value.FromAttribute.Value);
			}

			Write(value.Attribute);

			Write(value.ToAttribute.HasValue);
			if (value.ToAttribute.HasValue)
			{
				Write(value.ToAttribute.Value);
			}

			Write(value.CurrentTransitionTicks);
			Write(value.TotalTransitionTicks);
			Write(easingType);

			// Convert string back to EaseType
			value.EaseType = (int)EasingType.EasingTypeFromString(easingType);
		}

		public EnvironmentAttributeData ReadEnvironmentAttributeData()
		{
			var data = new EnvironmentAttributeData();

			string easingType = ReadString();
			data.EaseType =  (int)EasingType.EasingTypeFromString(easingType);

			data.AttributeName = ReadString();

			if (ReadBool())
			{
				data.FromAttribute = new Optional<AttributeData>(ReadAttributeData());
			}

			data.Attribute = ReadAttributeData();

			if (ReadBool())
			{
				data.ToAttribute = new Optional<AttributeData>(ReadAttributeData());
			}

			data.CurrentTransitionTicks = ReadUint();
			data.TotalTransitionTicks = ReadUint();

			return data;
		}

		public void Write(AttributeLayerData value)
		{
			Write(value.Name);
			WriteSignedVarInt(value.DimensionID);
			Write(value.Settings);

			if (value.EnvironmentAttributes != null)
			{
				WriteSliceVarint32Length(value.EnvironmentAttributes.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}
		}

		public AttributeLayerData ReadAttributeLayerData()
		{
			var data = new AttributeLayerData
			{
				Name = ReadString(),
				DimensionID = ReadSignedVarInt(),
				Settings = ReadAttributeLayerSettings()
			};

			var envArray = ReadSliceVarint32Length(ReadEnvironmentAttributeData);
			data.EnvironmentAttributes = new System.Collections.Generic.List<EnvironmentAttributeData>(envArray);

			return data;
		}

	}
}
