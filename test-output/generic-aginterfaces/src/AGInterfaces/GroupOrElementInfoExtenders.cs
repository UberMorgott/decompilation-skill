using System.Collections.Generic;
using System.Linq;

namespace AGInterfaces;

public static class GroupOrElementInfoExtenders
{
	public static void FillChildrens(this IEnumerable<GroupOrElementInfo> nodes)
	{
		Dictionary<GroupOrElementInfo, GroupOrElementInfo[]> dictionary = (from p in nodes
			where p.Parent != null
			group p by p.Parent).ToDictionary((IGrouping<GroupOrElementInfo, GroupOrElementInfo> p) => p.Key, (IGrouping<GroupOrElementInfo, GroupOrElementInfo> p) => p.ToArray());
		foreach (GroupOrElementInfo node in nodes)
		{
			if (dictionary.TryGetValue(node, out var value) && value.Length != 0)
			{
				node.Children = value;
			}
			else
			{
				node.Children = null;
			}
		}
	}

	public static bool parentPairContainsPair(this KeyValuePair<GroupOrElementInfo, bool> parentPair, KeyValuePair<GroupOrElementInfo, bool> pair)
	{
		GroupOrElementInfo key = parentPair.Key;
		GroupOrElementInfo groupOrElementInfo = pair.Key;
		if (key != groupOrElementInfo && key.Type == GroupNodeType.Group)
		{
			if (parentPair.Value)
			{
				while (groupOrElementInfo.Parent != null)
				{
					if ((groupOrElementInfo = groupOrElementInfo.Parent) == key)
					{
						return true;
					}
				}
			}
			else if (groupOrElementInfo.Parent != null && groupOrElementInfo.Parent == key && groupOrElementInfo.Type != GroupNodeType.Group)
			{
				return true;
			}
		}
		return false;
	}
}
