using System.Numerics;

namespace Protocol.Network.MinecraftPacket;
public class McbeSpawnParticleEffect : Packet
{
    public byte dimensionId;
    public long entityId;
    public string molangVariablesJson;
    public string particleName;
    public Vector3 position;
    public McbeSpawnParticleEffect()
    {
        Id = 0x76;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(dimensionId);
        WriteSignedVarLong(entityId);
        Write(position);
        Write(particleName);
        Write(molangVariablesJson);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        dimensionId = ReadByte();
        entityId = ReadSignedVarLong();
        position = ReadVector3();
        particleName = ReadString();
        molangVariablesJson = ReadString();
    }
}
