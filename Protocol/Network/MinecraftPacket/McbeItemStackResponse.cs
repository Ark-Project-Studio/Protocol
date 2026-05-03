using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
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
