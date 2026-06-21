using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public class McbeLevelSoundEvent : Packet
{
    public int blockId;
    public long entityId = -1;
    public string entityType;
    public Optional<Vector3> fireAtPosition;
    public bool isBabyMob;
    public bool isGlobal;
    public Vector3 position;
    public string SoundEvent;
    public McbeLevelSoundEvent()
    {
        Id = 0x7b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(SoundEvent);
        Write(position);
        WriteSignedVarInt(blockId);
        Write(entityType);
        Write(isBabyMob);
        Write(isGlobal);
        Write(entityId);
        Write(fireAtPosition.HasValue);
        if (fireAtPosition.HasValue)
        {
            Write(fireAtPosition.Value);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        SoundEvent = ReadString();
        position = ReadVector3();
        blockId = ReadSignedVarInt();
        entityType = ReadString();
        isBabyMob = ReadBool();
        isGlobal = ReadBool();
        entityId = ReadLong();
        if (ReadBool())
        {
            fireAtPosition = new Optional<Vector3>(ReadVector3());
        }
    }
}
