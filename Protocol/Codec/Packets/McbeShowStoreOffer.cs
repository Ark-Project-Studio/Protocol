using Protocol.Network;
using Protocol.Utils;

namespace Protocol.Codec.Packets;
public class McbeShowStoreOffer : Packet
{
    public UUID OfferID;
    public byte type;
    public McbeShowStoreOffer()
    {
        Id = 0x5b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(OfferID);
        Write(type);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        OfferID = ReadUUID();
        type = ReadByte();
    }
}
