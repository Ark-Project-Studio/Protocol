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
    public Nbt Payload { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(RouteToManager);
        Write(Payload);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        RouteToManager = ReadBool();
        Payload = ReadNbt();
    }
}
