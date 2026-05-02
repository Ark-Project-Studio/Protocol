namespace Protocol.Network.MinecraftPacket;
public class McbePlayerUpdateEntityOverrides : Packet
{
    public enum UpdateType : byte
    {
        ClearOverrides = 0,
        RemoveOverride = 1,
        SetIntOverride = 2,
        SetFloatOverride = 3
    }

    public McbePlayerUpdateEntityOverrides()
    {
        Id = 325;
        IsMcbe = true;
    }

    public ulong EntityRuntimeID { get; set; }
    public uint PropertyIndex { get; set; }
    public UpdateType Type { get; set; }
    public int IntValue { get; set; }
    public float FloatValue { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(EntityRuntimeID);
        WriteUnsignedVarInt(PropertyIndex);
        Write((byte)Type);
        if (Type == UpdateType.SetIntOverride)
            Write(IntValue);
        else if (Type == UpdateType.SetFloatOverride)
            Write(FloatValue);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        EntityRuntimeID = ReadUnsignedVarLong();
        PropertyIndex = ReadUnsignedVarInt();
        Type = (UpdateType)ReadByte();
        IntValue = 0;
        FloatValue = 0.0f;
        if (Type == UpdateType.SetIntOverride)
            IntValue = ReadInt();
        else if (Type == UpdateType.SetFloatOverride)
            FloatValue = ReadFloat();
    }
}
