using System.Numerics;

namespace Protocol.Network.MinecraftPacket;
public class McbeEntityEvent : Packet
{
    public int data;
    public byte eventId;
    public Optional<Vector3> FireAtPosition;
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
        Write(FireAtPosition.HasValue);
        if (FireAtPosition.HasValue)
        {
            Write(FireAtPosition.Value);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        eventId = ReadByte();
        data = ReadSignedVarInt();
        if (ReadBool())
        {
            FireAtPosition = new Optional<Vector3>(ReadVector3());
        }
    }
}
