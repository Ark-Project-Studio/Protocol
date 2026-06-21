using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeToastRequest : Packet
{
    public McbeToastRequest()
    {
        Id = 186;
        IsMcbe = true;
    }

    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(Title);
        Write(Content);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Title = ReadString();
        Content = ReadString();
    }
}
