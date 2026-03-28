namespace Protocol.Network.MinecraftPacket;
public class McbeContainerClose : Packet
{
    public bool server;
    public byte windowId;
    public McbeContainerClose()
    {
        Id = 0x2f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(windowId);
        Write((byte)0);
        Write(server);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        windowId = ReadByte();
        ReadByte();
        server = ReadBool();
    }
}
