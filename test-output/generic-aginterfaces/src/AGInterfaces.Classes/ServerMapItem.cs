using System;

namespace AGInterfaces.Classes;

public record ServerMapItem(string Name, int Length, DateTime Updated)
{
	public DateTime UpdatedLocal => Updated.ToLocalTime();
}
