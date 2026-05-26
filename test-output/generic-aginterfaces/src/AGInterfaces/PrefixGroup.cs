using System.Collections.Generic;

namespace AGInterfaces;

public class PrefixGroup
{
	public string GroupName { get; set; }

	public List<PrefixItem> PrefixItems { get; set; }

	public PrefixGroup()
	{
		PrefixItems = new List<PrefixItem>();
	}

	public PrefixGroup(string groupName)
	{
		GroupName = groupName;
		PrefixItems = new List<PrefixItem>();
	}
}
