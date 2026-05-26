using System;

namespace AGInterfaces;

public class DeviceTrack
{
	public Guid DeviceId { get; set; }

	public ImplementTrack[] ImplementTracks { get; set; }

	public DeviceTrack(Guid deviceId, ImplementTrack[] implementTracks)
	{
		DeviceId = deviceId;
		ImplementTracks = implementTracks;
	}

	public DeviceTrack()
	{
	}
}
