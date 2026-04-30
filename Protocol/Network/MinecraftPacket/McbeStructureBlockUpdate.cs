using System.Numerics;
using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeStructureBlockUpdate : Packet
{
    public BlockCoordinates BlockPosition { get; set; }
    public StructureEditorData StructureData { get; set; } = new();
    public bool Trigger { get; set; }
    public bool IsWaterLogged { get; set; }

    public McbeStructureBlockUpdate()
    {
        Id = 0x5a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(BlockPosition);
        Write(StructureData);
        Write(Trigger);
        Write(IsWaterLogged);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        BlockPosition = ReadBlockCoordinates();
        StructureData = ReadStructureEditorData();
        Trigger = ReadBool();
        IsWaterLogged = ReadBool();
    }

    private void Write(StructureEditorData value)
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

    private StructureEditorData ReadStructureEditorData()
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

    private void Write(StructureSettings value)
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

    private StructureSettings ReadStructureSettings()
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
