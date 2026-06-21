using Protocol.Network;

public class EducationExternalLinkSettings
{
	public string URL { get; set; } = "";


	public string DisplayName { get; set; } = "";
}

public class EducationAgentCapabilities
{
	public Optional<bool> CanModifyBlocks { get; set; } = new();
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


	public Optional<EducationAgentCapabilities> AgentCapabilities { get; set; } = new();


	public Optional<string> OverrideURI { get; set; } = new();


	public bool AlwaysFalse { get; set; }


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


		Write(AgentCapabilities.HasValue);
		if (AgentCapabilities.HasValue)
		{
			Write(AgentCapabilities.Value.CanModifyBlocks.HasValue);
			if (AgentCapabilities.Value.CanModifyBlocks.HasValue) Write(AgentCapabilities.Value.CanModifyBlocks.Value);
		}


		Write(OverrideURI.HasValue);
		if (OverrideURI.HasValue) Write(OverrideURI.Value);

		Write(AlwaysFalse);


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


		AgentCapabilities.HasValue = ReadBool();
		if (AgentCapabilities.HasValue)
		{
			var capabilities = new EducationAgentCapabilities();
			capabilities.CanModifyBlocks.HasValue = ReadBool();
			if (capabilities.CanModifyBlocks.HasValue) capabilities.CanModifyBlocks.Value = ReadBool();
			AgentCapabilities.Value = capabilities;
		}


		OverrideURI.HasValue = ReadBool();
		if (OverrideURI.HasValue) OverrideURI.Value = ReadString();

		AlwaysFalse = ReadBool();


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
