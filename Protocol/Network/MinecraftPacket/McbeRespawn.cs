namespace Protocol.Network.MinecraftPacket;
public class McbeRespawn : Packet
{
    public enum PlayerRespawnState : byte
    {
        SearchingForSpawn = 0,
        ReadyToSpawn = 1,
        ClientReadyToSpawn = 2
    }

    public ulong runtimeEntityId;
    public PlayerRespawnState state;
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
        Write((byte)state);
        WriteUnsignedVarLong(runtimeEntityId);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        x = ReadFloat();
        y = ReadFloat();
        z = ReadFloat();
        state = (PlayerRespawnState)ReadByte();
        runtimeEntityId = ReadUnsignedVarLong();
    }
}
