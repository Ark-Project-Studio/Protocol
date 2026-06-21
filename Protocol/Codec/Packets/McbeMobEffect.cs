using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum MobEffectEvent : byte
{
    Invalid = 0,
    Add = 1,
    Update = 2,
    Remove = 3
}

public class McbeMobEffect : Packet
{
    public int amplifier;
    public int duration;
    public int effectId;
    public MobEffectEvent eventId;
    public bool ambient;
    public bool particles;
    public ulong runtimeEntityId;
    public ulong tick;
    public McbeMobEffect()
    {
        Id = 0x1c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write((byte)eventId);
        WriteSignedVarInt(effectId);
        WriteSignedVarInt(amplifier);
        Write(particles);
        WriteSignedVarInt(duration);
        WriteUnsignedVarLong(tick);
        Write(ambient);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        eventId = (MobEffectEvent)ReadByte();
        effectId = ReadSignedVarInt();
        amplifier = ReadSignedVarInt();
        particles = ReadBool();
        duration = ReadSignedVarInt();
        tick = ReadUnsignedVarLong();
        ambient = ReadBool();
    }
}
