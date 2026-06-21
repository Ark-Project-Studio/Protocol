using System;
using System.IO;
using Protocol.Minecraft;
using Protocol.Minecraft.Actor;
using Protocol.Minecraft.Actor.Player;

namespace Protocol.Network
{
	public partial class Packet
	{
		public AttributeModifiers ReadAttributeModifiers()
		{
			var modifiers = new AttributeModifiers();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				AttributeModifier modifier = new AttributeModifier
				{
					Id = ReadString(),
					Name = ReadString(),
					Amount = ReadFloat(),
					Operations = ReadInt(),
					Operand = ReadInt(),
					Serializable = ReadBool(),
				};
				modifiers[modifier.Name] = modifier;
			}

			return modifiers;
		}

		public void Write(AttributeModifiers modifiers)
		{
			WriteUnsignedVarInt((uint)modifiers.Count);
			foreach (AttributeModifier modifier in modifiers.Values)
			{
				Write(modifier.Id);
				Write(modifier.Name);
				Write(modifier.Amount);
				Write(modifier.Operations);
				Write(modifier.Operand);
				Write(modifier.Serializable);
			}
		}

		public void Write(EntityAttributes attributes)
		{
			if (attributes == null)
			{
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint)attributes.Count);
			foreach (EntityAttribute attribute in attributes.Values)
			{
				Write(attribute.Name);
				Write(attribute.MinValue);
				Write(attribute.Value);
				Write(attribute.MaxValue);
			}
		}

		public EntityAttributes ReadEntityAttributes()
		{
			var attributes = new EntityAttributes();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				EntityAttribute attribute = new EntityAttribute
				{
					Name = ReadString(),
					MinValue = ReadFloat(),
					Value = ReadFloat(),
					MaxValue = ReadFloat(),
				};

				attributes[attribute.Name] = attribute;
			}

			return attributes;
		}

		public void Write(EntityLink link)
		{
			WriteVarLong(link.FromEntityId);
			WriteVarLong(link.ToEntityId);
			Write((byte)link.Type);
			Write(link.Immediate);
			Write(link.CausedByRider);
			Write(link.VehicleAngularVelocity);
		}

		public EntityLink ReadEntityLink()
		{
			var from = ReadVarLong();
			var to = ReadVarLong();
			var type = (EntityLink.EntityLinkType)ReadByte();
			var immediate = ReadBool();
			var causedByRider = ReadBool();
			var vehicleAngularVelocity = ReadFloat();

			return new EntityLink(from, to, type, immediate, causedByRider, vehicleAngularVelocity);
		}

		public void Write(EntityLink[] links)
		{
			WriteSlice(links ?? [], Write);
		}

		public EntityLink[] ReadEntityLinks()
		{
			return ReadSlice(ReadEntityLink);
		}

		public void Write(PropertySyncData syncData)
		{
			if (syncData == null)
			{
				WriteUnsignedVarInt(0);
				WriteUnsignedVarInt(0);
				return;
			}

			WriteUnsignedVarInt((uint)syncData.intProperties.Count);

			foreach (var intP in syncData.intProperties)
			{
				WriteUnsignedVarInt(intP.Key);
				WriteSignedVarInt(intP.Value);
			}

			WriteUnsignedVarInt((uint)syncData.floatProperties.Count);

			foreach (var intF in syncData.floatProperties)
			{
				WriteUnsignedVarInt(intF.Key);
				Write(intF.Value);
			}
		}

		public PropertySyncData ReadPropertySyncData()
		{
			PropertySyncData syncData = new PropertySyncData();
			var countInt = ReadUnsignedVarInt();
			for (int i = 0; i < countInt; i++)
			{
				syncData.intProperties.Add(ReadUnsignedVarInt(), ReadVarInt());
			}

			var countFloat = ReadUnsignedVarInt();
			for (int i = 0; i < countFloat; i++)
			{
				syncData.floatProperties.Add(ReadUnsignedVarInt(), ReadFloat());
			}

			return syncData;
		}
	}
}
