using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using AGInterfaces.Classes;
using AGInterfaces.ReportDynamicData;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public interface IDeviceDataHandler : IAutoGRAPHModule
{
	SourceInfo[] GetSourcesInfo(IAutoGRAPHShell shellProvider, Guid guid);

	void InvalidSources(IAutoGRAPHShell shellProvider, SourceChangeType changeType, Guid senderGuid);

	ITimeFiltrationSettings GetAffectTimeSettings(IAutoGRAPHShell shellProvider, Guid guid);

	Tuple<DateTime[], HandlerArrays, TimeSpan> SetTimeDependentValues(IAutoGRAPHShell shellProvider, LoaderInfo[] loadersInfo, object affectTimeSettings);

	void SetDTArrayFromDraft(LoaderDraftInfo loaderDraftInfo, object affectTimeSettings);

	InitComputeInfo GetInitComputeInfo(IAutoGRAPHShell shellProvider, Guid guid, DataArrays arrays, ColoringSettings cursorColoring, ImagingSettings cursorImaging, ImagesSize mainImageSize, ImagesSize statusImagesSize, TimeZoneInfo alternateTimeZoneInfo = null, GeoFenceArrays alternateGeoFenceArrays = null, IEnumerable<Guid> alternateGroupAndGFGuids = null, IEnumerable<Guid> gfGuidsAsAreas = null, CancellationToken cancellationToken = default(CancellationToken), string areaSubFolder = null, bool areaMode = false, IProcessedAreaCalculation areaCalculation = null, ReportProgress areaProgress = null, AreaProgressInfo areaProgressInfo = null, bool fullPhoto = false, NeedParamsForReport needParamsForReport = null, HashSet<string> techControlPrms = null);

	Tuple<Guid[], bool[], Dictionary<PropertyTable, bool>> GetGFSettings(IAutoGRAPHShell shellProvider, Guid guid);

	void SetCrdsValues(IAutoGRAPHShell shellProvider, DataArrays arrays, HandlerArrays harrays, ref CrdIndependentFields instance, CancellationToken cancellationToken, bool disableMultithread);

	void SetTabularValues(IAutoGRAPHShell shellProvider, DataArrays arrays, HandlerArrays harrays, OnlineInfo onlineInfo, ReportTimeSpan reportTimeSpan, IDisposable aga2GlobalObj, Dictionary<Guid, Tuple<string, DateTime[], Coordinates[]>> mobileChPCrds, ref CrdIndependentFields instance, CancellationToken cancellationToken, ReportProgressPercent reportProgress, bool disableMultithread);

	void SetTripValues(IAutoGRAPHShell shellProvider, DataArrays arrays, HandlerArrays harrays, OnlineInfo onlineInfo, IDisposable aga2GlobalObj, CrdIndependentFields instance, CancellationToken cancellationToken, ReportProgressPercent reportProgress, bool disableMultithread, Dictionary<Guid, ImplementTrack[]> slavesImplementTracks = null);

	void SetFinalValues(IAutoGRAPHShell shellProvider, DataArrays arrays, HandlerArrays harrays, OnlineInfo onlineInfo, IDisposable aga2GlobalObj, CrdIndependentFields instance, bool setTrackColorByOnlineTrack, CancellationToken cancellationToken, ReportProgressPercent reportProgress, bool disableMultithread);

	void SetImages(IAutoGRAPHShell shellProvider, DataArrays arrays, OnlineInfo onlineInfo);

	DevPrmInfo[] GetTechControlPrms(IAutoGRAPHShell shellProvider, Guid guid);

	ImplementTrack[] GetTrackPointsForArea(IAutoGRAPHShell shellProvider, DataArrays arrays, HandlerArrays harrays);

	DevPrmsGroupInfo[] GetDevPrmsGroupInfoArray(IAutoGRAPHShell shellProvider, Guid guid, DevPrmsType type, ImagesSize imagesSize);

	DevPrmTreeListNode[] GetDevPrmsGroupInfoTreeListNodes(IAutoGRAPHShell shellProvider, Guid guid, DevPrmsType type, ImagesSize imagesSize);

	DevPrmsRouteInfo[] GetDevPrmsRouteInfoArray(IAutoGRAPHShell shellProvider, Guid guid);

	void SetDevPrmsRoutes(IAutoGRAPHShell shellProvider, Guid guid, DevPrmsRouteInfo[] routesSettings);

	void DeleteDevPrmsRoutes(IAutoGRAPHShell shellProvider, Guid guid, Guid[] routesGuids);

	GroupOrElementInfo[] GetDevPrmsTaskInfoArray(IAutoGRAPHShell shellProvider, Guid guid);

	void SetDevPrmsTasks(IAutoGRAPHShell shellProvider, Guid guid, GroupOrElementInfo[] tasksSettings);

	void DeleteDevPrmsTasks(IAutoGRAPHShell shellProvider, Guid guid, Guid[] tasksGuids);

	bool IsStationary(InitComputeInfo computeInfo);

	AGBandInfo[] GetDevPrmBands(IAutoGRAPHShell shellProvider, DevPrmsGroupInfo[] devPrmsGroupInfoArray, float columnsWidth);

	LookUpEditItem[] GetGFTypeDescriptions(IAutoGRAPHShell shellProvider);

	LookUpEditItem[] GetRouteTypeDescriptions(IAutoGRAPHShell shellProvider);

	ReportTimeSpan GetEntriesLimits(IAutoGRAPHShell shellProvider, Guid guid, TimeZoneInfo timeZoneInfo, out int entriesLimit12bit, out int entriesLimit16bit);

	bool IsEnableAreaCalculation(IAutoGRAPHShell shellProvider, Guid guid);

	Array[] GetStageArray(IAutoGRAPHShell shellProvider, int startPos, int endPos, DataArrays arrays, bool findUsingValues);

	Dictionary<string, Type> GetParametersDictionary(IAutoGRAPHShell shellProvider, DevPrmsType type);

	List<string> GetCommonStatusNameArray(IAutoGRAPHShell shellProvider);

	object GetClassicDataList(string name);

	void AddDataTableEntries(IAutoGRAPHShell shellProvider, IDisposable aga2Obj, ReportDataSet table, DevPrmsType type, string stageName, DataArrays arrays, ExportTrackSettings exportTrackSettings, OnlineInfo onlineInfo, CancellationToken cancellationToken);

	void AddClassicDataEntries(IAutoGRAPHShell shellProvider, IDisposable aga2Obj, object table, DevPrmsType type, string stageName, DataArrays arrays, ExportTrackSettings exportTrackSettings, OnlineInfo onlineInfo, CancellationToken cancellationToken);

	void AddClassicDataEntries2(IAutoGRAPHShell shellProvider, object table, DevPrmsType type, DataArrays arrays, StageArrays stArr, int filterByStatus);

	string GetParameterNameByAlias(DataArrays arrays, string alias);

	bool NeedDetectMobileCHP(DataArrays arrays);

	InspectedObject[] GetInspectedObjects(IAutoGRAPHShell shellProvider, Guid guid);

	List<PrefixGroup> GetPrefixesList();

	List<OperationGroup> GetOperationsList();

	OnlineInfo[] GetOnlineInfoArray(IAutoGRAPHShell shellProvider, Guid guid);

	Dictionary<Guid, OnlineInfo[]> GetOnlineInfoArray(IAutoGRAPHShell shellProvider, IEnumerable<Guid> guids);

	Dictionary<Guid, OnlineInfo[]> GetChildrenOnlineInfoArrays(IAutoGRAPHShell shellProvider, Guid guid);
}
