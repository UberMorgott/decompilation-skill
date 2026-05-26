using System.Linq;
using KML;

namespace AGInterfaces;

public sealed class AgGeoObjectPolyline : AgGeoObject
{
	public int Exterior { get; set; }

	public override GeoObjectType Type => GeoObjectType.Polyline;

	public AgGeoObjectPolyline()
	{
	}

	public AgGeoObjectPolyline(KMLPolyline kmlPolyline)
	{
		base.Points = ((KMLMultipointObject)kmlPolyline).Points.Select((KMLSinglePoint p) => new AgGeoObjectPoint(p.Lng, p.Lat, p.Altitude)).ToArray();
		Exterior = kmlPolyline.Exterior;
	}

	public override bool IsEqual(AgGeoObject to)
	{
		if (!(to is AgGeoObjectPolyline agGeoObjectPolyline))
		{
			return false;
		}
		if (base.Points.Length != agGeoObjectPolyline.Points.Length || Exterior != agGeoObjectPolyline.Exterior)
		{
			return false;
		}
		for (int i = 0; i < base.Points.Length; i++)
		{
			if (base.Points[i] != agGeoObjectPolyline.Points[i])
			{
				return false;
			}
		}
		return true;
	}

	public override string ToString()
	{
		return $"Polyline: Points = {base.Points.Length}";
	}
}
