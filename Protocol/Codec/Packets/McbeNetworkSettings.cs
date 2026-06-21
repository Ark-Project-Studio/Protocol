using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeNetworkSettings : Packet
{
    public enum CompressionAlgorithm : short
    {
        None = -1,
        Zlib = 0,
        Snappy = 1
    }

    public bool clientThrottleEnabled;
    public float clientThrottleScalar;
    public byte clientThrottleThreshold;
    public CompressionAlgorithm compressionAlgorithm;
    public short compressionThreshold;
    public McbeNetworkSettings()
    {
        Id = 0x8f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(compressionThreshold);
        Write((short)compressionAlgorithm);
        Write(clientThrottleEnabled);
        Write(clientThrottleThreshold);
        Write(clientThrottleScalar);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        compressionThreshold = ReadShort();
        compressionAlgorithm = (CompressionAlgorithm)ReadShort();
        clientThrottleEnabled = ReadBool();
        clientThrottleThreshold = ReadByte();
        clientThrottleScalar = ReadFloat();
    }
}
