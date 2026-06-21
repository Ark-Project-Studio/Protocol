using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum CreditsState : byte
{
    Start = 0,
    Finished = 1,
}

public class McbeShowCredits : Packet
{
    public ulong runtimeEntityId;
    public CreditsState status;
    public McbeShowCredits()
    {
        Id = 0x4b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write((byte)status);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        status = (CreditsState)ReadByte();
    }
}
