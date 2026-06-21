using Protocol.Minecraft.Actor.Player;
using Protocol.Minecraft.Level.Block;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbePlayerAction : Packet
{
    public PlayerActionType actionId;
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
        WriteSignedVarInt((int)actionId);
        Write(coordinates);
        Write(resultCoordinates);
        WriteSignedVarInt(face);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        runtimeEntityId = ReadUnsignedVarLong();
        actionId = (PlayerActionType)ReadSignedVarInt();
        coordinates = ReadBlockCoordinates();
        resultCoordinates = ReadBlockCoordinates();
        face = ReadSignedVarInt();
    }
}
