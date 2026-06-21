using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum Difficulty : uint
{
    Peaceful = 0,
    Easy = 1,
    Normal = 2,
    Hard = 3,
    Count = 4,
    Unknown = 5
}

public class McbeSetDifficulty : Packet
{
    public Difficulty difficulty;
    public McbeSetDifficulty()
    {
        Id = 0x3c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt((uint)difficulty);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        difficulty = (Difficulty)ReadUnsignedVarInt();
    }
}
