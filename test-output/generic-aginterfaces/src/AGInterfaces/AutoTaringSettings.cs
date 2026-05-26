using System;

namespace AGInterfaces;

public sealed class AutoTaringSettings
{
	public DateTime TaringPeriodStart { get; set; }

	public DateTime TaringPeriodEnd { get; set; }

	public DeviceInput DeviceInput { get; set; }
}
