namespace Protocol.Network.MinecraftPacket;

public class McbePositionTrackingDBClientRequest : Packet
{
	public enum Action
	{
		Query = 0
	}


	public McbePositionTrackingDBClientRequest()
	{
		Id = 154;
		IsMcbe = true;
	}


	public byte RequestAction { get; set; }


	public int TrackingID { get; set; }


	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(RequestAction);
		WriteSignedVarInt(TrackingID);
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		RequestAction = ReadByte();
		TrackingID = ReadSignedVarInt();
	}
}
