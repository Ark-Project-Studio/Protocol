using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public class McbePlayerInput : Packet
{
    public Vector2 move;
    public bool jumping;
    public bool sneaking;

    public McbePlayerInput()
    {
        Id = 0x39;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(move);
        Write(jumping);
        Write(sneaking);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        move = ReadVector2();
        jumping = ReadBool();
        sneaking = ReadBool();
    }
}
