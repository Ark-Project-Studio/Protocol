namespace Protocol.Network.RaknetPacket;
public class UnconnectedPong : Packet
{
    public readonly byte[] offlineMessageDataId = new byte[]
    {
        0x00,
        0xff,
        0xff,
        0x00,
        0xfe,
        0xfe,
        0xfe,
        0xfe,
        0xfd,
        0xfd,
        0xfd,
        0xfd,
        0x12,
        0x34,
        0x56,
        0x78
    };
    public long pingId;
    public long serverId;
    public string serverName;
    public UnconnectedPong()
    {
        Id = 0x1c;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(pingId);
        Write(serverId);
        Write(offlineMessageDataId);
        WriteFixedString(serverName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        pingId = ReadLong();
        serverId = ReadLong();
        ReadBytes(offlineMessageDataId.Length);
        serverName = ReadFixedString();
    }
}
