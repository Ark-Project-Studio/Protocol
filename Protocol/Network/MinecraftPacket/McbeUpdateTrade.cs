using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateTrade : Packet
{
    public string displayName;
    public bool useNewTradeScreen;
    public bool useEconomyTrade;
    public Nbt namedtag;
    public long playerEntityId;
    public long traderEntityId;
    public int size;
    public int tier;
    public byte windowId;
    public byte windowType;
    public McbeUpdateTrade()
    {
        Id = 0x50;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(windowId);
        Write(windowType);
        WriteVarInt(size);
        WriteVarInt(tier);
        WriteSignedVarLong(traderEntityId);
        WriteSignedVarLong(playerEntityId);
        Write(displayName);
        Write(useNewTradeScreen);
        Write(useEconomyTrade);
        Write(namedtag);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        windowId = ReadByte();
        windowType = ReadByte();
        size = ReadVarInt();
        tier = ReadVarInt();
        traderEntityId = ReadSignedVarLong();
        playerEntityId = ReadSignedVarLong();
        displayName = ReadString();
        useNewTradeScreen = ReadBool();
        useEconomyTrade = ReadBool();
        namedtag = ReadNbt();
    }
}
