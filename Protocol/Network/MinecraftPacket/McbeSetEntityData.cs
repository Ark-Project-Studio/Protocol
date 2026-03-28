using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeSetEntityData : Packet
{
    public MetadataDictionary metadata;
    public ulong runtimeEntityId;
    public PropertySyncData syncdata;
    public ulong tick;
    public McbeSetEntityData()
    {
        Id = 0x27;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(metadata);
        Write(syncdata);
        WriteUnsignedVarLong(tick);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        metadata = ReadMetadataDictionary();
        syncdata = ReadPropertySyncData();
        tick = ReadUnsignedVarLong();
    }
}
