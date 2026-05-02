namespace Protocol.Network.MinecraftPacket;
public struct PlayerArmourDamageEntry
{
    public ArmorSlot ArmourSlot { get; set; }
    public short Damage { get; set; }

    public PlayerArmourDamageEntry(ArmorSlot armourSlot, short damage)
    {
        ArmourSlot = armourSlot;
        Damage = damage;
    }
}

public enum ArmorSlot : byte
{
    Head = 0,
    Torso = 1,
    Legs = 2,
    Feet = 3,
    Body = 4
}

public class McbePlayerArmourDamage : Packet
{
    [Flags]
    public enum PlayerArmorDamageFlags : byte
    {
        Head = 0,
        Torso = 1,
        Legs = 2,
        Feet = 3,
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
            Write((byte)entry.ArmourSlot);
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
            ArmorSlot armourSlot = (ArmorSlot)ReadByte();
            short damage = ReadShort();
            playerArmourDamageEntries.Add(new PlayerArmourDamageEntry(armourSlot, damage));
        }
    }
}
