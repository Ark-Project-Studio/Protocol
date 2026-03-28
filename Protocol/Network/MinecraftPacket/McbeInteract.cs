using System.Numerics;

namespace Protocol.Network.MinecraftPacket;

public enum InteractActionType : byte
{
	LeaveVehicle = 3,
	MouseOverEntity = 4,
	NPCOpen = 5,
	OpenInventory = 6
}
public class McbeInteract : Packet
{
	public byte ActionType { get; set; }
	public ulong TargetEntityRuntimeID { get; set; }
	public Optional<System.Numerics.Vector3> Position { get; set; }
	public McbeInteract()
    {
        Id = 0x21;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
		Write(ActionType);
		WriteUnsignedVarLong(TargetEntityRuntimeID);

		Write(Position.HasValue);
		if (Position.HasValue)
		{
			Write(Position.Value);
		}
	}

    protected override void DecodePacket()
    {
        base.DecodePacket();

        ActionType = ReadByte();
        TargetEntityRuntimeID = ReadUnsignedVarLong();

		if (ReadBool())
		{
			Position = new Optional<System.Numerics.Vector3>(ReadVector3());
		}
	}
}
