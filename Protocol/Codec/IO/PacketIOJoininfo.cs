using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Codec.Level;
using Protocol.Codec.Packets;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(GatheringsConfigurationJoinInfo value)
		{
			Write(value.ExperienceId);
			Write(value.ExperienceName);
			Write(value.ExperienceWorldId);
			Write(value.ExperienceWorldName);
			Write(value.CreatorId);
			Write(value.TargetId);
			Write(value.ScenarioId);
			Write(value.ServerId);
		}

		public GatheringsConfigurationJoinInfo ReadGatheringsConfigurationJoinInfo()
		{
			return new GatheringsConfigurationJoinInfo
			{
				ExperienceId = ReadUUID(),
				ExperienceName = ReadString(),
				ExperienceWorldId = ReadUUID(),
				ExperienceWorldName = ReadString(),
				CreatorId = ReadString(),
				TargetId = ReadUUID(),
				ScenarioId = ReadString(),
				ServerId = ReadString()
			};
		}

		public void Write(GatheringsConfigurationClientStoreEntryPointInfo value)
		{
			Write(value.StoreId);
			Write(value.StoreName);
		}

		public GatheringsConfigurationClientStoreEntryPointInfo ReadGatheringsConfigurationClientStoreEntryPointInfo()
		{
			return new GatheringsConfigurationClientStoreEntryPointInfo
			{
				StoreId = ReadString(),
				StoreName = ReadString()
			};
		}

		public void Write(ServerConfigurationJoinInfo value)
		{
			Write(value.GatheringsConfiguration.HasValue);
			if (value.GatheringsConfiguration.HasValue)
			{
				Write(value.GatheringsConfiguration.Value);
			}

			Write(value.StoreEntryPointInfo.HasValue);
			if (value.StoreEntryPointInfo.HasValue)
			{
				Write(value.StoreEntryPointInfo.Value);
			}

			Write(value.PresenceInfo.HasValue);
			if (value.PresenceInfo.HasValue)
			{
				value.PresenceInfo.Value.Write(this);
			}
		}

		public ServerConfigurationJoinInfo ReadServerConfigurationJoinInfo()
		{
			var result = new ServerConfigurationJoinInfo();

			if (ReadBool())
			{
				result.GatheringsConfiguration = new Optional<GatheringsConfigurationJoinInfo>(ReadGatheringsConfigurationJoinInfo());
			}

			if (ReadBool())
			{
				result.StoreEntryPointInfo = new Optional<GatheringsConfigurationClientStoreEntryPointInfo>(ReadGatheringsConfigurationClientStoreEntryPointInfo());
			}

			if (ReadBool())
			{
				var presence = new PresenceConfiguration();
				presence.Read(this);
				result.PresenceInfo = new Optional<PresenceConfiguration>(presence);
			}

			return result;
		}
	}
}
