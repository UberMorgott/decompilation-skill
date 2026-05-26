using System.Collections.Generic;

namespace AutoGRAPHShell.Modules.OnlineDeviceProvider;

public sealed class ServerDeviceItemEqualityComparer : IEqualityComparer<ServerDeviceItem>
{
	private readonly bool isOnlySerial;

	public ServerDeviceItemEqualityComparer(bool onlySerial)
	{
		isOnlySerial = onlySerial;
	}

	public bool Equals(ServerDeviceItem x, ServerDeviceItem y)
	{
		if ((x != null || y != null) && !x.Equals(y))
		{
			return x.Serial == y.Serial;
		}
		return true;
	}

	public int GetHashCode(ServerDeviceItem obj)
	{
		return obj.GetHashCode();
	}
}
