using System;
using System.Collections.Generic;

namespace AGInterfaces;

public class TareTableItem : IComparer<TareTableItem>, IComparable<TareTableItem>, ICloneable
{
	public virtual double? outputVal { get; set; }

	public virtual double? inputVal { get; set; }

	public virtual double? supplyVal { get; set; }

	public TareTableItem()
	{
	}

	public TareTableItem(TareTableItem item)
	{
		outputVal = item.outputVal;
		inputVal = item.inputVal;
		supplyVal = item.supplyVal;
	}

	public object Clone()
	{
		return new TareTableItem(this);
	}

	public int Compare(TareTableItem x, TareTableItem y)
	{
		return Nullable.Compare(y.outputVal, x.outputVal);
	}

	public int CompareTo(TareTableItem other)
	{
		return Nullable.Compare(other.outputVal, outputVal);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is TareTableItem tareTableItem))
		{
			return false;
		}
		if (Nullable.Compare(outputVal, tareTableItem.outputVal) == 0 && Nullable.Compare(inputVal, tareTableItem.inputVal) == 0)
		{
			return Nullable.Compare(supplyVal, tareTableItem.supplyVal) == 0;
		}
		return false;
	}
}
