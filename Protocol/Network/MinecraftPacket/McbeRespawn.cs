namespace Protocol.Network.MinecraftPacket;
public class McbeRespawn : Packet
{
    public enum RespawnState
    {
        Search = 0,
        Ready = 1,
        ClientReady = 2
    }

    public ulong runtimeEntityId;
    public byte state;
    public float x;
    public float y;
    public float z;
    public McbeRespawn()
    {
        Id = 0x2d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(x);
        Write(y);
        Write(z);
        Write(state);
        WriteUnsignedVarLong(runtimeEntityId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        x = ReadFloat();
        y = ReadFloat();
        z = ReadFloat();
        state = ReadByte();
        runtimeEntityId = ReadUnsignedVarLong();
    }
}
