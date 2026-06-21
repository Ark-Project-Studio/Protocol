using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public class McbeCorrectPlayerMovement : Packet
{
    public bool OnGround;
    public Vector3 Postition;
    public ulong Tick;
    public byte Type;
    public Vector3 Velocity;
    public McbeCorrectPlayerMovement()
    {
        Id = 0xA1;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Type);
        Write(Postition);
        Write(Velocity);
        Write(OnGround);
        WriteUnsignedVarLong(Tick);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Type = ReadByte();
        Postition = ReadVector3();
        Velocity = ReadVector3();
        OnGround = ReadBool();
        Tick = ReadUnsignedVarLong();
    }
}
