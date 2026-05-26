using System.Collections.Generic;
using System.Linq;

namespace AutoGRAPHShell.Modules.OnlineDeviceProvider;

public sealed class StringServerDeviceItemEqualityComparer : IEqualityComparer<KeyValuePair<string, ServerDeviceItem[]>>
{
	public bool Equals(KeyValuePair<string, ServerDeviceItem[]> x, KeyValuePair<string, ServerDeviceItem[]> y)
	{
		if (x.Key != null && y.Key != null && (x.Key == y.Key || string.Compare(x.Key, y.Key, ignoreCase: true) == 0) && x.Value.Length == y.Value.Length)
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

	public int GetHashCode(KeyValuePair<string, ServerDeviceItem[]> obj)
	{
		int num = obj.Key.GetHashCode();
		if (obj.Value != null && obj.Value.Any())
		{
			num ^= obj.Value.Select((ServerDeviceItem p) => p.Serial * (p.SortData ? 1 : (-1))).Aggregate((int acc, int sel) => acc ^ sel);
		}
		return num;
	}
}
