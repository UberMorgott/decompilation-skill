using System;
using System.Collections.Generic;
using System.Linq;
using KML;

namespace AGInterfaces;

public sealed class GeoCache
{
	public AgGeoFence GeoFence;

	public Coordinates MinCoord;

	public Coordinates MaxCoord;

	public GeoCache()
	{
		MinCoord = new Coordinates(180.0, 90.0, 0.0);
		MaxCoord = new Coordinates(-180.0, -90.0, 0.0);
	}

	public KMLBaseObject GetKMLObject(string name = "kml", KMLStyle style = null, Guid ID = default(Guid))
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		if (GeoFence.GeoObjects.Count == 0)
		{
			return null;
		}
		AgGeoObject agGeoObject = GeoFence.GeoObjects[0];
		if (agGeoObject.Type == GeoObjectType.Circle)
		{
			return (KMLBaseObject)new KMLPoint(style, ID, name, agGeoObject.Points[0].Latitude, agGeoObject.Points[0].Longitude)
			{
				Radius = ((AgGeoObjectCircle)agGeoObject).Radius
			};
		}
		if (agGeoObject.Type == GeoObjectType.Polyline)
		{
			return (KMLBaseObject)new KMLPolyline(ID, name, style, ((IEnumerable<AgGeoObjectPoint>)agGeoObject.Points).Select((Func<AgGeoObjectPoint, KMLSinglePoint>)((AgGeoObjectPoint p) => new KMLSinglePoint(p.Latitude, p.Longitude, p.Altitude))))
			{
				Exterior = ((AgGeoObjectPolyline)agGeoObject).Exterior
			};
		}
		KMLPolygon val = new KMLPolygon(ID, name, style, ((IEnumerable<AgGeoObjectPoint>)agGeoObject.Points).Select((Func<AgGeoObjectPoint, KMLSinglePoint>)((AgGeoObjectPoint p) => new KMLSinglePoint(p.Latitude, p.Longitude, p.Altitude))).ToArray());
		if (GeoFence.Holes.Count > 0)
		{
			foreach (AgGeoObject hole in GeoFence.Holes)
			{
				if (hole.Type == GeoObjectType.Polygon)
				{
					val.Holes.Add(((IEnumerable<AgGeoObjectPoint>)hole.Points).Select((Func<AgGeoObjectPoint, KMLSinglePoint>)((AgGeoObjectPoint p) => new KMLSinglePoint(p.Latitude, p.Longitude, p.Altitude))).ToList());
				}
			}
		}
		return (KMLBaseObject)(object)val;
	}

	public AgGeoObject GetFirstGeoObject()
	{
		if (GeoFence.GeoObjects.Count == 0)
		{
			return null;
		}
		return GeoFence.GeoObjects[0];
	}
}
