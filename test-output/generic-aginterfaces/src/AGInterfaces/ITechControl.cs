using System;
using System.Collections.Generic;

namespace AGInterfaces;

public interface ITechControl
{
	event EventHandler MileagesUpdated;

	InspectedObjectData[] GetInspectedObjectData();

	Dictionary<string, double> GetCurrentValues(Guid device);

	DateTime GetPredictableNextInspectionDateTime(Guid device);

	InspectionCalcStatus GetCalcStatus(Guid device);

	InspectionNotification GetNotification(Guid device);
}
