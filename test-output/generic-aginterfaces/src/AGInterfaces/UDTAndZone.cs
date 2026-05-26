using System;

namespace AGInterfaces;

public class UDTAndZone
{
	public DateTime udt;

	public TimeZoneInfo zone;

	public UDTAndZone(DateTime udt, TimeZoneInfo zone)
	{
		this.udt = udt;
		this.zone = zone;
	}
}
