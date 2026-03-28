namespace Protocol.Network.MinecraftPacket;
public class McbeAdventureSettings : Packet
{
    public uint actionPermissions;
    public uint commandPermission;
    public uint customStoredPermissions;
    public long entityUniqueId;
    public uint flags;
    public uint permissionLevel;
    public McbeAdventureSettings()
    {
        Id = 0x37;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(flags);
        WriteUnsignedVarInt(commandPermission);
        WriteUnsignedVarInt(actionPermissions);
        WriteUnsignedVarInt(permissionLevel);
        WriteUnsignedVarInt(customStoredPermissions);
        Write(entityUniqueId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        flags = ReadUnsignedVarInt();
        commandPermission = ReadUnsignedVarInt();
        actionPermissions = ReadUnsignedVarInt();
        permissionLevel = ReadUnsignedVarInt();
        customStoredPermissions = ReadUnsignedVarInt();
        entityUniqueId = ReadLong();
    }
}
