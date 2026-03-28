namespace Protocol.Network.MinecraftPacket;

public class McbeSettingsCommand : Packet
{
	public McbeSettingsCommand()
	{
		Id = 140;
		IsMcbe = true;
	}


	public string CommandLine { get; set; }


	public bool SuppressOutput { get; set; }

	protected override void EncodePacket()
	{
		base.EncodePacket();
		Write(CommandLine);
		Write(SuppressOutput);
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		CommandLine = ReadString();
		SuppressOutput = ReadBool();
	}
}
