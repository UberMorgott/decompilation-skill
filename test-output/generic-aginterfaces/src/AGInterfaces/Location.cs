using System;

namespace AGInterfaces;

[Serializable]
public struct Location : IEquatable<Location>
{
	public static readonly Location Empty = new Location
	{
		addr = LocationAddr.Empty
	};

	public int gf1;

	public int gf2;

	public int gf3;

	public int gf4;

	public LocationAddr addr;

	public Coordinates crds;

	public bool HasGeoFences
	{
		get
		{
			if (gf1 == 0 && gf2 == 0 && gf3 == 0)
			{
				return gf4 != 0;
			}
			return true;
		}
	}

	public bool HasAddress => addr.addrID >= 0;

	public override string ToString()
	{
		return $"gf={gf1},{gf2},{gf3},{gf4} {addr} crds={crds}";
	}

	public string GFsToString(DevSwitchPrmStatusInfo[] gfArray)
	{
		string text = null;
		if (gfArray != null && HasGeoFences)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = gfs(i);
				if (0 < num && num <= gfArray.Length)
				{
					if (text != null)
					{
						text += "; ";
					}
					text += gfArray[num - 1].description;
				}
			}
		}
		return text;
	}

	public static bool operator ==(Location a, Location b)
	{
		if (a.gf1 == b.gf1 && a.gf2 == b.gf2 && a.gf3 == b.gf3 && a.gf4 == b.gf4 && a.addr == b.addr)
		{
			return Coordinates.Equals2D(a.crds, b.crds);
		}
		return false;
	}

	public static bool operator !=(Location a, Location b)
	{
		if (a.gf1 == b.gf1 && a.gf2 == b.gf2 && a.gf3 == b.gf3 && a.gf4 == b.gf4 && !(a.addr != b.addr))
		{
			return !Coordinates.Equals2D(a.crds, b.crds);
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		if (obj is Location)
		{
			return Equals((Location)obj);
		}
		return false;
	}

	public bool Equals(Location location)
	{
		return this == location;
	}

	public static bool Equals(Location location1, Location location2)
	{
		return location1 == location2;
	}

	public override int GetHashCode()
	{
		return gf1 ^ gf2 ^ gf3 ^ gf4 ^ addr.GetHashCode() ^ crds.GetHashCode();
	}

	private int gfs(int i)
	{
		return i switch
		{
			0 => gf1, 
			1 => gf2, 
			2 => gf3, 
			3 => gf4, 
			_ => 0, 
		};
	}
}
