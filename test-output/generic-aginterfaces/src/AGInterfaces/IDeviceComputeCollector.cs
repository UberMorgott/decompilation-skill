using System;

namespace AGInterfaces;

public interface IDeviceComputeCollector
{
	void Add(Guid schemeGuid, int? serial);
}
