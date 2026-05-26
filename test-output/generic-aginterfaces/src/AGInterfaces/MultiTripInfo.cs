using System;
using System.Collections.Generic;
using System.Linq;

namespace AGInterfaces;

public sealed class MultiTripInfo
{
	public OnlineInfo masterOnlineInfo;

	public Dictionary<Guid, SlaveSourcesInfo> slavesSourcesInfo;

	public MultiTripInfo(OnlineInfo masterOnlineInfo, Dictionary<Guid, SlaveSourcesInfo> slavesSourcesInfo)
	{
		this.masterOnlineInfo = masterOnlineInfo;
		this.slavesSourcesInfo = slavesSourcesInfo;
	}

	public bool SlaveSourceEquals(Dictionary<Guid, SlaveSourcesInfo> slavesSourcesInfo)
	{
		if (this.slavesSourcesInfo == slavesSourcesInfo)
		{
			return true;
		}
		if (this.slavesSourcesInfo == null || slavesSourcesInfo == null)
		{
			return false;
		}
		if (this.slavesSourcesInfo.Count != slavesSourcesInfo.Count)
		{
			return false;
		}
		foreach (KeyValuePair<Guid, SlaveSourcesInfo> item in this.slavesSourcesInfo)
		{
			if (!slavesSourcesInfo.TryGetValue(item.Key, out var value))
			{
				return false;
			}
			if (!SlaveSourcesInfo.Equals(item.Value, value))
			{
				return false;
			}
		}
		return true;
	}

	public bool SlaveSourcePartEquals(Dictionary<Guid, SlaveSourcesInfo> slavesSourcesInfo)
	{
		if (this.slavesSourcesInfo == slavesSourcesInfo)
		{
			return true;
		}
		if (this.slavesSourcesInfo == null || slavesSourcesInfo == null)
		{
			return false;
		}
		Dictionary<Guid, SlaveSourcesInfo> dictionary = new Dictionary<Guid, SlaveSourcesInfo>();
		Dictionary<Guid, SlaveSourcesInfo> dictionary2 = new Dictionary<Guid, SlaveSourcesInfo>();
		foreach (KeyValuePair<Guid, SlaveSourcesInfo> item in this.slavesSourcesInfo)
		{
			SourceInfo[] array = ((item.Value.sourcesInfo != null) ? item.Value.sourcesInfo.Where((SourceInfo s) => s.length > 0).ToArray() : new SourceInfo[0]);
			SourceInfo[] array2 = ((item.Value.routesInfo != null) ? item.Value.routesInfo.Where((SourceInfo s) => s.length > 0).ToArray() : new SourceInfo[0]);
			if (array.Any() || array2.Any())
			{
				dictionary.Add(item.Key, new SlaveSourcesInfo(item.Value.serialNo, array, array2));
			}
		}
		foreach (KeyValuePair<Guid, SlaveSourcesInfo> item2 in slavesSourcesInfo)
		{
			SourceInfo[] array3 = ((item2.Value.sourcesInfo != null) ? item2.Value.sourcesInfo.Where((SourceInfo s) => s.length > 0).ToArray() : new SourceInfo[0]);
			SourceInfo[] array4 = ((item2.Value.routesInfo != null) ? item2.Value.routesInfo.Where((SourceInfo s) => s.length > 0).ToArray() : new SourceInfo[0]);
			if (array3.Any() || array4.Any())
			{
				dictionary2.Add(item2.Key, new SlaveSourcesInfo(item2.Value.serialNo, array3, array4));
			}
		}
		if (dictionary.Count != dictionary2.Count)
		{
			return false;
		}
		foreach (KeyValuePair<Guid, SlaveSourcesInfo> item3 in dictionary)
		{
			if (!dictionary2.TryGetValue(item3.Key, out var value))
			{
				return false;
			}
			if (!SlaveSourcesInfo.PartEquals(item3.Value, value))
			{
				return false;
			}
		}
		return true;
	}
}
