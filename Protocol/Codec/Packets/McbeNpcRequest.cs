using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbeNpcRequest : Packet
{
    public ulong runtimeEntityId;
    public byte requestType;
    public string actions;
    public byte actionIndex;
    public string sceneName;
    public McbeNpcRequest()
    {
        Id = 0x62;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(requestType);
        Write(actions);
        Write(actionIndex);
        Write(sceneName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        requestType = ReadByte();
        actions = ReadString();
        actionIndex = ReadByte();
        sceneName = ReadString();
    }
}
