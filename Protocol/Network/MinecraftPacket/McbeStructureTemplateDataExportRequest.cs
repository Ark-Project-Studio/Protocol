namespace Protocol.Network.MinecraftPacket;
public class McbeStructureTemplateDataExportRequest : Packet
{
    public McbeStructureTemplateDataExportRequest()
    {
        Id = 0x84;
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
