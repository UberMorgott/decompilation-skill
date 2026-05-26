using System;

namespace AGInterfaces;

public class ReportTimeSpanAndZone
{
	public DateTime initDateTime;

	public readonly ReportTimeSpan span;

	public ReportTimeSpan[] noTripSpans;

	public ReportTimeSpan[] blindSpans;

	public ReportTimeSpan[] parkSpans;

	public readonly TimeZoneInfo zone;

	public ReportTimeSpanAndZone(DateTime startDateTime, DateTime endDateTime, DataArrays arrays)
	{
		initDateTime = startDateTime;
		span.startDateTime = startDateTime;
		span.endDateTime = endDateTime;
		noTripSpans = arrays.noTripSpans;
		blindSpans = arrays.blindSpans;
		parkSpans = arrays.parkSpans;
		zone = arrays.computeInfo.initInfo.timeZoneInfo;
	}
}
