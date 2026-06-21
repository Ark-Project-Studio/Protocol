using Protocol.Minecraft;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeEmoteList : Packet
{
    public EmoteIds emoteIds;
    public ulong runtimeEntityId;
    public McbeEmoteList()
    {
        Id = 0x98;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(emoteIds);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        emoteIds = ReadEmoteId();
    }
}
