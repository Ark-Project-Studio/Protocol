using System.Drawing;
using System.Numerics;

namespace Protocol.Minecraft;

using System;
using System.Numerics;
// Enums
public enum AimAssistTargetMode
{
	Angle = 0,
	Distance = 1
}

public enum AudioListener
{
	Camera = 0,
	Player = 1
}

public static class SplineType
{
	public const string CatmullRom = "catmullrom";
	public const string Linear = "linear";
}

// Structures
public struct CameraEase
{
	public byte Type { get; set; }
	public float Duration { get; set; }
}

public struct CameraInstructionSet
{
	public uint Preset { get; set; }
	public Optional<CameraEase> Ease { get; set; }
	public Optional<System.Numerics.Vector3> Position { get; set; }
	public Optional<System.Numerics.Vector2> Rotation { get; set; }
	public Optional<System.Numerics.Vector3> Facing { get; set; }
	public Optional<System.Numerics.Vector2> ViewOffset { get; set; }
	public Optional<System.Numerics.Vector3> EntityOffset { get; set; }
	public Optional<bool> Default { get; set; }
	public bool IgnoreStartingValuesComponent { get; set; }
}

public struct CameraFadeTimeData
{
	public float FadeInDuration { get; set; }
	public float WaitDuration { get; set; }
	public float FadeOutDuration { get; set; }
}

public struct CameraInstructionFade
{
	public Optional<CameraFadeTimeData> TimeData { get; set; }
	public Optional<System.Drawing.Color> Colour { get; set; }
}

public struct CameraInstructionTarget
{
	public Optional<System.Numerics.Vector3> CenterOffset { get; set; }
	public long EntityUniqueID { get; set; }
}

public struct CameraInstructionFieldOfView
{
	public float FieldOfView { get; set; }
	public float EaseTime { get; set; }
	public int EaseType { get; set; }
	public bool Clear { get; set; }
}

public struct CameraPreset
{
	public string Name { get; set; }
	public string Parent { get; set; }
	public Optional<float> PosX { get; set; }
	public Optional<float> PosY { get; set; }
	public Optional<float> PosZ { get; set; }
	public Optional<float> RotX { get; set; }
	public Optional<float> RotY { get; set; }
	public Optional<float> RotationSpeed { get; set; }
	public Optional<bool> SnapToTarget { get; set; }
	public Optional<System.Numerics.Vector2> HorizontalRotationLimit { get; set; }
	public Optional<System.Numerics.Vector2> VerticalRotationLimit { get; set; }
	public Optional<bool> ContinueTargeting { get; set; }
	public Optional<float> TrackingRadius { get; set; }
	public Optional<System.Numerics.Vector2> ViewOffset { get; set; }
	public Optional<System.Numerics.Vector3> EntityOffset { get; set; }
	public Optional<float> Radius { get; set; }
	public Optional<float> MinYawLimit { get; set; }
	public Optional<float> MaxYawLimit { get; set; }
	public Optional<byte> AudioListener { get; set; }
	public Optional<bool> PlayerEffects { get; set; }
	public Optional<CameraPresetAimAssist> AimAssist { get; set; }
	public Optional<byte> ControlScheme { get; set; }
}

public struct CameraPresetAimAssist
{
	public Optional<string> Preset { get; set; }
	public Optional<int> TargetMode { get; set; }
	public Optional<System.Numerics.Vector2> Angle { get; set; }
	public Optional<float> Distance { get; set; }
}

public struct CameraAimAssistCategory
{
	public string Name { get; set; }
	public CameraAimAssistPriorities Priorities { get; set; }
}

public struct CameraAimAssistPriorities
{
	public System.Collections.Generic.List<CameraAimAssistPriority> Entities { get; set; }
	public System.Collections.Generic.List<CameraAimAssistPriority> Blocks { get; set; }
	public System.Collections.Generic.List<CameraAimAssistPriority> BlockTags { get; set; }
	public System.Collections.Generic.List<CameraAimAssistPriority> EntityTypeFamilies { get; set; }
	public Optional<int> EntityDefault { get; set; }
	public Optional<int> BlockDefault { get; set; }
}

public struct CameraAimAssistPriority
{
	public string Identifier { get; set; }
	public int Priority { get; set; }
}

public struct CameraAimAssistPreset
{
	public string Identifier { get; set; }
	public System.Collections.Generic.List<string> BlockExclusions { get; set; }
	public System.Collections.Generic.List<string> EntityExclusions { get; set; }
	public System.Collections.Generic.List<string> BlockTagExclusions { get; set; }
	public System.Collections.Generic.List<string> EntityTypeFamilyExclusions { get; set; }
	public System.Collections.Generic.List<string> LiquidTargets { get; set; }
	public System.Collections.Generic.List<CameraAimAssistItemSettings> ItemSettings { get; set; }
	public Optional<string> DefaultItemSettings { get; set; }
	public Optional<string> HandSettings { get; set; }
}

public struct CameraAimAssistItemSettings
{
	public string Item { get; set; }
	public string Category { get; set; }
}

public struct CameraRotationOption
{
	public System.Numerics.Vector3 Value { get; set; }
	public float Time { get; set; }
	public int EaseType { get; set; }
}

public struct CameraProgressOption
{
	public float Value { get; set; }
	public float Time { get; set; }
	public int EaseType { get; set; }
}

public struct CameraSplineInstruction
{
	public float TotalTime { get; set; }
	public Optional<byte> SplineType { get; set; }
	public System.Collections.Generic.List<System.Numerics.Vector3> Curve { get; set; }
	public System.Collections.Generic.List<CameraProgressOption> ProgressKeyFrames { get; set; }
	public System.Collections.Generic.List<CameraRotationOption> RotationOptions { get; set; }
	public Optional<string> SplineIdentifier { get; set; }
	public Optional<bool> LoadFromJson { get; set; }
}

public struct CameraSplineDefinition
{
	public string Name { get; set; }
	public float TotalTime { get; set; }
	public Optional<string> SplineType { get; set; }
	public System.Collections.Generic.List<System.Numerics.Vector3> ControlPoints { get; set; }
	public System.Collections.Generic.List<CameraProgressOption> ProgressKeyFrames { get; set; }
	public System.Collections.Generic.List<CameraRotationOption> RotationKeyFrames { get; set; }
}

public struct CameraAimAssistActorPriorityData
{
	public int PresetIndex { get; set; }
	public int CategoryIndex { get; set; }
	public int ActorIndex { get; set; }
	public int Priority { get; set; }
}