using Protocol.Network;

public class EducationExternalLinkSettings
{
	public string URL { get; set; } = "";


	public string DisplayName { get; set; } = "";
}

public class McbeEducationSettings : Packet
{
	public McbeEducationSettings()
	{
		Id = 137;
		IsMcbe = true;
	}


	public string CodeBuilderDefaultURI { get; set; } = "";


	public string CodeBuilderTitle { get; set; } = "";


	public bool CanResizeCodeBuilder { get; set; }


	public bool DisableLegacyTitleBar { get; set; }


	public string PostProcessFilter { get; set; } = "";


	public string ScreenshotBorderPath { get; set; } = "";


	public Optional<bool> CanModifyBlocks { get; set; } = new();


	public Optional<string> OverrideURI { get; set; } = new();


	public bool HasQuiz { get; set; }


	public Optional<EducationExternalLinkSettings> ExternalLinkSettings { get; set; } = new();

	protected override void EncodePacket()
	{
		base.EncodePacket();
		Write(CodeBuilderDefaultURI);
		Write(CodeBuilderTitle);
		Write(CanResizeCodeBuilder);
		Write(DisableLegacyTitleBar);
		Write(PostProcessFilter);
		Write(ScreenshotBorderPath);


		Write(CanModifyBlocks.HasValue);
		if (CanModifyBlocks.HasValue) Write(CanModifyBlocks.Value);


		Write(OverrideURI.HasValue);
		if (OverrideURI.HasValue) Write(OverrideURI.Value);

		Write(HasQuiz);


		Write(ExternalLinkSettings.HasValue);
		if (ExternalLinkSettings.HasValue)
		{
			Write(ExternalLinkSettings.Value.URL);
			Write(ExternalLinkSettings.Value.DisplayName);
		}
	}

	protected override void DecodePacket()
	{
		base.DecodePacket();
		CodeBuilderDefaultURI = ReadString();
		CodeBuilderTitle = ReadString();
		CanResizeCodeBuilder = ReadBool();
		DisableLegacyTitleBar = ReadBool();
		PostProcessFilter = ReadString();
		ScreenshotBorderPath = ReadString();


		CanModifyBlocks.HasValue = ReadBool();
		if (CanModifyBlocks.HasValue) CanModifyBlocks.Value = ReadBool();


		OverrideURI.HasValue = ReadBool();
		if (OverrideURI.HasValue) OverrideURI.Value = ReadString();

		HasQuiz = ReadBool();


		ExternalLinkSettings.HasValue = ReadBool();
		if (ExternalLinkSettings.HasValue)
		{
			var settings = new EducationExternalLinkSettings();
			settings.URL = ReadString();
			settings.DisplayName = ReadString();
			ExternalLinkSettings.Value = settings;
		}
	}
}
