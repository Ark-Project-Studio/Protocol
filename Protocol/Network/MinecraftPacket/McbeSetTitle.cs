namespace Protocol.Network.MinecraftPacket;
public class McbeSetTitle : Packet
{
    public int fadeInTime;
    public int fadeOutTime;
    public string filteredString;
    public string platformOnlineId;
    public int stayTime;
    public string text;
    public int type;
    public string xuid;
    public McbeSetTitle()
    {
        Id = 0x58;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(type);
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
        type = ReadSignedVarInt();
        text = ReadString();
        fadeInTime = ReadSignedVarInt();
        stayTime = ReadSignedVarInt();
        fadeOutTime = ReadSignedVarInt();
        xuid = ReadString();
        platformOnlineId = ReadString();
        filteredString = ReadString();
    }
}
