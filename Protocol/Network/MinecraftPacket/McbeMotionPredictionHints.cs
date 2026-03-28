using System.Numerics;

namespace Protocol.Network.MinecraftPacket;

public class McbeMotionPredictionHints : Packet
{
	public McbeMotionPredictionHints()
	{
		Id = 157;
		IsMcbe = true;
	}


	public ulong EntityRuntimeID { get; set; }


	public Vector3 Velocity { get; set; }


	public bool OnGround { get; set; }


	protected override void EncodePacket()
	{
		base.EncodePacket();

		WriteUnsignedVarLong(EntityRuntimeID);
		Write(Velocity);
		Write(OnGround);
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		EntityRuntimeID = ReadUnsignedVarLong();
		Velocity = ReadVector3();
		OnGround = ReadBool();
	}
}
