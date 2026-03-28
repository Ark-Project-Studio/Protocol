namespace Protocol.Network.MinecraftPacket;
public class McbeStructureTemplateDataExportResponse : Packet
{
    public McbeStructureTemplateDataExportResponse()
    {
        Id = 0x85;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
    }
}
