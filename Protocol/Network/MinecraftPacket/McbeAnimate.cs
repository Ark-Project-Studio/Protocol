namespace Protocol.Network.MinecraftPacket;
public class McbeAnimate : Packet
{
    public byte actionId;
    public ulong runtimeEntityId;
    public float Data;
    public Optional<string> swingSource = new();
    public McbeAnimate()
    {
        Id = 0x2c;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(actionId);
        WriteUnsignedVarLong(runtimeEntityId);
        Write(Data);
        Write(swingSource.HasValue);
        if (swingSource.HasValue)
            Write(swingSource.Value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        actionId = ReadByte();
        runtimeEntityId = ReadUnsignedVarLong();
        Data = ReadFloat();
        if (ReadBool())
            swingSource = new Optional<string>(ReadString());
    }
}
