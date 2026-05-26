namespace AGInterfaces.Wlp;

public class Trip
{
	public int gpsCorrection { get; set; }

	public double maxMessagesDistance { get; set; }

	public double minMovingSpeed { get; set; }

	public double minSat { get; set; }

	public double minStayTime { get; set; }

	public double minTripDistance { get; set; }

	public double minTripTime { get; set; }

	public int type { get; set; }
}
