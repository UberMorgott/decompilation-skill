namespace AGInterfaces;

public sealed class ConditionProps
{
	public string parameter { get; set; }

	public Operator oper { get; set; }

	public double value { get; set; }

	public ConditionProps()
	{
	}

	public ConditionProps(ConditionProps props)
	{
		parameter = props.parameter;
		oper = props.oper;
		value = props.value;
	}

	public override string ToString()
	{
		return parameter + " " + "≠=≤>≥<"[(int)oper] + " " + value;
	}
}
