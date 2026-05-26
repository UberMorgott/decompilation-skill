using System.Collections.Generic;

namespace AGInterfaces;

public sealed class DateTable
{
	public SortedSet<DateTableItem> items { get; set; }

	public DateTable()
	{
		items = new SortedSet<DateTableItem>();
	}

	public override string ToString()
	{
		if (items.Count <= 0)
		{
			return string.Empty;
		}
		return "[" + items.Count + "]";
	}
}
