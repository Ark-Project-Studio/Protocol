namespace Protocol.Network.MinecraftPacket;
public class McbeHurtArmor : Packet
{
    public ulong armorSlotFlags;
    public int cause;
    public int health;
    public McbeHurtArmor()
    {
        Id = 0x26;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteVarInt(cause);
        WriteSignedVarInt(health);
        WriteUnsignedVarLong(armorSlotFlags);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        cause = ReadVarInt();
        health = ReadSignedVarInt();
        armorSlotFlags = ReadUnsignedVarLong();
    }
}
