namespace Protocol.Network.MinecraftPacket;
public class McbeMapCreateLockedCopy : Packet
{
    public long originalMapId;
    public long newMapId;

    public McbeMapCreateLockedCopy()
    {
        Id = 0x83;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(originalMapId);
        WriteSignedVarLong(newMapId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        originalMapId = ReadSignedVarLong();
        newMapId = ReadSignedVarLong();
    }
}
