using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeEditorNetwork : Packet
{
    public McbeEditorNetwork()
    {
        Id = 190;
        IsMcbe = true;
    }

    public bool RouteToManager { get; set; }
    public string RawVariantName { get; set; } = string.Empty;
    public string RawVariantData { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(RouteToManager);
        Write(RawVariantName);
        Write(RawVariantData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        RouteToManager = ReadBool();
        RawVariantName = ReadString();
        RawVariantData = ReadString();
    }
}
