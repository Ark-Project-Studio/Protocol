using Protocol.Network;

namespace Protocol.Codec.Packets;

public enum AgentAnimationType : byte
{
    ArmSwing = 0,
    Shrug = 1
}

public class McbeAgentAnimation : Packet
{
    public McbeAgentAnimation()
    {
        Id = 304;
        IsMcbe = true;
    }

    public AgentAnimationType AgentAnimation { get; set; }
    public ulong RuntimeId { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)AgentAnimation);
        WriteUnsignedVarLong(RuntimeId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        AgentAnimation = (AgentAnimationType)ReadByte();
        RuntimeId = ReadUnsignedVarLong();
    }
}
