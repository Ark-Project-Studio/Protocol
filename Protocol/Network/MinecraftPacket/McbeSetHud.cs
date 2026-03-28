namespace Protocol.Network.MinecraftPacket;
public static class HudElement
{
    public const int PaperDoll = 0;
    public const int Armour = 1;
    public const int ToolTips = 2;
    public const int TouchControls = 3;
    public const int Crosshair = 4;
    public const int HotBar = 5;
    public const int Health = 6;
    public const int ProgressBar = 7;
    public const int Hunger = 8;
    public const int AirBubbles = 9;
    public const int HorseHealth = 10;
    public const int StatusEffects = 11;
    public const int ItemText = 12;
}

public static class HudVisibility
{
    public const int Hide = 0;
    public const int Reset = 1;
}

public class McbeSetHud : Packet
{
    public McbeSetHud()
    {
        Id = 308;
        IsMcbe = true;
    }

    public int[] Elements { get; set; } = new int[0];
    public int Visibility { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt((uint)(Elements?.Length ?? 0));
        if (Elements != null)
            foreach (var element in Elements)
                WriteSignedVarInt(element);
        WriteSignedVarInt(Visibility);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var count = ReadUnsignedVarInt();
        Elements = new int[count];
        for (var i = 0; i < count; i++)
            Elements[i] = ReadSignedVarInt();
        Visibility = ReadSignedVarInt();
    }
}
