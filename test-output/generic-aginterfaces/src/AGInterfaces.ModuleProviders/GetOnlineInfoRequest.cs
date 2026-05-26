using System;
using System.Collections.Generic;

namespace AGInterfaces.ModuleProviders;

public class GetOnlineInfoRequest
{
	public Guid guid;

	public IDisposable aga2GlobalObj;

	public OnlineInfo[] curOnlineInfoArray;

	public Dictionary<Guid, DataArrays> slavesDataArrays;

	public Dictionary<Guid, SlaveSourcesInfo> slavesSourcesInfo;

	public GetOnlineInfoRequest(Guid guid, IDisposable aga2GlobalObj, OnlineInfo[] curOnlineInfoArray, Dictionary<Guid, DataArrays> slavesDataArrays, Dictionary<Guid, SlaveSourcesInfo> slavesSourcesInfo)
	{
		this.guid = guid;
		this.aga2GlobalObj = aga2GlobalObj;
		this.curOnlineInfoArray = curOnlineInfoArray;
		this.slavesDataArrays = slavesDataArrays;
		this.slavesSourcesInfo = slavesSourcesInfo;
	}
}
