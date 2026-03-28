namespace Protocol.Network.MinecraftPacket;
public class McbeStructureBlockUpdate : Packet
{
    public McbeStructureBlockUpdate()
    {
        Id = 0x5a;
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
