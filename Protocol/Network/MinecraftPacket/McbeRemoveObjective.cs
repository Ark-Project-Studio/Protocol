namespace Protocol.Network.MinecraftPacket;
public class McbeRemoveObjective : Packet
{
    public string objectiveName;
    public McbeRemoveObjective()
    {
        Id = 0x6a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(objectiveName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        objectiveName = ReadString();
    }
}
