namespace AGInterfaces;

public struct ValuePriorityPair(int Value, int Priority)
{
	public int Value = Value;

	public int Priority = Priority;

	public override string ToString()
	{
		return $"{Value} (pr={Priority})";
	}

	public static implicit operator int(ValuePriorityPair a)
	{
		return a.Value;
	}
}
