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

    public Optional<byte> GraphicsMode { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(GraphicsMode.HasValue);
        if (GraphicsMode.HasValue)
            Write(GraphicsMode.Value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var hasGraphicsMode = ReadBool();
        if (hasGraphicsMode)
        {
            var graphicsModeValue = ReadByte();
            GraphicsMode = new Optional<byte>(graphicsModeValue);
        }
    }
}
