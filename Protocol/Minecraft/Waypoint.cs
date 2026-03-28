using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Utils;

namespace Protocol.Minecraft
{
	// Enums
	public enum WaypointAction : byte
	{
		None = 0,
		Add = 1,
		Remove = 2,
		Update = 3
	}

	public enum WaypointUpdateFlag
	{
		Visible = 1 << 0,
		Position = 1 << 1,
		TextureID = 1 << 2,
		Colour = 1 << 3,
		ClientPositionAuthority = 1 << 4,
		ActorUniqueID = 1 << 5
	}

	public enum WaypointTexture : uint
	{
		Square = 2,
		Circle = 3,
		SmallSquare = 4,
		SmallStar = 5
	}

	// Structures
	public struct WaypointWorldPosition
	{
		public System.Numerics.Vector3 Position { get; set; }
		public int DimensionID { get; set; }
	}

	public struct Waypoint
	{
		public uint UpdateFlag { get; set; }
		public Optional<bool> Visible { get; set; }
		public Optional<WaypointWorldPosition> WorldPosition { get; set; }
		public Optional<uint> TextureID { get; set; }
		public Optional<int> Colour { get; set; }
		public Optional<bool> ClientPositionAuthority { get; set; }
		public Optional<long> ActorUniqueID { get; set; }
	}

	public struct LocatorBarWaypoint
	{
		public UUID GroupHandle { get; set; }
		public Waypoint Waypoint { get; set; }
		public byte Action { get; set; }
	}
}
