using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;

namespace Protocol.Network
{
	public partial class Packet
	{
		
		public void Write(SyncWorldClockStateData value)
		{
			WriteUnsignedVarLong(value.ClockID);
			WriteSignedVarInt(value.Time);
			Write(value.Paused);
		}

		public SyncWorldClockStateData ReadSyncWorldClockStateData()
		{
			return new SyncWorldClockStateData
			{
				ClockID = (ulong)ReadUnsignedVarLong(),
				Time = ReadSignedVarInt(),
				Paused = ReadBool()
			};
		}

		public void Write(TimeMarkerData value)
		{
			WriteUnsignedVarLong(value.ID);
			Write(value.Name);
			WriteSignedVarInt(value.Time);

			Write(value.Period.HasValue);
			if (value.Period.HasValue)
			{
				Write(value.Period.Value);
			}
		}

		public TimeMarkerData ReadTimeMarkerData()
		{
			var data = new TimeMarkerData
			{
				ID = (ulong)ReadUnsignedVarLong(),
				Name = ReadString(),
				Time = ReadSignedVarInt()
			};

			if (ReadBool())
			{
				data.Period = new Optional<int>(ReadInt());
			}

			return data;
		}

		public void Write(WorldClockData value)
		{
			WriteUnsignedVarLong(value.ID);
			Write(value.Name);
			WriteSignedVarInt(value.Time);
			Write(value.Paused);

			if (value.TimeMarkers != null)
			{
				WriteSliceVarint32Length(value.TimeMarkers.ToArray(), Write);
			}
			else
			{
				WriteSignedVarInt(0);
			}
		}

		public WorldClockData ReadWorldClockData()
		{
			var data = new WorldClockData
			{
				ID = (ulong)ReadUnsignedVarLong(),
				Name = ReadString(),
				Time = ReadSignedVarInt(),
				Paused = ReadBool()
			};

			var markerArray = ReadSliceVarint32Length(ReadTimeMarkerData);
			data.TimeMarkers = new System.Collections.Generic.List<TimeMarkerData>(markerArray);

			return data;
		}
	}
}
