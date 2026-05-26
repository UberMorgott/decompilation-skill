using System.Collections.Generic;

namespace AGInterfaces;

public sealed class GeoFenceArrays
{
	public readonly GroupOrElementInfo[][] geoFences;

	public readonly GroupOrElementInfo[] geoFencesFromProps;

	public readonly GroupOrElementInfo[] groupsFromProps;

	public readonly List<bool> supportOverlays;

	public readonly List<bool> detectRoutes;

	public readonly List<bool> asAreas;

	public readonly List<bool> byProperty;

	public readonly List<string> propertyNames;

	public readonly bool asAreasCreateError;

	public GeoFenceArrays(GroupOrElementInfo[][] geoFences, GroupOrElementInfo[] gfFromProps, GroupOrElementInfo[] grFromProps, List<bool> supportOverlays, List<bool> detectRoutes, List<bool> asAreas, List<bool> byProperty, List<string> propertyNames, bool asAreasCreateError)
	{
		this.geoFences = geoFences;
		geoFencesFromProps = gfFromProps;
		groupsFromProps = grFromProps;
		this.supportOverlays = supportOverlays;
		this.detectRoutes = detectRoutes;
		this.asAreas = asAreas;
		this.byProperty = byProperty;
		this.propertyNames = propertyNames;
		this.asAreasCreateError = asAreasCreateError;
	}
}
