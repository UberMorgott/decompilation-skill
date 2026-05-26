using System;
using System.Collections.Generic;
using System.Threading;

namespace AGInterfaces;

public interface ITripsSelector : IAutoGRAPHModule
{
	void InvalidSources(IAutoGRAPHShell shellProvider, SourceChangeType changeType, Guid senderGuid);

	bool HasAreaSharer(IAutoGRAPHShell shellProvider, DataArrays arrays, int splittersIndex, bool[] splitTripsArray);

	bool HasEmptyImplFiltrExists(IAutoGRAPHShell shellProvider, DataArrays arrays, int splittersIndex, bool[] splitTripsArray);

	void ShareTrips(IAutoGRAPHShell shellProvider, DataArrays arrays, OnlineInfo onlineInfo, ReportTimeSpan reportTimeSpan, int splittersIndex, bool[] splitTripsArray, ImplementTimeSpan[] timeSpans, CancellationToken cancellationToken, ReportProgressPercent reportProgress);

	GraphArrays GetCurrentTripGraph(IAutoGRAPHShell shellProvider, DataArrays currArrays, int tripIndex, string ordinateName, AbscissType abscissType);

	HashSet<string> GetPrmsNameFromTripSharer(IAutoGRAPHShell shellProviderm, Guid guid, int splittersIndex, bool[] splitTripsArray);
}
