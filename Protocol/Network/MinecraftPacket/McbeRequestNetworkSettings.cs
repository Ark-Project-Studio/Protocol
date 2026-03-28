namespace Protocol.Network.MinecraftPacket;
public class McbeRequestNetworkSettings : Packet
{
    public int protocolVersion;
    public McbeRequestNetworkSettings()
    {
        Id = 0xc1;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteBe(protocolVersion);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        protocolVersion = ReadIntBe();
    }
}
