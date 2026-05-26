namespace AGInterfaces.Wlp;

public class ReportProps
{
	public double fuelRateCoefficient { get; set; }

	public double maxMessagesInterval { get; set; }

	public double mileageCoefficient { get; set; }

	public double urbanMaxSpeed { get; set; }

	public double speedingMode { get; set; }

	public double speedLimit { get; set; }

	public double speedingTolerance { get; set; }

	public double speedingMinDuration { get; set; }

	public DriverActivity driver_activity { get; set; }

	public double dailyEngineHoursRate { get; set; }
}
