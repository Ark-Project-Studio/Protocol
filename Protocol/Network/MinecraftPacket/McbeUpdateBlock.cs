using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;

public enum Flags
{
	None = 0,
	Neighbors = 1,
	Network = 2,
	Nographic = 4,
	Priority = 8,
	All = Neighbors | Network,
	AllPriority = All | Priority
}
public class McbeUpdateBlock : Packet
{
   

    public uint blockPriority;
    public uint blockRuntimeId;
    public BlockCoordinates coordinates;
    public uint storage;
    public McbeUpdateBlock()
    {
        Id = 0x15;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(coordinates);
        WriteUnsignedVarInt(blockRuntimeId);
        WriteUnsignedVarInt(blockPriority);
        WriteUnsignedVarInt(storage);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        coordinates = ReadBlockCoordinates();
        blockRuntimeId = ReadUnsignedVarInt();
        blockPriority = ReadUnsignedVarInt();
        storage = ReadUnsignedVarInt();
    }
}
