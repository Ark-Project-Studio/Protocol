using System.Numerics;
using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeStructureTemplateDataExportRequest : Packet
{
    public McbeStructureTemplateDataExportRequest()
    {
        Id = 0x84;
        IsMcbe = true;
    }

    public string StructureName { get; set; } = string.Empty;
    public BlockCoordinates StructurePosition { get; set; }
    public StructureSettings Settings { get; set; } = new();
    public byte RequestedOperation { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(StructureName);
        Write(StructurePosition);
        Write(Settings);
        Write(RequestedOperation);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        StructureName = ReadString();
        StructurePosition = ReadBlockCoordinates();
        Settings = ReadStructureSettings();
        RequestedOperation = ReadByte();
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
