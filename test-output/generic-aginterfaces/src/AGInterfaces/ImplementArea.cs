using System;

namespace AGInterfaces;

public class ImplementArea
{
	public Guid Id { get; set; }

	public Area Area { get; set; }

	public ImplementArea(Guid id, Area area)
	{
		Id = id;
		Area = area;
	}
}
