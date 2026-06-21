using Protocol.Minecraft.Level.Block;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeOpenSign : Packet
{
    public BlockCoordinates coordinates;
    public bool front;
    public McbeOpenSign()
    {
        Id = 0x12f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(coordinates);
        Write(front);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        coordinates = ReadBlockCoordinates();
        front = ReadBool();
    }
}
