using System;
using System.Collections.Generic;
using Protocol.Minecraft;
using Protocol.Minecraft.Actor;
using Protocol.Minecraft.Actor.Player;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(AbilityData data)
		{
			if (data == null)
			{
				Write(0L);
				Write((byte)0);
				Write((byte)0);
				WriteUnsignedVarInt(0);
				return;
			}

			Write(data.EntityUniqueID);
			Write(data.PlayerPermissions);
			Write(data.CommandPermissions);
			WriteUnsignedVarInt((uint)(data.Layers?.Length ?? 0));
			if (data.Layers != null)
				foreach (var layer in data.Layers)
					Write(layer);
		}

		public AbilityData ReadAbilityData()
		{
			var entityUniqueID = ReadLong();
			var playerPermissions = ReadByte();
			var commandPermissions = ReadByte();
			var layersCount = ReadUnsignedVarInt();
			var layers = new AbilityLayer[layersCount];
			for (var i = 0; i < layersCount; i++)
				layers[i] = ReadAbilityLayer();
			return new AbilityData(entityUniqueID, playerPermissions, commandPermissions, layers);
		}
		public void Write(PlayerListEntry entry)
		{
			Write(entry.uuid);
			WriteSignedVarLong(entry.actorUniqueId);
			Write(entry.playerName ?? string.Empty);
			Write(entry.xuid ?? string.Empty);
			Write(entry.platformChatId ?? string.Empty);
			Write(entry.buildPlatform);
			Write(entry.serializedSkin ?? new Skin());
			Write(entry.isTeacher);
			Write(entry.isHost);
			Write(entry.isSubClient);
			Write(entry.color);
		}
		

		public PlayerListEntry ReadPlayerListEntry()
		{
			return new PlayerListEntry
			{
				uuid = ReadUUID(),
				actorUniqueId = ReadSignedVarLong(),
				playerName = ReadString(),
				xuid = ReadString(),
				platformChatId = ReadString(),
				buildPlatform = ReadInt(),
				serializedSkin = ReadSkin(),
				isTeacher = ReadBool(),
				isHost = ReadBool(),
				isSubClient = ReadBool(),
				color = ReadInt(),
			};
		}

		public void Write(Skin skin)
		{
			Write(skin.SkinId);
			Write(skin.PlayFabId);
			Write(skin.ResourcePatch);
			Write(skin.Width);
			Write(skin.Height);
			WriteByteArray(skin.Data);

			if (skin.Animations?.Count > 0)
			{
				Write(skin.Animations.Count);
				foreach (Animation animation in skin.Animations)
				{
					Write(animation.ImageWidth);
					Write(animation.ImageHeight);
					WriteByteArray(animation.Image);
					Write(animation.Type);
					Write(animation.FrameCount);
					Write(animation.Expression);
				}
			}
			else
			{
				Write(0);
			}

			Write(skin.Cape.ImageWidth);
			Write(skin.Cape.ImageHeight);
			WriteByteArray(skin.Cape.Data);
			Write(skin.GeometryData);
			Write(skin.GeometryDataVersion);
			Write(skin.AnimationData);

			Write(skin.Cape.Id);
			Write(skin.SkinId + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
			Write(skin.ArmSize);
			Write(skin.SkinColor);
			Write(skin.PersonaPieces.Count);
			foreach (PersonaPiece piece in skin.PersonaPieces)
			{
				Write(piece.PieceId);
				Write(piece.PieceType);
				Write(piece.PackId);
				Write(piece.IsDefaultPiece);
				Write(piece.ProductId);
			}

			Write(skin.SkinPieces.Count);
			foreach (SkinPiece skinPiece in skin.SkinPieces)
			{
				Write(skinPiece.PieceType);
				Write(skinPiece.Colors.Count);
				foreach (string color in skinPiece.Colors)
				{
					Write(color);
				}
			}

			Write(skin.IsPremiumSkin);
			Write(skin.IsPersonaSkin);
			Write(skin.Cape.OnClassicSkin);
			Write(skin.IsPrimaryUser);
			Write(skin.isOverride);
		}

		public Skin ReadSkin()
		{
			Skin skin = new Skin();

			skin.SkinId = ReadString();
			skin.PlayFabId = ReadString();
			skin.ResourcePatch = ReadString();
			skin.Width = ReadInt();
			skin.Height = ReadInt();
			skin.Data = ReadByteArray(false);

			int animationCount = ReadInt();
			for (int i = 0; i < animationCount; i++)
			{
				skin.Animations.Add(
					new Animation()
					{
						ImageWidth = ReadInt(),
						ImageHeight = ReadInt(),
						Image = ReadByteArray(false),
						Type = ReadInt(),
						FrameCount = ReadFloat(),
						Expression = ReadInt()
					}
				);
			}

			skin.Cape.ImageWidth = ReadInt();
			skin.Cape.ImageHeight = ReadInt();
			skin.Cape.Data = ReadByteArray(false);
			skin.GeometryData = ReadString();
			skin.GeometryDataVersion = ReadString();
			skin.AnimationData = ReadString();

			skin.Cape.Id = ReadString();
			ReadString();
			skin.ArmSize = ReadString();
			skin.SkinColor = ReadString();
			int personaPieceCount = ReadInt();
			for (int i = 0; i < personaPieceCount; i++)
			{
				var p = new PersonaPiece();
				p.PieceId = ReadString();
				p.PieceType = ReadString();
				p.PackId = ReadString();
				p.IsDefaultPiece = ReadBool();
				p.ProductId = ReadString();
				skin.PersonaPieces.Add(p);
			}

			int skinPieceCount = ReadInt();
			for (int i = 0; i < skinPieceCount; i++)
			{
				var piece = new SkinPiece();
				piece.PieceType = ReadString();
				int colorAmount = ReadInt();
				for (int i2 = 0; i2 < colorAmount; i2++)
				{
					piece.Colors.Add(ReadString());
				}

				skin.SkinPieces.Add(piece);
			}

			skin.IsPremiumSkin = ReadBool();
			skin.IsPersonaSkin = ReadBool();
			skin.Cape.OnClassicSkin = ReadBool();
			skin.IsPrimaryUser = ReadBool();
			skin.isOverride = ReadBool();


			return skin;
		}

		public PlayerAttributes ReadPlayerAttributes()
		{
			var attributes = new PlayerAttributes();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				PlayerAttribute attribute = new PlayerAttribute
				{
					MinValue = ReadFloat(),
					MaxValue = ReadFloat(),
					Value = ReadFloat(),
					DefaultMinValue = ReadFloat(),
					DefaultMaxValue = ReadFloat(),
					Default = ReadFloat(),
					Name = ReadString(),
					Modifiers = ReadAttributeModifiers()
				};
				attributes[attribute.Name] = attribute;
			}

			return attributes;
		}

		public void Write(PlayerAttributes attributes)
		{
			WriteUnsignedVarInt((uint)attributes.Count);
			foreach (PlayerAttribute attribute in attributes.Values)
			{
				Write(attribute.MinValue);
				Write(attribute.MaxValue);
				Write(attribute.Value);
				Write(attribute.DefaultMinValue == 0.0f ? attribute.MinValue : attribute.DefaultMinValue);
				Write(attribute.DefaultMaxValue == 0.0f ? attribute.MaxValue : attribute.DefaultMaxValue);
				Write(attribute.Default);
				Write(attribute.Name);
				Write(attribute.Modifiers);
			}
		}

		public void Write(AbilityLayer layer)
		{
			Write((ushort)layer.Type);
			Write((uint)layer.Abilities);
			Write((uint)layer.Values);
			Write(layer.FlySpeed);
			Write(layer.WalkSpeed);
			Write(layer.VerticalFlySpeed);
		}

		public AbilityLayer ReadAbilityLayer()
		{
			AbilityLayer layer = new AbilityLayer();
			layer.Type = (AbilityLayerType)ReadUshort();
			layer.Abilities = (PlayerAbility)ReadUint();
			layer.Values = ReadUint();
			layer.FlySpeed = ReadFloat();
			layer.WalkSpeed = ReadFloat();
			layer.VerticalFlySpeed = ReadFloat();

			return layer;
		}

		public void Write(AbilityLayer[] layers)
		{
			WriteSliceUint8Length(layers ?? [], Write);
		}

		public AbilityLayer[] ReadAbilityLayers()
		{
			return ReadSliceUint8Length(ReadAbilityLayer);
		}

		public void Write(AnimationKey[] keys)
		{
			WriteUnsignedVarInt((uint)keys.Length);
			foreach (AnimationKey key in keys)
			{
				Write(key.ExecuteImmediate);
				Write(key.ResetBefore);
				Write(key.ResetAfter);
				Write(key.StartRotation);
				Write(key.EndRotation);
				WriteUnsignedVarInt(key.Duration);
			}
		}

		public AnimationKey[] ReadAnimationKeys()
		{
			var count = ReadUnsignedVarInt();
			var keys = new AnimationKey[count];
			for (int i = 0; i < count; i++)
			{
				AnimationKey key = new AnimationKey();
				key.ExecuteImmediate = ReadBool();
				key.ResetBefore = ReadBool();
				key.ResetAfter = ReadBool();
				key.StartRotation = ReadVector3();
				key.EndRotation = ReadVector3();
				key.Duration = ReadUnsignedVarInt();
				keys[i] = key;
			}

			return keys;
		}

		public EmoteIds ReadEmoteId()
		{
			EmoteIds Ids = new EmoteIds();
			var emoteCount = ReadUnsignedVarInt();
			for (int i = 0; i < (int)emoteCount; i++)
			{
				Ids.emoteId.Add(ReadUUID());
			}

			return Ids;
		}

		public void Write(EmoteIds Ids)
		{
			Write(Ids.emoteId.Count);
			foreach (var emoteIds in Ids.emoteId)
			{
				Write(emoteIds);
			}
		}
	}
}
