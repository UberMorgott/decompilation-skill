using System;

namespace AGInterfaces;

[Serializable]
public struct Coordinates(double longitude, double latitude, double altitude) : IEquatable<Coordinates>
{
	public static readonly Coordinates Empty;

	public const double UNKNOWN_CRD = -777.0;

	public const double MIN_LAT = -90.0;

	public const double MAX_LAT = 90.0;

	public const double MIN_LNG = -180.0;

	public const double MAX_LNG = 180.0;

	public double longitude = longitude;

	public double latitude = latitude;

	public double altitude = altitude;

	public override string ToString()
	{
		if (altitude != 0.0)
		{
			return $"lon={longitude:f6} lat={latitude:f6} alt={altitude:f1}";
		}
		return $"lon={longitude:f6} lat={latitude:f6}";
	}

	public static bool operator ==(Coordinates a, Coordinates b)
	{
		if (a.longitude == b.longitude && a.latitude == b.latitude)
		{
			return a.altitude == b.altitude;
		}
		return false;
	}

	public static bool operator !=(Coordinates a, Coordinates b)
	{
		if (a.longitude == b.longitude && a.latitude == b.latitude)
		{
			return a.altitude != b.altitude;
		}
		return true;
	}

	public static Coordinates operator +(Coordinates a, Coordinates b)
	{
		return new Coordinates(a.longitude + b.longitude, a.latitude + b.latitude, a.altitude + b.altitude);
	}

	public static Coordinates operator -(Coordinates a, Coordinates b)
	{
		return new Coordinates(a.longitude - b.longitude, a.latitude - b.latitude, a.altitude - b.altitude);
	}

	public static Coordinates operator *(double mul, Coordinates a)
	{
		return new Coordinates(a.longitude * mul, a.latitude * mul, a.altitude * mul);
	}

	public static Coordinates operator *(Coordinates a, double mul)
	{
		return new Coordinates(a.longitude * mul, a.latitude * mul, a.altitude * mul);
	}

	public static Coordinates operator /(Coordinates a, double div)
	{
		return new Coordinates(a.longitude / div, a.latitude / div, a.altitude / div);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Coordinates))
		{
			return false;
		}
		return Equals((Coordinates)obj);
	}

	public bool Equals(Coordinates crds)
	{
		if (Math.Abs(longitude - crds.longitude) < 1E-08 && Math.Abs(latitude - crds.latitude) < 1E-08)
		{
			return Math.Abs(altitude - crds.altitude) < 1E-08;
		}
		return false;
	}

	public bool Equals2D(Coordinates crds)
	{
		if (Math.Abs(longitude - crds.longitude) < 1E-08)
		{
			return Math.Abs(latitude - crds.latitude) < 1E-08;
		}
		return false;
	}

	public static bool Equals(Coordinates crds1, Coordinates crds2)
	{
		if (Math.Abs(crds1.longitude - crds2.longitude) < 1E-08 && Math.Abs(crds1.latitude - crds2.latitude) < 1E-08)
		{
			return Math.Abs(crds1.altitude - crds2.altitude) < 1E-08;
		}
		return false;
	}

	public static bool Equals2D(Coordinates crds1, Coordinates crds2)
	{
		if (Math.Abs(crds1.longitude - crds2.longitude) < 1E-08)
		{
			return Math.Abs(crds1.latitude - crds2.latitude) < 1E-08;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return longitude.GetHashCode() ^ latitude.GetHashCode() ^ altitude.GetHashCode();
	}

	public static bool ArrayEquals(Coordinates[] arr1, Coordinates[] arr2)
	{
		if (arr1 != arr2)
		{
			if (arr1 == null || arr2 == null)
			{
				return false;
			}
			if (arr1.Length != arr2.Length)
			{
				return false;
			}
			int i = 0;
			for (int num = arr1.Length; i < num; i++)
			{
				if (!arr1[i].Equals(arr2[i]))
				{
					return false;
				}
			}
		}
		return true;
	}
}
