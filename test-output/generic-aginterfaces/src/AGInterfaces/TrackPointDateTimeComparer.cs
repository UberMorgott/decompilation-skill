using System.Collections.Generic;

namespace AGInterfaces;

public class TrackPointDateTimeComparer : IComparer<TrackPoint>
{
	public int Compare(TrackPoint x, TrackPoint y)
	{
		return x.udt.CompareTo(y.udt);
	}
}
