using System.Collections;
using System.Collections.Generic;

namespace AGInterfaces;

public class RefsMap
{
	public BitArray Refs;

	public string[] RefsName;

	public HashSet<string> GetDependencies()
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (Refs != null)
		{
			for (int i = 0; i < Refs.Length; i++)
			{
				if (Refs.Get(i))
				{
					hashSet.Add(RefsName[i]);
				}
			}
		}
		return hashSet;
	}
}
