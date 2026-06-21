using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeBossEvent : Packet
{
    public enum EventType : byte
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
    public long playerId;
    public EventType eventType;
    public string title;
    public string filteredTitle;
    public float healthPercent;
    public uint color;
    public uint overlay;
    public McbeBossEvent()
    {
        Id = 0x4a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarLong(bossEntityId);
        WriteSignedVarLong(playerId);
        WriteUnsignedVarInt((uint)eventType);
        Write(title);
        Write(filteredTitle);
        Write(healthPercent);
        WriteUnsignedVarInt(color);
        WriteUnsignedVarInt(overlay);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        bossEntityId = ReadSignedVarLong();
        playerId = ReadSignedVarLong();
        eventType = (EventType)ReadUnsignedVarInt();
        title = ReadString();
        filteredTitle = ReadString();
        healthPercent = ReadFloat();
        color = ReadUnsignedVarInt();
        overlay = ReadUnsignedVarInt();
    }
}
