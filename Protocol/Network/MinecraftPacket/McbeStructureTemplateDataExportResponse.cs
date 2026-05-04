using Protocol.Minecraft;
using Protocol.Minecraft.NBT;

namespace Protocol.Network.MinecraftPacket;
public class McbeStructureTemplateDataExportResponse : Packet
{
    public McbeStructureTemplateDataExportResponse()
    {
        Id = 0x85;
        IsMcbe = true;
    }

    public string StructureName { get; set; } = string.Empty;
    public Nbt StructureNbt { get; set; } = new();
    public byte ResponseType { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(StructureName);
        Write(StructureNbt);
        Write(ResponseType);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        StructureName = ReadString();
        StructureNbt = ReadNbt();
        ResponseType = ReadByte();
    }
}
