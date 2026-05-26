using System;
using System.Collections.Generic;
using System.Linq;

namespace AGInterfaces;

public sealed class TareTable : ICloneable
{
	public bool dependsOnSupply { get; set; }

	public ApproximationType approximation { get; set; }

	public SortedSet<TareTableItem> items { get; set; }

	public AutoTaringSettings AutoTaringSettings { get; set; }

	public TareTable()
	{
		items = new SortedSet<TareTableItem>();
	}

	public TareTable(TareTable table)
	{
		dependsOnSupply = table.dependsOnSupply;
		approximation = table.approximation;
		items = new SortedSet<TareTableItem>();
		foreach (TareTableItem item in table.items)
		{
			items.Add((TareTableItem)item.Clone());
		}
		if (table.AutoTaringSettings != null)
		{
			AutoTaringSettings = new AutoTaringSettings
			{
				TaringPeriodStart = table.AutoTaringSettings.TaringPeriodStart,
				TaringPeriodEnd = table.AutoTaringSettings.TaringPeriodEnd,
				DeviceInput = table.AutoTaringSettings.DeviceInput
			};
		}
	}

	public override string ToString()
	{
		if (items.Count <= 0)
		{
			return string.Empty;
		}
		return "[" + items.Count + "]";
	}

	public object Clone()
	{
		return new TareTable(this);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is TareTable tareTable))
		{
			return false;
		}
		if (dependsOnSupply != tareTable.dependsOnSupply || approximation != tareTable.approximation)
		{
			return false;
		}
		if (AutoTaringSettings != tareTable.AutoTaringSettings)
		{
			if (AutoTaringSettings == null)
			{
				return false;
			}
			if (tareTable.AutoTaringSettings == null)
			{
				return false;
			}
			if (AutoTaringSettings.TaringPeriodStart != tareTable.AutoTaringSettings.TaringPeriodStart || AutoTaringSettings.TaringPeriodEnd != tareTable.AutoTaringSettings.TaringPeriodEnd || AutoTaringSettings.DeviceInput != tareTable.AutoTaringSettings.DeviceInput)
			{
				return false;
			}
		}
		if (items == tareTable.items)
		{
			return true;
		}
		if (items != null)
		{
			return items.SequenceEqual(tareTable.items);
		}
		return false;
	}
}
