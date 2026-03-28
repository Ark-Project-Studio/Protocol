using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;

public class McbePositionTrackingDBServerBroadcast : Packet
{
	public enum Action
	{
		Update = 0,
		Destroy = 1,
		NotFound = 2
	}


	public McbePositionTrackingDBServerBroadcast()
	{
		Id = 153;
		IsMcbe = true;
	}


	public byte BroadcastAction { get; set; }


	public int TrackingID { get; set; }


	public Nbt Payload { get; set; }


	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(BroadcastAction);
		WriteSignedVarInt(TrackingID);


		Write(Payload ?? new Nbt());
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		BroadcastAction = ReadByte();
		TrackingID = ReadSignedVarInt();


		Payload = ReadNbt();
	}
}
