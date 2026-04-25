using System.Net;

namespace Protocol.Network.RaknetPacket;
public class ConnectionRequestAccepted : Packet
{
    public long incomingTimestamp;
    public long serverTimestamp;
    public IPEndPoint systemAddress;
    public IPEndPoint[] systemAddresses;
    public short systemIndex;
    public ConnectionRequestAccepted()
    {
        Id = 0x10;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(systemAddress);
        WriteBe(systemIndex);
        Write(systemAddresses);
        Write(incomingTimestamp);
        Write(serverTimestamp);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        systemAddress = ReadIPEndPoint();
        systemIndex = ReadShortBe();
        systemAddresses = ReadIPEndPoints(20);
        incomingTimestamp = ReadLong();
        serverTimestamp = ReadLong();
    }
}
