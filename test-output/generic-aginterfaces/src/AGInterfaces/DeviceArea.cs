using System;

namespace AGInterfaces;

public class DeviceArea
{
	public Guid Id { get; set; }

	public Area Area { get; set; }

	public ImplementArea[] ImplementAreas { get; set; }

	public DeviceArea(Guid id, Area area, ImplementArea[] implementAreas)
	{
		Id = id;
		Area = area;
		ImplementAreas = implementAreas;
	}
}
