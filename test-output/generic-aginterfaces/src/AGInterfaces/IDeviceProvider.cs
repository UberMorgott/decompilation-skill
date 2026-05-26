using System;
using System.Collections.Generic;

namespace AGInterfaces;

public interface IDeviceProvider : IAutoGRAPHModule
{
	IDeviceAccessInfo GetDevAccessInfo(IAutoGRAPHShell shellProvider);

	HashSet<Guid> GetReceivingArray(IAutoGRAPHShell shellProvider);

	void InvalidSources(IAutoGRAPHShell shellProvider, SourceChangeType changeType, Guid senderGuid);
}
