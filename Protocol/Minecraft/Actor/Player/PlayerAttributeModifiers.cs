using System.Text.Json.Serialization;

namespace Protocol.Minecraft.Actor.Player;

public class AttributeModifiers : Dictionary<string, AttributeModifier>
{
}

public class PlayerAttributes : Dictionary<string, PlayerAttribute>
{
}

public class EntityAttributes : Dictionary<string, EntityAttribute>
{
}

public class EntityLink
{
	public enum EntityLinkType : byte
	{
		None = 0,
		Riding = 1,
		Passenger = 2
	}

	public EntityLink(long fromEntityId, long toEntityId, EntityLinkType type, bool immediate, bool causedByRider,
		float vehicleAngularVelocity)
	{
		FromEntityId = fromEntityId;
		ToEntityId = toEntityId;
		Type = type;
		Immediate = immediate;
		CausedByRider = causedByRider;
		VehicleAngularVelocity = vehicleAngularVelocity;
	}

	public long FromEntityId { get; set; }
	public long ToEntityId { get; set; }
	public EntityLinkType Type { get; set; }
	public bool Immediate { get; set; }
	public bool CausedByRider { get; set; }
	public float VehicleAngularVelocity { get; set; }
}

public class Itemstate
{
	[JsonPropertyName("runtime_id")] public short Id { get; set; }

	[JsonPropertyName("name")] public string Name { get; set; }

	[JsonPropertyName("component_based")] public bool ComponentBased { get; set; }

	[JsonPropertyName("version")] public int Version { get; set; }

	[JsonPropertyName("components")] public byte[] Components { get; set; }
}