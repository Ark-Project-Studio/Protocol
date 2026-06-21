using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public class McbeSpawnParticleEffect : Packet
{
    public byte dimensionId;
    public long entityId;
    public Optional<string> molangVariablesJson = new();
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
        Write(molangVariablesJson.HasValue);
        if (molangVariablesJson.HasValue)
            Write(molangVariablesJson.Value);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        dimensionId = ReadByte();
        entityId = ReadSignedVarLong();
        position = ReadVector3();
        particleName = ReadString();
        if (ReadBool())
            molangVariablesJson = new Optional<string>(ReadString());
    }
}
