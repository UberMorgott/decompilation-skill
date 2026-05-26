using System.Collections.Generic;
using System.Linq;
using KML;

namespace AGInterfaces;

public sealed class AgGeoObjectPolygon : AgGeoObject
{
	public override GeoObjectType Type => GeoObjectType.Polygon;

	public AgGeoObjectPolygon()
	{
	}

	public AgGeoObjectPolygon(KMLPolygon kmlPoint)
	{
		base.Points = ((KMLMultipointObject)kmlPoint).Points.Select((KMLSinglePoint p) => new AgGeoObjectPoint(p.Lng, p.Lat, p.Altitude)).ToArray();
	}

	public AgGeoObjectPolygon(List<KMLSinglePoint> kmlPoint)
	{
		base.Points = kmlPoint.Select((KMLSinglePoint p) => new AgGeoObjectPoint(p.Lng, p.Lat, p.Altitude)).ToArray();
	}

	public override bool IsEqual(AgGeoObject to)
	{
		if (!(to is AgGeoObjectPolygon agGeoObjectPolygon))
		{
			return false;
		}
		if (base.Points.Length != agGeoObjectPolygon.Points.Length)
		{
			return false;
		}
		for (int i = 0; i < base.Points.Length; i++)
		{
			if (base.Points[i] != agGeoObjectPolygon.Points[i])
			{
				return false;
			}
		}
		return true;
	}

	public override string ToString()
	{
		return $"Polygon: Points = {base.Points.Length}";
	}
}
