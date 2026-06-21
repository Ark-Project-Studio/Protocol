using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum SimpleEventSubtype : byte
{
    UninitializedSubtype = 0,
    EnableCommands = 1,
    DisableCommands = 2,
    UnlockWorldTemplateSettings = 3,
}

public class McbeSimpleEvent : Packet
{
    public SimpleEventSubtype eventType;
    public McbeSimpleEvent()
    {
        Id = 0x40;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)eventType);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        eventType = (SimpleEventSubtype)ReadByte();
    }
}
