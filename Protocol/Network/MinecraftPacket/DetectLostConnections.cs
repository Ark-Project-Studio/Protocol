namespace Protocol.Network.MinecraftPacket;
public class DetectLostConnections : Packet
{
    public DetectLostConnections()
    {
        Id = 0x04;
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
