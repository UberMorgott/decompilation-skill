namespace AGInterfaces;

public class BindingPoint
{
	public int X;

	public int Y;

	public double Lng;

	public double Lat;

	public string LngType;

	public string LatType;

	public BindingPoint(int x, int y, double lat, double lng)
	{
		X = x;
		Y = y;
		Lng = lng;
		Lat = lat;
		LngType = ((Lng > 0.0) ? "E" : "W");
		LatType = ((Lat > 0.0) ? "N" : "S");
	}
}
