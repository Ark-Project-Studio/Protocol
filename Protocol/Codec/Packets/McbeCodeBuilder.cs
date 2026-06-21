using Protocol.Network;

namespace Protocol.Codec.Packets;

public class McbeCodeBuilder : Packet
{
	public McbeCodeBuilder()
	{
		Id = 150;
		IsMcbe = true;
	}


	public string URL { get; set; } = "";


	public bool ShouldOpenCodeBuilder { get; set; }


	protected override void EncodePacket()
	{
		base.EncodePacket();

		Write(URL);
		Write(ShouldOpenCodeBuilder);
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		URL = ReadString();
		ShouldOpenCodeBuilder = ReadBool();
	}
}
