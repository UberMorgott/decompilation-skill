using System;
using System.Collections.Generic;
using System.Linq;

namespace AGInterfaces;

public class GroupOrElementInfo : IComparable<GroupOrElementInfo>
{
	public GroupNodeType Type;

	public Guid GUID;

	public string Name;

	public GroupOrElementInfo Parent;

	public GroupOrElementInfo[] Children;

	public string GroupPath
	{
		get
		{
			string text = string.Empty;
			if (Parent != null)
			{
				GroupOrElementInfo parent = Parent;
				text = parent.Name;
				while (parent.Parent != null)
				{
					text = parent.Parent.Name + " / " + text;
					parent = parent.Parent;
				}
			}
			return text;
		}
	}

	public string FullName
	{
		get
		{
			string groupPath = GroupPath;
			return ((!string.IsNullOrEmpty(groupPath)) ? (groupPath + " / ") : "") + Name;
		}
	}

	public GroupOrElementInfo()
	{
	}

	public GroupOrElementInfo(GroupOrElementInfo node)
	{
		Type = node.Type;
		GUID = node.GUID;
		Name = node.Name;
	}

	public GroupOrElementInfo(GroupNodeType Type, Guid GUID, string Name)
	{
		this.Type = Type;
		this.GUID = GUID;
		this.Name = Name;
	}

	public GroupOrElementInfo(GroupNodeType Type, string Name, GroupOrElementInfo Parent)
	{
		this.Type = Type;
		GUID = Guid.NewGuid();
		this.Name = Name;
		this.Parent = Parent;
	}

	public int CompareTo(GroupOrElementInfo other)
	{
		return GUID.CompareTo(other.GUID);
	}

	public IEnumerable<GroupOrElementInfo> ToList()
	{
		return new GroupOrElementInfo[1] { this }.Concat(Children ?? new GroupOrElementInfo[0]);
	}

	public IEnumerable<GroupOrElementInfo> Flatten(Func<GroupOrElementInfo, bool> predicate = null)
	{
		GroupOrElementInfo[] children = Children;
		if (children == null)
		{
			if (predicate == null || predicate(this))
			{
				return new GroupOrElementInfo[1] { this };
			}
			return new GroupOrElementInfo[0];
		}
		if (predicate == null || predicate(this))
		{
			return new GroupOrElementInfo[1] { this }.Concat(children.SelectMany((GroupOrElementInfo p) => p.Flatten(predicate))).ToArray();
		}
		return new GroupOrElementInfo[0];
	}
}
