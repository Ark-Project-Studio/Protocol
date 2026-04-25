using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeStructureTemplateDataResponse : Packet
{
    public McbeStructureTemplateDataResponse()
    {
        Id = 0x133;
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
