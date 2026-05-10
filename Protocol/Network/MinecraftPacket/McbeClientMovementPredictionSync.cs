using System.Numerics;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket;
public class McbeClientMovementPredictionSync : Packet
{
    private const int EntityDataFlagCount = 125;
    public McbeClientMovementPredictionSync()
    {
        Id = 322;
        IsMcbe = true;
    }

    public Bitset ActorFlags { get; set; } = new(EntityDataFlagCount, BigInteger.Zero);
    public float BoundingBoxScale { get; set; }
    public float BoundingBoxWidth { get; set; }
    public float BoundingBoxHeight { get; set; }
    public float MovementSpeed { get; set; }
    public float UnderwaterMovementSpeed { get; set; }
    public float LavaMovementSpeed { get; set; }
    public float JumpStrength { get; set; }
    public float Health { get; set; }
    public float Hunger { get; set; }
    public long EntityUniqueID { get; set; }
    public bool Flying { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteBitset(ActorFlags, EntityDataFlagCount);
        Write(BoundingBoxScale);
        Write(BoundingBoxWidth);
        Write(BoundingBoxHeight);
        Write(MovementSpeed);
        Write(UnderwaterMovementSpeed);
        Write(LavaMovementSpeed);
        Write(JumpStrength);
        Write(Health);
        Write(Hunger);
        Write(0.0f);
        Write(0x0f);
        Write(0x0f);
        WriteUnsignedVarInt((uint)EntityUniqueID);
        Write(Flying);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ActorFlags = ReadBitset(EntityDataFlagCount);
        BoundingBoxScale = ReadFloat();
        BoundingBoxWidth = ReadFloat();
        BoundingBoxHeight = ReadFloat();
        MovementSpeed = ReadFloat();
        UnderwaterMovementSpeed = ReadFloat();
        LavaMovementSpeed = ReadFloat();
        JumpStrength = ReadFloat();
        Health = ReadFloat();
        Hunger = ReadFloat();
        var f1 = ReadFloat();
        var f2 = ReadFloat();
        var f3 = ReadFloat();
        EntityUniqueID = ReadUnsignedVarInt();
        Flying = ReadBool();
    }
}
