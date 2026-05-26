using System;
using System.Collections.Generic;
using System.Threading;
using AGInterfaces.Classes;
using AGInterfaces.ReportDynamicData;

namespace AGInterfaces.ModuleProviders;

public interface IViewerProvider
{
	ReportDataStorage GetDataStorageForReport(DevPrmsType[] types, string[] stageNames, Guid[] guidArray = null, SourceType sourceType = SourceType.GSM, ReportTimeSpan reportTimeSpan = default(ReportTimeSpan), SourceInfo sourceInfo = null, int splittersIndex = -1, Func<HandlerArrays, Guid, ImplementTimeSpan[]> shareTrips = null, ExportTrackSettings exportTrackSettings = null, ImagesSize mainImageSize = ImagesSize.Large, ImagesSize statusImagesSize = ImagesSize.Small, TimeZoneInfo alternateTimeZoneInfo = null, GeoFenceArrays alternateGeoFenceArrays = null, IEnumerable<Guid> alternateGroupAndGFGuids = null, IEnumerable<Guid> mchpGuids = null, CancellationToken cancellationToken = default(CancellationToken), ReportProgress reportProgress = null, bool disableMultithread = true, bool preloadCasheFiles = false, NeedParamsForReport needParamsForReport = null);

	Tuple<object[], DataArrays[], ReportTimeSpan> GetClassicDataLists(string[] dataSetNames, Guid[] guidArray = null, SourceType sourceType = SourceType.GSM, ReportTimeSpan reportTimeSpan = default(ReportTimeSpan), SourceInfo sourceInfo = null, int splittersIndex = -1, Func<HandlerArrays, Guid, ImplementTimeSpan[]> shareTrips = null, ExportTrackSettings exportTrackSettings = null, TimeZoneInfo alternateTimeZoneInfo = null, GeoFenceArrays alternateGeoFenceArrays = null, IEnumerable<Guid> alternateGroupAndGFGuids = null, IEnumerable<Guid> mchpGuids = null, CancellationToken cancellationToken = default(CancellationToken), ReportProgress reportProgress = null, bool needDataArrays = false, bool disableMultithread = true, bool preloadCasheFiles = false, NeedParamsForReport needParamsForReport = null);

	Dictionary<string, Type> GetParametersDictionary(IAutoGRAPHShell shellProvider, DevPrmsType type);

	Dictionary<string, Type> GetAreaDSParameters();

	List<string> GetCommonStatusNameArray();

	Dictionary<Guid, OnlineInfo> GetReportInfo(Guid dataBaseGuid, SourceType sourceType, Dictionary<Guid, OnlineInfo> curOnlineInfoDict, Dictionary<Guid, DataArrays> arraysDict, bool fullUpdate, ReportTimeSpan reportTimeSpan, ImagesSize mainImageSize = ImagesSize.Small, ImagesSize statusImagesSize = ImagesSize.Small, TimeZoneInfo alternateTimeZoneInfo = null, GeoFenceArrays alternateGeoFenceArrays = null, IEnumerable<Guid> alternateGroupAndGFGuids = null, Func<HandlerArrays, Guid, ImplementTimeSpan[]> shareTrips = null, CancellationToken cancellationToken = default(CancellationToken), ReportProgress reportProgress = null, bool disableMultithread = false, bool preloadCasheFiles = false, Dictionary<Guid, NeedParamsForReport> needParamsForReport = null);

	ImplementInfo[] GetReportInfoAreaMode(Guid[] guidArray, Guid[] areaGuids, SourceType sourceType, ReportTimeSpan reportTimeSpan, bool groupingByArea, ImagesSize mainImageSize = ImagesSize.Large, ImagesSize statusImagesSize = ImagesSize.Small, TimeZoneInfo alternateTimeZoneInfo = null, CancellationToken cancellationToken = default(CancellationToken), ReportProgress reportProgress = null, bool disableMultithread = true, NeedParamsForReport needParamsForReport = null);

	DevPrmsGroupInfo[] GetDevPrmsGroupInfoArray(Guid guid, DevPrmsType type, ImagesSize imagesSize);

	AGBandInfo[] GetDevPrmBands(DevPrmsGroupInfo[] devPrmsGroupInfoArray, float columnsWidth);

	ReportDataStorage GetDataStorageForReportAreaMode(Guid[] guidArray, IEnumerable<Guid> areaGuids, SourceType sourceType, ReportTimeSpan reportTimeSpan, bool needAreaImages, bool groupingByArea = true, ImagesSize mainImageSize = ImagesSize.Large, ImagesSize statusImagesSize = ImagesSize.Small, TimeZoneInfo alternateTimeZoneInfo = null, CancellationToken cancellationToken = default(CancellationToken), ReportProgress reportProgress = null, bool disableMultithread = true, NeedParamsForReport needParamsForReport = null);

	Tuple<DataArrays, OnlineInfo> GetReportInfoFromFiles(Guid guid, SourceInfo[] sources, int? entriesLimit = null, SourceType sourceType = SourceType.GSM, CancellationToken cancellationToken = default(CancellationToken), ReportProgress reportProgress = null, bool disableMultithread = true, NeedParamsForReport needParamsForReport = null);
}
