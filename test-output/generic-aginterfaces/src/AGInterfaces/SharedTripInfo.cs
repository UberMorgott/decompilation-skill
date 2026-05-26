using System;
using System.Collections.Generic;

namespace AGInterfaces;

[Serializable]
public class SharedTripInfo
{
	public int startPos;

	public int endPos;

	public int startFileID;

	public int endFileID;

	public ReportTimeSpan timeSpan;

	public ReportTimeSpan timeSpanBySharer;

	public TripInfo rangeInfo;

	public Dictionary<Guid, TripInfo> multiTripInfo;

	public SortedDictionary<string, StageArrays> rangeStages;

	public string areaMapFileName;

	public string areaImgFileName;

	public SharedTripInfo()
	{
	}

	public SharedTripInfo(SharedTripInfo tripInfo)
	{
		startPos = tripInfo.startPos;
		endPos = tripInfo.endPos;
		startFileID = tripInfo.startFileID;
		endFileID = tripInfo.endFileID;
		timeSpan = tripInfo.timeSpan;
		timeSpanBySharer = tripInfo.timeSpanBySharer;
		rangeInfo = tripInfo.rangeInfo;
		multiTripInfo = tripInfo.multiTripInfo;
		rangeStages = tripInfo.rangeStages;
		areaMapFileName = tripInfo.areaMapFileName;
		areaImgFileName = tripInfo.areaImgFileName;
	}
}
