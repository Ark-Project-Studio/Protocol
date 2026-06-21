using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeRequestChunkRadius : Packet
{
    public int chunkRadius;
    public byte maxRadius;
    public McbeRequestChunkRadius()
    {
        Id = 0x45;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(chunkRadius);
        Write(maxRadius);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        chunkRadius = ReadSignedVarInt();
        maxRadius = ReadByte();
    }
}
