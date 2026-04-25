namespace Protocol.Network.MinecraftPacket;

public class McbeCameraShake : Packet
{
	public enum ShakeAction : byte
	{
		Add = 0,
		Stop = 1
	}


	public enum ShakeType : byte
	{
		Positional = 0,
		Rotational = 1
	}


	public McbeCameraShake()
	{
		Id = 159;
		IsMcbe = true;
	}


	public float Intensity { get; set; }


	public float Duration { get; set; }


	public ShakeType Type { get; set; }


	public ShakeAction Action { get; set; }


	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(Intensity);
		Write(Duration);
		Write((byte)Type);
		Write((byte)Action);
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		Intensity = ReadFloat();
		Duration = ReadFloat();
		Type = (ShakeType)ReadByte();
		Action = (ShakeAction)ReadByte();
	}
}
