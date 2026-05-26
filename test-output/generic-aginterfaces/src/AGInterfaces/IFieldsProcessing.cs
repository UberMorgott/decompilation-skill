using System;
using System.Threading;

namespace AGInterfaces;

public interface IFieldsProcessing
{
	AreaInfo CalculateSquare(TrackPoint[] trackPoints, Guid gfGuid, CancellationToken cancellationToken);

	Unit[] GetUnits(Guid id);

	void SetUnits(Guid id, Unit[] units);
}
