namespace AGInterfaces;

public sealed class SlaveSourcesInfo
{
	public int serialNo;

	public SourceInfo[] sourcesInfo;

	public SourceInfo[] routesInfo;

	public SlaveSourcesInfo(int serialNo, SourceInfo[] sourcesInfo, SourceInfo[] routesInfo)
	{
		this.serialNo = serialNo;
		this.sourcesInfo = sourcesInfo;
		this.routesInfo = routesInfo;
	}

	public static bool Equals(SlaveSourcesInfo slaveSourcesInfo1, SlaveSourcesInfo slaveSourcesInfo2)
	{
		if (slaveSourcesInfo1 == slaveSourcesInfo2)
		{
			return true;
		}
		if (slaveSourcesInfo1 == null || slaveSourcesInfo2 == null)
		{
			return false;
		}
		if (slaveSourcesInfo1.serialNo == slaveSourcesInfo2.serialNo && SourceInfo.ArraysEquals(slaveSourcesInfo1.sourcesInfo, slaveSourcesInfo2.sourcesInfo))
		{
			return SourceInfo.ArraysEquals(slaveSourcesInfo1.routesInfo, slaveSourcesInfo2.routesInfo);
		}
		return false;
	}

	public static bool PartEquals(SlaveSourcesInfo slaveSourcesInfo1, SlaveSourcesInfo slaveSourcesInfo2)
	{
		if (slaveSourcesInfo1 == slaveSourcesInfo2)
		{
			return true;
		}
		if (slaveSourcesInfo1 == null || slaveSourcesInfo2 == null)
		{
			return false;
		}
		if (slaveSourcesInfo1.serialNo == slaveSourcesInfo2.serialNo && SourceInfo.ArraysPartEquals(slaveSourcesInfo1.sourcesInfo, slaveSourcesInfo2.sourcesInfo))
		{
			return SourceInfo.ArraysPartEquals(slaveSourcesInfo1.routesInfo, slaveSourcesInfo2.routesInfo);
		}
		return false;
	}
}
