namespace Protocol.Network.MinecraftPacket;
public class McbeSetLastHurtBy : Packet
{
    public int unknown;
    public McbeSetLastHurtBy()
    {
        Id = 0x60;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteVarInt(unknown);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        unknown = ReadVarInt();
    }
}
