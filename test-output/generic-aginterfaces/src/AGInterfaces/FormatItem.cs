using System;
using System.Collections.Generic;

namespace AGInterfaces;

public class FormatItem
{
	public List<Tuple<ReturnType, string>> AccessibleList = new List<Tuple<ReturnType, string>>();

	public string Name { get; set; }

	public string Description { get; set; }

	public FormatItem()
	{
	}

	public FormatItem(string name, string description)
	{
		Name = name;
		Description = description;
	}
}
