using System;
using System.Numerics;
using Protocol.Minecraft.Actor.Player;
using Protocol.Minecraft.Level.Block;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(Vector2 vec)
		{
			Write((float)vec.X);
			Write((float)vec.Y);
		}

		public Vector2 ReadVector2()
		{
			return new Vector2(ReadFloat(), ReadFloat());
		}

		public void Write(Vector3 vec)
		{
			Write((float)vec.X);
			Write((float)vec.Y);
			Write((float)vec.Z);
		}

		public Vector3 ReadVector3()
		{
			return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
		}


		public void Write(BlockCoordinates coord)
		{
			WriteVarInt(coord.X);
			WriteVarInt(coord.Y);
			WriteVarInt(coord.Z);
		}

		public void WritePaintingCoordinates(BlockCoordinates coord)
		{
			Write((float)coord.X);
			Write((float)coord.Y);
			Write((float)coord.Z);
		}

		public BlockCoordinates ReadBlockCoordinates()
		{
			return new BlockCoordinates(ReadVarInt(), (int)ReadVarInt(), ReadVarInt());
		}

		public void Write(PlayerLocation location)
		{
			Write(location.X);
			Write(location.Y);
			Write(location.Z);
			var d = 256f / 360f;
			Write((byte)Math.Round(location.Pitch * d));
			Write((byte)Math.Round(location.HeadYaw * d));
			Write((byte)Math.Round(location.Yaw * d));
		}

		public PlayerLocation ReadPlayerLocation()
		{
			PlayerLocation location = new PlayerLocation();
			location.X = ReadFloat();
			location.Y = ReadFloat();
			location.Z = ReadFloat();
			location.Pitch = ReadByte() * 1f / 0.71f;
			location.HeadYaw = ReadByte() * 1f / 0.71f;
			location.Yaw = ReadByte() * 1f / 0.71f;

			return location;
		}
	}
}
