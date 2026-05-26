namespace AGInterfaces.Wlp;

public class MsgFilter
{
	public int enabled { get; set; }

	public double lbsCorrection { get; set; }

	public double maxHdop { get; set; }

	public double maxSpeed { get; set; }

	public double minSats { get; set; }

	public double skipInvalid { get; set; }
}
