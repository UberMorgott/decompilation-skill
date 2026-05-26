using System;

namespace AGInterfaces;

public interface IAreaPolygonStore
{
	AreaPolygonSaveResult Save(IAutoGRAPHShell shellProvider, Guid areaGuid, InitComputeInfo computeInfo, AreaInfo areaInfo, int? tripIndex = null);

	void DeleteUnusedAreaMaps(DataArrays arrays);
}
