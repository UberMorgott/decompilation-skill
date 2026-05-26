using System;
using System.Collections.Generic;
using System.Threading;
using AGInterfaces.ReportDynamicData;

namespace AGInterfaces.ModuleProviders;

public interface ITripsSelectorProvider
{
	DevPrmsGroupInfo[] GetDevPrmsGroupInfoArray(Guid selectorGuid, Guid guid, DevPrmsType type, ImagesSize imagesSize);

	Dictionary<Guid, MultiTripInfo> GetReportInfo(Guid selectorGuid, Dictionary<Guid, MultiTripInfo> curMultiTripInfoDict, bool fullUpdate, Dictionary<Guid, DataArrays> arraysDict, Dictionary<Guid, SourceInfo> sourceInfoDict, ReportTimeSpan reportTimeSpan, int splittersIndex = -1, bool[] splitTripsArray = null, Func<HandlerArrays, Guid, ImplementTimeSpan[]> getShareTimeSpans = null, ImagesSize mainImageSize = ImagesSize.Small, ImagesSize statusImagesSize = ImagesSize.Small, TimeZoneInfo alternateTimeZoneInfo = null, CancellationToken cancellationToken = default(CancellationToken), ReportProgress reportProgress = null, ReportProgress areaReportProgress = null, bool disableMultithread = false, Guid[] slaveGuids = null, Guid[] areaGuids = null, Action<SourceInfo[], SourceInfo[]> setNeededArrays = null, bool areaMode = false, bool preloadCasheFiles = false, Dictionary<Guid, NeedParamsForReport> needParamsForReport = null);

	Dictionary<Guid, MultiTripInfo> GetReportInfo(Guid selectorGuid, Dictionary<Guid, MultiTripInfo> curMultiTripInfoDict, bool fullUpdate, Dictionary<Guid, DataArrays> arraysDict, Dictionary<Guid, SourceInfo> sourceInfoDict, Dictionary<Guid, ReportTimeSpan> reportTimeSpan, int splittersIndex = -1, bool[] splitTripsArray = null, Func<HandlerArrays, Guid, ImplementTimeSpan[]> getShareTimeSpans = null, ImagesSize mainImageSize = ImagesSize.Small, ImagesSize statusImagesSize = ImagesSize.Small, TimeZoneInfo alternateTimeZoneInfo = null, CancellationToken cancellationToken = default(CancellationToken), ReportProgress reportProgress = null, ReportProgress areaReportProgress = null, bool disableMultithread = false, Guid[] slaveGuids = null, Guid[] areaGuids = null, Action<SourceInfo[], SourceInfo[]> setNeededArrays = null, bool areaMode = false, bool preloadCasheFiles = false, Dictionary<Guid, NeedParamsForReport> needParamsForReport = null);
}
