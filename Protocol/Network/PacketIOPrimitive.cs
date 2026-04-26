using System;
using System.Buffers.Binary;
using System.IO;
using System.Numerics;
using System.Text;
using Protocol.Minecraft;
using Protocol.Utils;
using Protocol.Utils.IO;

namespace Protocol.Network
{
	public partial class Packet
	{
		public void WritePackSetting(PackSetting setting)
		{
			if (setting == null)
			{
				Write(string.Empty);
				uint defaultType = (int)PackSettingType.String;
				WriteUnsignedVarInt(defaultType);
				Write(string.Empty);
				return;
			}


			Write(setting.Name ?? string.Empty);


			uint typeId;
			if (setting.Value is float floatValue)
			{
				typeId = (int)PackSettingType.Float;
				WriteUnsignedVarInt(typeId);
				Write(floatValue);
			}
			else if (setting.Value is bool boolValue)
			{
				typeId = (int)PackSettingType.Bool;
				WriteUnsignedVarInt(typeId);
				Write(boolValue);
			}
			else if (setting.Value is string stringValue)
			{
				typeId = (int)PackSettingType.String;
				WriteUnsignedVarInt(typeId);
				Write(stringValue);
			}
			else
			{
				throw new ArgumentException(
					$"Unknown type for PackSetting.Value: {setting.Value?.GetType().Name ?? "null"}. Expected float, bool, or string.");
			}
		}

		public void Write(sbyte value)
		{
			_writer.Write(value);
		}

		public void Write(byte value)
		{
			_writer.Write(value);
		}

		public void Write(bool value)
		{
			Write((byte)(value ? 1 : 0));
		}


		public PackSetting ReadPackSetting()
		{
			var setting = new PackSetting();


			setting.Name = ReadString();


			uint typeId = ReadUnsignedVarInt();


			switch ((PackSettingType)typeId)
			{
				case PackSettingType.Float:
					float floatValue = ReadFloat();
					setting.Value = floatValue;
					break;
				case PackSettingType.Bool:
					bool boolValue = ReadBool();
					setting.Value = boolValue;
					break;
				case PackSettingType.String:
					string stringValue = ReadString();
					setting.Value = stringValue;
					break;
				default:


					throw new InvalidOperationException(
						$"Unknown PackSetting type ID: {typeId}. Expected {PackSettingType.Float}, {PackSettingType.Bool}, or {PackSettingType.String}.");
			}

			return setting;
		}


		public void WriteBitset(Bitset bitset, int size)
		{
			if (BigInteger.Zero == bitset.IntValue)
			{
				Write((byte)0x00);
				return;
			}


			BigInteger valueToWrite = BigInteger.Abs(bitset.IntValue);


			while (valueToWrite >= 0x80)
			{
				byte b = (byte)((valueToWrite & 0x7F) | 0x80);


				Write(b);


				valueToWrite >>= 7;
			}


			Write((byte)(valueToWrite & 0x7F));
		}


		public Bitset ReadBitset(int size)
		{
			BigInteger value = BigInteger.Zero;


			int shift = 0;
			byte b;


			do
			{
				b = ReadByte();


				BigInteger chunk = new BigInteger(b & 0x7F);
				value += (chunk << shift);


				shift += 7;
			} while ((b & 0x80) != 0);


			return new Bitset(size, value);
		}

		public sbyte ReadSByte()
		{
			return (sbyte)_reader.ReadByte();
		}

		public byte ReadByte()
		{
			return (byte)_reader.ReadByte();
		}

		public bool ReadBool()
		{
			return _reader.ReadByte() != 0;
		}

		public void Write(System.Memory<byte> value)
		{
			Write((System.ReadOnlyMemory<byte>)value);
		}

		public void Write(System.ReadOnlyMemory<byte> value)
		{
			if (value.IsEmpty)
			{
				return;
			}

			_writer.Write(value.Span);
		}

		public void Write(byte[] value)
		{
			if (value == null)
			{
				return;
			}

			_writer.Write(value);
		}

		public System.ReadOnlyMemory<byte> Slice(int count)
		{
			return _reader.Read(count);
		}

