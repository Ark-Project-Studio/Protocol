namespace Protocol.Network.MinecraftPacket;
public class McbeStopSound : Packet
{
    public string name;
    public bool stopAll;
    public McbeStopSound()
    {
        Id = 0x57;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(name);
        Write(stopAll);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        name = ReadString();
        stopAll = ReadBool();
    }
}
