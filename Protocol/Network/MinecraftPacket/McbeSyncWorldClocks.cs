using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket
{
	public enum ClockPayloadType : uint
	{
		SyncState = 0,
		InitializeRegistry = 1,
		AddTimeMarker = 2,
		RemoveTimeMarker = 3
	}
	public class McbeSyncWorldClocks : Packet
	{
		
		public ClockPayloadType PayloadType { get; set; }
		public System.Collections.Generic.List<SyncWorldClockStateData> SyncStates { get; set; }
		public System.Collections.Generic.List<WorldClockData> Clocks { get; set; }
		public ulong AddClockID { get; set; }
		public System.Collections.Generic.List<TimeMarkerData> AddTimeMarkers { get; set; }
		public ulong RemoveClockID { get; set; }
		public System.Collections.Generic.List<ulong> RemoveTimeMarkerIDs { get; set; }
		public McbeSyncWorldClocks()
		{
			Id = 344;
			IsMcbe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();
			WriteUnsignedVarInt((uint)PayloadType);

			switch (PayloadType)
			{
				case ClockPayloadType.SyncState:
					if (SyncStates != null)
					{
						WriteSlice(SyncStates.ToArray(), Write);
					}
					else
					{
						WriteSignedVarInt(0);
					}
					break;

				case ClockPayloadType.InitializeRegistry:
					if (Clocks != null)
					{
						WriteSlice(Clocks.ToArray(), Write);
					}
					else
					{
						WriteSignedVarInt(0);
					}
					break;

				case ClockPayloadType.AddTimeMarker:
					WriteUnsignedVarLong(AddClockID);
					if (AddTimeMarkers != null)
					{
						WriteSlice(AddTimeMarkers.ToArray(), Write);
					}
					else
					{
						WriteSignedVarInt(0);
					}
					break;

				case ClockPayloadType.RemoveTimeMarker:
					WriteUnsignedVarLong(RemoveClockID);
					if (RemoveTimeMarkerIDs != null)
					{
						WriteSlice(RemoveTimeMarkerIDs.ToArray(), WriteUnsignedVarLong);
					}
					else
					{
						WriteSignedVarInt(0);
					}
					break;

				default:
					throw new ArgumentException($"Unknown clock payload type: {PayloadType}");
			}
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();

			PayloadType = (ClockPayloadType)ReadUnsignedVarInt();
			

			switch (PayloadType)
			{
				case ClockPayloadType.SyncState:
					SyncStates = new System.Collections.Generic.List<SyncWorldClockStateData>(
						ReadSlice(ReadSyncWorldClockStateData));
					break;

				case ClockPayloadType.InitializeRegistry:
					Clocks = new System.Collections.Generic.List<WorldClockData>(
						ReadSlice(ReadWorldClockData));
					break;

				case ClockPayloadType.AddTimeMarker:
					AddClockID = ReadUnsignedVarLong();
					AddTimeMarkers = new System.Collections.Generic.List<TimeMarkerData>(
						ReadSlice(ReadTimeMarkerData));
					break;

				case ClockPayloadType.RemoveTimeMarker:
					RemoveClockID = ReadUnsignedVarLong();
					RemoveTimeMarkerIDs = new System.Collections.Generic.List<ulong>(
						ReadSlice(ReadUnsignedVarLong));
					break;

				default:
					throw new FormatException($"Unknown clock payload type: {PayloadType}");
			}

		
		}
	}
}
