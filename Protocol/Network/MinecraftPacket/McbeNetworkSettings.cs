namespace Protocol.Network.MinecraftPacket;
public class McbeNetworkSettings : Packet
{
    public enum Compression
    {
        Nothing = 0,
        Everything = 1
    }

    public bool clientThrottleEnabled;
    public float clientThrottleScalar;
    public byte clientThrottleThreshold;
    public short compressionAlgorithm;
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
        Write(compressionAlgorithm);
        Write(clientThrottleEnabled);
        Write(clientThrottleThreshold);
        Write(clientThrottleScalar);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        compressionThreshold = ReadShort();
        compressionAlgorithm = ReadShort();
        clientThrottleEnabled = ReadBool();
        clientThrottleThreshold = ReadByte();
        clientThrottleScalar = ReadFloat();
    }
}
