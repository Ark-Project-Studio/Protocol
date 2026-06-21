using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeNetworkStackLatency : Packet
{
    public ulong timestamp;
    public bool fromServer;
    public McbeNetworkStackLatency()
    {
        Id = 0x73;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(timestamp);
        Write(fromServer);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        timestamp = ReadUlong();
        fromServer = ReadBool();
    }
}
