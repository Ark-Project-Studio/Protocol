namespace Protocol.Network.MinecraftPacket;
public enum PacketType : uint
{
    Empty = 0,
    InitiallyUnlockedRecipes = 1,
    NewlyUnlockedRecipes = 2,
    RemoveUnlockedRecipes = 3,
    RemoveAllUnlockedRecipes = 4
}

public class McbeUnlockedRecipes : Packet
{
    public McbeUnlockedRecipes()
    {
        Id = 199;
        IsMcbe = true;
    }

    public PacketType UnlockType { get; set; }
    public string[] Recipes { get; set; } = new string[0];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((uint)UnlockType);
        WriteUnsignedVarInt((uint)(Recipes?.Length ?? 0));
        if (Recipes != null)
            foreach (var recipe in Recipes)
                Write(recipe ?? string.Empty);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        UnlockType = (PacketType)ReadUint();
        var count = ReadUnsignedVarInt();
        Recipes = new string[count];
        for (var i = 0; i < count; i++)
            Recipes[i] = ReadString();
    }
}
