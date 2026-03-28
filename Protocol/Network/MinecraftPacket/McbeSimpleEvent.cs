namespace Protocol.Network.MinecraftPacket;
public class McbeSimpleEvent : Packet
{
    public ushort eventType;
    public McbeSimpleEvent()
    {
        Id = 0x40;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(eventType);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        eventType = ReadUshort();
    }
}