		public System.ReadOnlyMemory<byte> ReadReadOnlyMemory(int count, bool slurp = false)
		{
			if (!slurp && count == 0) return System.Memory<byte>.Empty;

			if (count == 0)
			{
				count = (int)(_reader.Length - _reader.Position);
			}

			System.ReadOnlyMemory<byte> readBytes = _reader.Read(count);
			if (readBytes.Length != count)
				throw new ArgumentOutOfRangeException($"Expected {count} bytes, only read {readBytes.Length}.");
			return readBytes;
		}

		public byte[] ReadBytes(int count, bool slurp = false)
		{
			if (!slurp && count == 0) return new byte[0];

			if (count == 0)
			{
				count = (int)(_reader.Length - _reader.Position);
			}

			System.ReadOnlyMemory<byte> readBytes = _reader.Read(count);
			if (readBytes.Length != count)
				throw new ArgumentOutOfRangeException($"Expected {count} bytes, only read {readBytes.Length}.");
			return readBytes.ToArray();
		}

		public void WriteByteArray(byte[] value)
		{
			if (value == null)
			{
				WriteLength(0);
				return;
			}

			WriteLength(value.Length);

			if (value.Length == 0) return;

			_writer.Write(value, 0, value.Length);
		}

		public byte[] ReadByteArray(bool slurp = false)
		{
			var len = ReadLength();
			var bytes = ReadBytes(len, slurp);
			return bytes;
		}

		public void Write(ulong[] value)
		{
			if (value == null)
			{
				WriteLength(0);
				return;
			}

			WriteLength(value.Length);

			if (value.Length == 0) return;
			for (int i = 0; i < value.Length; i++)
			{
				ulong val = value[i];
				Write(val);
			}
		}

		public ulong[] ReadUlongs(bool slurp = false)
		{
			var len = ReadLength();
			var ulongs = new ulong[len];
			for (int i = 0; i < ulongs.Length; i++)
			{
				ulongs[i] = ReadUlong();
			}

			return ulongs;
		}

		public void Write(short value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(BinaryPrimitives.ReverseEndianness(value));
			else _writer.Write(value);
		}

		public short ReadShort(bool bigEndian = false)
		{
			if (_reader.Position == _reader.Length) return 0;

			if (bigEndian) return BinaryPrimitives.ReverseEndianness(_reader.ReadInt16());

			return _reader.ReadInt16();
		}

		public void Write(ushort value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(BinaryPrimitives.ReverseEndianness(value));
			else _writer.Write(value);
		}

		public ushort ReadUshort(bool bigEndian = false)
		{
			if (_reader.Position == _reader.Length) return 0;

			if (bigEndian) return BinaryPrimitives.ReverseEndianness(_reader.ReadUInt16());

			return _reader.ReadUInt16();
		}

		public void WriteBe(short value)
		{
			_writer.Write(BinaryPrimitives.ReverseEndianness(value));
		}

		public short ReadShortBe()
		{
			if (_reader.Position == _reader.Length) return 0;

			return BinaryPrimitives.ReverseEndianness(_reader.ReadInt16());
		}

		public void Write(Int24 value)
		{
			_writer.Write(value.GetBytes());
		}

		public Int24 ReadLittle()
		{
			return new Int24(_reader.Read(3).Span);
		}

		public void Write(int value, bool bigEndian = false)
		{
			if (bigEndian) _writer.Write(BinaryPrimitives.ReverseEndianness(value));
			else _writer.Write(value);
		}

		public int ReadInt(bool bigEndian = false)
		{
			if (bigEndian) return BinaryPrimitives.ReverseEndianness(_reader.ReadInt32());

			return _reader.ReadInt32();
		}

		public void WriteBe(ushort value)
		{
			Write(value, true);
		}

		public void WriteBe(int value)
		{
			_writer.Write(BinaryPrimitives.ReverseEndianness(value));
		}

		public int ReadIntBe()
		{
			return BinaryPrimitives.ReverseEndianness(_reader.ReadInt32());
		}

		public void Write(uint value)
		{
			_writer.Write(value);
		}

		public uint ReadUint()
		{
			return _reader.ReadUInt32();
		}


		public void WriteVarInt(int value)
		{
			VarInt.WriteInt32(_buffer, value);
		}

		public int ReadVarInt()
		{
			return VarInt.ReadInt32(_reader);
		}

		public void WriteSignedVarInt(int value)
		{
			VarInt.WriteSInt32(_buffer, value);
		}

