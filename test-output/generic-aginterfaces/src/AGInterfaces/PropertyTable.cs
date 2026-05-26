using System;
using System.Collections.Generic;
using System.Linq;
using MsgPack.Serialization;
using Newtonsoft.Json;
using Polenter.Serialization;

namespace AGInterfaces;

public sealed class PropertyTable : ICloneable
{
	private PropertyTableItem[] _entries;

	public string CurrValue;

	public string LastValue;

	public PropType type { get; set; }

	public SortedSet<PropertyTableItem> items { get; set; }

	public bool IsNullOrEmpty
	{
		get
		{
			if (items != null)
			{
				return items.Count == 0;
			}
			return true;
		}
	}

	[MessagePackIgnore]
	[ExcludeFromSerialization]
	[JsonIgnore]
	public PropertyTableItem[] Entries
	{
		get
		{
			if (_entries == null && items != null)
			{
				_entries = items.ToArray();
			}
			return _entries;
		}
	}

	public int PrevOrCurrentEntryIndex
	{
		get
		{
			if (Entries.Length == 0)
			{
				return -1;
			}
			DateTime utcNow = DateTime.UtcNow;
			int num = 0;
			int num2 = Entries.Length - 1;
			while (num < num2)
			{
				int num3 = num + num2 + 1 >> 1;
				if (Nullable.Compare(utcNow, Entries[num3].startDT) < 0)
				{
					num2 = num3 - 1;
				}
				else
				{
					num = num3;
				}
			}
			return num;
		}
	}

	public void addEntry(PropertyTableItem item)
	{
		items.Add(item);
		_entries = null;
	}

	public void removeEntry(PropertyTableItem item)
	{
		items.Remove(item);
		_entries = null;
	}

	public void removeAllEntries()
	{
		items.Clear();
		_entries = null;
	}

	public void setEntries(IEnumerable<PropertyTableItem> newItems)
	{
		items = new SortedSet<PropertyTableItem>(newItems);
		_entries = null;
	}

	public PropertyTableItem GetPrevOrCurrentEntry()
	{
		int prevOrCurrentEntryIndex = PrevOrCurrentEntryIndex;
		if (0 > prevOrCurrentEntryIndex || prevOrCurrentEntryIndex >= Entries.Length)
		{
			return null;
		}
		return Entries[prevOrCurrentEntryIndex];
	}

	public object GetCurrentValue()
	{
		PropertyTableItem prevOrCurrentEntry = GetPrevOrCurrentEntry();
		if (prevOrCurrentEntry == null)
		{
			return null;
		}
		DateTime utcNow = DateTime.UtcNow;
		if (Nullable.Compare(utcNow, prevOrCurrentEntry.startDT) < 0)
		{
			return null;
		}
		if (!prevOrCurrentEntry.endDT.HasValue)
		{
			return prevOrCurrentEntry.value;
		}
		return (Nullable.Compare(utcNow, prevOrCurrentEntry.endDT) >= 0) ? null : prevOrCurrentEntry.value;
	}

	public object GetLastValue()
	{
		return Entries.LastOrDefault()?.value;
	}

	public PropertyTable()
	{
		items = new SortedSet<PropertyTableItem>();
	}

	public PropertyTable(PropType type)
		: this()
	{
		this.type = type;
	}

	public PropertyTable(PropertyTable table)
		: this()
	{
		type = table.type;
		CurrValue = table.CurrValue;
		LastValue = table.LastValue;
		foreach (PropertyTableItem item in table.items)
		{
			items.Add((PropertyTableItem)item.Clone());
		}
	}

	public bool Equals(PropertyTable table)
	{
		if (this == table)
		{
			return true;
		}
		if (type != table.type)
		{
			return false;
		}
		if (Entries.Length != table.Entries.Length)
		{
			return false;
		}
		for (int i = 0; i < Entries.Length; i++)
		{
			if (!Entries[i].Equals(table.Entries[i]))
			{
				return false;
			}
		}
		return true;
	}

	public override string ToString()
	{
		if (items.Count <= 0)
		{
			return string.Empty;
		}
		return "<color=#A9A9A9>[" + items.Count + "]</color> " + CurrValue;
	}

	public bool correctByType()
	{
		bool flag = false;
		if (items == null)
		{
			return false;
		}
		foreach (PropertyTableItem item in items)
		{
			object value = item.value;
			item.value = type.correctByType(item.value);
			flag |= value != item.value;
		}
		return flag;
	}

	public object Clone()
	{
		return new PropertyTable(this);
	}

	public DateTime? GetDateTimeFrom()
	{
		if (Entries.Length == 0)
		{
			return null;
		}
		DateTime utcNow = DateTime.UtcNow;
		int nearestEntryIndex = GetNearestEntryIndex(utcNow);
		if (Nullable.Compare(utcNow, _entries[nearestEntryIndex].startDT) < 0)
		{
			return null;
		}
		if (!_entries[nearestEntryIndex].endDT.HasValue)
		{
			return _entries[nearestEntryIndex].startDT;
		}
		if (Nullable.Compare(utcNow, _entries[nearestEntryIndex].endDT) < 0)
		{
			return _entries[nearestEntryIndex].startDT;
		}
		return null;
	}

	public DateTime? GetDateTimeTo()
	{
		if (Entries.Length == 0)
		{
			return null;
		}
		DateTime utcNow = DateTime.UtcNow;
		int nearestEntryIndex = GetNearestEntryIndex(utcNow);
		if (Nullable.Compare(utcNow, _entries[nearestEntryIndex].startDT) < 0)
		{
			return null;
		}
		if (!_entries[nearestEntryIndex].endDT.HasValue)
		{
			return _entries[nearestEntryIndex].endDT;
		}
		if (Nullable.Compare(utcNow, _entries[nearestEntryIndex].endDT) < 0)
		{
			return _entries[nearestEntryIndex].endDT;
		}
		return null;
	}

	public int GetNearestEntryIndex(DateTime dt, PositionType type = PositionType.End)
	{
		int num = 0;
		int num2 = Entries.Length - 1;
		if (type != PositionType.Start)
		{
			if (type == PositionType.End)
			{
				while (num < num2)
				{
					int num3 = num + num2 + 1 >> 1;
					if (Nullable.Compare(dt, _entries[num3].startDT) < 0)
					{
						num2 = num3 - 1;
					}
					else
					{
						num = num3;
					}
				}
			}
		}
		else
		{
			while (num < num2)
			{
				int num4 = num + num2 >> 1;
				if (Nullable.Compare(dt, _entries[num4].startDT) <= 0)
				{
					num2 = num4;
				}
				else
				{
					num = num4 + 1;
				}
			}
		}
		return num;
	}
}
