using System;
using System.Collections.Generic;
using System.Reflection;
using AGInterfaces.Extenders;
using MsgPack.Serialization;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public class LookUpEditItem : IComparer<LookUpEditItem>, IComparable<LookUpEditItem>
{
	public string Image;

	[MessagePackRuntimeType]
	public object Value { get; set; }

	[Translatable]
	public string Display { get; set; }

	public LookUpEditItem(object value, string display)
	{
		Value = value;
		Display = display;
	}

	public int Compare(LookUpEditItem x, LookUpEditItem y)
	{
		return x.CompareTo(y);
	}

	public int CompareTo(LookUpEditItem other)
	{
		return StringExtenders.NumCompare(Display, other.Display);
	}
}
