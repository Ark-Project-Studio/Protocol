namespace Protocol.Network.MinecraftPacket;
public class McbeShowCredits : Packet
{
    public ulong runtimeEntityId;
    public int status;
    public McbeShowCredits()
    {
        Id = 0x4b;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        WriteSignedVarInt(status);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        status = ReadSignedVarInt();
    }
}
