namespace Protocol.Minecraft.Actor.Player;

public class AttributeModifier
{
	public string Id { get; set; }
	public string Name { get; set; }
	public float Amount { get; set; }
	public int Operations { get; set; }
	public int Operand { get; set; }
	public bool Serializable { get; set; }

	public override string ToString()
	{
		return
			$"{{Id: {Id}, Name: {Name}, Amount: {Amount}, Operations: {Operations}, Operand: {Operand}, Serializable: {Serializable}}}";
	}
}

public class PlayerAttribute
{
	public string Name { get; set; }
	public float MinValue { get; set; }
	public float MaxValue { get; set; }
	public float Value { get; set; }
	public float DefaultMinValue { get; set; }
	public float DefaultMaxValue { get; set; }
	public float Default { get; set; }
	public AttributeModifiers Modifiers { get; set; }

	public override string ToString()
	{
		return $"{{Name: {Name}, MinValue: {MinValue}, MaxValue: {MaxValue}, Value: {Value}, Default: {Default}}}";
	}
}

public class EntityAttribute
{
	public string Name { get; set; }
	public float MinValue { get; set; }
	public float MaxValue { get; set; }
	public float Value { get; set; }

	public override string ToString()
	{
		return $"{{Name: {Name}, MinValue: {MinValue}, MaxValue: {MaxValue}, Value: {Value}}}";
	}
}

public struct GameRule
{
	public string Name { get; set; }
	public bool CanBeModifiedByPlayer { get; set; }
	public object Value { get; set; }
}