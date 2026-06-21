using Protocol.Minecraft.Inventory.Transaction;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeItemStackResponse : Packet
{
    public ItemStackResponse[] responses;
    public McbeItemStackResponse()
    {
        Id = 0x94;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(responses);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        responses = ReadItemStackResponses();
    }
}
