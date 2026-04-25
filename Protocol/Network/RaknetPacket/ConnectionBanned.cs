namespace Protocol.Network.RaknetPacket;
public class ConnectionBanned : Packet
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
    public long serverGuid;
    public ConnectionBanned()
    {
        Id = 0x17;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(offlineMessageDataId);
        Write(serverGuid);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ReadBytes(offlineMessageDataId.Length);
        serverGuid = ReadLong();
    }
}
