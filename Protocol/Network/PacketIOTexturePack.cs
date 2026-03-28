using Protocol.Minecraft;

namespace Protocol.Network
{
	public partial class Packet 
	{
		public void Write(TexturePackInfo value)
		{
			Write(value.UUID);
			Write(value.Version);
			Write(value.Size);
			Write(value.ContentKey);
			Write(value.SubPackName);
			Write(value.ContentIdentity);
			Write(value.HasScripts);
			Write(value.AddonPack);
			Write(value.RTXEnabled);
			Write(value.DownloadURL);
		}

		public TexturePackInfo ReadTexturePackInfo()
		{
			return new TexturePackInfo
			{
				UUID = ReadUUID(),
				Version = ReadString(),
				Size = ReadUlong(),
				ContentKey = ReadString(),
				SubPackName = ReadString(),
				ContentIdentity = ReadString(),
				HasScripts = ReadBool(),
				AddonPack = ReadBool(),
				RTXEnabled = ReadBool(),
				DownloadURL = ReadString()
			};
		}
		public void Write(StackResourcePack value)
		{
			Write(value.UUID);
			Write(value.Version);
			Write(value.SubPackName);
		}

		public StackResourcePack ReadStackResourcePack()
		{
			return new StackResourcePack
			{
				UUID = ReadString(),
				Version = ReadString(),
				SubPackName = ReadString()
			};
		}
	}
}
