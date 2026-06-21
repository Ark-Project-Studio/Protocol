using Protocol.Minecraft;
using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public class McbeMoveEntity : Packet
{
    public byte flags;
    public Vector3 position;
    public byte rotationX;
    public byte rotationY;
    public byte rotationYHead;
    public bool forceCompletion;
    public ulong runtimeEntityId;
    public McbeMoveEntity()
    {
        Id = 0x12;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        Write(flags);
        Write(position);
        Write(rotationX);
        Write(rotationY);
        Write(rotationYHead);
        Write(forceCompletion);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        flags = ReadByte();
        position = ReadVector3();
        rotationX = ReadByte();
        rotationY = ReadByte();
        rotationYHead = ReadByte();
        forceCompletion = ReadBool();
    }
}
