using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeServerStats : Packet
{
    public McbeServerStats()
    {
        Id = 192;
        IsMcbe = true;
    }

    public float ServerTime { get; set; }
    public float NetworkTime { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(ServerTime);
        Write(NetworkTime);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ServerTime = ReadFloat();
        NetworkTime = ReadFloat();
    }
}
