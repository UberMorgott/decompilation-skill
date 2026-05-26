namespace AGInterfaces;

public class AgGeoObject
{
	public AgGeoObjectPoint[] Points { get; set; }

	public virtual GeoObjectType Type { get; }

	public virtual bool IsEqual(AgGeoObject to)
	{
		return this == to;
	}
}
