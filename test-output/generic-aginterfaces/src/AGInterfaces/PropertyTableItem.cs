using System;
using System.Collections.Generic;
using System.Reflection;
using MsgPack.Serialization;

namespace AGInterfaces;

[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
public sealed class PropertyTableItem : IEquatable<PropertyTableItem>, IComparer<PropertyTableItem>, IComparable<PropertyTableItem>, ICloneable
{
	public DateTime? startDT { get; set; }

	public DateTime? endDT { get; set; }

	[MessagePackRuntimeType]
	public object value { get; set; }

	public string comment { get; set; }

	public PropertyTableItem()
	{
	}

	public PropertyTableItem(PropertyTableItem item)
	{
		startDT = item.startDT;
		endDT = item.endDT;
		ICloneable cloneable = item.value as ICloneable;
		value = ((cloneable != null) ? cloneable.Clone() : item.value);
		comment = item.comment;
	}

	public PropertyTableItem(DateTime? startDTUTC, DateTime? endDTUTC, object value)
	{
		startDT = startDTUTC;
		endDT = endDTUTC;
		this.value = value;
	}

	public bool Equals(PropertyTableItem item)
	{
		if (item == null)
		{
			return false;
		}
		if (startDT == item.startDT)
		{
			DateTime? dateTime = endDT;
			DateTime? dateTime2 = item.endDT;
			if (dateTime.HasValue == dateTime2.HasValue && (!dateTime.HasValue || dateTime.GetValueOrDefault() == dateTime2.GetValueOrDefault()) && comment == item.comment)
			{
				return object.Equals(value, item.value);
			}
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is PropertyTableItem item)
		{
			return Equals(item);
		}
		return false;
	}

	public int Compare(PropertyTableItem x, PropertyTableItem y)
	{
		int num = Nullable.Compare(x.startDT, y.startDT);
		if (num == 0)
		{
			num = Nullable.Compare(x.endDT, y.endDT);
		}
		return num;
	}

	public int CompareTo(PropertyTableItem other)
	{
		int num = Nullable.Compare(startDT, other.startDT);
		if (num == 0)
		{
			num = Nullable.Compare(endDT, other.endDT);
		}
		return num;
	}

	public object Clone()
	{
		return new PropertyTableItem(this);
	}

	public static object GetCurrentValue(PropertyTableItem[] entries, DateTime dtUTC)
	{
		if (entries.Length == 0)
		{
			return null;
		}
		int num = 0;
		int num2 = entries.Length - 1;
		while (num < num2)
		{
			int num3 = num + num2 + 1 >> 1;
			if (Nullable.Compare(dtUTC, entries[num3].startDT) < 0)
			{
				num2 = num3 - 1;
			}
			else
			{
				num = num3;
			}
		}
		if (Nullable.Compare(dtUTC, entries[num].startDT) < 0)
		{
			return null;
		}
		if (!entries[num].endDT.HasValue)
		{
			return entries[num].value;
		}
		if (Nullable.Compare(dtUTC, entries[num].endDT) < 0)
		{
			return entries[num].value;
		}
		return null;
	}

	public static PropertyTableItem[] GetCroppedItems(PropertyTableItem[] srcEntries, DateTime sdtUTC, DateTime edtUTC)
	{
		List<PropertyTableItem> list = new List<PropertyTableItem>(srcEntries.Length);
		foreach (PropertyTableItem propertyTableItem in srcEntries)
		{
			if ((!propertyTableItem.startDT.HasValue || !(propertyTableItem.startDT.Value > edtUTC)) && (!propertyTableItem.endDT.HasValue || !(sdtUTC >= propertyTableItem.endDT.Value)))
			{
				list.Add(propertyTableItem);
			}
		}
		return list.ToArray();
	}

	public static PropertyTableItem[] GetCroppedItemsAsArrayOfGuid(PropertyTableItem[] srcEntries, DateTime sdtUTC, DateTime edtUTC)
	{
		List<PropertyTableItem> list = new List<PropertyTableItem>(srcEntries.Length);
		foreach (PropertyTableItem propertyTableItem in srcEntries)
		{
			if ((!propertyTableItem.startDT.HasValue || !(propertyTableItem.startDT.Value > edtUTC)) && (!propertyTableItem.endDT.HasValue || !(sdtUTC >= propertyTableItem.endDT.Value)) && propertyTableItem.value is Guid)
			{
				list.Add(new PropertyTableItem
				{
					startDT = propertyTableItem.startDT,
					endDT = propertyTableItem.endDT,
					value = new Guid[1] { (Guid)propertyTableItem.value },
					comment = propertyTableItem.comment
				});
			}
		}
		return list.ToArray();
	}
}
