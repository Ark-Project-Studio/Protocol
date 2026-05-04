using Protocol.Minecraft;
using Protocol.Minecraft.Command;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public class McbeCommandRequest : Packet
{
	public string CommandLine { get; set; }
	public CommandOrigin CommandOrigin { get; set; }
	public bool Internal { get; set; }
	public string Version { get; set; }
	public McbeCommandRequest()
    {
        Id = 0x4d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
		Write(CommandLine);
		Write(CommandOrigin);
		Write(Internal);
		Write(Version);
	}

    protected override void DecodePacket()
    {
        base.DecodePacket();
        CommandLine = ReadString();
        CommandOrigin = ReadCommandOrigin();
        Internal = ReadBool();
        Version = ReadString();

    }
}
