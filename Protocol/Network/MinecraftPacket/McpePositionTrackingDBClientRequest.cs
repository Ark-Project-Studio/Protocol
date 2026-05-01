namespace Protocol.Network.MinecraftPacket;

public class McbePositionTrackingDBClientRequest : Packet
{
	public enum Action : byte
	{
		Query = 0
	}


	public McbePositionTrackingDBClientRequest()
	{
		Id = 154;
		IsMcbe = true;
	}


	public Action RequestAction { get; set; }


	public int TrackingID { get; set; }


	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((byte)RequestAction);
		WriteSignedVarInt(TrackingID);
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		RequestAction = (Action)ReadByte();
		TrackingID = ReadSignedVarInt();
	}
}
