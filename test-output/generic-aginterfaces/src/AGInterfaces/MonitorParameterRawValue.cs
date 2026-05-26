namespace AGInterfaces;

public class MonitorParameterRawValue
{
	public readonly string Name;

	public readonly object Value;

	public readonly ReturnType Type;

	public MonitorParameterRawValue(string name, ReturnType type, object value)
	{
		Name = name;
		Type = type;
		Value = value;
	}
}
