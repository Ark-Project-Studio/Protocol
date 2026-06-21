using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeClientBoundCloseForm : Packet
{
    public McbeClientBoundCloseForm()
    {
        Id = 310;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
    }
}
