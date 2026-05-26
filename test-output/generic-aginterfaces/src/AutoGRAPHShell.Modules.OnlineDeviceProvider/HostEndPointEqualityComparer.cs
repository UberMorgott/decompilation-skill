using System.Collections.Generic;

namespace AutoGRAPHShell.Modules.OnlineDeviceProvider;

public sealed class HostEndPointEqualityComparer : IEqualityComparer<HostEndPoint>
{
	public bool Equals(HostEndPoint x, HostEndPoint y)
	{
		return x.Equals(y);
	}

	public int GetHashCode(HostEndPoint obj)
	{
		return obj.GetHashCode();
	}
}
