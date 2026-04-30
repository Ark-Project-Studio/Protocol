using System.Numerics;

namespace Protocol.Network.MinecraftPacket;
public partial class McbeSpawnExperienceOrb : Packet
{
    public int xpValue;
    public Vector3 position;
    public McbeSpawnExperienceOrb()
    {
        Id = 0x42;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        BeforeEncode();
        Write(position);
        WriteSignedVarInt(xpValue);
        AfterEncode();
    }

    partial void BeforeEncode();
    partial void AfterEncode();
    protected override void DecodePacket()
    {
        base.DecodePacket();
        BeforeDecode();
        position = ReadVector3();
        xpValue = ReadSignedVarInt();
        AfterDecode();
    }

    partial void BeforeDecode();
    partial void AfterDecode();
}
