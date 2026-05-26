using System;

namespace AGInterfaces;

public class DevPrmsRouteInfo
{
	public Guid guid;

	public Guid reserveGuid;

	public string caption;

	public Guid[] geoFences;

	public DevPrmsRouteInfo(Guid guid, Guid reserveGuid, string caption, Guid[] geoFences)
	{
		this.guid = guid;
		this.reserveGuid = reserveGuid;
		this.caption = caption;
		this.geoFences = geoFences;
	}
}
