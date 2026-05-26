using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AGInterfaces;

public class CustomRoute
{
	public Guid RouteGuid { get; set; }

	public Guid RouteReserveGuid { get; set; }

	public List<Guid> GeoFences { get; set; }

	[JsonProperty("_description")]
	public StoreMultiCaption description { get; set; }

	public CustomRoute()
	{
		RouteGuid = Guid.NewGuid();
		RouteReserveGuid = Guid.NewGuid();
		GeoFences = new List<Guid>();
	}
}
