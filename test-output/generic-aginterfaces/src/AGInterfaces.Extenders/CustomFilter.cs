using System;

namespace AGInterfaces.Extenders;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class CustomFilter : Attribute
{
	public string Name { get; set; }

	public AddValueType ValueType { get; set; }

	public CustomFilter(AddValueType addValueType, string Name)
	{
		this.Name = Name;
		ValueType = addValueType;
	}

	public CustomFilter(AddValueType addValueType)
	{
		ValueType = addValueType;
	}
}
