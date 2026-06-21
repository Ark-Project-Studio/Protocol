using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeSendPartyDestinationCookie : Packet
{
    public enum PartyDestinationCookieIntent : byte
    {
        Notify = 0,
        OptIn = 1,
        OptOut = 2,
    }

    public string Cookie { get; set; } = string.Empty;
    public PartyDestinationCookieIntent Intent { get; set; }
    public string DestinationName { get; set; } = string.Empty;

    public McbeSendPartyDestinationCookie()
    {
        Id = 349;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Cookie);
        Write((byte)Intent);
        Write(DestinationName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Cookie = ReadString();
        Intent = (PartyDestinationCookieIntent)ReadByte();
        DestinationName = ReadString();
    }
}
