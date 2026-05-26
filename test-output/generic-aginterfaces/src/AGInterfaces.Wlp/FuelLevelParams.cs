namespace AGInterfaces.Wlp;

public class FuelLevelParams
{
	public double extraFillingTimeout { get; set; }

	public double fillingsJoinInterval { get; set; }

	public int filterQuality { get; set; }

	public int flags { get; set; }

	public double ignoreStayTimeout { get; set; }

	public double minFillingVolume { get; set; }

	public double minTheftTimeout { get; set; }

	public double minTheftVolume { get; set; }

	public double theftsJoinInterval { get; set; }
}
