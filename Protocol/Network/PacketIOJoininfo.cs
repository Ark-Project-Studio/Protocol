using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(GatheringJoinInfo value)
		{
			Write(value.ExperienceID);
			Write(value.ExperienceName);
			Write(value.ExperienceWorldID);
			Write(value.ExperienceWorldName);
			Write(value.CreatorID);
			Write(value.StoreID);
		}

		public GatheringJoinInfo ReadGatheringJoinInfo()
		{
			return new GatheringJoinInfo
			{
				ExperienceID = ReadString(),
				ExperienceName = ReadString(),
				ExperienceWorldID = ReadString(),
				ExperienceWorldName = ReadString(),
				CreatorID = ReadString(),
				StoreID = ReadString()
			};
		}

		public void Write(ServerJoinInformation value)
		{
			Write(value.GatheringJoinInfo.HasValue);
			if (value.GatheringJoinInfo.HasValue)
			{
				Write(value.GatheringJoinInfo.Value);
			}
		}

		public ServerJoinInformation ReadServerJoinInformation()
		{
			var result = new ServerJoinInformation();

			var hasGatheringJoinInfo = ReadBool();
			if (hasGatheringJoinInfo)
			{
				result.GatheringJoinInfo = new Optional<GatheringJoinInfo>(ReadGatheringJoinInfo());
			}

			return result;
		}
	}
}
