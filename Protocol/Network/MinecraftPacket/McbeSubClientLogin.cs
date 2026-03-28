namespace Protocol.Network.MinecraftPacket;
public class McbeSubClientLogin : Packet
{
    public McbeSubClientLogin()
    {
        Id = 0x5e;
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
