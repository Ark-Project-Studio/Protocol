using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateBlockSynced : Packet
{
    public uint blockPriority;
    public uint blockRuntimeId;
    public BlockCoordinates coordinates;
    public uint dataLayerId;
    public ulong unknown0;
    public ulong unknown1;
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
        WriteUnsignedVarLong(unknown0);
        WriteUnsignedVarLong(unknown1);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        coordinates = ReadBlockCoordinates();
        blockRuntimeId = ReadUnsignedVarInt();
        blockPriority = ReadUnsignedVarInt();
        dataLayerId = ReadUnsignedVarInt();
        unknown0 = ReadUnsignedVarLong();
        unknown1 = ReadUnsignedVarLong();
    }
}
