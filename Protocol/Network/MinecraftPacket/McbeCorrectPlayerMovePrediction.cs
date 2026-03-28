using System.Numerics;

namespace Protocol.Network.MinecraftPacket;

public class McbeCorrectPlayerMovePrediction : Packet
{
	public enum PredictionType : byte
	{
		Player = 0,
		Vehicle = 1
	}


	public McbeCorrectPlayerMovePrediction()
	{
		Id = 161;
		IsMcbe = true;
	}


	public PredictionType Type { get; set; }


	public Vector3 Position { get; set; }


	public Vector3 Delta { get; set; }


	public Vector2 Rotation { get; set; }


	public Optional<float> VehicleAngularVelocity { get; set; } = new();


	public bool OnGround { get; set; }


	public ulong Tick { get; set; }


	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write((byte)Type);
		Write(Position);
		Write(Delta);
		Write(Rotation);


		Write(VehicleAngularVelocity.HasValue);
		if (VehicleAngularVelocity.HasValue) Write(VehicleAngularVelocity.Value);

		Write(OnGround);
		WriteUnsignedVarLong(Tick);
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		Type = (PredictionType)ReadByte();
		Position = ReadVector3();
		Delta = ReadVector3();
		Rotation = ReadVector2();


		VehicleAngularVelocity.HasValue = ReadBool();
		if (VehicleAngularVelocity.HasValue) VehicleAngularVelocity.Value = ReadFloat();

		OnGround = ReadBool();
		Tick = ReadUnsignedVarLong();
	}
}
