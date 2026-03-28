namespace Protocol.Network.MinecraftPacket;
public class McbeResourcePackChunkData : Packet
{
    public uint chunkIndex;
    public string packageId;
    public byte[] payload;
    public ulong progress;
    public McbeResourcePackChunkData()
    {
        Id = 0x53;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(packageId);
        Write(chunkIndex);
        Write(progress);
        WriteByteArray(payload);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        packageId = ReadString();
        chunkIndex = ReadUint();
        progress = ReadUlong();
        payload = ReadByteArray();
    }
}
