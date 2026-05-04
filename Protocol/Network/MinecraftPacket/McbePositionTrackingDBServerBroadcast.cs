using Protocol.Minecraft;
using Protocol.Minecraft.NBT;

namespace Protocol.Network.MinecraftPacket;

public class McbePositionTrackingDBServerBroadcast : Packet
{
	public enum Action : byte
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


	public Action BroadcastAction { get; set; }


	public int TrackingID { get; set; }


	public Nbt Payload { get; set; }


	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((byte)BroadcastAction);
		WriteSignedVarInt(TrackingID);


		Write(Payload ?? new Nbt());
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		BroadcastAction = (Action)ReadByte();
		TrackingID = ReadSignedVarInt();


		Payload = ReadNbt();
	}
}
