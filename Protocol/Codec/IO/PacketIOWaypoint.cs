using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;

namespace Protocol.Network
{
	public partial class Packet
	{
		// Write methods
		public void Write(WaypointWorldPosition value)
		{
			Write(value.Position);
			WriteSignedVarInt(value.DimensionID);
		}

		public void Write(Waypoint value)
		{
			Write(value.UpdateFlag);

			Write(value.Visible.HasValue);
			if (value.Visible.HasValue)
			{
				Write(value.Visible.Value);
			}

			Write(value.WorldPosition.HasValue);
			if (value.WorldPosition.HasValue)
			{
				Write(value.WorldPosition.Value);
			}

			Write(value.TextureID.HasValue);
			if (value.TextureID.HasValue)
			{
				Write(value.TextureID.Value);
			}

			Write(value.Colour.HasValue);
			if (value.Colour.HasValue)
			{
				Write(value.Colour.Value);
			}

			Write(value.ClientPositionAuthority.HasValue);
			if (value.ClientPositionAuthority.HasValue)
			{
				Write(value.ClientPositionAuthority.Value);
			}

			Write(value.ActorUniqueID.HasValue);
			if (value.ActorUniqueID.HasValue)
			{
				WriteSignedVarLong(value.ActorUniqueID.Value);
			}
		}

		public void Write(LocatorBarWaypoint value)
		{
			Write(value.GroupHandle);
			Write(value.Waypoint);
			Write(value.Action);
		}
		// Read methods
		public WaypointWorldPosition ReadWaypointWorldPosition()
		{
			return new WaypointWorldPosition
			{
				Position = ReadVector3(),
				DimensionID = ReadSignedVarInt()
			};
		}

		public Waypoint ReadWaypoint()
		{
			var value = new Waypoint
			{
				UpdateFlag = ReadUint()
			};

			if (ReadBool())
			{
				value.Visible = new Optional<bool>(ReadBool());
			}

			if (ReadBool())
			{
				value.WorldPosition = new Optional<WaypointWorldPosition>(ReadWaypointWorldPosition());
			}

			if (ReadBool())
			{
				value.TextureID = new Optional<uint>(ReadUint());
			}

			if (ReadBool())
			{
				value.Colour = new Optional<int>(ReadInt());
			}

			if (ReadBool())
			{
				value.ClientPositionAuthority = new Optional<bool>(ReadBool());
			}

			if (ReadBool())
			{
				value.ActorUniqueID = new Optional<long>(ReadSignedVarLong());
			}

			return value;
		}

		public LocatorBarWaypoint ReadLocatorBarWaypoint()
		{
			return new LocatorBarWaypoint
			{
				GroupHandle = ReadUUID(),
				Waypoint = ReadWaypoint(),
				Action = ReadByte()
			};
		}
	}
}
