namespace Protocol.Network.MinecraftPacket;
public class McbeInitiateWebSocketConnection : Packet
{
    public string server;
    public McbeInitiateWebSocketConnection()
    {
        Id = 0x5f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(server);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        server = ReadString();
    }
}
