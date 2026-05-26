namespace AGInterfaces;

public class BorderFrameMap
{
	public double minLat;

	public double maxLat;

	public double minLng;

	public double maxLng;

	public BorderFrameMap(double minLat, double minLng, double maxLat, double maxLng)
	{
		this.minLat = minLat;
		this.minLng = minLng;
		this.maxLat = maxLat;
		this.maxLng = maxLng;
	}
}
