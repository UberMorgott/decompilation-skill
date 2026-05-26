using KML;

namespace AGInterfaces;

public sealed class AgGeoObjectPoint
{
	public static readonly AgGeoObjectPoint Empty;

	public double Longitude { get; set; }

	public double Latitude { get; set; }

	public double Altitude { get; set; }

	public AgGeoObjectPoint()
	{
	}

	public AgGeoObjectPoint(double longitude, double latitude, double altitude)
	{
		Longitude = longitude;
		Latitude = latitude;
		Altitude = altitude;
	}

	public AgGeoObjectPoint(KMLPoint point)
	{
		Longitude = point.Lng;
		Latitude = point.Lat;
		Altitude = point.Altitude;
	}

	public bool IsEqual(AgGeoObjectPoint point)
	{
		if (point == null)
		{
			return false;
		}
		if (Longitude == point.Longitude && Latitude == point.Latitude)
		{
			return Altitude == point.Altitude;
		}
		return false;
	}

	public static bool operator ==(AgGeoObjectPoint a, AgGeoObjectPoint b)
	{
		if ((object)a == b)
		{
			return true;
		}
		if ((object)a == null || (object)b == null)
		{
			return false;
		}
		return a.IsEqual(b);
	}

	public static bool operator !=(AgGeoObjectPoint a, AgGeoObjectPoint b)
	{
		if ((object)a == b)
		{
			return false;
		}
		if ((object)a == null || (object)b == null)
		{
			return true;
		}
		return !a.IsEqual(b);
	}

	public override bool Equals(object obj)
	{
		if (this != obj)
		{
			if (obj is AgGeoObjectPoint point)
			{
				return IsEqual(point);
			}
			return false;
		}
		return true;
	}

	public override int GetHashCode()
	{
		return Longitude.GetHashCode() ^ Latitude.GetHashCode() ^ Altitude.GetHashCode();
	}
}
