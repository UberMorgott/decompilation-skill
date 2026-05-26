using System;
using System.Collections.Generic;
using MsgPack.Serialization;

namespace AGInterfaces;

public sealed class DateTableItem : IComparer<DateTableItem>, IComparable<DateTableItem>
{
	public DateTime dateTime { get; set; }

	[MessagePackRuntimeType]
	public object value { get; set; }

	public int Compare(DateTableItem x, DateTableItem y)
	{
		return x.dateTime.CompareTo(y.dateTime);
	}

	public int CompareTo(DateTableItem other)
	{
		return dateTime.CompareTo(other.dateTime);
	}
}
