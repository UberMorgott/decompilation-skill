using System;
using System.Collections.Generic;

namespace AGInterfaces.ModuleProviders;

public interface IDeviceSelectorProvider
{
	DevPrmsRouteInfo[] GetDevPrmsRouteInfoArray(Guid guid);

	DevPrmsGroupInfo[] GetDevPrmsGroupInfoArray(Guid guid, DevPrmsType type, ImagesSize imagesSize);

	void InvalidSources(SourceChangeType changeType, Guid senderGuid);

	Tuple<Guid[], bool[], Dictionary<PropertyTable, bool>> GetGFSettings(Guid guid);

	Array[] GetStageArray(int startPos, int endPos, DataArrays arrays, bool findUsingValues);

	AGBandInfo[] GetDevPrmBands(DevPrmsGroupInfo[] devPrmsGroupInfoArray, float columnsWidth);
}
