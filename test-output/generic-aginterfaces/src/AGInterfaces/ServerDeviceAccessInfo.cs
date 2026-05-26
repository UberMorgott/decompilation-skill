using System.Collections.Generic;
using System.Linq;
using AutoGRAPHShell.Modules.OnlineDeviceProvider;

namespace AGInterfaces;

public class ServerDeviceAccessInfo : IDeviceAccessInfo
{
	private readonly Dictionary<HostEndPoint, HashSet<int>> deviceList;

	private readonly Dictionary<HostEndPoint, int> errorCodes;

	private readonly ISCAccessInfo scAccess;

	public ServerDeviceAccessInfo(Dictionary<HostEndPoint, HashSet<int>> deviceList, Dictionary<HostEndPoint, int> errorCodes, ISCAccessInfo scAccess)
	{
		this.errorCodes = errorCodes;
		this.deviceList = deviceList ?? new Dictionary<HostEndPoint, HashSet<int>>();
		this.scAccess = scAccess;
	}

	Dictionary<HostEndPoint, HashSet<int>> IDeviceAccessInfo.GetDeviceList()
	{
		return deviceList;
	}

	Dictionary<HostEndPoint, int> IDeviceAccessInfo.GetLastErrorCodes(int ID)
	{
		return errorCodes;
	}

	bool IDeviceAccessInfo.IsDeviceAllowed(int ID)
	{
		if (!scAccess.IsAuthenticated)
		{
			return deviceList.Values.Any((HashSet<int> p) => p.Contains(ID));
		}
		return true;
	}
}
