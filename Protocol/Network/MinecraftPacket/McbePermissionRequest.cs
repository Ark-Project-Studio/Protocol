namespace Protocol.Network.MinecraftPacket;
public class McbePermissionRequest : Packet
{
    public ushort flagss;
    public int permission;
    public long runtimeEntityId;
    public McbePermissionRequest()
    {
        Id = 0xb9;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(runtimeEntityId);
        WriteSignedVarInt(permission);
        Write(flagss);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadLong();
        permission = ReadSignedVarInt();
        flagss = ReadUshort();
    }
}
