using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft.Level.Chunk;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(ChunkPos value)
		{
			WriteSignedVarInt(value.X);
			WriteSignedVarInt(value.Z);
		}

		public ChunkPos ReadChunkPos()
		{
			int x = ReadSignedVarInt();
			int z = ReadSignedVarInt();
			return new ChunkPos(x, z);
		}

		public void Write(SubChunkPos value)
		{
			WriteSignedVarInt(value.X);
			WriteSignedVarInt(value.Y);
			WriteSignedVarInt(value.Z);
		}

		public SubChunkPos ReadSubChunkPos()
		{
			int x = ReadSignedVarInt();
			int y = ReadSignedVarInt();
			int z = ReadSignedVarInt();
			return new SubChunkPos(x, y, z);
		}

		public void WriteSubChunkPosCereal(SubChunkPos value)
		{
			Write(value.X);
			Write(value.Y);
			Write(value.Z);
		}

		public SubChunkPos ReadSubChunkPosCereal()
		{
			int x = ReadInt();
			int y = ReadInt();
			int z = ReadInt();
			return new SubChunkPos(x, y, z);
		}
		// Write methods
		public void Write(SubChunkOffset value)
		{
			Write(value.X);
			Write(value.Y);
			Write(value.Z);
		}

		public void Write(SubChunkEntry value)
		{
			Write(value.Offset);
			Write(value.Result);

			if (value.Result != (byte)SubChunkResult.SuccessAllAir)
			{
				WriteByteArray(value.RawPayload);
			}

			Write(value.HeightMapType);
			if (value.HeightMapType == (byte)SubChunkPacketHeightMapType.HasData)
			{
				WriteSliceOfLen(value.HeightMapData, 256, Write);
			}

			Write(value.RenderHeightMapType);
			if (value.RenderHeightMapType == (byte)SubChunkPacketHeightMapType.HasData)
			{
				WriteSliceOfLen(value.RenderHeightMapData, 256, Write);
			}

			Write(value.BlobHash);
		}

		// Read methods
		public SubChunkOffset ReadSubChunkOffset()
		{
			return new SubChunkOffset
			{
				X = ReadSByte(),
				Y = ReadSByte(),
				Z = ReadSByte()
			};
		}

		public SubChunkEntry ReadSubChunkEntry()
		{
			var entry = new SubChunkEntry
			{
				Offset = ReadSubChunkOffset(),
				Result = ReadByte()
			};

			if (entry.Result != (byte)SubChunkResult.SuccessAllAir)
			{
				entry.RawPayload = ReadByteArray();
			}

			entry.HeightMapType = ReadByte();
			if (entry.HeightMapType == (byte)SubChunkPacketHeightMapType.HasData)
			{
				entry.HeightMapData = ReadSliceOfLen(256, ReadSByte);
			}

			entry.RenderHeightMapType = ReadByte();
			if (entry.RenderHeightMapType == (byte)SubChunkPacketHeightMapType.HasData)
			{
				entry.RenderHeightMapData = ReadSliceOfLen(256, ReadSByte);
			}

			entry.BlobHash = ReadUlong();

			return entry;
		}

		
		public void WriteSubChunkEntryNoCache(SubChunkEntry value)
		{
			Write(value.Offset);
			Write(value.Result);
			WriteByteArray(value.RawPayload);
			Write(value.HeightMapType);
			if (value.HeightMapType == (byte)SubChunkPacketHeightMapType.HasData)
			{
				WriteSliceOfLen(value.HeightMapData, 256, Write);
			}
			Write(value.RenderHeightMapType);
			if (value.RenderHeightMapType == (byte)SubChunkPacketHeightMapType.HasData)
			{
				WriteSliceOfLen(value.RenderHeightMapData, 256, Write);
			}
		}

		public SubChunkEntry ReadSubChunkEntryNoCache()
		{
			var entry = new SubChunkEntry
			{
				Offset = ReadSubChunkOffset(),
				Result = ReadByte(),
				RawPayload = ReadByteArray(true),
				HeightMapType = ReadByte()
			};

			if (entry.HeightMapType == (byte)SubChunkPacketHeightMapType.HasData)
			{
				entry.HeightMapData = ReadSliceOfLen(256, ReadSByte);
			}

			entry.RenderHeightMapType = ReadByte();
			if (entry.RenderHeightMapType == (byte)SubChunkPacketHeightMapType.HasData)
			{
				entry.RenderHeightMapData = ReadSliceOfLen(256, ReadSByte);
			}

			return entry;
		}
	}
}
