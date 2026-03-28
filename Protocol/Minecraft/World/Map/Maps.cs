using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft.World.Map
{
	// Enums
	public enum MapDecorationType : byte
	{
		MarkerWhite = 0,
		MarkerGreen = 1,
		MarkerRed = 2,
		MarkerBlue = 3,
		CrossWhite = 4,
		TriangleRed = 5,
		SquareWhite = 6,
		MarkerSign = 7,
		MarkerPink = 8,
		MarkerOrange = 9,
		MarkerYellow = 10,
		MarkerTeal = 11,
		TriangleGreen = 12,
		SmallSquareWhite = 13,
		Mansion = 14,
		Monument = 15,
		NoDraw = 16,
		VillageDesert = 17,
		VillagePlains = 18,
		VillageSavanna = 19,
		VillageSnowy = 20,
		VillageTaiga = 21,
		JungleTemple = 22,
		WitchHut = 23
	}

	public enum MapObjectType : int
	{
		Entity = 0,
		Block = 1
	}

	// Structures
	public struct MapTrackedObject
	{
		public int Type { get; set; }
		public long EntityUniqueID { get; set; }
		public BlockCoordinates BlockPosition { get; set; }
	}

	public struct MapDecoration
	{
		public byte Type { get; set; }
		public byte Rotation { get; set; }
		public byte X { get; set; }
		public byte Y { get; set; }
		public string Label { get; set; }
		public System.Drawing.Color Colour { get; set; }
	}

	public struct PixelRequest
	{
		public System.Drawing.Color Colour { get; set; }
		public ushort Index { get; set; }
	}
}
