namespace Protocol.Network.MinecraftPacket;
public class McbeSetLocalPlayerAsInitialized : Packet
{
    public ulong runtimeEntityId;
    public McbeSetLocalPlayerAsInitialized()
    {
        Id = 0x71;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
    }
}
