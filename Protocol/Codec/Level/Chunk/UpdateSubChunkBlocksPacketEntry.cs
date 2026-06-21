using Protocol.Minecraft.Level.Block;

namespace Protocol.Minecraft.Level.Chunk
{
	public class UpdateSubChunkBlocksPacketEntry
	{
		public BlockCoordinates Coordinates { get; set; }
		public uint BlockRuntimeId { get; set; }
		public uint Flags { get; set; }
		public long SyncedUpdatedEntityUniqueId { get; set; }
		public uint SyncedUpdateType { get; set; }
	}
}