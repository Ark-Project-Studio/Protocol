namespace Protocol.Network.MinecraftPacket;
public class McbeMapCreateLockedCopy : Packet
{
    public McbeMapCreateLockedCopy()
    {
        Id = 0x83;
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
