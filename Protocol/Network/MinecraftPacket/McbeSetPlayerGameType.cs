namespace Protocol.Network.MinecraftPacket;
public class McbeSetPlayerGameType : Packet
{
    public int gamemode;
    public McbeSetPlayerGameType()
    {
        Id = 0x3e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(gamemode);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        gamemode = ReadSignedVarInt();
    }
}
