namespace Protocol.Network.MinecraftPacket;
public class McbeSetDifficulty : Packet
{
    public uint difficulty;
    public McbeSetDifficulty()
    {
        Id = 0x3c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(difficulty);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        difficulty = ReadUnsignedVarInt();
    }
}
