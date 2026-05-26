using System;

namespace AGInterfaces;

public class ImplementTimeSpan
{
	public Guid Implement;

	public ReportTimeSpan Time;

	public string[] Sensors;

	public ImplementTimeSpan()
	{
	}

	public ImplementTimeSpan(DateTime startDateTime, DateTime endDateTime)
	{
		Time.startDateTime = startDateTime;
		Time.endDateTime = endDateTime;
	}

	public ImplementTimeSpan(ReportTimeSpan time)
	{
		Time = time;
	}

	public ImplementTimeSpan(Guid implement, ReportTimeSpan time, string[] sensors)
	{
		Implement = implement;
		Time = time;
		Sensors = sensors;
	}
}
