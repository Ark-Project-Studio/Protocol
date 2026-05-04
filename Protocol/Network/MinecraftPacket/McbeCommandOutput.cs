using Protocol.Minecraft;
using Protocol.Minecraft.Command;

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
	public CommandOutputType OutputType { get; set; }
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
	private string CommandOutputTypeToString(CommandOutputType outputType)
	{
		switch (outputType)
		{
			case CommandOutputType.None:
				return "none";
			case CommandOutputType.LastOutput:
				return "lastoutput";
			case CommandOutputType.Silent:
				return "silent";
			case CommandOutputType.AllOutput:
				return "alloutput";
			case CommandOutputType.DataSet:
				return "dataset";
			default:
				return "unknown";
		}
	}

	private CommandOutputType CommandOutputTypeFromString(string s)
	{
		switch (s)
		{
			case "none":
				return CommandOutputType.None;
			case "lastoutput":
				return CommandOutputType.LastOutput;
			case "silent":
				return CommandOutputType.Silent;
			case "alloutput":
				return CommandOutputType.AllOutput;
			case "dataset":
				return CommandOutputType.DataSet;
			default:
				throw new FormatException($"Unknown output type: {s}");
		}
	}

}
