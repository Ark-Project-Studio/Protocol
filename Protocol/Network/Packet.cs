using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using fNbt;
using Microsoft.IO;
using Protocol.Utils.IO;



namespace Protocol.Network
{
	public abstract partial class Packet
	{
		private byte[] _encodedMessage;

		[JsonIgnore] public int Id;
		[JsonIgnore] public bool IsMcbe;

		protected MemoryStreamReader _reader;
		protected private Stream _buffer;
		private BinaryWriter _writer;

		[JsonIgnore] public System.ReadOnlyMemory<byte> Bytes { get; private set; }

		public Packet()
		{
		}
	

		public bool CanRead()
		{
			return _reader.Position < _reader.Length;
		}

		private object _encodeSync = new object();

		private static RecyclableMemoryStreamManager _streamManager = new RecyclableMemoryStreamManager();
		private static ConcurrentDictionary<int, bool> _isLob = new ConcurrentDictionary<int, bool>();

		public virtual byte[] Encode()
		{
			byte[] cache = _encodedMessage;
			if (cache != null) return cache;

			lock (_encodeSync)
			{
				if (_encodedMessage != null) return _encodedMessage;


				bool isLob = _isLob.ContainsKey(Id);
				_buffer = isLob ? _streamManager.GetStream() : new MemoryStream();
				using (_writer = new BinaryWriter(_buffer, Encoding.UTF8, true))
				{
					EncodePacket();

					_writer.Flush();


					var buffer = (MemoryStream)_buffer;
					_encodedMessage = buffer.ToArray();
					if (!isLob && _encodedMessage.Length >= 85_000)
					{
						_isLob.TryAdd(Id, true);
					}
				}

				_buffer.Dispose();

				_writer = null;
				_buffer = null;
				Bytes = _encodedMessage;
				return _encodedMessage;
			}
		}

		protected virtual void EncodePacket()
		{
			_buffer.Position = 0;
			if (IsMcbe) WriteVarInt(Id);
			else Write((byte)Id);
		}

		[Obsolete("Use decode with ReadOnlyMemory<byte> instead.")]
		public virtual Packet Decode(byte[] buffer)
		{
			return Decode(new ReadOnlyMemory<byte>(buffer));
		}

		public virtual Packet Decode(ReadOnlyMemory<byte> buffer)
		{
			Bytes = buffer;
			_reader = new MemoryStreamReader(buffer);

			DecodePacket();

			Debug.Assert(_reader.Position == _reader.Length);
			_reader.Dispose();
			_reader = null;
			
				
			
			
			return this;
		}

		protected virtual void DecodePacket()
		{
			Id = IsMcbe ? ReadVarInt() : ReadByte();
		}

		public static string HexDump(ReadOnlyMemory<byte> bytes, int bytesPerLine = 16, bool printLineCount = false)
		{
			return HexDump(bytes.Span, bytesPerLine, printLineCount);
		}

		private static string HexDump(ReadOnlySpan<byte> bytes, in int bytesPerLine, in bool printLineCount)
		{
			var sb = new StringBuilder();
			for (int line = 0; line < bytes.Length; line += bytesPerLine)
			{
				byte[] lineBytes = bytes.Slice(line).ToArray().Take(bytesPerLine).ToArray();
				if (printLineCount) sb.AppendFormat("{0:x8} ", line);
				sb.Append(string.Join(" ", lineBytes.Select(b => b.ToString("x2"))
						.ToArray())
					.PadRight(bytesPerLine * 3));
				sb.Append(" ");
				sb.Append(new string(lineBytes.Select(b => b < 32 ? '.' : (char)b)
					.ToArray()));
				sb.AppendLine();
			}

			return sb.ToString();
		}
	}
}
