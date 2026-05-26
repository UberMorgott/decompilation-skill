using System.Collections.Generic;
using AutoGRAPHShell.Modules.OnlineDeviceProvider;

namespace AGInterfaces;

public interface IDeviceAccessInfo
{
	bool IsDeviceAllowed(int ID);

	Dictionary<HostEndPoint, HashSet<int>> GetDeviceList();

	Dictionary<HostEndPoint, int> GetLastErrorCodes(int ID);
}
