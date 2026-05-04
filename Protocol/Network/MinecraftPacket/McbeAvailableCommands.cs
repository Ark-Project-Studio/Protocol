using Protocol.Minecraft;
using Protocol.Minecraft.Command;

namespace Protocol.Network.MinecraftPacket;
public class EnumData
{
    public EnumData(string name, string[] values)
    {
        Name = name;
        Values = values;
    }

    public string Name { get; set; }
    public string[] Values { get; set; }
}

public class McbeAvailableCommands : Packet
{
	public System.Collections.Generic.List<string> EnumValues { get; set; }
	public System.Collections.Generic.List<string> ChainedSubcommandValues { get; set; }
	public System.Collections.Generic.List<string> Suffixes { get; set; }
	public System.Collections.Generic.List<CommandEnum> Enums { get; set; }
	public System.Collections.Generic.List<ChainedSubcommand> ChainedSubcommands { get; set; }
	public System.Collections.Generic.List<Command> Commands { get; set; }
	public System.Collections.Generic.List<DynamicEnum> DynamicEnums { get; set; }
	public System.Collections.Generic.List<CommandEnumConstraint> Constraints { get; set; }
	public McbeAvailableCommands()
    {
        Id = 0x4c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
		if (EnumValues != null)
		{
			WriteSliceVarint32Length(EnumValues.ToArray(), Write);
		}
		else
		{
			WriteSignedVarInt(0);
		}

		if (ChainedSubcommandValues != null)
		{
			WriteSliceVarint32Length(ChainedSubcommandValues.ToArray(), Write);
		}
		else
		{
			WriteSignedVarInt(0);
		}

		if (Suffixes != null)
		{
			WriteSliceVarint32Length(Suffixes.ToArray(), Write);
		}
		else
		{
			WriteSignedVarInt(0);
		}

		if (Enums != null)
		{
			WriteSliceVarint32Length(Enums.ToArray(), (v) => Write(v, EnumValues));
		}
		else
		{
			WriteSignedVarInt(0);
		}

		if (ChainedSubcommands != null)
		{
			WriteSliceVarint32Length(ChainedSubcommands.ToArray(), Write);
		}
		else
		{
			WriteSignedVarInt(0);
		}

		if (Commands != null)
		{
			WriteSliceVarint32Length(Commands.ToArray(), Write);
		}
		else
		{
			WriteSignedVarInt(0);
		}

		if (DynamicEnums != null)
		{
			WriteSliceVarint32Length(DynamicEnums.ToArray(), Write);
		}
		else
		{
			WriteSignedVarInt(0);
		}

		if (Constraints != null)
		{
			WriteSliceVarint32Length(Constraints.ToArray(), Write);
		}
		else
		{
			WriteSignedVarInt(0);
		}
	}

    protected override void DecodePacket()
    {
        base.DecodePacket();

        EnumValues = new System.Collections.Generic.List<string>(ReadSlice(ReadString));
        ChainedSubcommandValues = new System.Collections.Generic.List<string>(ReadSlice(ReadString));
        Suffixes = new System.Collections.Generic.List<string>(ReadSlice(ReadString));
		

		// Read Enums with context
		uint enumCount = ReadUnsignedVarInt();
		Enums = new System.Collections.Generic.List<CommandEnum>();
		for (int i = 0; i < enumCount; i++)
		{
			Enums.Add(ReadCommandEnum(EnumValues));
		}

		ChainedSubcommands = new System.Collections.Generic.List<ChainedSubcommand>(ReadSlice(ReadChainedSubcommand));
		Commands = new System.Collections.Generic.List<Command>(ReadSlice(ReadCommand));
		DynamicEnums = new System.Collections.Generic.List<DynamicEnum>(ReadSlice(ReadDynamicEnum));
		Constraints = new System.Collections.Generic.List<CommandEnumConstraint>(ReadSlice(ReadCommandEnumConstraint));

	}

  
}
