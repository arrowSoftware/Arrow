public class StatModifier
{
	public readonly float Value;
	public readonly StatModifierType Type;
	public readonly int Order;
	public readonly object Source;

	public StatModifier(float value, StatModifierType type, int order, object source)
	{
		Value = value;
		Type = type;
		Order = order;
		Source = source;
	}

	// Calls the "Main" constructor and sets Order and Source to their default values: (int)type and null, respectively.
	public StatModifier(float value, StatModifierType type) : this(value, type, (int)type, null) { }

	// Sets Source to its default value: null
	public StatModifier(float value, StatModifierType type, int order) : this(value, type, order, null) { }

	// Sets Order to its default value: (int)Type
	public StatModifier(float value, StatModifierType type, object source) : this(value, type, (int)type, source) { }
}