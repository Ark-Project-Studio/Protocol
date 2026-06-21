using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeDebugInfo : Packet
{
	public McbeDebugInfo()
	{
		Id = 155;
		IsMcbe = true;
	}


	public long PlayerUniqueID { get; set; }


	public byte[] Data { get; set; } = new byte[0];


	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteSignedVarLong(PlayerUniqueID);
		WriteByteArray(Data);
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		PlayerUniqueID = ReadSignedVarLong();
		Data = ReadByteArray();
	}
}
