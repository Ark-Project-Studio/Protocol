using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum MovementEffectType
{
    Invalid = -1,
    GlideBoost = 0,
    DolphinBoost = 1,
    Count = 2
}

public class McbeMovementEffect : Packet
{
    public McbeMovementEffect()
    {
        Id = 318;
        IsMcbe = true;
    }

    public ulong EntityRuntimeID { get; set; }
    public MovementEffectType Type { get; set; }
    public int Duration { get; set; }
    public ulong Tick { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(EntityRuntimeID);
        WriteSignedVarInt((int)Type);
        WriteSignedVarInt(Duration);
        WriteUnsignedVarLong(Tick);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        EntityRuntimeID = ReadUnsignedVarLong();
        Type = (MovementEffectType)ReadSignedVarInt();
        Duration = ReadSignedVarInt();
        Tick = ReadUnsignedVarLong();
    }
}
