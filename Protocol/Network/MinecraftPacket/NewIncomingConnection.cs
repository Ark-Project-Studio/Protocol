using System.Net;

namespace Protocol.Network.MinecraftPacket;
public class NewIncomingConnection : Packet
{
    public IPEndPoint clientendpoint;
    public long incomingTimestamp;
    public long serverTimestamp;
    public IPEndPoint[] systemAddresses;
    public NewIncomingConnection()
    {
        Id = 0x13;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(clientendpoint);
        Write(systemAddresses);
        Write(incomingTimestamp);
        Write(serverTimestamp);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        clientendpoint = ReadIPEndPoint();
        systemAddresses = ReadIPEndPoints(20);
        incomingTimestamp = ReadLong();
        serverTimestamp = ReadLong();
    }
}
