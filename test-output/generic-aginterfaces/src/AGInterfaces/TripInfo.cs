using System;
using System.Collections.Generic;
using System.Drawing;

namespace AGInterfaces;

[Serializable]
public sealed class TripInfo
{
	public Circles[] circles;

	public Coordinates[] lines;

	public TrackPoint[] trackPoints;

	public SortedDictionary<string, TrackStatusesInfo> trackStatuses;

	public PhotoInfo[] photoPoints;

	public Color defaultTrackColor;

	public Coordinates minCoords;

	public Coordinates maxCoords;

	public DeviceTrack[] tracksForArea;

	public IUnitPolygon[] trackPolygonsForArea;

	public TripInfo(Circles[] circles, Coordinates[] lines, TrackPoint[] trackPoints, SortedDictionary<string, TrackStatusesInfo> trackStatuses, Color defaultTrackColor, Coordinates minCoords, Coordinates maxCoords, DeviceTrack[] tracksForArea)
	{
		this.circles = circles;
		this.lines = lines;
		this.trackPoints = trackPoints;
		this.trackStatuses = trackStatuses;
		this.defaultTrackColor = defaultTrackColor;
		this.minCoords = minCoords;
		this.maxCoords = maxCoords;
		this.tracksForArea = tracksForArea;
	}

	public TripInfo(TripInfo tripInfo)
	{
		circles = tripInfo.circles;
		lines = tripInfo.lines;
		trackPoints = tripInfo.trackPoints;
		trackStatuses = tripInfo.trackStatuses;
		photoPoints = tripInfo.photoPoints;
		defaultTrackColor = tripInfo.defaultTrackColor;
		minCoords = tripInfo.minCoords;
		maxCoords = tripInfo.maxCoords;
		tracksForArea = tripInfo.tracksForArea;
		trackPolygonsForArea = tripInfo.trackPolygonsForArea;
	}

	public bool Equals(TripInfo tripInfo)
	{
		if (!minCoords.Equals(tripInfo.minCoords) || !maxCoords.Equals(tripInfo.maxCoords) || !Circles.ArrayEquals(circles, tripInfo.circles) || !TrackPoint.ArrayEquals(trackPoints, tripInfo.trackPoints))
		{
			return false;
		}
		if (trackStatuses != tripInfo.trackStatuses)
		{
			if (trackStatuses == null || tripInfo.trackStatuses == null)
			{
				return false;
			}
			if (trackStatuses.Count != tripInfo.trackStatuses.Count)
			{
				return false;
			}
			foreach (KeyValuePair<string, TrackStatusesInfo> trackStatus in trackStatuses)
			{
				if (!tripInfo.trackStatuses.TryGetValue(trackStatus.Key, out var value))
				{
					return false;
				}
				if (!TrackStatusesInfo.Equals(trackStatus.Value, value))
				{
					return false;
				}
			}
		}
		return true;
	}
}
