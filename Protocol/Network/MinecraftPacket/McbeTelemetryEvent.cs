namespace Protocol.Network.MinecraftPacket;
public class McbeTelemetryEvent : Packet
{
    public byte[] auxData;
    public int eventData;
    public byte eventType;
    public ulong runtimeEntityId;
    public McbeTelemetryEvent()
    {
        Id = 0x41;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        WriteSignedVarInt(eventData);
        Write(eventType);
        Write(auxData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        eventData = ReadSignedVarInt();
        eventType = ReadByte();
        auxData = ReadBytes(0, true);
    }
}
