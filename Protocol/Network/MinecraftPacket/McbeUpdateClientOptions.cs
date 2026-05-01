namespace Protocol.Network.MinecraftPacket;
public enum GraphicsMode : byte
{
    Simple = 0,
    Fancy = 1,
    Advanced = 2,
    RayTraced = 3
}

public class McbeUpdateClientOptions : Packet
{
    public McbeUpdateClientOptions()
    {
        Id = 323;
        IsMcbe = true;
    }

    public Optional<GraphicsMode> GraphicsModeChange { get; set; } = new();
    public Optional<bool> FilterProfanity { get; set; } = new();

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(GraphicsModeChange.HasValue);
        if (GraphicsModeChange.HasValue)
            Write((byte)GraphicsModeChange.Value);
        Write(FilterProfanity.HasValue);
        if (FilterProfanity.HasValue)
            Write(FilterProfanity.Value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        if (ReadBool())
            GraphicsModeChange = new Optional<GraphicsMode>((GraphicsMode)ReadByte());
        if (ReadBool())
            FilterProfanity = new Optional<bool>(ReadBool());
    }
}
