using System.Numerics;

namespace Protocol.Network.MinecraftPacket;

public class McbeServerPlayerPostMovePosition : Packet
{
    public McbeServerPlayerPostMovePosition()
    {
        Id = 16;
        IsMcbe = true;
    }

    public Vector3 Pos { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Pos);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Pos = ReadVector3();
    }
}
