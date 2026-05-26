using System.Collections.Generic;

namespace AGInterfaces;

public abstract class CustomTreeListNode
{
	public string groupName;

	public string name;

	public string alias;

	public string groupDescription;

	public bool isGroup;

	public string Description { get; set; }

	public int ID { get; set; }

	public int ParentID { get; set; }

	public abstract bool IsSame(CustomTreeListNode anotherNode);

	public static bool sameList<T>(List<T> nodes1, List<T> nodes2) where T : CustomTreeListNode
	{
		if (nodes1 == nodes2)
		{
			return true;
		}
		int num = nodes1?.Count ?? 0;
		int num2 = nodes2?.Count ?? 0;
		if (num != num2)
		{
			return false;
		}
		for (int i = 0; i < num; i++)
		{
			if (!nodes1[i].IsSame(nodes2[i]))
			{
				return false;
			}
		}
		return true;
	}

	public CustomTreeListNode(string groupName, string name, string alias, string groupDescription, string description, bool isGroup, int ID, int ParentID)
	{
		this.groupName = groupName;
		this.name = name;
		this.alias = alias;
		this.groupDescription = groupDescription;
		this.isGroup = isGroup;
		Description = description;
		this.ID = ID;
		this.ParentID = ParentID;
	}
}
