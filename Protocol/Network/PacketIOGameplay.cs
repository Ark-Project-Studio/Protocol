using System;
using System.Collections.Generic;
using System.Linq;
using Protocol.Minecraft;
using Protocol.Minecraft.Graphic;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(GameRule value)
		{
			Write(value.Name);
			Write(value.CanBeModifiedByPlayer);

			uint type;
			if (value.Value is bool boolValue)
			{
				type = 1;
				WriteUnsignedVarInt(type);
				Write(boolValue);
			}
			else if (value.Value is uint uintValue)
			{
				type = 2;
				WriteUnsignedVarInt(type);
				Write(uintValue);
			}
			else if (value.Value is float floatValue)
			{
				type = 3;
				WriteUnsignedVarInt(type);
				Write(floatValue);
			}
			else if (value.Value is int intValue)
			{
				type = 2;
				WriteUnsignedVarInt(type);
				Write((uint)intValue);
			}
			else
			{
				throw new ArgumentException($"Unknown game rule value type: {value.Value?.GetType()}");
			}
		}

		public GameRule ReadGameRule()
		{
			var rule = new GameRule
			{
				Name = ReadString(),
				CanBeModifiedByPlayer = ReadBool()
			};

			uint type = ReadUnsignedVarInt();

			switch (type)
			{
				case 1:
					rule.Value = ReadBool();
					break;
				case 2:
					rule.Value = ReadUint();
					break;
				case 3:
					rule.Value = ReadFloat();
					break;
				default:
					throw new FormatException($"Unknown game rule type: {type}");
			}

			return rule;
		}

		public void WriteGameRuleLegacy(GameRule value)
		{
			Write(value.Name);
			Write(value.CanBeModifiedByPlayer);

			uint type;
			if (value.Value is bool boolValue)
			{
				type = 1;
				WriteUnsignedVarInt(type);
				Write(boolValue);
			}
			else if (value.Value is uint uintValue)
			{
				type = 2;
				WriteUnsignedVarInt(type);
				WriteUnsignedVarInt(uintValue);
			}
			else if (value.Value is float floatValue)
			{
				type = 3;
				WriteUnsignedVarInt(type);
				Write(floatValue);
			}
			else if (value.Value is int intValue)
			{
				type = 2;
				WriteUnsignedVarInt(type);
				WriteUnsignedVarInt((uint)intValue);
			}
			else
			{
				throw new ArgumentException($"Unknown game rule value type: {value.Value?.GetType()}");
			}
		}

		public GameRule ReadGameRuleLegacy()
		{
			var rule = new GameRule
			{
				Name = ReadString(),
				CanBeModifiedByPlayer = ReadBool()
			};

			uint type = ReadUnsignedVarInt();

			switch (type)
			{
				case 1:
					rule.Value = ReadBool();
					break;
				case 2:
					rule.Value = ReadUnsignedVarInt();
					break;
				case 3:
					rule.Value = ReadFloat();
					break;
				default:
					throw new FormatException($"Unknown game rule type: {type}");
			}

			return rule;
		}

		public void Write(Records records)
		{
			WriteUnsignedVarInt((uint)records.Count);
			foreach (BlockCoordinates coord in records)
			{
				Write(coord);
			}
		}

		public Records ReadRecords()
		{
			var records = new Records();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				var coord = ReadBlockCoordinates();
				records.Add(coord);
			}

			return records;
		}

		public void Write(EnchantOptions options)
		{
			WriteUnsignedVarInt((uint)options.Count);
			foreach (EnchantOption option in options)
			{
				WriteUnsignedVarInt(option.Cost);
				Write(option.Flags);
				WriteEnchants(option.EquipActivatedEnchantments);
				WriteEnchants(option.HeldActivatedEnchantments);
				WriteEnchants(option.SelfActivatedEnchantments);
				Write(option.Name);
				WriteVarInt(option.OptionId);
			}
		}

		private void WriteEnchants(List<Enchant> enchants)
		{
			WriteUnsignedVarInt((uint)enchants.Count);
			foreach (Enchant enchant in enchants)
			{
				Write(enchant.Id);
				Write(enchant.Level);
			}
		}

		private List<Enchant> ReadEnchants()
		{
			List<Enchant> enchants = new List<Enchant>();
			var count = ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				Enchant enchant = new Enchant(ReadByte(), ReadByte());
				enchants.Add(enchant);
			}

			return enchants;
		}

		public EnchantOptions ReadEnchantOptions()
		{
			var options = new EnchantOptions();
			var count = ReadUnsignedVarInt();

			for (int i = 0; i < count; i++)
			{
				EnchantOption option = new EnchantOption();
				option.Cost = ReadUnsignedVarInt();
				option.Flags = ReadInt();
				option.EquipActivatedEnchantments = ReadEnchants();
				option.HeldActivatedEnchantments = ReadEnchants();
				option.SelfActivatedEnchantments = ReadEnchants();
				option.Name = ReadString();
				option.OptionId = ReadVarInt();

				options.Add(option);
			}

			return options;
		}

		public void Write(Experiments experiments)
		{
			if (experiments == null)
			{
				Write(0);
				return;
			}

			Write(experiments.Count);

			foreach (var experiment in experiments)
			{
				Write(experiment.Name);
				Write(experiment.Enabled);
			}
		}

		public Experiments ReadExperiments()
		{
			Experiments experiments = new Experiments();
			var count = ReadInt();

			for (int i = 0; i < count; i++)
			{
				var experimentName = ReadString();
				var enabled = ReadBool();
				experiments.Add(new Experiments.Experiment(experimentName, enabled));
			}

			return experiments;
		}

		public void Write(ScoreEntries list)
		{
			if (list == null) list = new ScoreEntries();

			Write((byte)(list.FirstOrDefault() is ScoreEntryRemove
				? McbeSetScore.Types.Remove
				: McbeSetScore.Types.Change));
			WriteUnsignedVarInt((uint)list.Count);
			foreach (var entry in list)
			{
				WriteSignedVarLong(entry.Id);
				Write(entry.ObjectiveName);
				Write(entry.Score);

				if (entry is ScoreEntryRemove)
				{
					continue;
				}

				if (entry is ScoreEntryChangePlayer player)
				{
					Write((byte)McbeSetScore.ChangeTypes.Player);
					WriteSignedVarLong(player.EntityId);
				}
				else if (entry is ScoreEntryChangeEntity entity)
				{
					Write((byte)McbeSetScore.ChangeTypes.Entity);
					WriteSignedVarLong(entity.EntityId);
				}
				else if (entry is ScoreEntryChangeFakePlayer fakePlayer)
				{
					Write((byte)McbeSetScore.ChangeTypes.FakePlayer);
					Write(fakePlayer.CustomName);
				}
			}
		}

		public ScoreEntries ReadScoreEntries()
		{
			var list = new ScoreEntries();
			byte type = ReadByte();
			var length = ReadUnsignedVarInt();
			for (var i = 0; i < length; ++i)
			{
				var entryId = ReadSignedVarLong();
				var entryObjectiveName = ReadString();
				var entryScore = ReadUint();

				ScoreEntry entry = null;

				if (type == (int)McbeSetScore.Types.Remove)
				{
					entry = new ScoreEntryRemove();
				}
				else
				{
					McbeSetScore.ChangeTypes changeType = (McbeSetScore.ChangeTypes)ReadByte();
					switch (changeType)
					{
						case McbeSetScore.ChangeTypes.Player:
							entry = new ScoreEntryChangePlayer { EntityId = ReadSignedVarLong() };
							break;
						case McbeSetScore.ChangeTypes.Entity:
							entry = new ScoreEntryChangeEntity { EntityId = ReadSignedVarLong() };
							break;
						case McbeSetScore.ChangeTypes.FakePlayer:
							entry = new ScoreEntryChangeFakePlayer { CustomName = ReadString() };
							break;
					}
				}

				if (entry == null) continue;

				entry.Id = entryId;
				entry.ObjectiveName = entryObjectiveName;
				entry.Score = entryScore;

				list.Add(entry);
			}

			return list;
		}

		public void Write(ScoreboardIdentityEntries list)
		{
			if (list == null) list = new ScoreboardIdentityEntries();

			Write((byte)(list.FirstOrDefault() is ScoreboardClearIdentityEntry
				? McbeSetScoreboardIdentity.Operations.ClearIdentity
				: McbeSetScoreboardIdentity.Operations.RegisterIdentity));
			WriteUnsignedVarInt((uint)list.Count);
			foreach (var entry in list)
			{
				WriteSignedVarLong(entry.Id);
				if (entry is ScoreboardRegisterIdentityEntry reg)
				{
					WriteSignedVarLong(reg.EntityId);
				}
			}
		}

		public ScoreboardIdentityEntries ReadScoreboardIdentityEntries()
		{
			ScoreboardIdentityEntries list = new ScoreboardIdentityEntries();

			McbeSetScoreboardIdentity.Operations type = (McbeSetScoreboardIdentity.Operations)ReadByte();
			var length = ReadUnsignedVarInt();
			for (var i = 0; i < length; ++i)
			{
				var scoreboardId = ReadSignedVarLong();

				switch (type)
				{
					case McbeSetScoreboardIdentity.Operations.RegisterIdentity:
						list.Add(new ScoreboardRegisterIdentityEntry()
						{
							Id = scoreboardId,
							EntityId = ReadSignedVarLong()
						});
						break;
					case McbeSetScoreboardIdentity.Operations.ClearIdentity:
						list.Add(new ScoreboardClearIdentityEntry() { Id = scoreboardId });
						break;
				}
			}

			return list;
		}

		public void Write(ParameterKeyframeValue value)
		{
			Write(value.Time);
			Write(value.Value);
		}

		public ParameterKeyframeValue ReadParameterKeyframeValue()
		{
			return new ParameterKeyframeValue()
			{
				Time = ReadFloat(),
				Value = ReadVector3()
			};
		}

		public fogStack Read()
		{
			fogStack stack = new fogStack();
			var effectCount = ReadUnsignedVarInt();
			for (int i = 0; i < (int)effectCount; i++)
			{
				stack.fogList.Add(ReadString());
			}

			return stack;
		}

		public void Write(fogStack stack)
		{
			WriteUnsignedVarInt((uint)stack.fogList.Count);
			foreach (string effect in stack.fogList)
			{
				Write(effect);
			}
		}
	}
}
