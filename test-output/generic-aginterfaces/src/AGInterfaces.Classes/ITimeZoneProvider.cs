using System;

namespace AGInterfaces.Classes;

public interface ITimeZoneProvider
{
	TimeZoneInfo GetTimeZone();
}
