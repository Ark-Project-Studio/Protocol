namespace Protocol.Network.MinecraftPacket;
public class McbeSetLastHurtBy : Packet
{
    public int lastHurtBy;
    public McbeSetLastHurtBy()
    {
        Id = 0x60;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteVarInt(lastHurtBy);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        lastHurtBy = ReadVarInt();
    }
}
