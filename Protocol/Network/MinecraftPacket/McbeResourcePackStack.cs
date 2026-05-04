using Protocol.Minecraft;
using Protocol.Minecraft.Level;
using Protocol.Minecraft.Level.ResourcePacks;

namespace Protocol.Network.MinecraftPacket;

public class McbeResourcePackStack : Packet
{
	public bool TexturePackRequired { get; set; }
	public StackResourcePack[] TexturePacks { get; set; }
	public string BaseGameVersion { get; set; }
	public Experiments.Experiment[] Experiments { get; set; }
	public bool ExperimentsPreviouslyToggled { get; set; }
	public bool IncludeEditorPacks { get; set; }

	public McbeResourcePackStack()
	{
		Id = 0x07;
		IsMcbe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		Write(TexturePackRequired);

		WriteSlice(TexturePacks,Write);

		Write(BaseGameVersion);

		Write(Experiments);

		Write(ExperimentsPreviouslyToggled);
		Write(IncludeEditorPacks);
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();


		TexturePackRequired = ReadBool();
		TexturePacks = ReadSlice(ReadStackResourcePack);
		BaseGameVersion = ReadString();

		// Read Experiments using SliceUint32Length
		Experiments = ReadExperiments();

		ExperimentsPreviouslyToggled = ReadBool();
		IncludeEditorPacks = ReadBool();

	}

}
