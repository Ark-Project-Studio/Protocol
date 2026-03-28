namespace Protocol.Network.MinecraftPacket;
public class ConnectedPing : Packet
{
    public long sendpingtime;
    public ConnectedPing()
    {
        Id = 0x00;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(sendpingtime);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        sendpingtime = ReadLong();
    }
}
