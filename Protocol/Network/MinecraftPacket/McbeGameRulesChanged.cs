using Protocol.Minecraft.Actor.Player;

namespace Protocol.Network.MinecraftPacket;
public class McbeGameRulesChanged : Packet
{
	public System.Collections.Generic.List<GameRule> GameRules { get; set; }
	public McbeGameRulesChanged()
    {
        Id = 0x48;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSlice(GameRules.ToArray(),Write);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        GameRules = ReadSlice((() =>
        {
	        return ReadGameRule();
        })).ToList();
    }
}
