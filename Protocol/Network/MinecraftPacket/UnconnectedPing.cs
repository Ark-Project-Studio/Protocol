namespace Protocol.Network.MinecraftPacket;
public class UnconnectedPing : Packet
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
    public long guid;
    public long pingId;
    public UnconnectedPing()
    {
        Id = 0x01;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(pingId);
        Write(offlineMessageDataId);
        Write(guid);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        pingId = ReadLong();
        ReadBytes(offlineMessageDataId.Length);
        guid = ReadLong();
    }
}
