namespace Protocol.Network.RaknetPacket;
public class DisconnectionNotification : Packet
{
    public DisconnectionNotification()
    {
        Id = 0x15;
        IsMcbe = false;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
    }
}