		public int ReadSignedVarInt()
		{
			return VarInt.ReadSInt32(_reader);
		}

		public void WriteUnsignedVarInt(uint value)
		{
			VarInt.WriteUInt32(_buffer, value);
		}

		public uint ReadUnsignedVarInt()
		{
			return VarInt.ReadUInt32(_reader);
		}

		public int ReadLength()
		{
			return (int)VarInt.ReadUInt32(_reader);
		}

		public void WriteLength(int value)
		{
			VarInt.WriteUInt32(_buffer, (uint)value);
		}

		public void WriteVarLong(long value)
		{
			VarInt.WriteInt64(_buffer, value);
		}

		public long ReadVarLong()
		{
			return VarInt.ReadInt64(_reader);
		}

		public void WriteEntityId(long value)
		{
			WriteSignedVarLong(value);
		}

		public void WriteSignedVarLong(long value)
		{
			VarInt.WriteSInt64(_buffer, value);
		}

		public long ReadSignedVarLong()
		{
			return VarInt.ReadSInt64(_reader);
		}

		public void WriteRuntimeEntityId(ulong value)
		{
			WriteUnsignedVarLong(value);
		}

		public void WriteUnsignedVarLong(ulong value)
		{
			VarInt.WriteUInt64(_buffer, (ulong)value);
		}

		public ulong ReadUnsignedVarLong()
		{
			return (ulong)VarInt.ReadUInt64(_reader);
		}

		public void Write(long value)
		{
			_writer.Write(BinaryPrimitives.ReverseEndianness(value));
		}

		public long ReadLong()
		{
			return BinaryPrimitives.ReverseEndianness(_reader.ReadInt64());
		}

		public void Write(ulong value)
		{
			_writer.Write(value);
		}

		public ulong ReadUlong()
		{
			return _reader.ReadUInt64();
		}

		public void Write(float value)
		{
			_writer.Write(value);
		}

		public float ReadFloat()
		{
			return _reader.ReadSingle();
		}
		public void Write(double value)
		{
			_writer.Write(value);
		}

		public double ReadDouble()
		{
			return _reader.ReadDouble();
		}

		public void Write(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				WriteLength(0);
				return;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(value);

			WriteLength(bytes.Length);
			Write(bytes);
		}

		public string ReadString()
		{
			if (_reader.Position == _reader.Length) return string.Empty;
			uint len = ReadUnsignedVarInt();
			if (len <= 0) return string.Empty;
			return Encoding.UTF8.GetString(ReadBytes((int)len));
		}

		public void WriteFixedString(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Write((short)0, true);
				return;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(value);

			Write((short)bytes.Length, true);
			Write(bytes);
		}

		public string ReadFixedString()
		{
			if (_reader.Position == _reader.Length) return string.Empty;
			short len = ReadShort(true);
			if (len <= 0) return string.Empty;
			return Encoding.UTF8.GetString(_reader.Read(len).Span);
		}
		public void Write(MemoryCategoryCounter value)
		{
			Write(value.Category);
			Write(value.Bytes);
		}

		// Read method
		public MemoryCategoryCounter ReadMemoryCategoryCounter()
		{
			return new MemoryCategoryCounter
			{
				Category = ReadByte(),
				Bytes = ReadUlong()
			};
		}

		public void Write(EntityDiagnosticTimingInfo value)
		{
			Write(value.DisplayName);
			Write(value.Entity);
			Write(value.TimeInNS);
			Write(value.PercentOfTotal);
		}

		public EntityDiagnosticTimingInfo ReadEntityDiagnosticTimingInfo()
		{
			return new EntityDiagnosticTimingInfo
			{
				DisplayName = ReadString(),
				Entity = ReadString(),
				TimeInNS = ReadUlong(),
				PercentOfTotal = ReadByte()
			};
		}

		public void Write(SystemDiagnosticTimingInfo value)
		{
			Write(value.DisplayName);
			Write(value.SystemIndex);
			Write(value.TimeInNS);
			Write(value.PercentOfTotal);
		}

		public SystemDiagnosticTimingInfo ReadSystemDiagnosticTimingInfo()
		{
			return new SystemDiagnosticTimingInfo
			{
				DisplayName = ReadString(),
				SystemIndex = ReadUlong(),
				TimeInNS = ReadUlong(),
				PercentOfTotal = ReadByte()
			};
		}
	}
}
