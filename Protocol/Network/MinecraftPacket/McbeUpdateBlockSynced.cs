using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateBlockSynced : Packet
{
    public uint blockPriority;
    public uint blockRuntimeId;
    public BlockCoordinates coordinates;
    public uint dataLayerId;
    public ulong uniqueActorId;
    public ulong actorSyncMessage;
    public McbeUpdateBlockSynced()
    {
        Id = 0x6e;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(coordinates);
        WriteUnsignedVarInt(blockRuntimeId);
        WriteUnsignedVarInt(blockPriority);
        WriteUnsignedVarInt(dataLayerId);
        WriteUnsignedVarLong(uniqueActorId);
        WriteUnsignedVarLong(actorSyncMessage);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        coordinates = ReadBlockCoordinates();
        blockRuntimeId = ReadUnsignedVarInt();
        blockPriority = ReadUnsignedVarInt();
        dataLayerId = ReadUnsignedVarInt();
        uniqueActorId = ReadUnsignedVarLong();
        actorSyncMessage = ReadUnsignedVarLong();
    }
}
