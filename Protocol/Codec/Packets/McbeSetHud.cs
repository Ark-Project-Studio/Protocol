using Protocol.Network;

namespace Protocol.Codec.Packets;
public enum HudElement : int
{
    PaperDoll = 0,
    Armor = 1,
    ToolTips = 2,
    TouchControls = 3,
    Crosshair = 4,
    HotBar = 5,
    Health = 6,
    ProgressBar = 7,
    Hunger = 8,
    AirBubbles = 9,
    HorseHealth = 10,
    StatusEffects = 11,
    ItemText = 12,
    Count = 13
}

public enum HudVisibility : int
{
    Hide = 0,
    Reset = 1,
    Count = 2
}

public class McbeSetHud : Packet
{
    public McbeSetHud()
    {
        Id = 308;
        IsMcbe = true;
    }

    public HudElement[] Elements { get; set; } = new HudElement[0];
    public HudVisibility Visibility { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt((uint)(Elements?.Length ?? 0));
        if (Elements != null)
            foreach (var element in Elements)
                WriteSignedVarInt((int)element);
            WriteSignedVarInt((int)Visibility);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var count = ReadUnsignedVarInt();
        Elements = new HudElement[count];
        for (var i = 0; i < count; i++)
            Elements[i] = (HudElement)ReadSignedVarInt();
        Visibility = (HudVisibility)ReadSignedVarInt();
    }
}
