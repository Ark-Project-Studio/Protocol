using Protocol.Utils;

namespace Protocol.Minecraft;


public enum PackSettingType : int
{
	Float = 0,
	Bool = 1,
	String = 2,
}

public class PackSetting
{
	public string Name { get; set; } = string.Empty;


	public object Value { get; set; } = null;


	public PackSetting()
	{
	}


	public PackSetting(string name, object value)
	{
		Name = name ?? string.Empty;
		Value = value;
	}
}

public struct TexturePackInfo
{
	public Protocol.Utils.UUID UUID { get; set; }
	public string Version { get; set; }
	public ulong Size { get; set; }
	public string ContentKey { get; set; }
	public string SubPackName { get; set; }
	public string ContentIdentity { get; set; }
	public bool HasScripts { get; set; }
	public bool AddonPack { get; set; }
	public bool RTXEnabled { get; set; }
	public string DownloadURL { get; set; }
}
public struct StackResourcePack
{
	public string UUID { get; set; }
	public string Version { get; set; }
	public string SubPackName { get; set; }
}

public struct ResourcePackStack
{
	public bool TexturePackRequired { get; set; }
	public System.Collections.Generic.List<StackResourcePack> TexturePacks { get; set; }
	public string BaseGameVersion { get; set; }
	public Experiments Experiments { get; set; }
	public bool ExperimentsPreviouslyToggled { get; set; }
	public bool IncludeEditorPacks { get; set; }
}

public class PackIdVersion
{
	public string Id { get; set; }
	public string Version { get; set; }
	public string SubPackName { get; set; }
}

public class ResourcePackIds : List<string>
{
}

public enum ResourcePackType : byte
{
	Addon = 1,
	Cached = 2,
	CopyProtected = 3,
	Behaviour = 4,
	PersonaPiece = 5,
	Resources = 6,
	Skins = 7,
	WorldTemplate = 8
}

public class Header
{
	public string Description { get; set; }
	public string Name { get; set; }
	public string Uuid { get; set; }
	public List<int> Version { get; set; }
	public List<int> MinEngineVersion { get; set; }
}

public class Module
{
	public string Type { get; set; }
	public string Uuid { get; set; }
	public List<int> Version { get; set; }
}

public class manifestStructure
{
	public int FormatVersion { get; set; }
	public Header Header { get; set; }
	public List<Module> Modules { get; set; }
}

public class PlayerPackMapData
{
	public string pack { get; set; }
	public ResourcePackType type { get; set; }
}