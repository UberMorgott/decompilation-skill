using System;
using System.Diagnostics;

namespace AGInterfaces;

[Serializable]
public struct ReportTimeSpan
{
	public DateTime startDateTime;

	public DateTime endDateTime;

	public static readonly ReportTimeSpan Empty;

	public string UniqID => startDateTime.ToString("yyyyMMddHHmmss") + "-" + endDateTime.ToString("yyyyMMddHHmmss");

	public static bool operator !=(ReportTimeSpan a, ReportTimeSpan b)
	{
		if (!(a.startDateTime != b.startDateTime))
		{
			return a.endDateTime != b.endDateTime;
		}
		return true;
	}

	public static bool operator ==(ReportTimeSpan a, ReportTimeSpan b)
	{
		if (a.startDateTime == b.startDateTime)
		{
			return a.endDateTime == b.endDateTime;
		}
		return false;
	}

	public override string ToString()
	{
		return startDateTime.ToString() + " - " + endDateTime;
	}

	public override bool Equals(object value)
	{
		if (!(value is ReportTimeSpan))
		{
			return false;
		}
		return this == (ReportTimeSpan)value;
	}

	public override int GetHashCode()
	{
		return startDateTime.GetHashCode() ^ endDateTime.GetHashCode();
	}

	[DebuggerStepThrough]
	public ReportTimeSpan(DateTime startDateTime, DateTime endDateTime)
	{
		this.startDateTime = startDateTime;
		this.endDateTime = endDateTime;
	}
}
