namespace Protocol.Network.MinecraftPacket;
public enum TitleType : int
{
    Clear = 0,
    Reset = 1,
    Title = 2,
    Subtitle = 3,
    Actionbar = 4,
    Times = 5,
    TitleTextObject = 6,
    SubtitleTextObject = 7,
    ActionbarTextObject = 8
}

public class McbeSetTitle : Packet
{
    public int fadeInTime;
    public int fadeOutTime;
    public string filteredString;
    public string platformOnlineId;
    public int stayTime;
    public string text;
    public TitleType type;
    public string xuid;
    public McbeSetTitle()
    {
        Id = 0x58;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt((int)type);
        Write(text);
        WriteSignedVarInt(fadeInTime);
        WriteSignedVarInt(stayTime);
        WriteSignedVarInt(fadeOutTime);
        Write(xuid);
        Write(platformOnlineId);
        Write(filteredString);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        type = (TitleType)ReadSignedVarInt();
        text = ReadString();
        fadeInTime = ReadSignedVarInt();
        stayTime = ReadSignedVarInt();
        fadeOutTime = ReadSignedVarInt();
        xuid = ReadString();
        platformOnlineId = ReadString();
        filteredString = ReadString();
    }
}
