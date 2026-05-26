using System;

namespace AGInterfaces.Classes;

public class TimeSpanSelectorCopyItem
{
	public int TimeSpanMode { get; set; }

	public DateTime StartDateTime { get; set; }

	public DateTime EndDateTime { get; set; }

	public TimeSpanSelectorCopyItem()
	{
	}

	public TimeSpanSelectorCopyItem(int mode, DateTime sDateTime, DateTime eDateTime)
	{
		TimeSpanMode = mode;
		StartDateTime = sDateTime;
		EndDateTime = eDateTime;
	}
}
