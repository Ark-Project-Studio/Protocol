using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Protocol.Minecraft;
using Protocol.Minecraft.Camera;

namespace Protocol.Network
{
	public partial class Packet
	{
		// RGB - writes as 3 float32s (0-1 range)
		public void WriteRGB(System.Drawing.Color value)
		{
			float red = value.R / 255f;
			float green = value.G / 255f;
			float blue = value.B / 255f;
			Write(red);
			Write(green);
			Write(blue);
		}

		public System.Drawing.Color ReadRGB()
		{
			float red = ReadFloat();
			float green = ReadFloat();
			float blue = ReadFloat();
			return System.Drawing.Color.FromArgb(
				(int)(red * 255),
				(int)(green * 255),
				(int)(blue * 255)
			);
		}

		// RGBA - writes as uint32 (R,G,B,A)
		public void WriteRGBA(System.Drawing.Color value)
		{
			uint val = (uint)value.R | (uint)value.G << 8 | (uint)value.B << 16 | (uint)value.A << 24;
			Write(val);
		}

		public System.Drawing.Color ReadRGBA()
		{
			uint v = ReadUint();
			byte r = (byte)v;
			byte g = (byte)(v >> 8);
			byte b = (byte)(v >> 16);
			byte a = (byte)(v >> 24);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		// ARGB - writes as int32 (A,R,G,B)
		public void WriteARGB(System.Drawing.Color value)
		{
			int val = value.A | value.R << 8 | value.G << 16 | value.B << 24;
			Write(val);
		}

		public System.Drawing.Color ReadARGB()
		{
			int v = ReadInt();
			byte a = (byte)v;
			byte r = (byte)(v >> 8);
			byte g = (byte)(v >> 16);
			byte b = (byte)(v >> 24);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		// BEARGB - writes as big endian int32 (A,R,G,B)
		public void WriteBEARGB(System.Drawing.Color value)
		{
			int val = value.A | value.R << 8 | value.G << 16 | value.B << 24;
			WriteBe(val);
		}

		public System.Drawing.Color ReadBEARGB()
		{
			int v = ReadIntBe();
			byte a = (byte)v;
			byte r = (byte)(v >> 8);
			byte g = (byte)(v >> 16);
			byte b = (byte)(v >> 24);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}

		// VarRGBA - writes as varuint32 (R,G,B,A)
		public void WriteVarRGBA(System.Drawing.Color value)
		{
			uint val = (uint)value.R | (uint)value.G << 8 | (uint)value.B << 16 | (uint)value.A << 24;
			WriteUnsignedVarInt(val);
		}

		public System.Drawing.Color ReadVarRGBA()
		{
			uint v = ReadUnsignedVarInt();
			byte r = (byte)v;
			byte g = (byte)(v >> 8);
			byte b = (byte)(v >> 16);
			byte a = (byte)(v >> 24);
			return System.Drawing.Color.FromArgb(a, r, g, b);
		}
		public CameraEase ReadCameraEase()
		{
			return new CameraEase
			{
				Type = (EasingTypeEnum)ReadByte(),
				Duration = ReadFloat()
			};
		}

		public CameraInstruction ReadCameraInstruction()
		{
			var value = new CameraInstruction();

			if (ReadBool()) value.Set = new Optional<CameraInstructionSet>(ReadCameraInstructionSet());
			if (ReadBool()) value.Clear = new Optional<bool>(ReadBool());
			if (ReadBool()) value.Fade = new Optional<CameraInstructionFade>(ReadCameraInstructionFade());
			if (ReadBool()) value.Target = new Optional<CameraInstructionTarget>(ReadCameraInstructionTarget());
			if (ReadBool()) value.RemoveTarget = new Optional<bool>(ReadBool());
			if (ReadBool()) value.FieldOfView = new Optional<CameraInstructionFieldOfView>(ReadCameraInstructionFieldOfView());
			if (ReadBool()) value.Spline = new Optional<CameraSplineInstruction>(ReadCameraSplineInstruction());
			if (ReadBool()) value.Attach = new Optional<CameraAttachToEntityInstruction>(ReadCameraAttachToEntityInstruction());
			if (ReadBool()) value.DetachFromEntity = new Optional<bool>(ReadBool());

			return value;
		}

		public CameraInstructionSet ReadCameraInstructionSet()
		{
			var value = new CameraInstructionSet
			{
				Preset = ReadUint()
			};

			if (ReadBool())
			{
				value.Ease = new Optional<CameraEase>(ReadCameraEase());
			}

			if (ReadBool())
			{
				value.Position = new Optional<System.Numerics.Vector3>(ReadVector3());
			}

			if (ReadBool())
			{
				value.Rotation = new Optional<System.Numerics.Vector2>(ReadVector2());
			}

			if (ReadBool())
			{
				value.Facing = new Optional<System.Numerics.Vector3>(ReadVector3());
			}

			if (ReadBool())
			{
				value.ViewOffset = new Optional<System.Numerics.Vector2>(ReadVector2());
			}

			if (ReadBool())
			{
				value.EntityOffset = new Optional<System.Numerics.Vector3>(ReadVector3());
			}

			if (ReadBool())
			{
				value.Default = new Optional<bool>(ReadBool());
			}

			value.RemoveIgnoreStartingValuesComponent = ReadBool();

			return value;
		}

		public CameraFadeTimeData ReadCameraFadeTimeData()
		{
			return new CameraFadeTimeData
			{
				FadeInDuration = ReadFloat(),
				WaitDuration = ReadFloat(),
				FadeOutDuration = ReadFloat()
			};
		}

		public CameraInstructionFade ReadCameraInstructionFade()
		{
			var value = new CameraInstructionFade();

			if (ReadBool())
			{
				value.TimeData = new Optional<CameraFadeTimeData>(ReadCameraFadeTimeData());
			}

			if (ReadBool())
			{
				value.Colour = new Optional<System.Drawing.Color>(ReadRGB());
			}

			return value;
		}

		public CameraInstructionTarget ReadCameraInstructionTarget()
		{
			var value = new CameraInstructionTarget();

			if (ReadBool())
			{
				value.CenterOffset = new Optional<System.Numerics.Vector3>(ReadVector3());
			}

			value.EntityUniqueID = ReadLong();

			return value;
		}

		public CameraInstructionFieldOfView ReadCameraInstructionFieldOfView()
		{
			var value = new CameraInstructionFieldOfView
			{
				FieldOfView = ReadFloat(),
				EaseTime = ReadFloat()
			};

			string easingType = ReadString();
			value.EaseType = EasingType.EasingTypeFromString(easingType);
			value.Clear = ReadBool();

			return value;
		}

		public CameraAttachToEntityInstruction ReadCameraAttachToEntityInstruction()
		{
			return new CameraAttachToEntityInstruction
			{
				ActorUniqueId = ReadLong()
			};
		}

		public CameraPreset ReadCameraPreset()
		{
			var value = new CameraPreset
			{
				Name = ReadString(),
				Parent = ReadString()
			};

			if (ReadBool()) value.PosX = new Optional<float>(ReadFloat());
			if (ReadBool()) value.PosY = new Optional<float>(ReadFloat());
			if (ReadBool()) value.PosZ = new Optional<float>(ReadFloat());
			if (ReadBool()) value.RotX = new Optional<float>(ReadFloat());
			if (ReadBool()) value.RotY = new Optional<float>(ReadFloat());
			if (ReadBool()) value.RotationSpeed = new Optional<float>(ReadFloat());
			if (ReadBool()) value.SnapToTarget = new Optional<bool>(ReadBool());
			if (ReadBool()) value.HorizontalRotationLimit = new Optional<System.Numerics.Vector2>(ReadVector2());
			if (ReadBool()) value.VerticalRotationLimit = new Optional<System.Numerics.Vector2>(ReadVector2());
			if (ReadBool()) value.ContinueTargeting = new Optional<bool>(ReadBool());
			if (ReadBool()) value.BlockListingRadius = new Optional<float>(ReadFloat());
			if (ReadBool()) value.ViewOffset = new Optional<System.Numerics.Vector2>(ReadVector2());
			if (ReadBool()) value.EntityOffset = new Optional<System.Numerics.Vector3>(ReadVector3());
			if (ReadBool()) value.Radius = new Optional<float>(ReadFloat());
			if (ReadBool()) value.MinYawLimit = new Optional<float>(ReadFloat());
			if (ReadBool()) value.MaxYawLimit = new Optional<float>(ReadFloat());
			if (ReadBool()) value.AudioListener = new Optional<AudioListener>((AudioListener)ReadByte());
			if (ReadBool()) value.PlayerEffects = new Optional<bool>(ReadBool());
			if (ReadBool()) value.AimAssist = new Optional<CameraPresetAimAssist>(ReadCameraPresetAimAssist());
			if (ReadBool()) value.ControlScheme = new Optional<byte>(ReadByte());

			return value;
		}

		public CameraAimAssistCategoryDefinition ReadCameraAimAssistCategoryDefinition()
		{
			return new CameraAimAssistCategoryDefinition
			{
				Name = ReadString(),
				Priorities = ReadCameraAimAssistPriorities()
			};
		}

		public CameraAimAssistCategoriesDefinition ReadCameraAimAssistCategoriesDefinition()
		{
			return new CameraAimAssistCategoriesDefinition
			{
				Identifier = ReadString(),
				Category = new System.Collections.Generic.List<CameraAimAssistCategoryDefinition>(ReadSlice(ReadCameraAimAssistCategoryDefinition))
			};
		}

		public CameraPresetAimAssist ReadCameraPresetAimAssist()
		{
			var value = new CameraPresetAimAssist();

			if (ReadBool()) value.Preset = new Optional<string>(ReadString());
			if (ReadBool()) value.TargetMode = new Optional<AimAssistTargetMode>((AimAssistTargetMode)ReadByte());
			if (ReadBool()) value.Angle = new Optional<System.Numerics.Vector2>(ReadVector2());
			if (ReadBool()) value.Distance = new Optional<float>(ReadFloat());

			return value;
		}

		public CameraAimAssistCategory ReadCameraAimAssistCategory()
		{
			return new CameraAimAssistCategory
			{
				Name = ReadString(),
				Priorities = ReadCameraAimAssistPriorities()
			};
		}

		public CameraAimAssistPriorities ReadCameraAimAssistPriorities()
		{
			var value =  new CameraAimAssistPriorities
			{
				Entities = new System.Collections.Generic.List<CameraAimAssistPriority>(ReadSlice(ReadCameraAimAssistPriority)),
				Blocks = new System.Collections.Generic.List<CameraAimAssistPriority>(ReadSlice(ReadCameraAimAssistPriority)),
				BlockTags = new System.Collections.Generic.List<CameraAimAssistPriority>(ReadSlice(ReadCameraAimAssistPriority)),
				EntityTypeFamilies = new System.Collections.Generic.List<CameraAimAssistPriority>(ReadSlice(ReadCameraAimAssistPriority))
			};

			if (ReadBool()) value.EntityDefault = new Optional<int>(ReadInt());
			if (ReadBool()) value.BlockDefault = new Optional<int>(ReadInt());

			return value;
		}

		public CameraAimAssistPriority ReadCameraAimAssistPriority()
		{
			return new CameraAimAssistPriority
			{
				Identifier = ReadString(),
				Priority = ReadInt()
			};
		}

		public CameraAimAssistPreset ReadCameraAimAssistPreset()
		{
			var value = new CameraAimAssistPreset
			{
				Identifier = ReadString(),
				BlockExclusions = new System.Collections.Generic.List<string>(ReadSlice(ReadString)),
				EntityExclusions = new System.Collections.Generic.List<string>(ReadSlice(ReadString)),
				BlockTagExclusions = new System.Collections.Generic.List<string>(ReadSlice(ReadString)),
				EntityTypeFamilyExclusions = new System.Collections.Generic.List<string>(ReadSlice(ReadString)),
				LiquidTargets = new System.Collections.Generic.List<string>(ReadSlice(ReadString)),
				ItemSettings = new System.Collections.Generic.List<CameraAimAssistItemSettings>(ReadSlice(ReadCameraAimAssistItemSettings))
			};

			if (ReadBool()) value.DefaultItemSettings = new Optional<string>(ReadString());
			if (ReadBool()) value.HandSettings = new Optional<string>(ReadString());

			return value;
		}
		public void Write(CameraAimAssistPreset value)
		{
			Write(value.Identifier);

			if (value.BlockExclusions != null)
			{
				WriteSlice(value.BlockExclusions.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.EntityExclusions != null)
			{
				WriteSlice(value.EntityExclusions.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.BlockTagExclusions != null)
			{
				WriteSlice(value.BlockTagExclusions.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.EntityTypeFamilyExclusions != null)
			{
				WriteSlice(value.EntityTypeFamilyExclusions.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.LiquidTargets != null)
			{
				WriteSlice(value.LiquidTargets.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.ItemSettings != null)
			{
				WriteSlice(value.ItemSettings.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			Write(value.DefaultItemSettings.HasValue);
			if (value.DefaultItemSettings.HasValue)
			{
				Write(value.DefaultItemSettings.Value);
			}

			Write(value.HandSettings.HasValue);
			if (value.HandSettings.HasValue)
			{
				Write(value.HandSettings.Value);
			}
		}

		public CameraAimAssistItemSettings ReadCameraAimAssistItemSettings()
		{
			return new CameraAimAssistItemSettings
			{
				Item = ReadString(),
				Category = ReadString()
			};
		}

		public CameraRotationOption ReadCameraRotationOption()
		{
			var value = new CameraRotationOption
			{
				Value = ReadVector3(),
				Time = ReadFloat()
			};

			string easingType = ReadString();
			value.EaseType = EasingType.EasingTypeFromString(easingType);

			return value;
		}

		public CameraProgressOption ReadCameraProgressOption()
		{
			var value = new CameraProgressOption
			{
				Value = ReadFloat(),
				Time = ReadFloat()
			};

			string easingType = ReadString();
			value.EaseType = EasingType.EasingTypeFromString(easingType);

			return value;
		}

		public CameraSplineInstruction ReadCameraSplineInstruction()
		{
			var value = new CameraSplineInstruction
			{
				TotalTime = ReadFloat()
			};

			value.SplineType = ReadByte();

			value.Curve = new System.Collections.Generic.List<System.Numerics.Vector3>(ReadSlice(ReadVector3));
			value.ProgressKeyFrames = new System.Collections.Generic.List<CameraProgressOption>(ReadSlice(ReadCameraProgressOption));
			value.RotationOptions = new System.Collections.Generic.List<CameraRotationOption>(ReadSlice(ReadCameraRotationOption));

			if (ReadBool()) value.SplineIdentifier = new Optional<string>(ReadString());
			if (ReadBool()) value.LoadFromJson = new Optional<bool>(ReadBool());

			return value;
		}

		public CameraSplineDefinition ReadCameraSplineDefinition()
		{
			var value = new CameraSplineDefinition
			{
				Name = ReadString(),
				TotalTime = ReadFloat()
			};

			value.SplineType = ReadString();

			value.ControlPoints = new System.Collections.Generic.List<System.Numerics.Vector3>(ReadSlice(ReadVector3));
			value.ProgressKeyFrames = new System.Collections.Generic.List<CameraProgressOption>(ReadSlice(ReadCameraProgressOption));
			value.RotationKeyFrames = new System.Collections.Generic.List<CameraRotationOption>(ReadSlice(ReadCameraRotationOption));

			return value;
		}

		public CameraAimAssistActorPriorityData ReadCameraAimAssistActorPriorityData()
		{
			return new CameraAimAssistActorPriorityData
			{
				PresetIndex = ReadInt(),
				CategoryIndex = ReadInt(),
				ActorIndex = ReadInt(),
				Priority = ReadInt()
			};
		}
		public void Write(CameraEase value)
		{
			Write((byte)value.Type);
			Write(value.Duration);
		}

		public void Write(CameraInstruction value)
		{
			Write(value.Set.HasValue);
			if (value.Set.HasValue) Write(value.Set.Value);

			Write(value.Clear.HasValue);
			if (value.Clear.HasValue) Write(value.Clear.Value);

			Write(value.Fade.HasValue);
			if (value.Fade.HasValue) Write(value.Fade.Value);

			Write(value.Target.HasValue);
			if (value.Target.HasValue) Write(value.Target.Value);

			Write(value.RemoveTarget.HasValue);
			if (value.RemoveTarget.HasValue) Write(value.RemoveTarget.Value);

			Write(value.FieldOfView.HasValue);
			if (value.FieldOfView.HasValue) Write(value.FieldOfView.Value);

			Write(value.Spline.HasValue);
			if (value.Spline.HasValue) Write(value.Spline.Value);

			Write(value.Attach.HasValue);
			if (value.Attach.HasValue) Write(value.Attach.Value);

			Write(value.DetachFromEntity.HasValue);
			if (value.DetachFromEntity.HasValue) Write(value.DetachFromEntity.Value);
		}

		public void Write(CameraInstructionSet value)
		{
			Write(value.Preset);

			Write(value.Ease.HasValue);
			if (value.Ease.HasValue)
			{
				Write(value.Ease.Value);
			}

			Write(value.Position.HasValue);
			if (value.Position.HasValue)
			{
				Write(value.Position.Value);
			}

			Write(value.Rotation.HasValue);
			if (value.Rotation.HasValue)
			{
				Write(value.Rotation.Value);
			}

			Write(value.Facing.HasValue);
			if (value.Facing.HasValue)
			{
				Write(value.Facing.Value);
			}

			Write(value.ViewOffset.HasValue);
			if (value.ViewOffset.HasValue)
			{
				Write(value.ViewOffset.Value);
			}

			Write(value.EntityOffset.HasValue);
			if (value.EntityOffset.HasValue)
			{
				Write(value.EntityOffset.Value);
			}

			Write(value.Default.HasValue);
			if (value.Default.HasValue)
			{
				Write(value.Default.Value);
			}

			Write(value.RemoveIgnoreStartingValuesComponent);
		}

		public void Write(CameraFadeTimeData value)
		{
			Write(value.FadeInDuration);
			Write(value.WaitDuration);
			Write(value.FadeOutDuration);
		}

		public void Write(CameraInstructionFade value)
		{
			Write(value.TimeData.HasValue);
			if (value.TimeData.HasValue)
			{
				Write(value.TimeData.Value);
			}

			Write(value.Colour.HasValue);
			if (value.Colour.HasValue)
			{
				WriteRGB(value.Colour.Value);
			}
		}

		public void Write(CameraInstructionTarget value)
		{
			Write(value.CenterOffset.HasValue);
			if (value.CenterOffset.HasValue)
			{
				Write(value.CenterOffset.Value);
			}

			Write(value.EntityUniqueID);
		}

		public void Write(CameraInstructionFieldOfView value)
		{
			string easingType = EasingType.EasingTypeToString((int)value.EaseType);
			Write(value.FieldOfView);
			Write(value.EaseTime);
			Write(easingType);
			Write(value.Clear);
		}

		public void Write(CameraAttachToEntityInstruction value)
		{
			Write(value.ActorUniqueId);
		}

		public void Write(CameraPreset value)
		{
			Write(value.Name);
			Write(value.Parent);

			Write(value.PosX.HasValue);
			if (value.PosX.HasValue) Write(value.PosX.Value);

			Write(value.PosY.HasValue);
			if (value.PosY.HasValue) Write(value.PosY.Value);

			Write(value.PosZ.HasValue);
			if (value.PosZ.HasValue) Write(value.PosZ.Value);

			Write(value.RotX.HasValue);
			if (value.RotX.HasValue) Write(value.RotX.Value);

			Write(value.RotY.HasValue);
			if (value.RotY.HasValue) Write(value.RotY.Value);

			Write(value.RotationSpeed.HasValue);
			if (value.RotationSpeed.HasValue) Write(value.RotationSpeed.Value);

			Write(value.SnapToTarget.HasValue);
			if (value.SnapToTarget.HasValue) Write(value.SnapToTarget.Value);

			Write(value.HorizontalRotationLimit.HasValue);
			if (value.HorizontalRotationLimit.HasValue) Write(value.HorizontalRotationLimit.Value);

			Write(value.VerticalRotationLimit.HasValue);
			if (value.VerticalRotationLimit.HasValue) Write(value.VerticalRotationLimit.Value);

			Write(value.ContinueTargeting.HasValue);
			if (value.ContinueTargeting.HasValue) Write(value.ContinueTargeting.Value);

			Write(value.BlockListingRadius.HasValue);
			if (value.BlockListingRadius.HasValue) Write(value.BlockListingRadius.Value);

			Write(value.ViewOffset.HasValue);
			if (value.ViewOffset.HasValue) Write(value.ViewOffset.Value);

			Write(value.EntityOffset.HasValue);
			if (value.EntityOffset.HasValue) Write(value.EntityOffset.Value);

			Write(value.Radius.HasValue);
			if (value.Radius.HasValue) Write(value.Radius.Value);

			Write(value.MinYawLimit.HasValue);
			if (value.MinYawLimit.HasValue) Write(value.MinYawLimit.Value);

			Write(value.MaxYawLimit.HasValue);
			if (value.MaxYawLimit.HasValue) Write(value.MaxYawLimit.Value);

			Write(value.AudioListener.HasValue);
			if (value.AudioListener.HasValue) Write((byte)value.AudioListener.Value);

			Write(value.PlayerEffects.HasValue);
			if (value.PlayerEffects.HasValue) Write(value.PlayerEffects.Value);

			//Write(value.AlignTargetAndCameraForward.HasValue);
			//if (value.AlignTargetAndCameraForward.HasValue) Write(value.AlignTargetAndCameraForward.Value);

			Write(value.AimAssist.HasValue);
			if (value.AimAssist.HasValue) Write(value.AimAssist.Value);

			Write(value.ControlScheme.HasValue);
			if (value.ControlScheme.HasValue) Write(value.ControlScheme.Value);
		}

		public void Write(CameraPresetAimAssist value)
		{
			Write(value.Preset.HasValue);
			if (value.Preset.HasValue) Write(value.Preset.Value);

			Write(value.TargetMode.HasValue);
			if (value.TargetMode.HasValue) Write((byte)value.TargetMode.Value);

			Write(value.Angle.HasValue);
			if (value.Angle.HasValue) Write(value.Angle.Value);

			Write(value.Distance.HasValue);
			if (value.Distance.HasValue) Write(value.Distance.Value);
		}

		public void Write(CameraAimAssistCategory value)
		{
			Write(value.Name);
			Write(value.Priorities);
		}

		public void Write(CameraAimAssistCategoryDefinition value)
		{
			Write(value.Name);
			Write(value.Priorities);
		}

		public void Write(CameraAimAssistCategoriesDefinition value)
		{
			Write(value.Identifier ?? string.Empty);
			if (value.Category != null)
			{
				WriteSlice(value.Category.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		public void Write(CameraAimAssistPriorities value)
		{
			if (value.Entities != null)
			{
				WriteSlice(value.Entities.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.Blocks != null)
			{
				WriteSlice(value.Blocks.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.BlockTags != null)
			{
				WriteSlice(value.BlockTags.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.EntityTypeFamilies != null)
			{
				WriteSlice(value.EntityTypeFamilies.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			Write(value.EntityDefault.HasValue);
			if (value.EntityDefault.HasValue) Write(value.EntityDefault.Value);

			Write(value.BlockDefault.HasValue);
			if (value.BlockDefault.HasValue) Write(value.BlockDefault.Value);
		}

		public void Write(CameraAimAssistPriority value)
		{
			Write(value.Identifier);
			Write(value.Priority);
		}

		public void Write(CameraAimAssistItemSettings value)
		{
			Write(value.Item);
			Write(value.Category);
		}

		public void Write(CameraRotationOption value)
		{
			string easingType = EasingType.EasingTypeToString((int)value.EaseType);
			Write(value.Value);
			Write(value.Time);
			Write(easingType);
		}

		public void Write(CameraProgressOption value)
		{
			string easingType = EasingType.EasingTypeToString((int)value.EaseType);
			Write(value.Value);
			Write(value.Time);
			Write(easingType);
		}

		public void Write(CameraSplineInstruction value)
		{
			Write(value.TotalTime);

			Write(value.SplineType);

			if (value.Curve != null)
			{
				WriteSlice(value.Curve.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.ProgressKeyFrames != null)
			{
				WriteSlice(value.ProgressKeyFrames.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.RotationOptions != null)
			{
				WriteSlice(value.RotationOptions.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			Write(value.SplineIdentifier.HasValue);
			if (value.SplineIdentifier.HasValue)
			{
				Write(value.SplineIdentifier.Value);
			}

			Write(value.LoadFromJson.HasValue);
			if (value.LoadFromJson.HasValue)
			{
				Write(value.LoadFromJson.Value);
			}
		}

		public void Write(CameraSplineDefinition value)
		{
			Write(value.Name);
			Write(value.TotalTime);

			Write(value.SplineType ?? string.Empty);

			if (value.ControlPoints != null)
			{
				WriteSlice(value.ControlPoints.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.ProgressKeyFrames != null)
			{
				WriteSlice(value.ProgressKeyFrames.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}

			if (value.RotationKeyFrames != null)
			{
				WriteSlice(value.RotationKeyFrames.ToArray(), Write);
			}
			else
			{
				WriteUnsignedVarInt(0);
			}
		}

		public void Write(CameraAimAssistActorPriorityData value)
		{
			Write(value.PresetIndex);
			Write(value.CategoryIndex);
			Write(value.ActorIndex);
			Write(value.Priority);
		}
	}
}
