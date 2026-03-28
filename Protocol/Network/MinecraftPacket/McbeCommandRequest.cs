using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public class McbeCommandRequest : Packet
{
    public string command;
    public uint commandType;
    public bool isinternal;
    public string requestId;
    public UUID unknownUuid;
    public int version;
    public McbeCommandRequest()
    {
        Id = 0x4d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(command);
        WriteUnsignedVarInt(commandType);
        Write(unknownUuid);
        Write(requestId);
        Write(isinternal);
        WriteSignedVarInt(version);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        command = ReadString();
        commandType = ReadUnsignedVarInt();
        unknownUuid = ReadUUID();
        requestId = ReadString();
        isinternal = ReadBool();
        version = ReadSignedVarInt();
    }
}
