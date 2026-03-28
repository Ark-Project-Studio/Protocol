namespace Protocol.Network.MinecraftPacket;
public class McbeMovePlayer : Packet
{
    public enum Mode
    {
        Normal = 0,
        Reset = 1,
        Teleport = 2,
        Rotation = 3
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

    public float headYaw;
    public byte mode;
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
        Write(mode);
        Write(onGround);
        WriteUnsignedVarLong(otherRuntimeEntityId);
        if (mode == 2)
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
        mode = ReadByte();
        onGround = ReadBool();
        otherRuntimeEntityId = ReadUnsignedVarLong();
        if (mode == 2)
        {
            ReadInt();
            ReadInt();
        }

        tick = ReadUnsignedVarLong();
    }
}
