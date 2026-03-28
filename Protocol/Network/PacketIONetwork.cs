using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using fNbt;
using Protocol.Minecraft;
using Protocol.Utils;
using Protocol.Network.MinecraftPacket;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void Write(IPEndPoint endpoint)
		{
			if (endpoint.AddressFamily == AddressFamily.InterNetwork)
			{
				Write((byte)4);
				var parts = endpoint.Address.ToString().Split('.');
				foreach (var part in parts)
				{
					Write((byte)~byte.Parse(part));
				}

				Write((short)endpoint.Port, true);
			}
		}


		public IPEndPoint ReadIPEndPoint()
		{
			byte ipVersion = ReadByte();

			IPAddress address = IPAddress.Any;
			int port = 0;

			if (ipVersion == 4)
			{
				string ipAddress = $"{(byte)~ReadByte()}.{(byte)~ReadByte()}.{(byte)~ReadByte()}.{(byte)~ReadByte()}";
				address = IPAddress.Parse(ipAddress);
				port = (ushort)ReadShort(true);
			}
			else if (ipVersion == 6)
			{
				ReadShort();
				port = (ushort)ReadShort(true);
				ReadLong();
				var addressBytes = ReadBytes(16);
				address = new IPAddress(addressBytes);
			}
			else
			{
			}

			return new IPEndPoint(address, port);
		}

		public void Write(IPEndPoint[] endpoints)
		{
			foreach (var endpoint in endpoints)
			{
				Write(endpoint);
			}
		}

		public IPEndPoint[] ReadIPEndPoints(int count)
		{
			if (count == 20 && _reader.Length < 120) count = 10;
			var endPoints = new IPEndPoint[count];
			for (int i = 0; i < endPoints.Length; i++)
			{
				endPoints[i] = ReadIPEndPoint();
			}

			return endPoints;
		}

		public void Write(UUID uuid)
		{
			if (uuid == null) throw new Exception("Expected UUID, required");
			Write(uuid.GetBytes());
		}

		public UUID ReadUUID()
		{
			UUID uuid = new UUID(ReadBytes(16));
			return uuid;
		}

		public void Write(Nbt nbt)
		{
			Write(nbt, _writer.BaseStream,
				nbt.NbtFile.UseVarInt || this is McbeBlockEntityData || this is McbeUpdateEquipment);
		}

		public static void Write(Nbt nbt, Stream stream, bool useVarInt)
		{
			NbtFile file = nbt.NbtFile;
			file.BigEndian = false;
			file.UseVarInt = useVarInt;

			byte[] saveToBuffer = file.SaveToBuffer(NbtCompression.None);
			stream.Write(saveToBuffer, 0, saveToBuffer.Length);
		}


		public Nbt ReadNbt()
		{
			return ReadNbt(_reader);
		}

		public static Nbt ReadNbt(Stream stream, bool allowAlternativeRootTag = true, bool useVarInt = true)
		{
			Nbt nbt = new Nbt();
			NbtFile nbtFile = new NbtFile();
			nbtFile.BigEndian = false;
			nbtFile.UseVarInt = useVarInt;
			nbtFile.AllowAlternativeRootTag = allowAlternativeRootTag;

			nbt.NbtFile = nbtFile;
			nbtFile.LoadFromStream(stream, NbtCompression.AutoDetect);

			return nbt;
		}

		public static NbtCompound ReadNbtCompound(Stream stream, bool useVarInt = false)
		{
			NbtFile file = new NbtFile();
			file.BigEndian = false;
			file.UseVarInt = useVarInt;
			file.AllowAlternativeRootTag = false;

			file.LoadFromStream(stream, NbtCompression.None);

			return (NbtCompound)file.RootTag;
		}

		public void Write(ResourcePackIds ids)
		{
			if (ids == null)
			{
				Write((short)0);
				return;
			}

			Write((short)ids.Count);

			foreach (var id in ids)
			{
				Write(id);
			}
		}

		public ResourcePackIds ReadResourcePackIds()
		{
			int count = ReadShort();

			var ids = new ResourcePackIds();
			for (int i = 0; i < count; i++)
			{
				var id = ReadString();
				ids.Add(id);
			}

			return ids;
		}

		public void Write(EducationUriResource resource)
		{
			Write(resource.ButtonName);
			Write(resource.LinkUri);
		}

		public EducationUriResource ReadEducationUriResource()
		{
			string name = ReadString();
			var uri = ReadString();

			return new EducationUriResource(name, uri);
		}

		public void Write(Rules rules)
		{
			_writer.Write(rules.Count);
			foreach (var rule in rules)
			{
				Write(rule.Name);
				Write(rule.Unknown1);
				Write(rule.Unknown2);
			}
		}

		public Rules ReadRules()
		{
			int count = _reader.ReadInt32();

			var rules = new Rules();
			for (int i = 0; i < count; i++)
			{
				RuleData rule = new RuleData();
				rule.Name = ReadString();
				rule.Unknown1 = ReadBool();
				rule.Unknown2 = ReadBool();
				rules.Add(rule);
			}

			return rules;
		}
	}
}
