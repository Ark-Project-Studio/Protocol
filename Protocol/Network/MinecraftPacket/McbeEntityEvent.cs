namespace Protocol.Network.MinecraftPacket;
public class McbeEntityEvent : Packet
{
    public int data;
    public byte eventId;
    public System.Numerics.Vector3 fireAtPosition;
    public ulong runtimeEntityId;
    public McbeEntityEvent()
    {
        Id = 0x1b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(eventId);
        WriteSignedVarInt(data);
        Write(fireAtPosition);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        eventId = ReadByte();
        data = ReadSignedVarInt();
        if (CanRead())
            fireAtPosition = ReadVector3();
    }
}
