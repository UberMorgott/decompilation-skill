using System;
using System.Collections.Generic;
using System.Reflection;
using AGInterfaces.ModuleProviders;
using KML;

namespace AGInterfaces;

public interface IAutoGRAPHShell
{
	Guid SchemeGuid { get; }

	string SchemeFileName { get; }

	Guid Guid { get; }

	Guid Initiator { get; }

	IAutoGRAPHModule module { get; }

	ILoggerProvider Logger { get; }

	ITechControlProvider TechControl { get; }

	IDeviceDataHandlerProvider DeviceDataHandler { get; }

	IDeviceSelectorProvider DeviceSelector { get; }

	IPropertiesRegistryProvider PropertiesRegistry { get; }

	ITripsSelectorProvider TripsSelector { get; }

	IViewerProvider Viewer { get; }

	IPhotoStorage PhotoStorage { get; }

	Guid ActiveModuleGuid { get; }

	string TwoLetterLang { get; }

	string ShellFolder { get; }

	string ModulesFolder { get; }

	string CommonDataFolder { get; }

	string UserDataFolder { get; }

	SourceInfo[] GetSourcesInfo(Guid guid);

	Guid GetSettingsOwner(SettingsType type, ElementType element, Guid id);

	IAutoGRAPHSettings[] GetMySettings(SettingsType type, ElementType element = ElementType.Module, Guid guid = default(Guid), InheritType inherit = InheritType.Yes);

	SetSettingsResult SetMySettings(IAutoGRAPHSettings[] settingsArray, SettingsType type, ElementType element = ElementType.Module, Guid guid = default(Guid));

	GroupOrElementInfo GetElementNodeInfo(ElementType elementType, Guid guid);

	MainImageProps GetImageProps(ElementType elementType, Guid guid);

	int? GetSerialNumber(Guid guid);

	TimeZoneInfo GetDeviceTimeZone(Guid guid);

	GroupOrElementInfo[] GetElementNodes(ElementType elementType, Guid guid = default(Guid), bool copyingSettings = true);

	Dictionary<Guid, GroupOrElementInfo> GetElementGuids(ElementType elementType);

	Dictionary<string, PropertyTable> GetElementStrIDs(ElementType elementType);

	GroupOrElementInfo[] GetAccessibleDeviceNodes();

	KMLFile GetGeoFencesKMLFile();

	InspectedObject[] GetInspectedObjects(Guid deviceGuid);

	SetSettingsResult SetImageProps(ElementType elementType, MainImageProps imageProps, Guid guid);

	SetSettingsResult SetImageProps(ElementType elementType, Dictionary<Guid, MainImageProps> imgDict);

	SetSettingsResult SetSerialNumber(int? serialNum, Guid guid);

	SetSettingsResult SetSerialNumber(Dictionary<Guid, int?> snDict);

	SetSettingsResult SetDeviceTimeZone(TimeZoneInfo timeZoneInfo, Guid guid);

	void FireSetSettings(SettingsType type, ElementType element, IAutoGRAPHSettings[] prevCommonSettings, IAutoGRAPHSettings[] prevIndividualSettings);

	object InvokeMethod(Delegate method);

	object InvokeMethod(Delegate method, params object[] args);

	object ExecuteByAnotherProvider(Func<IAutoGRAPHShell, object> executeMethod, Guid guid);

	Guid[] GetModulesGuidArray(Func<IAutoGRAPHModule, bool> conditionMethod = null);

	ModuleInfo GetModuleInfo(Guid guid);

	Dictionary<ModuleInfo, DevPrmsGroupInfo[]> GetDeviceDataHandlersDevPrmInfo(Guid guid, DevPrmsType type, ImagesSize imagesSize);

	string GetAbsoluteFileName(string baseFolder, string fileName, bool expandEnvVariables = false);

	string GetRelativeFileName(string baseFolder, string fileName, bool absoluteIfBranching);

	IDisposable CreateAga2Obj(bool IsCache);

	string GetAddressFromLocation(Location location, DevSwitchPrmStatusInfo[] gfArray, AddressDisplayPriority priority, string format, bool addDistance, IDisposable aga2Obj);

	string GetRegionFromLocation(LocationAddr addr, IDisposable aga2Obj);

	string GetCityFromLocation(LocationAddr addr, IDisposable aga2Obj);

	HashSet<string> GetReservedNames();

	IEnumerable<AGBaseMember> GetBaseMembers(DevPrmsType prmsType, bool addBasic, MemberTypes memberType, AGInfoGroupType groupType);

	IEnumerable<AGBaseMember> GetNumericBaseMembers(DevPrmsType prmsType, bool addBasic, MemberTypes memberType);

	bool DeleteElementNodes(ElementType elementType, Guid[] IDs);

	bool SetElementNodes(ElementType elementType, GroupOrElementInfo[] elements);

	bool AddNewElementNodes(ElementType elementType, GroupOrElementInfo[] elements);

	bool RefreshElementNodeSettings(ElementType elementType, SettingsType settingsType, GroupOrElementInfo element);

	Tuple<double, double> CalculateSquarePerimeter(KMLBaseObject kmlObject);

	ILocale GetLocale();

	IDataViewerFormatters GetFormatters();

	void AddMissingSettingsElement(GroupOrElementInfo elementInfo);

	void ApplyNodeChanges(Guid parentGuid, List<KMLBaseObject> kmlObjects, Dictionary<string, KMLStyle> kmlStyles);

	IParameterMapping[] GetParametersMapping(Guid guid);
}
