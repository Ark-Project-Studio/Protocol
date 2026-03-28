namespace Protocol.Network.MinecraftPacket;
public class McbeRiderJump : Packet
{
    public int unknown;
    public McbeRiderJump()
    {
        Id = 0x14;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(unknown);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        unknown = ReadSignedVarInt();
    }
}
