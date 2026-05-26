namespace AGInterfaces;

public sealed class StageInfo
{
	public int firstIndex;

	public int lastIndex;

	public Coordinates centr;

	public double radius;

	public int prevLandingIndex;

	public int nextTakeoffIndex;

	public StageInfo(int firstIndex, int lastIndex, Coordinates centr, double radius)
	{
		this.firstIndex = firstIndex;
		this.lastIndex = lastIndex;
		this.centr = centr;
		this.radius = radius;
		prevLandingIndex = -1;
		nextTakeoffIndex = -1;
	}
}
