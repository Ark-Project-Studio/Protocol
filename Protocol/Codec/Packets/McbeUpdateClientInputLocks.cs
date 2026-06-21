using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets;
public static class ClientInputLocks
{
    public const uint Camera = 1 << (0 + 1);
    public const uint Movement = 1 << (1 + 1);
}

public class McbeUpdateClientInputLocks : Packet
{
    public McbeUpdateClientInputLocks()
    {
        Id = 196;
        IsMcbe = true;
    }

    public uint InputLockComponentData { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt(InputLockComponentData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        InputLockComponentData = ReadUnsignedVarInt();
    }
}
