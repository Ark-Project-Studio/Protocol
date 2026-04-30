namespace Protocol.Network.MinecraftPacket;
public class McbeBossEvent : Packet
{
    public enum Type
    {
        AddBoss = 0,
        AddPlayer = 1,
        RemoveBoss = 2,
        RemovePlayer = 3,
        UpdateProgress = 4,
        UpdateName = 5,
        UpdateOptions = 6,
        UpdateStyle = 7,
        Query = 8
    }

    public long bossEntityId;
    public uint color = 0xff00ff00;
    public uint eventType;
    public string filteredTitle;
    public float healthPercent;
    public uint overlay = 0xff00ff00;
    public long playerId;
    public string title;
    public ushort darkenScreen;
    public McbeBossEvent()
    {
        Id = 0x4a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(bossEntityId);
        WriteUnsignedVarInt(eventType);
        switch ((Type)eventType)
        {
            case Type.AddPlayer:
            case Type.RemovePlayer:
                WriteSignedVarLong(playerId);
                break;
            case Type.UpdateProgress:
                Write(healthPercent);
                break;
            case Type.UpdateName:
                Write(title);
                break;
            case Type.AddBoss:
                Write(title);
                Write(filteredTitle);
                Write(healthPercent);
                Write(darkenScreen);
                WriteUnsignedVarInt(color);
                WriteUnsignedVarInt(overlay);
                break;
            case Type.UpdateOptions:
                Write(darkenScreen);
                WriteUnsignedVarInt(color);
                WriteUnsignedVarInt(overlay);
                break;
            case Type.UpdateStyle:
                WriteUnsignedVarInt(color);
                WriteUnsignedVarInt(overlay);
                break;
            case Type.Query:
                WriteSignedVarLong(playerId);
                break;
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        bossEntityId = ReadSignedVarLong();
        eventType = ReadUnsignedVarInt();
        switch ((Type)eventType)
        {
            case Type.AddPlayer:
            case Type.RemovePlayer:
                playerId = ReadSignedVarLong();
                break;
            case Type.UpdateProgress:
                healthPercent = ReadFloat();
                break;
            case Type.UpdateName:
                title = ReadString();
                break;
            case Type.AddBoss:
                title = ReadString();
                filteredTitle = ReadString();
                healthPercent = ReadFloat();
                darkenScreen = ReadUshort();
                color = ReadUnsignedVarInt();
                overlay = ReadUnsignedVarInt();
                break;
            case Type.UpdateOptions:
                darkenScreen = ReadUshort();
                color = ReadUnsignedVarInt();
                overlay = ReadUnsignedVarInt();
                break;
            case Type.UpdateStyle:
                color = ReadUnsignedVarInt();
                overlay = ReadUnsignedVarInt();
                break;
            case Type.Query:
                playerId = ReadSignedVarLong();
                break;
        }
    }
}
