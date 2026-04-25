namespace Protocol.Network.RaknetPacket;
public class ConnectedPong : Packet
{
    public long sendpingtime;
    public long sendpongtime;
    public ConnectedPong()
    {
        Id = 0x03;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(sendpingtime);
        Write(sendpongtime);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        sendpingtime = ReadLong();
        sendpongtime = ReadLong();
    }
}
