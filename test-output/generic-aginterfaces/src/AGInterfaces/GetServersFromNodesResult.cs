using System.Collections.Generic;
using AutoGRAPHShell.Modules.OnlineDeviceProvider;

namespace AGInterfaces;

public sealed class GetServersFromNodesResult
{
	public readonly Dictionary<HostEndPoint, ServerDeviceItem[]> ServerItems;

	public readonly Dictionary<string, ServerDeviceItem[]> DataFolderItems;

	public GetServersFromNodesResult(Dictionary<HostEndPoint, ServerDeviceItem[]> serverItems, Dictionary<string, ServerDeviceItem[]> dataFolderItems)
	{
		ServerItems = serverItems;
		DataFolderItems = dataFolderItems;
	}
}
