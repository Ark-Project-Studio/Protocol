using Protocol.Utils;

namespace Protocol.Minecraft.Actor.Player;

public class PlayerListEntry
{
    public UUID uuid;
    public long actorUniqueId;
    public string playerName;
    public string xuid;
    public string platformChatId;
    public int buildPlatform;
    public Skin serializedSkin;
    public bool isTeacher;
    public bool isHost;
    public bool isSubClient;
    public int color;
    public bool skinTrusted;
}
