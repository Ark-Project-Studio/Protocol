namespace Protocol.Network.MinecraftPacket;
public class McbeClientCacheStatus : Packet
{
    public bool enabled;
    public McbeClientCacheStatus()
    {
        Id = 0x81;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(enabled);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        enabled = ReadBool();
    }
}
