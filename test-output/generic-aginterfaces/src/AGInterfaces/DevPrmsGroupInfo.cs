using System;
using System.Linq;
using Newtonsoft.Json;

namespace AGInterfaces;

[Serializable]
public class DevPrmsGroupInfo
{
	public string name;

	public string caption;

	[JsonIgnore]
	public StoreMultiCaption description;

	public DevPrmInfo[] devPrmInfoArray;

	public static DevPrmInfo getPrmInfoByAlias(string stageName, string alias, AddValueType addValueType, DevPrmsGroupInfo[] prmsGroupInfoArray, out DevPrmsGroupInfo grpInfo)
	{
		grpInfo = null;
		if (prmsGroupInfoArray == null)
		{
			return null;
		}
		if (alias != null)
		{
			try
			{
				alias = string.Format(alias, stageName);
			}
			catch
			{
			}
		}
		if (string.IsNullOrEmpty(alias))
		{
			return null;
		}
		DevPrmInfo devPrmInfo = null;
		foreach (DevPrmsGroupInfo devPrmsGroupInfo in prmsGroupInfoArray)
		{
			if (devPrmsGroupInfo is DevPrmInfo)
			{
				if (((DevPrmInfo)devPrmsGroupInfo).addValueType == addValueType && ((DevPrmInfo)devPrmsGroupInfo).alias == alias)
				{
					devPrmInfo = (DevPrmInfo)devPrmsGroupInfo;
					break;
				}
			}
			else
			{
				devPrmInfo = devPrmsGroupInfo.devPrmInfoArray.FirstOrDefault((DevPrmInfo p) => p.addValueType == addValueType && p.alias == alias);
			}
			if (devPrmInfo != null)
			{
				grpInfo = devPrmsGroupInfo;
				break;
			}
		}
		return devPrmInfo;
	}

	public static DevPrmInfo getPrmInfoByName(string stageAlias, string name, AddValueType addValueType, DevPrmsGroupInfo[] prmsGroupInfoArray, out DevPrmsGroupInfo grpInfo)
	{
		grpInfo = null;
		if (prmsGroupInfoArray == null)
		{
			return null;
		}
		if (name != null)
		{
			try
			{
				name = string.Format(name, stageAlias);
			}
			catch
			{
			}
		}
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		DevPrmInfo devPrmInfo = null;
		foreach (DevPrmsGroupInfo devPrmsGroupInfo in prmsGroupInfoArray)
		{
			if (devPrmsGroupInfo is DevPrmInfo)
			{
				if (((DevPrmInfo)devPrmsGroupInfo).addValueType == addValueType && devPrmsGroupInfo.name == name)
				{
					devPrmInfo = (DevPrmInfo)devPrmsGroupInfo;
					break;
				}
			}
			else
			{
				devPrmInfo = devPrmsGroupInfo.devPrmInfoArray.FirstOrDefault((DevPrmInfo p) => p.addValueType == addValueType && p.name == name);
			}
			if (devPrmInfo != null)
			{
				grpInfo = devPrmsGroupInfo;
				break;
			}
		}
		return devPrmInfo;
	}

	public static DevPrmInfo getPrmInfoByName(string name, DevPrmsGroupInfo[] prmsGroupInfoArray, out DevPrmsGroupInfo grpInfo)
	{
		grpInfo = null;
		if (prmsGroupInfoArray == null)
		{
			return null;
		}
		DevPrmInfo devPrmInfo = null;
		foreach (DevPrmsGroupInfo devPrmsGroupInfo in prmsGroupInfoArray)
		{
			if (devPrmsGroupInfo is DevPrmInfo)
			{
				if (devPrmsGroupInfo.name == name)
				{
					devPrmInfo = (DevPrmInfo)devPrmsGroupInfo;
					break;
				}
			}
			else
			{
				devPrmInfo = devPrmsGroupInfo.devPrmInfoArray.FirstOrDefault((DevPrmInfo p) => p.name == name);
			}
			if (devPrmInfo != null)
			{
				grpInfo = devPrmsGroupInfo;
				break;
			}
		}
		return devPrmInfo;
	}

	public static DevPrmInfo getPrmInfoByName(string name, DataArrays arrays, out DevPrmsGroupInfo grpInfo)
	{
		grpInfo = null;
		if (arrays == null)
		{
			return null;
		}
		return getPrmInfoByName(name, arrays.tabularPrmsGroupInfoArray, out grpInfo);
	}
}
