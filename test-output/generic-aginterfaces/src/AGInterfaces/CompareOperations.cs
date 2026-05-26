namespace AGInterfaces;

public static class CompareOperations
{
	public static readonly CompareDouble[] compare = new CompareDouble[6]
	{
		(double val1, double val2) => val1 != val2,
		(double val1, double val2) => val1 == val2,
		(double val1, double val2) => val1 <= val2,
		(double val1, double val2) => val1 > val2,
		(double val1, double val2) => val1 >= val2,
		(double val1, double val2) => val1 < val2
	};
}
