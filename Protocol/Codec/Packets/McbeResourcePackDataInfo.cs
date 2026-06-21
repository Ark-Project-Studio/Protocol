using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeResourcePackDataInfo : Packet
{
    public uint chunkCount;
    public ulong compressedPackageSize;
    public byte[] hash;
    public bool isPremium;
    public uint maxChunkSize;
    public string packageId;
    public byte packType;
    public McbeResourcePackDataInfo()
    {
        Id = 0x52;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(packageId);
        Write(maxChunkSize);
        Write(chunkCount);
        Write(compressedPackageSize);
        WriteByteArray(hash);
        Write(isPremium);
        Write(packType);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        packageId = ReadString();
        maxChunkSize = ReadUint();
        chunkCount = ReadUint();
        compressedPackageSize = ReadUlong();
        hash = ReadByteArray();
        isPremium = ReadBool();
        packType = ReadByte();
    }
}
