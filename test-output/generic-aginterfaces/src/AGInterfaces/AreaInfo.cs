using System.Drawing;

namespace AGInterfaces;

public struct AreaInfo
{
	public static readonly AreaInfo Empty;

	public Bitmap BmpImage;

	public BindingPoint[] BindingPoints;

	public IUnitPolygon[] TrackPolygons;

	public Area CommomArea { get; set; }

	public DeviceArea[] DeviceAreas { get; set; }

	public override string ToString()
	{
		return $"{CommomArea}, DeviceAreas={DeviceAreas?.Length.ToString() ?? "-"}";
	}
}
