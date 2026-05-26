using System;
using System.Collections.Generic;

namespace AGInterfaces;

[Serializable]
public sealed class ValInfoLists
{
	public List<int> u;

	public List<int> count;

	public List<int> tcount;

	public List<int> duration;

	public List<int> distance;

	public List<int> valid;

	public List<int> level;

	public List<int> spec;

	public List<int> limit;

	public void Clear()
	{
		u.Clear();
		count.Clear();
		tcount.Clear();
		duration.Clear();
		distance.Clear();
		valid.Clear();
		level.Clear();
		spec.Clear();
		limit.Clear();
	}

	private void init()
	{
		u = new List<int>();
		count = new List<int>();
		tcount = new List<int>();
		duration = new List<int>();
		distance = new List<int>();
		valid = new List<int>();
		level = new List<int>();
		spec = new List<int>();
		limit = new List<int>();
	}

	public ValInfoLists()
	{
		init();
	}

	public ValInfoLists(bool withInit)
	{
		if (withInit)
		{
			init();
		}
		else
		{
			u = new List<int>();
		}
	}

	public ValInfoLists(ValInfoLists src)
	{
		if (src == null)
		{
			init();
			return;
		}
		u = new List<int>(src.u);
		count = new List<int>(src.count);
		tcount = new List<int>(src.tcount);
		duration = new List<int>(src.duration);
		distance = new List<int>(src.distance);
		valid = new List<int>(src.valid);
		level = new List<int>(src.level);
		spec = new List<int>(src.spec);
		limit = new List<int>(src.limit);
	}

	public static ValInfoLists create(ValInfoLists src, RefsMap prmRefs, HashSet<string> UsedParameter, Dictionary<string, RefsMap> classRefs, ValInfoLists prevList = null)
	{
		ValInfoLists valInfoLists = new ValInfoLists(withInit: false);
		if (src != null)
		{
			if (prevList == null)
			{
				prevList = valInfoLists;
			}
			createList(prmRefs, src.u, valInfoLists.u, prevList.u.Count, UsedParameter);
			if (classRefs != null)
			{
				if (classRefs.TryGetValue("count", out var value))
				{
					valInfoLists.count = new List<int>();
					createList(value, src.count, valInfoLists.count, prevList.count?.Count ?? 0, UsedParameter);
				}
				if (classRefs.TryGetValue("tcount", out value))
				{
					valInfoLists.tcount = new List<int>();
					createList(value, src.tcount, valInfoLists.tcount, prevList.tcount?.Count ?? 0, UsedParameter);
				}
				if (classRefs.TryGetValue("duration", out value))
				{
					valInfoLists.duration = new List<int>();
					createList(value, src.duration, valInfoLists.duration, prevList.duration?.Count ?? 0, UsedParameter);
				}
				if (classRefs.TryGetValue("distance", out value))
				{
					valInfoLists.distance = new List<int>();
					createList(value, src.distance, valInfoLists.distance, prevList.distance?.Count ?? 0, UsedParameter);
				}
				if (classRefs.TryGetValue("valid", out value))
				{
					valInfoLists.valid = new List<int>();
					createList(value, src.valid, valInfoLists.valid, prevList.valid?.Count ?? 0, UsedParameter);
				}
				if (classRefs.TryGetValue("level", out value))
				{
					valInfoLists.level = new List<int>();
					createList(value, src.level, valInfoLists.level, prevList.level?.Count ?? 0, UsedParameter);
				}
				if (classRefs.TryGetValue("spec", out value))
				{
					valInfoLists.spec = new List<int>();
					createList(value, src.spec, valInfoLists.spec, prevList.spec?.Count ?? 0, UsedParameter);
				}
				if (classRefs.TryGetValue("limit", out value))
				{
					valInfoLists.limit = new List<int>();
					createList(value, src.limit, valInfoLists.limit, prevList.limit?.Count ?? 0, UsedParameter);
				}
			}
		}
		return valInfoLists;
	}

	private static void createList(RefsMap refs, List<int> src, List<int> dst, int offs, HashSet<string> UsedParams)
	{
		if (refs == null || refs.Refs == null)
		{
			return;
		}
		int num = offs;
		if (offs > 0 && UsedParams != null)
		{
			num = 0;
			int num2 = 0;
			for (; num < refs.RefsName.Length; num++)
			{
				if (UsedParams.Contains(refs.RefsName[num]))
				{
					num2++;
				}
				if (num2 == offs)
				{
					break;
				}
			}
			num++;
		}
		int i = 0;
		int j = 0;
		for (int num3 = src.Count; i < num3; i++)
		{
			if (UsedParams != null)
			{
				for (; j < refs.RefsName.Length && !UsedParams.Contains(refs.RefsName[j + num + i]); j++)
				{
				}
			}
			if (refs.Refs[i + num + j])
			{
				dst.Add(src[i]);
			}
		}
	}
}
