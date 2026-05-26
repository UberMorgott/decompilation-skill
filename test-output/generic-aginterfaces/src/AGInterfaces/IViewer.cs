using System;

namespace AGInterfaces;

public interface IViewer : IAutoGRAPHModule
{
	void InvalidSources(IAutoGRAPHShell shellProvider, SourceChangeType changeType, Guid senderGuid);
}
