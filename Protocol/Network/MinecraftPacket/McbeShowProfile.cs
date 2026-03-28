namespace Protocol.Network.MinecraftPacket;
public class McbeShowProfile : Packet
{
    public string xuid;
    public McbeShowProfile()
    {
        Id = 0x68;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(xuid);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        xuid = ReadString();
    }
}
