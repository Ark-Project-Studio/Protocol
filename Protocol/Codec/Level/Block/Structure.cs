using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Protocol.Minecraft.Level.Block
{
	public class StructureEditorData
	{
		public string StructureName { get; set; } = string.Empty;
		public string FilteredStructureName { get; set; } = string.Empty;
		public string DataField { get; set; } = string.Empty;
		public bool IncludePlayer { get; set; }
		public bool ShowBoundingBox { get; set; }
		public int StructureBlockType { get; set; }
		public StructureSettings StructureSettings { get; set; } = new();
		public int RedstoneSaveMode { get; set; }
	}

	public class StructureSettings
	{
		public string PaletteName { get; set; } = string.Empty;
		public bool IgnoreEntities { get; set; }
		public bool IgnoreBlocks { get; set; }
		public bool AllowNonTickingPlayerAndTickingAreaChunks { get; set; }
		public BlockCoordinates Size { get; set; }
		public BlockCoordinates Offset { get; set; }
		public long LastEditPlayer { get; set; }
		public byte Rotation { get; set; }
		public byte Mirror { get; set; }
		public byte AnimationMode { get; set; }
		public float AnimationSeconds { get; set; }
		public float IntegrityValue { get; set; }
		public uint IntegritySeed { get; set; }
		public Vector3 RotationPivot { get; set; }
	}
}
