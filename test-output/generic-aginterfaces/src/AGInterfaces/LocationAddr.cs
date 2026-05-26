using System;

namespace AGInterfaces;

public struct LocationAddr : IEquatable<LocationAddr>
{
	public static readonly LocationAddr Empty = new LocationAddr
	{
		addrID = -2
	};

	public int baseID;

	public int addrID;

	public double distance;

	public string nearestGFName;

	public double nearestGFDist;

	public override string ToString()
	{
		if (addrID < 0)
		{
			return "no address";
		}
		return $"baseID={baseID} addrID={addrID} distance={distance:f1}";
	}

	public static bool operator ==(LocationAddr a, LocationAddr b)
	{
		if (a.baseID == b.baseID && a.addrID == b.addrID)
		{
			return a.distance == b.distance;
		}
		return false;
	}

	public static bool operator !=(LocationAddr a, LocationAddr b)
	{
		if (a.baseID == b.baseID && a.addrID == b.addrID)
		{
			return a.distance != b.distance;
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj is LocationAddr)
		{
			return Equals((LocationAddr)obj);
		}
		return false;
	}

	public bool Equals(LocationAddr addr)
	{
		return this == addr;
	}

	public static bool Equals(LocationAddr addr1, LocationAddr addr2)
	{
		return addr1 == addr2;
	}

	public override int GetHashCode()
	{
		return baseID ^ addrID ^ distance.GetHashCode();
	}
}
