using System;
using System.Collections.Generic;
using System.Threading;
using AGInterfaces.Classes;

namespace AGInterfaces;

public class ExpressionBaseInitInfo
{
	public readonly IAutoGRAPHShell shellProvider;

	public readonly TimeZoneInfo timeZoneInfo;

	public readonly IPhotoStorage photoStorage;

	public readonly DateTime nextInspDT;

	public readonly InspectionCalcStatus inspCalcStatus;

	public readonly InspectionNotification inspNotification;

	public readonly Dictionary<string, double> techControlValues;

	public readonly Dictionary<string, object> properties;

	public readonly Dictionary<int, string> gfTypes;

	public readonly Dictionary<int, string> implTypes;

	public readonly IElements Devices;

	public readonly IElements GeoFences;

	public readonly IElements Drivers;

	public readonly IElements Implements;

	public readonly IElements Tasks;

	public readonly GeoFenceArrays geoFenceArrays;

	public readonly ReportTimeSpan summerNorm;

	public readonly Func<IDisposable> createAga2Obj;

	public readonly Func<LocationAddr, IDisposable, string> getRegionFromLocation;

	public readonly Func<LocationAddr, IDisposable, string> getCityFromLocation;

	public readonly CustomRoute[] routes;

	public readonly Dictionary<Guid, CustomRoute> routeByGuidDict;

	public DevSwitchPrmStatusInfo[] gfArray;

	public Dictionary<Quadro<Guid>, int> gfStatuses;

	public readonly CancellationToken cancellationToken;

	public readonly IProcessedAreaCalculation AreaCalculation;

	public readonly ReportProgress AreaProgress;

	public readonly AreaProgressInfo AreaProgressInfo;

	public readonly bool FullPhoto;

	public ExpressionBaseInitInfo(IAutoGRAPHShell shellProvider, TimeZoneInfo timeZoneInfo, IPhotoStorage photoStorage, ReportTimeSpan summerNorm = default(ReportTimeSpan), DateTime nextInspDT = default(DateTime), InspectionCalcStatus inspCalcStatus = InspectionCalcStatus.Unknown, InspectionNotification inspNotification = InspectionNotification.None, Dictionary<string, double> techControlValues = null, Dictionary<string, object> properties = null, Dictionary<int, string> gfTypes = null, Dictionary<int, string> implTypes = null, IElements devices = null, IElements geoFences = null, IElements drivers = null, IElements implements = null, IElements tasks = null, GeoFenceArrays geoFenceArrays = null, Func<IDisposable> createAga2Obj = null, Func<LocationAddr, IDisposable, string> getRegionFromLocation = null, Func<LocationAddr, IDisposable, string> getCityFromLocation = null, CustomRoute[] routes = null, Dictionary<Guid, CustomRoute> routeByGuidDict = null, CancellationToken cancellationToken = default(CancellationToken), IProcessedAreaCalculation areaCalculation = null, ReportProgress areaProgress = null, AreaProgressInfo areaProgressInfo = null, bool fullPhoto = false)
	{
		this.shellProvider = shellProvider;
		this.timeZoneInfo = timeZoneInfo;
		this.photoStorage = photoStorage;
		this.summerNorm = summerNorm;
		this.nextInspDT = nextInspDT;
		this.inspCalcStatus = inspCalcStatus;
		this.inspNotification = inspNotification;
		this.techControlValues = techControlValues;
		this.properties = properties;
		this.gfTypes = gfTypes;
		this.implTypes = implTypes;
		Devices = devices;
		GeoFences = geoFences;
		Drivers = drivers;
		Implements = implements;
		Tasks = tasks;
		this.geoFenceArrays = geoFenceArrays;
		this.createAga2Obj = createAga2Obj;
		this.getRegionFromLocation = getRegionFromLocation;
		this.getCityFromLocation = getCityFromLocation;
		this.routes = routes;
		this.routeByGuidDict = routeByGuidDict;
		this.cancellationToken = cancellationToken;
		AreaCalculation = areaCalculation;
		AreaProgress = areaProgress;
		AreaProgressInfo = areaProgressInfo;
		FullPhoto = fullPhoto;
	}
}
