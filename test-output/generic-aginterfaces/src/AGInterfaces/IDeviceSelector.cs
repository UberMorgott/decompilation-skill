using System;
using System.Collections.Generic;
using System.Reflection;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public interface IDeviceSelector : IAutoGRAPHModule
{
	void InvalidSources(IAutoGRAPHShell shellProvider, SourceChangeType changeType, Guid senderGuid);

	DevPrmsGroupInfo[] GetDevPrmsGroupInfoArray(IAutoGRAPHShell shellProvider, Guid guid, DevPrmsType type, ImagesSize imagesSize);

	AGBandInfo[] GetDevPrmBands(IAutoGRAPHShell shellProvider, DevPrmsGroupInfo[] devPrmsGroupInfoArray, float columnsWidth);

	ViewProperties GetGroupOrDeviceProps(IAutoGRAPHShell shellProvider, Guid guid, OnlineInfo onlineInfo);

	Dictionary<Guid, ViewProperties> GetGroupChildrenProps(IAutoGRAPHShell shellProvider, Guid guid, Dictionary<Guid, OnlineInfo> onlineInfoDict);

	Dictionary<Guid, ViewProperties> GetGroupChildrenProps(IAutoGRAPHShell shellProvider, Guid guid, Dictionary<Guid, OnlineInfo> onlineInfoDict, HashSet<Guid> checkedChildren, out Dictionary<GroupOrElementInfo, bool> geoFenceInfo, out Dictionary<PropertyTable, bool> visibleGFTables);

	ColoringSettings GetCursorColoring(IAutoGRAPHShell shellProvider, Guid guid);

	ImagingSettings GetCursorImaging(IAutoGRAPHShell shellProvider, Guid guid, ImagesSize imagesSize = ImagesSize.Small);
}
