using Protocol.Minecraft;
using Protocol.Minecraft.Actor;

namespace Protocol.Network.MinecraftPacket;
public class McbeClientCheatAbility : Packet
{
    public McbeClientCheatAbility()
    {
        Id = 197;
        IsMcbe = true;
    }

    public AbilityData AbilityData { get; set; } = new();

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(AbilityData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        AbilityData = ReadAbilityData();
    }


   
}
