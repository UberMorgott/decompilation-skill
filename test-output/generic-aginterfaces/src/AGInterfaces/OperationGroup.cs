using System.Collections.Generic;

namespace AGInterfaces;

public class OperationGroup
{
	public string GroupName { get; set; }

	public List<OperationItem> OperationItems { get; set; }

	public OperationGroup()
	{
		OperationItems = new List<OperationItem>();
	}

	public OperationGroup(string groupName)
	{
		GroupName = groupName;
		OperationItems = new List<OperationItem>();
	}
}
