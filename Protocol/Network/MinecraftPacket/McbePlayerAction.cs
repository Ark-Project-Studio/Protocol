using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbePlayerAction : Packet
{
    public int actionId;
    public BlockCoordinates coordinates;
    public int face;
    public BlockCoordinates resultCoordinates;
    public ulong runtimeEntityId;
    public McbePlayerAction()
    {
        Id = 0x24;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarLong(runtimeEntityId);
        WriteSignedVarInt(actionId);
        Write(coordinates);
        Write(resultCoordinates);
        WriteSignedVarInt(face);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        actionId = ReadSignedVarInt();
        coordinates = ReadBlockCoordinates();
        resultCoordinates = ReadBlockCoordinates();
        face = ReadSignedVarInt();
    }
}
