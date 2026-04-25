namespace Protocol.Network.MinecraftPacket;
public enum GraphicsModeType : byte
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

    public GraphicsModeType GraphicsMode { get; set; }
    public bool FilterProfanity { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)GraphicsMode);
        Write(FilterProfanity);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        GraphicsMode = (GraphicsModeType)ReadByte();
        FilterProfanity = ReadBool();
    }
}
