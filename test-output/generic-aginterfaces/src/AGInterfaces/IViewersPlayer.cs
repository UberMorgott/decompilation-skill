using System;

namespace AGInterfaces;

public interface IViewersPlayer : IAutoGRAPHModule
{
	void InvalidSources(IAutoGRAPHShell shellProvider, SourceChangeType changeType, Guid senderTSGuid, bool playerSync, bool posAlreadyRestored, bool canPlay);
}
