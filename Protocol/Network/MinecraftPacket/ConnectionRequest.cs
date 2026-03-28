namespace Protocol.Network.MinecraftPacket;
public class ConnectionRequest : Packet
{
    public long clientGuid;
    public byte doSecurity;
    public long timestamp;
    public ConnectionRequest()
    {
        Id = 0x09;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(clientGuid);
        Write(timestamp);
        Write(doSecurity);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        clientGuid = ReadLong();
        timestamp = ReadLong();
        doSecurity = ReadByte();
    }
}
