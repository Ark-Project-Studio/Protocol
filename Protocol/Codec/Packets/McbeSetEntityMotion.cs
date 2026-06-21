using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public class McbeSetEntityMotion : Packet
{
    public ulong runtimeEntityId;
    public ulong tick;
    public Vector3 velocity;
    public McbeSetEntityMotion()
    {
        Id = 0x28;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(velocity);
        WriteUnsignedVarLong(tick);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        velocity = ReadVector3();
        tick = ReadUnsignedVarLong();
    }
}
