using System;

namespace AGInterfaces;

public class ImplementTrack
{
	public Guid ImplementId { get; set; }

	public Guid AreaId { get; set; }

	public TrackPoint[] TrackPoints { get; set; }

	public bool IsNotEmpty { get; set; }

	public ImplementTrack(Guid implementId, Guid areaId, TrackPoint[] trackPoints, bool isNotEmpty)
	{
		ImplementId = implementId;
		AreaId = areaId;
		TrackPoints = trackPoints;
		IsNotEmpty = isNotEmpty;
	}

	public ImplementTrack()
	{
	}
}
