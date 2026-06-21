using Protocol.Network;

namespace Protocol.Codec.Packets;

public class UnknownPacket : Packet
{
	public UnknownPacket() : this(0, null)
	{
	}

	public UnknownPacket(byte id, ReadOnlyMemory<byte> message)
	{
		Message = message;
		Id = id;
	}

	public ReadOnlyMemory<byte> Message { get; private set; }
}
