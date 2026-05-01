using System.Numerics;

namespace Protocol.Network.MinecraftPacket;

public enum InteractActionType : byte
{
	Invalid = 0,
	StopRiding = 3,
	InteractUpdate = 4,
	NpcOpen = 5,
	OpenInventory = 6
}
public class McbeInteract : Packet
{
	public InteractActionType ActionType { get; set; }
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
		Write((byte)ActionType);
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

		ActionType = (InteractActionType)ReadByte();
        TargetEntityRuntimeID = ReadUnsignedVarLong();

		if (ReadBool())
		{
			Position = new Optional<System.Numerics.Vector3>(ReadVector3());
		}
	}
}
