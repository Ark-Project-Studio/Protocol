using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbePartyDestinationCookieResponse : Packet
{
    public string Cookie { get; set; } = string.Empty;
    public bool Accepted { get; set; }

    public McbePartyDestinationCookieResponse()
    {
        Id = 350;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Cookie);
        Write(Accepted);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Cookie = ReadString();
        Accepted = ReadBool();
    }
}
