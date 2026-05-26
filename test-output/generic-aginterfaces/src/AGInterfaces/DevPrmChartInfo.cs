using System;
using AGInterfaces.Classes;

namespace AGInterfaces;

[Serializable]
public class DevPrmChartInfo : DevPrmsGroupChartInfo
{
	public ChartType chartType;

	public string group;

	public string unit;

	public string format;

	public int graphColor;

	public int lineThickness;

	public string ordinate;

	public DisplaySettingsStripe[] stripes;

	public double? min;

	public double? max;

	public DevSwitchPrmStatusInfo[] onStatusesInfo;

	public DevSwitchPrmStatusInfo[] offStatusInfo;

	[NonSerialized]
	public int ThroughIndex;

	public DevPrmChartInfo()
	{
	}

	public DevPrmChartInfo(DevPrmInfo from)
		: base(from)
	{
		group = ((from.parentGroup != null && from.parentGroup.description != null) ? from.parentGroup.description.ToString() : string.Empty);
		unit = from.unit;
		format = from.format;
		graphColor = from.graphColor;
		ordinate = from.ordinate;
		offStatusInfo = from.offStatusInfo;
		onStatusesInfo = from.onStatusesInfo;
		chartType = ((!from.hasStatuses) ? ChartType.Line : ChartType.StatusLine);
	}
}
