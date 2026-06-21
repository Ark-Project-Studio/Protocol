using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeServerToClientHandshake : Packet
{
    public string token;
    public McbeServerToClientHandshake()
    {
        Id = 0x03;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(token);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        token = ReadString();
    }
}
