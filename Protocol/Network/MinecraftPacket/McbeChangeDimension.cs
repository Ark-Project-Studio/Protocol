using System.Numerics;

namespace Protocol.Network.MinecraftPacket;
public class McbeChangeDimension : Packet
{
    public int dimension;
    public Vector3 position;
    public bool respawn;
    public Optional<uint> loadingScreenId;

    public McbeChangeDimension()
    {
        Id = 0x3d;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteSignedVarInt(dimension);
        Write(position);
        Write(respawn);
        Write(loadingScreenId.HasValue);
        if (loadingScreenId.HasValue)
        {
            Write(loadingScreenId.Value);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        dimension = ReadSignedVarInt();
        position = ReadVector3();
        respawn = ReadBool();
        bool hasLoadingScreenId = ReadBool();
        if (hasLoadingScreenId)
        {
            loadingScreenId = new Optional<uint>(ReadUint());
        }
    }
}
