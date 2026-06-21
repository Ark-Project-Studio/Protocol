using Protocol.Network;

namespace Protocol.Codec.Packets;
    public enum PositionMode : byte
    {
        Normal = 0,
        Respawn = 1,
        Teleport = 2,
        OnlyHeadRot = 3
    }

    public enum Teleportcause
    {
        Unknown = 0,
        Projectile = 1,
        ChorusFruit = 2,
        Command = 3,
        Behavior = 4,
        Count = 5
    }

public class McbeMovePlayer : Packet
{

    public float headYaw;
    public PositionMode mode;
    public bool onGround;
    public ulong otherRuntimeEntityId;
    public float pitch;
    public ulong runtimeEntityId;
    public ulong tick;
    public float x;
    public float y;
    public float yaw;
    public float z;
    public McbeMovePlayer()
    {
        Id = 0x13;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(x);
        Write(y);
        Write(z);
        Write(pitch);
        Write(yaw);
        Write(headYaw);
        Write((byte)mode);
        Write(onGround);
        WriteUnsignedVarLong(otherRuntimeEntityId);
        if (mode == PositionMode.Teleport)
        {
            Write(0);
            Write(0);
        }

        WriteUnsignedVarLong(tick);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        x = ReadFloat();
        y = ReadFloat();
        z = ReadFloat();
        pitch = ReadFloat();
        yaw = ReadFloat();
        headYaw = ReadFloat();
        mode = (PositionMode)ReadByte();
        onGround = ReadBool();
        otherRuntimeEntityId = ReadUnsignedVarLong();
        if (mode == PositionMode.Teleport)
        {
            ReadInt();
            ReadInt();
        }

        tick = ReadUnsignedVarLong();
    }
}
