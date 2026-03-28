namespace Protocol.Network.MinecraftPacket;
public class McbeScriptCustomEvent : Packet
{
    public string eventData;
    public string eventName;
    public McbeScriptCustomEvent()
    {
        Id = 0x75;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(eventName);
        Write(eventData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        eventName = ReadString();
        eventData = ReadString();
    }
}
