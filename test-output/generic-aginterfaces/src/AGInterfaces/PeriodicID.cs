using System;
using System.Collections.Generic;

namespace AGInterfaces;

public struct PeriodicID<T>(T id, DateTime? startDT, DateTime? endDT) : IComparable<PeriodicID<T>>, IComparer<PeriodicID<T>> where T : IComparable
{
	public T id = id;

	public DateTime? startDT = startDT;

	public DateTime? endDT = endDT;

	public int Compare(PeriodicID<T> x, PeriodicID<T> y)
	{
		ref T reference = ref x.id;
		object obj = y.id;
		int num = reference.CompareTo(obj);
		if (num == 0)
		{
			num = Nullable.Compare(x.startDT, y.startDT);
			if (num == 0)
			{
				num = Nullable.Compare(x.endDT, y.endDT);
			}
		}
		return num;
	}

	public int CompareTo(PeriodicID<T> other)
	{
		return Compare(this, other);
	}

	public bool HasDT()
	{
		if (!startDT.HasValue)
		{
			return endDT.HasValue;
		}
		return true;
	}

	public bool InsideDT(DateTime currDT)
	{
		if (Nullable.Compare(currDT, startDT) < 0)
		{
			return false;
		}
		if (endDT.HasValue && Nullable.Compare(currDT, endDT) >= 0)
		{
			return false;
		}
		return true;
	}
}
