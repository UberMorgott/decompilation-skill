using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;
using AGInterfaces.Classes;

namespace AGInterfaces;

public sealed class InitComputeInfo
{
	public Guid guid;

	public object prmsInfo;

	public ColoringSettings cursorColoring;

	public ImagingSettings cursorImaging;

	public Dictionary<GroupOrElementInfo, bool> geoFenceInfo;

	public Dictionary<PropertyTable, bool> visibleGFTables;

	public string areaFolder;

	public bool areaMode;

	public ImageFormat areaImageFormat;

	public double workZone;

	public AddressDisplayPriority addrDisplayPriority;

	public bool addDistanceToAddress;

	public TimeSpan reportBeginExtension;

	public TimeSpan reportEndExtension;

	public HashSet<string> techControlPrms;

	public ExpressionBaseInitInfo initInfo;

	public InitComputeInfo(IAutoGRAPHShell shellProvider, Guid guid, object prmsInfo, ColoringSettings cursorColoring, ImagingSettings cursorImaging, Dictionary<GroupOrElementInfo, bool> geoFenceInfo, Dictionary<PropertyTable, bool> visibleGFTables, TimeZoneInfo timeZoneInfo, IPhotoStorage photoStorage, ReportTimeSpan summerNorm, double workZone, AddressDisplayPriority addrDisplayPriority, bool addDistanceToAddress, TimeSpan reportBeginExtension, TimeSpan reportEndExtension, DateTime nextInspDT, InspectionCalcStatus inspCalcStatus, InspectionNotification inspNotification, Dictionary<string, double> techControlValues, Dictionary<string, object> properties, Dictionary<int, string> gfTypes, Dictionary<int, string> implTypes, IElements devices, IElements geoFences, IElements drivers, IElements implements, IElements tasks, Func<IDisposable> createAga2Obj, Func<LocationAddr, IDisposable, string> getRegionFromLocation, Func<LocationAddr, IDisposable, string> getCityFromLocation, GeoFenceArrays geoFenceArrays, CustomRoute[] routes, Dictionary<Guid, CustomRoute> routeByGuidDict, CancellationToken cancellationToken, string areaFolder, bool areaMode, ImageFormat areaImageFormat, IProcessedAreaCalculation areaCalculation, ReportProgress areaProgress, AreaProgressInfo areaProgressInfo, bool fullPhoto, HashSet<string> techControlPrms)
	{
		this.guid = guid;
		this.prmsInfo = prmsInfo;
		this.cursorColoring = cursorColoring;
		this.cursorImaging = cursorImaging;
		this.geoFenceInfo = geoFenceInfo;
		this.visibleGFTables = visibleGFTables;
		this.areaFolder = areaFolder;
		this.areaMode = areaMode;
		this.areaImageFormat = areaImageFormat;
		this.workZone = workZone;
		this.addrDisplayPriority = addrDisplayPriority;
		this.addDistanceToAddress = addDistanceToAddress;
		this.reportBeginExtension = reportBeginExtension;
		this.reportEndExtension = reportEndExtension;
		this.techControlPrms = techControlPrms ?? new HashSet<string>();
		initInfo = new ExpressionBaseInitInfo(shellProvider, timeZoneInfo, photoStorage, summerNorm, nextInspDT, inspCalcStatus, inspNotification, techControlValues, properties, gfTypes, implTypes, devices, geoFences, drivers, implements, tasks, geoFenceArrays, createAga2Obj, getRegionFromLocation, getCityFromLocation, routes, routeByGuidDict, cancellationToken, areaCalculation, areaProgress, areaProgressInfo, fullPhoto);
	}
}
