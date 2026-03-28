namespace Protocol.Network.MinecraftPacket;
public class McbeChunkRadiusUpdate : Packet
{
    public int chunkRadius;
    public McbeChunkRadiusUpdate()
    {
        Id = 0x46;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(chunkRadius);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        chunkRadius = ReadSignedVarInt();
    }
}
