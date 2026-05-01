using static System.Net.Mime.MediaTypeNames;

namespace Protocol.Network.MinecraftPacket;
// 鏂囨湰绫诲瀷鏋氫妇
public enum TextType : byte
{
	Raw = 0,
	Chat = 1,
	Translate = 2,
	Popup = 3,
	JukeboxPopup = 4,
	Tip = 5,
	SystemMessage = 6,
	Whisper = 7,
	Announcement = 8,
	TextObjectWhisper = 9,
	TextObject = 10,
	TextObjectAnnouncement = 11
}

public enum TextCategory
{
	MessageOnly = 0,
	AuthoredMessage,
	MessageWithParameters
}
public class McbeText : Packet
{

	public TextType TextType { get; set; }
	public bool NeedsTranslation { get; set; }
	public string SourceName { get; set; }
	public string Message { get; set; }
	public System.Collections.Generic.List<string> Parameters { get; set; }
	public string XUID { get; set; }
	public string PlatformChatID { get; set; }
	public Optional<string> FilteredMessage { get; set; }

	public McbeText()
	{
		Id = 0x09;
		IsMcbe = true;
	}

	protected override void EncodePacket()
	{
		base.EncodePacket();
		Write(NeedsTranslation);

		byte categoryType;
		switch ((TextType)TextType)
		{
			case TextType.Raw:
			case TextType.Tip:
			case TextType.SystemMessage:
			case TextType.TextObjectWhisper:
			case TextType.TextObjectAnnouncement:
			case TextType.TextObject:
				categoryType = (byte)TextCategory.MessageOnly;
				break;
			case TextType.Chat:
			case TextType.Whisper:
			case TextType.Announcement:
				categoryType = (byte)TextCategory.AuthoredMessage;
				break;
			default:
				categoryType = (byte)TextCategory.MessageWithParameters;
				break;
		}

		Write(categoryType);
		Write((byte)TextType);

		switch ((TextType)TextType)
		{
			case TextType.Chat:
			case TextType.Whisper:
			case TextType.Announcement:
				Write(SourceName);
				Write(Message);
				break;
			case TextType.Raw:
			case TextType.Tip:
			case TextType.SystemMessage:
			case TextType.TextObject:
			case TextType.TextObjectWhisper:
			case TextType.TextObjectAnnouncement:
				Write(Message);
				break;
			case TextType.Translate:
			case TextType.Popup:
			case TextType.JukeboxPopup:
				Write(Message);
				if (Parameters != null)
				{
					WriteSlice(Parameters.ToArray(), Write);
				}
				else
				{
					WriteSignedVarInt(0);
				}
				break;
		}

		if (string.IsNullOrEmpty(Message))
		{
			throw new FormatException("Message cannot be empty");
		}

		Write(XUID);
		Write(PlatformChatID);

		Write(FilteredMessage.HasValue);
		if (FilteredMessage.HasValue)
		{
			Write(FilteredMessage.Value);
		}
	}


	protected override void DecodePacket()
	{
		base.DecodePacket();

		NeedsTranslation = ReadBool();
		

		var categoryType = ReadByte();
		TextType = (TextType)ReadByte();

		switch ((TextType)TextType)
		{
			case TextType.Chat:
			case TextType.Whisper:
			case TextType.Announcement:
				SourceName = ReadString();
				Message = ReadString();
				break;
			case TextType.Raw:
			case TextType.Tip:
			case TextType.System:
			case TextType.Object:
			case TextType.ObjectWhisper:
			case TextType.ObjectAnnouncement:
				Message = ReadString();
				break;
			case TextType.Translation:
			case TextType.Popup:
			case TextType.JukeboxPopup:
				Message = ReadString();
				var paramArray = ReadSlice(ReadString);
				Parameters = new System.Collections.Generic.List<string>(paramArray);
				break;
		}

		if (string.IsNullOrEmpty(Message))
		{
			throw new FormatException("Message cannot be empty");
		}

		XUID = ReadString();
		PlatformChatID = ReadString();

		var hasFilteredMessage = ReadBool();
		FilteredMessage = new Optional<string>();
		if (hasFilteredMessage)
		{
			FilteredMessage.Value = ReadString();
		}
	}


	
}
