using System;
using System.Collections.Generic;

namespace AGInterfaces.ModuleProviders;

public interface ITechControlProvider
{
	string CounterDataSetName { get; }

	InspectionCalcStatus GetCalcStatus(Guid guid);

	DateTime GetNextInspDateTime(Guid guid);

	InspectionNotification GetNotification(Guid guid);

	Dictionary<string, double> GetValues(Guid guid);

	Dictionary<string, Type> GetCountersDSParameters();
}
