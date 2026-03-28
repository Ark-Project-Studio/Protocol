using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;

public enum CommandOutputType : byte
{
	None = 0,
	LastOutput = 1,
	Silent = 2,
	AllOutput = 3,
	DataSet = 4
}

public class McbeCommandOutput : Packet
{
    public McbeCommandOutput()
    {
        Id = 0x4f;
        IsMcbe = true;
    }

	public CommandOrigin CommandOrigin { get; set; }
	public byte OutputType { get; set; }
	public uint SuccessCount { get; set; }
	public System.Collections.Generic.List<CommandOutputMessage> OutputMessages { get; set; }
	public Optional<string> DataSet { get; set; }

	protected override void EncodePacket()
    {
        base.EncodePacket();
		Write(CommandOrigin);

		string outputTypeStr = CommandOutputTypeToString(OutputType);
		Write(outputTypeStr);

		Write(SuccessCount);

		if (OutputMessages != null)
		{
			WriteSlice(OutputMessages.ToArray(), Write);
		}
		else
		{
			WriteSignedVarInt(0);
		}

		Write(DataSet.HasValue);
		if (DataSet.HasValue)
		{
			Write(DataSet.Value);
		}
	}

    protected override void DecodePacket()
    {
        base.DecodePacket();

        CommandOrigin = ReadCommandOrigin();


		string outputTypeStr = ReadString();
		OutputType = CommandOutputTypeFromString(outputTypeStr);
		SuccessCount = ReadUint();
		OutputMessages = new System.Collections.Generic.List<CommandOutputMessage>(ReadSlice(ReadCommandOutputMessage));

		if (ReadBool())
		{
			DataSet = new Optional<string>(ReadString());
		}
	}
	private string CommandOutputTypeToString(byte outputType)
	{
		switch (outputType)
		{
			case (byte)CommandOutputType.None:
				return "none";
			case (byte)CommandOutputType.LastOutput:
				return "lastoutput";
			case (byte)CommandOutputType.Silent:
				return "silent";
			case (byte)CommandOutputType.AllOutput:
				return "alloutput";
			case (byte)CommandOutputType.DataSet:
				return "dataset";
			default:
				return "unknown";
		}
	}

	private byte CommandOutputTypeFromString(string s)
	{
		switch (s)
		{
			case "none":
				return (byte)CommandOutputType.None;
			case "lastoutput":
				return (byte)CommandOutputType.LastOutput;
			case "silent":
				return (byte)CommandOutputType.Silent;
			case "alloutput":
				return (byte)CommandOutputType.AllOutput;
			case "dataset":
				return (byte)CommandOutputType.DataSet;
			default:
				throw new FormatException($"Unknown output type: {s}");
		}
	}

}
