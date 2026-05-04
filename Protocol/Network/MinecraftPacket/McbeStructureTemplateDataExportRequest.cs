using System.Numerics;
using Protocol.Minecraft.Level.Block;

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

}
