namespace Protocol.Network.MinecraftPacket;
public class McbeServerboundLoadingScreen : Packet
{
    public int? ScreenId;
    public int ScreenType;
    public McbeServerboundLoadingScreen()
    {
        Id = 0x138;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(ScreenType);
        Write(ScreenId.HasValue);
        if (ScreenId.HasValue)
            Write(ScreenId.Value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ScreenType = ReadSignedVarInt();
        if (ReadBool())
            ScreenId = ReadInt();
    }
}
