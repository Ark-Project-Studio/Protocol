namespace Protocol.Network.MinecraftPacket;
public class McbeCamera : Packet
{
    public long unknown1;
    public long unknown2;
    public McbeCamera()
    {
        Id = 0x49;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(unknown1);
        WriteSignedVarLong(unknown2);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        unknown1 = ReadSignedVarLong();
        unknown2 = ReadSignedVarLong();
    }
}
