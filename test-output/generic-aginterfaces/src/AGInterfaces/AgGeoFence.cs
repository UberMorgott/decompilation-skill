using System.Collections.Generic;
using KML;
using MsgPack.Serialization;

namespace AGInterfaces;

public class AgGeoFence
{
	[MessagePackRuntimeCollectionItemType]
	public List<AgGeoObject> GeoObjects { get; set; }

	[MessagePackRuntimeCollectionItemType]
	public List<AgGeoObject> Holes { get; set; }

	public AgGeoFence()
	{
		GeoObjects = new List<AgGeoObject>();
		Holes = new List<AgGeoObject>();
	}

	public bool IsEquals(AgGeoFence to)
	{
		if (to == null)
		{
			return false;
		}
		if (GeoObjects.Count != to.GeoObjects.Count)
		{
			return false;
		}
		for (int i = 0; i < GeoObjects.Count; i++)
		{
			if (!GeoObjects[i].IsEqual(to.GeoObjects[i]))
			{
				return false;
			}
		}
		if (Holes.Count != to.Holes.Count)
		{
			return false;
		}
		for (int j = 0; j < Holes.Count; j++)
		{
			if (!Holes[j].IsEqual(to.Holes[j]))
			{
				return false;
			}
		}
		return true;
	}

	public bool IsEquals(AgGeoObject to)
	{
		if (to == null)
		{
			return false;
		}
		if (Holes.Count != 0 || GeoObjects.Count != 1)
		{
			return false;
		}
		return GeoObjects[0].IsEqual(to);
	}

	public bool IsEquals(KMLBaseObject to)
	{
		if (to == null)
		{
			return false;
		}
		if (GeoObjects.Count != 1)
		{
			return false;
		}
		if (Holes.Count != (((KMLPolygon)(((to is KMLPolygon) ? to : null)?)).Holes.Count ?? 0))
		{
			return false;
		}
		if (Holes.Count > 0)
		{
			List<List<KMLSinglePoint>> list = ((KMLPolygon)(((to is KMLPolygon) ? to : null)?)).Holes;
			if (list == null || list.Count != Holes.Count)
			{
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (Holes[i].Points.Length != list[i].Count)
				{
					return false;
				}
				for (int j = 0; j < list[i].Count; j++)
				{
					AgGeoObjectPoint agGeoObjectPoint = Holes[i].Points[j];
					KMLSinglePoint val = list[i][j];
					if (agGeoObjectPoint.Longitude != val.Lng || agGeoObjectPoint.Latitude != val.Lat || agGeoObjectPoint.Altitude != val.Altitude)
					{
						return false;
					}
				}
			}
		}
		AgGeoObject to2 = null;
		KMLPoint val2 = (KMLPoint)(object)((to is KMLPoint) ? to : null);
		if (val2 != null)
		{
			to2 = new AgGeoObjectCircle(val2);
		}
		else
		{
			KMLPolygon val3 = (KMLPolygon)(object)((to is KMLPolygon) ? to : null);
			if (val3 != null)
			{
				to2 = new AgGeoObjectPolygon(val3);
			}
			else
			{
				KMLPolyline val4 = (KMLPolyline)(object)((to is KMLPolyline) ? to : null);
				if (val4 != null)
				{
					to2 = new AgGeoObjectPolyline(val4);
				}
			}
		}
		return GeoObjects[0].IsEqual(to2);
	}
}
