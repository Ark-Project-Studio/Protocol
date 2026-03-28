using Protocol.Minecraft.World.Chunk;

namespace Protocol.Network.MinecraftPacket;

public enum SubChunkRequestMode : uint
{
	Limitless = uint.MaxValue - 0,
	Limited = uint.MaxValue - 1
}

public class McbeLevelChunk : Packet
{
	public ChunkPos Position { get; set; }
	public int Dimension { get; set; }
	public ushort HighestSubChunk { get; set; }
	public uint SubChunkCount { get; set; }
	public bool CacheEnabled { get; set; }
	public System.Collections.Generic.List<ulong> BlobHashes { get; set; }
	public byte[] RawPayload { get; set; }
	public McbeLevelChunk()
    {
        Id = 0x3a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
		Write(Position);
		WriteVarInt(Dimension);
		WriteUnsignedVarInt(SubChunkCount);
		if (SubChunkCount == (uint)SubChunkRequestMode.Limited)
		{
			Write((ushort)HighestSubChunk);
		}
		Write(CacheEnabled);
		if (CacheEnabled)
		{
			WriteSlice(BlobHashes.ToArray(), Write);
		}
		WriteByteArray(RawPayload);
	}

    protected override void DecodePacket()
    {
        base.DecodePacket();
		Position = ReadChunkPos();
		Dimension = ReadVarInt();
		SubChunkCount = ReadUnsignedVarInt();
		if (SubChunkCount == (uint)SubChunkRequestMode.Limited)
		{
			HighestSubChunk = ReadUshort();
		}
		CacheEnabled = ReadBool();
		if (CacheEnabled)
		{
			BlobHashes = ReadSlice(() => ReadUlong()).ToList();
		}
		RawPayload = ReadByteArray(true);
	}
}
