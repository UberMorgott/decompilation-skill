using System;
using System.Linq;

namespace AGInterfaces;

[Serializable]
public class DevPrmsGroupChartInfo
{
	public string name;

	public string description;

	public DevPrmChartInfo[] devPrmInfoArray;

	public DevPrmsGroupChartInfo()
	{
	}

	public DevPrmsGroupChartInfo(DevPrmsGroupInfo from)
	{
		name = from.name;
		description = ((from.description != null) ? from.description.ToString() : name);
		if (from.devPrmInfoArray != null)
		{
			devPrmInfoArray = from.devPrmInfoArray.Select((DevPrmInfo p) => new DevPrmChartInfo(p)).ToArray();
		}
	}
}
