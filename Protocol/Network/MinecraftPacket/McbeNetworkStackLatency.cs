namespace Protocol.Network.MinecraftPacket;
public class McbeNetworkStackLatency : Packet
{
    public ulong timestamp;
    public byte unknownFlag;
    public McbeNetworkStackLatency()
    {
        Id = 0x73;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(timestamp);
        Write(unknownFlag);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        timestamp = ReadUlong();
        unknownFlag = ReadByte();
    }
}
