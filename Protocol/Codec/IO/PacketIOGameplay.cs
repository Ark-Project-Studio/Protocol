using System;
using System.Collections.Generic;
using System.Linq;
using Protocol.Codec.Packets;
using Protocol.Minecraft;
using Protocol.Minecraft.Actor.Player;
using Protocol.Minecraft.Graphic;
using Protocol.Minecraft.Inventory.Enchant;
using Protocol.Minecraft.Level;
using Protocol.Minecraft.Level.Block;
using Protocol.Minecraft.Level.Scoreboard;

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

		public void Write(BlockCoordinates[] records)
		{
			WriteSlice(records ?? [], Write);
		}

		public BlockCoordinates[] ReadRecords()
		{
			return ReadSlice(ReadBlockCoordinates);
		}

		public void Write(EnchantOption[] options)
		{
			WriteSlice(options ?? [], option =>
			{
				WriteUnsignedVarInt(option.Cost);
				Write(option.Flags);
				WriteEnchants(option.EquipActivatedEnchantments);
				WriteEnchants(option.HeldActivatedEnchantments);
				WriteEnchants(option.SelfActivatedEnchantments);
				Write(option.Name);
				WriteVarInt(option.OptionId);
			});
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

		public EnchantOption[] ReadEnchantOptions()
		{
			return ReadSlice(() =>
			{
				EnchantOption option = new EnchantOption();
				option.Cost = ReadUnsignedVarInt();
				option.Flags = ReadInt();
				option.EquipActivatedEnchantments = ReadEnchants();
				option.HeldActivatedEnchantments = ReadEnchants();
				option.SelfActivatedEnchantments = ReadEnchants();
				option.Name = ReadString();
				option.OptionId = ReadVarInt();

				return option;
			});
		}

		public void Write(Experiments.Experiment[] experiments)
		{
			WriteSliceUint32Length(experiments ?? [], experiment =>
			{
				Write(experiment.Name);
				Write(experiment.Enabled);
			});
		}

		public Experiments.Experiment[] ReadExperiments()
		{
			return ReadSliceUint32Length(() =>
			{
				var experimentName = ReadString();
				var enabled = ReadBool();
				return new Experiments.Experiment(experimentName, enabled);
			});
		}

		public void Write(ScoreEntry[] list)
		{
			list ??= [];

			Write((byte)(list.FirstOrDefault() is ScoreEntryRemove
				? McbeSetScore.PacketType.Remove
				: McbeSetScore.PacketType.Change));
			WriteSlice(list, entry =>
			{
				WriteSignedVarLong(entry.Id);
				Write(entry.ObjectiveName);
				Write((int)entry.Score);

				if (entry is not ScoreEntryRemove)
				{
					if (entry is ScoreEntryChangePlayer player)
					{
						Write((byte)McbeSetScore.IdentityType.Player);
						WriteSignedVarLong(player.EntityId);
					}
					else if (entry is ScoreEntryChangeEntity entity)
					{
						Write((byte)McbeSetScore.IdentityType.Entity);
						WriteSignedVarLong(entity.EntityId);
					}
					else if (entry is ScoreEntryChangeFakePlayer fakePlayer)
					{
						Write((byte)McbeSetScore.IdentityType.FakePlayer);
						Write(fakePlayer.CustomName);
					}
				}
			});
		}

		public ScoreEntry[] ReadScoreEntries()
		{
			byte type = ReadByte();
			return ReadSlice<ScoreEntry>(() =>
			{
				var entryId = ReadSignedVarLong();
				var entryObjectiveName = ReadString();
				var entryScore = (uint)ReadInt();

				ScoreEntry entry = null;

				if (type == (int)McbeSetScore.PacketType.Remove)
				{
					entry = new ScoreEntryRemove();
				}
				else
				{
					McbeSetScore.IdentityType changeType = (McbeSetScore.IdentityType)ReadByte();
					switch (changeType)
					{
						case McbeSetScore.IdentityType.Player:
							entry = new ScoreEntryChangePlayer { EntityId = ReadSignedVarLong() };
							break;
						case McbeSetScore.IdentityType.Entity:
							entry = new ScoreEntryChangeEntity { EntityId = ReadSignedVarLong() };
							break;
						case McbeSetScore.IdentityType.FakePlayer:
							entry = new ScoreEntryChangeFakePlayer { CustomName = ReadString() };
							break;
					}
				}

				if (entry == null) return null;

				entry.Id = entryId;
				entry.ObjectiveName = entryObjectiveName;
				entry.Score = entryScore;

				return entry;
			}).Where(entry => entry != null).ToArray();
		}

		public void Write(ScoreboardIdentityEntry[] list)
		{
			list ??= [];

			Write((byte)(list.FirstOrDefault() is ScoreboardClearIdentityEntry
				? McbeSetScoreboardIdentity.Type.Remove
				: McbeSetScoreboardIdentity.Type.Update));
			WriteSlice(list, entry =>
			{
				WriteSignedVarLong(entry.Id);
				if (entry is ScoreboardRegisterIdentityEntry reg)
				{
					WriteSignedVarLong(reg.EntityId);
				}
			});
		}

		public ScoreboardIdentityEntry[] ReadScoreboardIdentityEntries()
		{
			McbeSetScoreboardIdentity.Type type = (McbeSetScoreboardIdentity.Type)ReadByte();
			return ReadSlice<ScoreboardIdentityEntry>(() =>
			{
				var scoreboardId = ReadSignedVarLong();

				return type switch
				{
					McbeSetScoreboardIdentity.Type.Update => new ScoreboardRegisterIdentityEntry()
						{
							Id = scoreboardId,
							EntityId = ReadSignedVarLong()
						},
					McbeSetScoreboardIdentity.Type.Remove => new ScoreboardClearIdentityEntry() { Id = scoreboardId },
					_ => null
				};
			}).Where(entry => entry != null).ToArray();
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
