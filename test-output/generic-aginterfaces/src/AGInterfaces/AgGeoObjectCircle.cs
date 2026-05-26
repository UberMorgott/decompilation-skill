using KML;

namespace AGInterfaces;

public sealed class AgGeoObjectCircle : AgGeoObject
{
	public AgGeoObjectPoint Centr => base.Points[0];

	public int Radius { get; set; }

	public override GeoObjectType Type => GeoObjectType.Circle;

	public AgGeoObjectCircle()
	{
	}

	public AgGeoObjectCircle(double lng, double lat, int radius, double alt = 0.0)
	{
		base.Points = new AgGeoObjectPoint[1]
		{
			new AgGeoObjectPoint(lng, lat, alt)
		};
		Radius = radius;
	}

	public AgGeoObjectCircle(KMLPoint kmlPoint)
	{
		base.Points = new AgGeoObjectPoint[1]
		{
			new AgGeoObjectPoint(kmlPoint.Lng, kmlPoint.Lat, kmlPoint.Altitude)
		};
		Radius = kmlPoint.Radius;
	}

	public override string ToString()
	{
		return $"Circle: Center = {Centr.ToString()}, Radius = {Radius}";
	}

	public override bool IsEqual(AgGeoObject to)
	{
		if (!(to is AgGeoObjectCircle agGeoObjectCircle))
		{
			return false;
		}
		if (Centr == agGeoObjectCircle.Centr)
		{
			return Radius == agGeoObjectCircle.Radius;
		}
		return false;
	}
}
