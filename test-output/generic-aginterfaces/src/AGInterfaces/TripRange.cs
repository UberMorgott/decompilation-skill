using System;
using System.Collections.Generic;
using System.Linq;

namespace AGInterfaces;

[Serializable]
public sealed class TripRange
{
	public CustomSharerInfo sharer;

	public int startPos;

	public int endPos;

	public ReportTimeSpan timeSpanBySharer;

	public TripRange[] tripRanges;

	public SortedDictionary<string, StageArrays> tripStages;

	public ImplementTimeSpan implement;

	public TripRange parent;

	public TripRange()
	{
	}

	public TripRange(CustomSharerInfo sharer, int startPos, int endPos, DateTime sdt, DateTime edt)
	{
		this.sharer = sharer;
		this.startPos = startPos;
		this.endPos = endPos;
		timeSpanBySharer = new ReportTimeSpan(sdt, edt);
	}

	public CustomSharerInfo[] GetSharers()
	{
		List<CustomSharerInfo> list = new List<CustomSharerInfo>();
		list.Add(sharer);
		for (TripRange tripRange = parent; tripRange != null; tripRange = tripRange.parent)
		{
			list.Add(tripRange.sharer);
		}
		return Enumerable.Reverse(list).ToArray();
	}

	public void setParent(TripRange _parent, List<TripRange> target)
	{
		parent = _parent;
		if (tripRanges == null)
		{
			target.Add(this);
			return;
		}
		TripRange[] array = tripRanges;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].setParent(this, target);
		}
	}
}
