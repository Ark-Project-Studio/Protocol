namespace Protocol.Network.MinecraftPacket;
public struct PlayerArmourDamageEntry
{
    public byte ArmourSlot { get; set; }
    public short Damage { get; set; }

    public PlayerArmourDamageEntry(byte armourSlot, short damage)
    {
        ArmourSlot = armourSlot;
        Damage = damage;
    }
}

public class McbePlayerArmourDamage : Packet
{
    [Flags]
    public enum PlayerArmorDamageFlags : byte
    {
        Helmet = 0,
        Chestplate = 1,
        Leggings = 2,
        Boots = 3,
        Body = 4
    }

    public List<PlayerArmourDamageEntry> playerArmourDamageEntries;
    public McbePlayerArmourDamage()
    {
        Id = 149;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        int count = playerArmourDamageEntries?.Count ?? 0;
        WriteVarInt(count);
        foreach (var entry in playerArmourDamageEntries)
        {
            Write(entry.ArmourSlot);
            Write(entry.Damage);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        playerArmourDamageEntries = new List<PlayerArmourDamageEntry>();
        int count = ReadVarInt();
        for (int i = 0; i < count; i++)
        {
            byte armourSlot = ReadByte();
            short damage = ReadShort();
            playerArmourDamageEntries.Add(new PlayerArmourDamageEntry(armourSlot, damage));
        }
    }
}
