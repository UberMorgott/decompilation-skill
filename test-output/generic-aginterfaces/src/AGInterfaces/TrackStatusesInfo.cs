using System;

namespace AGInterfaces;

[Serializable]
public sealed class TrackStatusesInfo
{
	public TrackStatuses[] trackStatuses;

	public string name;

	public string description;

	public StatusesDefinedType statusListType;

	public int firstStatusIndex;

	public int lastStatusIndex;

	public TrackStatusesInfo()
	{
	}

	public TrackStatusesInfo(TrackStatuses[] trackStatuses, string name, string description, StatusesDefinedType statusListType, int firstStatusIndex, int lastStatusIndex)
	{
		this.trackStatuses = trackStatuses;
		this.name = name;
		this.description = description;
		this.statusListType = statusListType;
		this.firstStatusIndex = firstStatusIndex;
		this.lastStatusIndex = lastStatusIndex;
	}

	public static bool Equals(TrackStatusesInfo trackStatusesInfo1, TrackStatusesInfo trackStatusesInfo2)
	{
		if (trackStatusesInfo1 == trackStatusesInfo2)
		{
			return true;
		}
		if (trackStatusesInfo1 == null || trackStatusesInfo2 == null)
		{
			return false;
		}
		if (string.Compare(trackStatusesInfo1.description, trackStatusesInfo2.description) == 0 && trackStatusesInfo1.firstStatusIndex == trackStatusesInfo2.firstStatusIndex && trackStatusesInfo1.lastStatusIndex == trackStatusesInfo2.lastStatusIndex)
		{
			return TrackStatuses.ArrayEquals(trackStatusesInfo1.trackStatuses, trackStatusesInfo2.trackStatuses);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is TrackStatusesInfo trackStatusesInfo))
		{
			return base.Equals(obj);
		}
		return Equals(this, trackStatusesInfo);
	}
}
