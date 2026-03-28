using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeAnvilDamage : Packet
{
    public BlockCoordinates coordinates;
    public byte damageAmount;
    public McbeAnvilDamage()
    {
        Id = 0x8D;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(damageAmount);
        Write(coordinates);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        damageAmount = ReadByte();
        coordinates = ReadBlockCoordinates();
    }
}
