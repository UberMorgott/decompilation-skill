using System;

namespace AGInterfaces;

[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class AGPrmInfoAttribute : Attribute
{
	private readonly string title;

	private readonly double minValue;

	private readonly double maxValue;

	public string Title => title;

	public double MinValue => minValue;

	public double MaxValue => maxValue;

	public AGPrmInfoAttribute(string title, double minValue = double.MinValue, double maxValue = double.MinValue)
	{
		this.title = title;
		this.minValue = minValue;
		this.maxValue = maxValue;
	}
}
