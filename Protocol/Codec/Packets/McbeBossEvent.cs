using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeBossEvent : Packet
{
    public enum EventType : uint
    {
        Add = 0,
        PlayerAdded = 1,
        Remove = 2,
        PlayerRemoved = 3,
        UpdatePercent = 4,
        UpdateName = 5,
        UpdateProperties = 6,
        UpdateStyle = 7,
        Query = 8
    }

    public long bossEntityId;
    public uint color = 0xff00ff00;
    public EventType eventType;
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
        WriteUnsignedVarInt((uint)eventType);
        switch (eventType)
        {
            case EventType.PlayerAdded:
            case EventType.PlayerRemoved:
                WriteSignedVarLong(playerId);
                break;
            case EventType.UpdatePercent:
                Write(healthPercent);
                break;
            case EventType.UpdateName:
                Write(title);
                break;
            case EventType.Add:
                Write(title);
                Write(filteredTitle);
                Write(healthPercent);
                Write(darkenScreen);
                WriteUnsignedVarInt(color);
                WriteUnsignedVarInt(overlay);
                break;
            case EventType.UpdateProperties:
                Write(darkenScreen);
                WriteUnsignedVarInt(color);
                WriteUnsignedVarInt(overlay);
                break;
            case EventType.UpdateStyle:
                WriteUnsignedVarInt(color);
                WriteUnsignedVarInt(overlay);
                break;
            case EventType.Query:
                WriteSignedVarLong(playerId);
                break;
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        bossEntityId = ReadSignedVarLong();
        eventType = (EventType)ReadUnsignedVarInt();
        switch (eventType)
        {
            case EventType.PlayerAdded:
            case EventType.PlayerRemoved:
                playerId = ReadSignedVarLong();
                break;
            case EventType.UpdatePercent:
                healthPercent = ReadFloat();
                break;
            case EventType.UpdateName:
                title = ReadString();
                break;
            case EventType.Add:
                title = ReadString();
                filteredTitle = ReadString();
                healthPercent = ReadFloat();
                darkenScreen = ReadUshort();
                color = ReadUnsignedVarInt();
                overlay = ReadUnsignedVarInt();
                break;
            case EventType.UpdateProperties:
                darkenScreen = ReadUshort();
                color = ReadUnsignedVarInt();
                overlay = ReadUnsignedVarInt();
                break;
            case EventType.UpdateStyle:
                color = ReadUnsignedVarInt();
                overlay = ReadUnsignedVarInt();
                break;
            case EventType.Query:
                playerId = ReadSignedVarLong();
                break;
        }
    }
}
