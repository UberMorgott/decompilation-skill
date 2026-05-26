using System.Collections.Generic;
using System.Linq;

namespace AutoGRAPHShell.Modules.OnlineDeviceProvider;

public sealed class HostEndPointServerDeviceItemEqualityComparer : IEqualityComparer<KeyValuePair<HostEndPoint, ServerDeviceItem[]>>
{
	public bool Equals(KeyValuePair<HostEndPoint, ServerDeviceItem[]> x, KeyValuePair<HostEndPoint, ServerDeviceItem[]> y)
	{
		if (x.Key != null && y.Key != null && (x.Key == y.Key || (x.Key.hostName == y.Key.hostName && x.Key.port == y.Key.port && x.Key.version == y.Key.version)) && x.Key.Secondary.Count() == y.Key.Secondary.Count() && x.Key.Secondary.SequenceEqual(y.Key.Secondary) && x.Value.Length == y.Value.Length)
		{
			return (from p in x.Value
				select p.Serial + "-" + p.SortData into p
				orderby p
				select p).SequenceEqual(from p in y.Value
				select p.Serial + "-" + p.SortData into p
				orderby p
				select p);
		}
		return false;
	}

	public int GetHashCode(KeyValuePair<HostEndPoint, ServerDeviceItem[]> obj)
	{
		int num = obj.Key.GetHashCode();
		if (obj.Value != null && obj.Value.Any())
		{
			num ^= obj.Value.Select((ServerDeviceItem p) => p.Serial * (p.SortData ? 1 : (-1))).Aggregate((int acc, int sel) => acc ^ sel);
		}
		return num;
	}
}
