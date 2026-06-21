using Protocol.Network;

namespace Protocol.Codec.Packets;
public static class SimulationTypes
{
    public const byte Game = 0;
    public const byte Editor = 1;
    public const byte Test = 2;
    public const byte Invalid = 3;
}

public class McbeSimulationType : Packet
{
    public McbeSimulationType()
    {
        Id = 168;
        IsMcbe = true;
    }

    public byte SimulationType { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write(SimulationType);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        SimulationType = ReadByte();
    }
}
