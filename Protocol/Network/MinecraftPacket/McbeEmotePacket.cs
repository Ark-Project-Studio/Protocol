namespace Protocol.Network.MinecraftPacket;
public class McbeEmotePacket : Packet
{
    public string emoteId;
    public byte flags;
    public string platformId;
    public ulong runtimeEntityId;
    public uint tick;
    public string xuid;
    public McbeEmotePacket()
    {
        Id = 0x8a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(emoteId);
        WriteUnsignedVarInt(tick);
        Write(xuid);
        Write(platformId);
        Write(flags);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        emoteId = ReadString();
        tick = ReadUnsignedVarInt();
        xuid = ReadString();
        platformId = ReadString();
        flags = ReadByte();
    }
}
