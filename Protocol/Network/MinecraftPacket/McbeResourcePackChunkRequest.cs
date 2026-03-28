namespace Protocol.Network.MinecraftPacket;
public class McbeResourcePackChunkRequest : Packet
{
    public uint chunkIndex;
    public string packageId;
    public McbeResourcePackChunkRequest()
    {
        Id = 0x54;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(packageId);
        Write(chunkIndex);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        packageId = ReadString();
        chunkIndex = ReadUint();
    }
}
