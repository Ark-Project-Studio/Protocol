namespace Protocol.Network.MinecraftPacket;
public class McbeUpdateSoftEnum : Packet
{
    public enum SoftEnumUpdateType : byte
    {
        Add = 0,
        Remove = 1,
        Replace = 2
    }

    public string enumName;
    public string[] values;
    public SoftEnumUpdateType updateType;

    public McbeUpdateSoftEnum()
    {
        Id = 0x72;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(enumName);
        WriteSlice(values ?? System.Array.Empty<string>(), Write);
        Write((byte)updateType);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        enumName = ReadString();
        values = ReadSlice(ReadString);
        updateType = (SoftEnumUpdateType)ReadByte();
    }
}
