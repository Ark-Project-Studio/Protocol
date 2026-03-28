namespace Protocol.Network.MinecraftPacket;
public class McbeRefreshEntitlements : Packet
{
    public McbeRefreshEntitlements()
    {
        Id = 305;
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
