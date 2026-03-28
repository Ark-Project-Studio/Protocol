namespace Protocol.Network.MinecraftPacket;
public class McbeTransfer : Packet
{
    public ushort port;
    public bool reload;
    public string serverAddress;
    public McbeTransfer()
    {
        Id = 0x55;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(serverAddress);
        Write(port);
        Write(reload);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        serverAddress = ReadString();
        port = ReadUshort();
        reload = ReadBool();
    }
}
