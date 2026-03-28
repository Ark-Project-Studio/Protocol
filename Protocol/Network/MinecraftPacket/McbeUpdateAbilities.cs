using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateAbilities : Packet
{
    public byte commandPermissions;
    public long entityUniqueId;
    public AbilityLayers layers;
    public byte playerPermissions;
    public McbeUpdateAbilities()
    {
        Id = 0xbb;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(entityUniqueId);
        Write(playerPermissions);
        Write(commandPermissions);
        Write(layers);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        entityUniqueId = ReadLong();
        playerPermissions = ReadByte();
        commandPermissions = ReadByte();
        layers = ReadAbilityLayers();
    }
}
