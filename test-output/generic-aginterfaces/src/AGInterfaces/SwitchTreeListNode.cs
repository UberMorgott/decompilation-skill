namespace AGInterfaces;

public sealed class SwitchTreeListNode : CustomTreeListNode
{
	public SwitchTreeListNode(string groupName, string name, string alias, string groupDescription, string description, bool isGroup, int ID, int ParentID)
		: base(groupName, name, alias, groupDescription, description, isGroup, ID, ParentID)
	{
	}

	public override bool IsSame(CustomTreeListNode anotherNode)
	{
		if (groupName == anotherNode.groupName && name == anotherNode.name && isGroup == anotherNode.isGroup && groupDescription == anotherNode.groupDescription && string.Compare(base.Description, anotherNode.Description) == 0 && base.ID == anotherNode.ID)
		{
			return base.ParentID == anotherNode.ParentID;
		}
		return false;
	}
}
