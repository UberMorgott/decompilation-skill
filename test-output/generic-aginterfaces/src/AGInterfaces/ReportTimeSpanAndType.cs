using System;

namespace AGInterfaces;

public class ReportTimeSpanAndType
{
	public ReportTimeSpanType type;

	public ReportTimeSpan span;

	public TimeZoneInfo zone;

	public ReportTimeSpanAndType(ReportTimeSpanType type, ReportTimeSpan span, TimeZoneInfo zone)
	{
		this.type = type;
		this.span = span;
		this.zone = zone;
	}
}
