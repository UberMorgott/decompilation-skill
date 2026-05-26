using System;
using System.Linq;
using DataLoader;

namespace AGInterfaces.Localization;

public sealed class CommonFields
{
	public EventDescription[] ed;

	public string[] gd;

	public string[] dd;

	public string[] td;

	[Translatable]
	public string str_Version = "Version: {0}.{1}.{2} (revision {3})";

	[Translatable]
	public string str_Ready = "Ready";

	[Translatable]
	public string str_Loading = "Loading...";

	[Translatable]
	public string str_Opening = "Opening...";

	[Translatable]
	public string str_Saving = "Saving...";

	[Translatable]
	public string str_ApplySettings = "Apply settings...";

	[Translatable]
	public string str_ThereAreExceptions = "There are exceptions:";

	[Translatable]
	public string str_NoOptions = "There are no options." + Environment.NewLine + "Choose other item.";

	[Translatable]
	public string str_NotAllowed = "You are not allowed." + Environment.NewLine + "Choose other item.";

	[Translatable]
	public string str_WaitingForLogin = "Please, waiting for log in";

	[Translatable]
	public string str_NotAllowedAdminRequest = "You are not allowed" + Environment.NewLine + "Please, log in with administrator rights on all servers";

	[Translatable]
	public string str_Folder = "Folder";

	[Translatable]
	public string str_FolderCreated = "Created";

	[Translatable]
	public string str_FolderModified = "Modified";

	[Translatable]
	public string str_FolderIsntCreated = "Folder is not created";

	[Translatable]
	public string str_File = "File";

	[Translatable]
	public string str_FileCreated = "Created";

	[Translatable]
	public string str_FileModified = "Modified";

	[Translatable]
	public string str_FileIsntCreated = "File is not created";

	[Translatable]
	public string str_Insert = "Insert";

	[Translatable]
	public string str_InsertDirectory = "Insert directory";

	[Translatable]
	public string str_Delete = "Delete";

	[Translatable]
	public string str_Copy = "Copy";

	[Translatable]
	public string str_Cut = "Cut";

	[Translatable]
	public string str_Paste = "Paste";

	[Translatable]
	public string str_CreateOrOpenUserDataFolderError = "Can't create or open user data folder";

	[Translatable]
	public string str_SaveCurrentSchemeQuestion = "Save current scheme?";

	[Translatable]
	public string str_RestoreCurrentSchemeQuestion = "Restore current scheme?" + Environment.NewLine + "All recent changes will be lost" + Environment.NewLine + "Continue?";

	[Translatable]
	public string str_SaveSchemesFolderInformationError = "Can't save schemes folder information";

	[Translatable]
	public string str_CreateOrOpenSchemesFolderError = "Can't create or open schemes folder";

	[Translatable]
	public string str_CreateOrOpenDevicesFolderError = "Can't create or open devices folder";

	[Translatable]
	public string str_CreateOrOpenGeoFencesFolderError = "Can't create or open geo-fences folder";

	[Translatable]
	public string str_CreateOrOpenDriversFolderError = "Can't create or open drivers folder";

	[Translatable]
	public string str_CreateOrOpenImplementsFolderError = "Can't create or open implements folder";

	[Translatable]
	public string str_CreateOrOpenTasksFolderError = "Can't create or open tasks folder";

	[Translatable]
	public string str_CreateOrOpenDesktopsFolderError = "Can't create or open desktops folder";

	[Translatable]
	public string str_SaveSchemeFileError = "Can't save scheme file";

	[Translatable]
	public string str_SaveSchemeInfoFileError = "Can't save scheme info file";

	[Translatable]
	public string str_SaveDevicesFileError = "Can't save devices file";

	[Translatable]
	public string str_SaveGeoFencesFileError = "Can't save geo-fences file";

	[Translatable]
	public string str_SaveDriversFileError = "Can't save drivers file";

	[Translatable]
	public string str_SaveImplementsFileError = "Can't save implements file";

	[Translatable]
	public string str_SaveTasksFileError = "Can't save tasks file";

	[Translatable]
	public string str_SaveDesktopFileError = "Can't save desktop file";

	[Translatable]
	public string str_SaveSecurityFileError = "Can't save security file";

	[Translatable]
	public string str_MoveSchemeFileError = "Can't move scheme file";

	[Translatable]
	public string str_MoveDevicesFileError = "Can't move devices file";

	[Translatable]
	public string str_MoveGeoFencesFileError = "Can't move geo-fences file";

	[Translatable]
	public string str_MoveDriversFileError = "Can't move drivers file";

	[Translatable]
	public string str_MoveImplementsFileError = "Can't move implements file";

	[Translatable]
	public string str_MoveTasksFileError = "Can't move tasks file";

	[Translatable]
	public string str_MoveDesktopFileError = "Can't move desktop file";

	[Translatable]
	public string str_MoveSecurityFileError = "Can't move security file";

	[Translatable]
	public string str_DeleteSchemeFileError = "Can't delete scheme file";

	[Translatable]
	public string str_DeleteDevicesFileError = "Can't delete devices file";

	[Translatable]
	public string str_DeleteGeoFencesFileError = "Can't delete geo-fences file";

	[Translatable]
	public string str_DeleteDriversFileError = "Can't delete drivers file";

	[Translatable]
	public string str_DeleteImplementsFileError = "Can't delete implements file";

	[Translatable]
	public string str_DeleteTasksFileError = "Can't delete tasks file";

	[Translatable]
	public string str_DeleteDesktopFileError = "Can't delete desktop file";

	[Translatable]
	public string str_DeleteSecurityFileError = "Can't delete security file";

	[Translatable]
	public string str_SchemeFiles = "Scheme files";

	[Translatable]
	public string str_AllFiles = "All files";

	[Translatable]
	public string str_All = "(All)";

	[Translatable]
	public string str_OpenNewerSchemeQuestion = "This scheme was created in newer version of software" + Environment.NewLine + "You can't save the scheme until update" + Environment.NewLine + "Update now?";

	[Translatable]
	public string str_SaveNewerSchemeError = "Unable to save changes because this scheme was created in newer version of software";

	[Translatable]
	public string str_RestoreDesktopQuestion = "There are differences between current scheme and current desktop" + Environment.NewLine + "Restore desktop?";

	[Translatable]
	public string str_SaveModifiedSchemeQuestion = "Scheme has been modified during opening" + Environment.NewLine + "Save modified scheme?";

	[Translatable]
	public string str_Scheme = "Scheme";

	[Translatable]
	public string str_SchemeIsModified = "Scheme is modified";

	[Translatable]
	public string str_FullScreen = "Full screen";

	[Translatable]
	public string str_RebootShellQuestion = "Changes will be applied after reboot shell" + Environment.NewLine + "Reboot shell now?";

	[Translatable]
	public string str_ReopenSchemeInformation = "Changes will be applied after reopen scheme";

	[Translatable]
	public string str_DeleteSchemeQuestion = "All files of the current scheme will be deleted!" + Environment.NewLine + "But the scheme will be available until the program closure";

	[Translatable]
	public string str_RootGroup = "Root group";

	[Translatable]
	public string str_Group = "Group";

	[Translatable]
	public string str_MonitoringObject = "Monitoring object";

	[Translatable]
	public string str_MonitoringObjects = "Monitoring objects";

	[Translatable]
	public string str_GeoFence = "Geo-fence";

	[Translatable]
	public string str_GeoFences = "Geo-fences";

	[Translatable]
	public string str_Driver = "Driver";

	[Translatable]
	public string str_Drivers = "Drivers";

	[Translatable]
	public string str_Implement = "Implement";

	[Translatable]
	public string str_Implements = "Implements";

	[Translatable]
	public string str_Task = "Task";

	[Translatable]
	public string str_Tasks = "Tasks";

	[Translatable]
	public string str_Module = "Module";

	[Translatable]
	public string str_dontSupportInterface = "don't support interface";

	[Translatable]
	public string str_CommonOptions = "Common options";

	[Translatable]
	public string str_IndividualOptions = "Individual options";

	[Translatable]
	public string str_WLon = "W";

	[Translatable]
	public string str_ELon = "E";

	[Translatable]
	public string str_SLat = "S";

	[Translatable]
	public string str_NLat = "N";

	[Translatable]
	public string str_Yes = "Yes";

	[Translatable]
	public string str_No = "No";

	[Translatable]
	public string str_On = "On";

	[Translatable]
	public string str_Off = "Off";

	[Translatable]
	public string str_Tabular = "Tabular";

	[Translatable]
	public string str_Interval = "Interval";

	[Translatable]
	public string str_Final = "Final";

	[Translatable]
	public string str_Basic = "Basic";

	[Translatable]
	public string str_Parameters = "Parameters";

	[Translatable]
	public string str_Value = "Value";

	[Translatable]
	public string str_from = "from";

	[Translatable]
	public string str_to = "to";

	[Translatable]
	public string str_ValueKind = "Filling type";

	[Translatable]
	public string[] str_ValueKindsDesc = new string[7] { "Instant, f(time)", "Instant, f(dist.)", "Instant, exponential", "Accumulable", "Flag", "Accumulable, converted from flag", "Flag, converted from accumulable" };

	[Translatable]
	public string str_ValueUpdates = "Value updates";

	[Translatable]
	public string str_Always = "Always";

	[Translatable]
	public string str_Never = "Never";

	[Translatable]
	public string str_ByFollowingEntries = "By following entries";

	[Translatable]
	public string str_Groups = "Groups";

	[Translatable]
	public string[] str_GroupsDesc = new string[20]
	{
		"Data", "Time", "Debug", "Navigation", "Sensor", "Counter", "Level", "Fuel level", "Pressure", "Rotation",
		"Frequency", "Temperature", "CAN", "RS-485", "1-wire", "Motohours", "Distance", "Consumption", "Auto cistern", "Identifiers"
	};

	[Translatable]
	public string str_Image = "(image)";

	[Translatable]
	public string str_OneYear = "year";

	[Translatable]
	public string str_YearLast1 = "years";

	[Translatable]
	public string str_YearLast2_4 = "years";

	[Translatable]
	public string str_YearLast5_0 = "years";

	[Translatable]
	public string str_OneMonth = "month";

	[Translatable]
	public string str_MonthLast1 = "months";

	[Translatable]
	public string str_MonthLast2_4 = "months";

	[Translatable]
	public string str_MonthLast5_0 = "months";

	[Translatable]
	public string str_Years = "y";

	[Translatable]
	public string str_Months = "mon";

	[Translatable]
	public string str_Weeks = "w";

	[Translatable]
	public string str_Days = "d";

	[Translatable]
	public string str_Hours = "h";

	[Translatable]
	public string str_Mins = "m";

	[Translatable]
	public string str_Secs = "s";

	[Translatable]
	public string str_OLEClientCount = "OLE clients";

	[Translatable]
	public string str_FieldName = "Field name";

	[Translatable]
	public string str_UserParam = "User parameter";

	[Translatable]
	public string str_AND = "AND";

	[Translatable]
	public string str_OR = "OR";

	[Translatable]
	public string str_XOR = "XOR";

	[Translatable]
	public string str_SUM = "SUM";

	[Translatable]
	public string str_DIFF = "DIFF";

	[Translatable]
	public string str_Common = "Common settings";

	[Translatable]
	public string str_Sensors = "Sensors";

	[Translatable]
	public string str_Levels = "Other levels";

	[Translatable]
	public string str_FuelLevels = "Fuel levels";

	[Translatable]
	public string str_Pressures = "Pressures";

	[Translatable]
	public string str_Rotations = "Rotations";

	[Translatable]
	public string str_Temperatures = "Temperatures";

	[Translatable]
	public string str_Tanks = "Tanks";

	[Translatable]
	public string str_Engines = "Engines";

	[Translatable]
	public string str_Consumptions = "Consumptions";

	[Translatable]
	public string str_Distances = "Distances";

	[Translatable]
	public string str_IDs = "ID's";

	[Translatable]
	public string str_BasicExpressions = "Basic expressions";

	[Translatable]
	public string str_LogicalsExpressions = "Logicals expressions";

	[Translatable]
	public string str_NumericsExpressions = "Numerics expressions";

	[Translatable]
	public string str_NumericParameters = "Numeric parameters";

	[Translatable]
	public string str_Speed = "Speed";

	[Translatable]
	public string str_AreaCalculation = "Area calculation";

	[Translatable]
	public string str_WorkingTimeControl = "Working time control";

	[Translatable]
	public string str_Tachograph = "Tachograph";

	[Translatable]
	public string str_TableParams = "Table parameters";

	[Translatable]
	public string str_FinalParams = "Final parameters";

	[Translatable]
	public string str_DrivingQuality = "Driving quality";

	[Translatable]
	public string str_TechControl = "Technical control";

	[Translatable]
	public string str_Sensor = "Sensor";

	[Translatable]
	public string str_Level = "Level";

	[Translatable]
	public string str_FuelLevel = "Fuel level";

	[Translatable]
	public string str_Pressure = "Pressure";

	[Translatable]
	public string str_Rotation = "Rotation";

	[Translatable]
	public string str_Temperature = "Temperature";

	[Translatable]
	public string str_Tank = "Tank";

	[Translatable]
	public string str_Engine = "Engine";

	[Translatable]
	public string str_Consumption = "Consumption";

	[Translatable]
	public string str_Distance = "Distance";

	[Translatable]
	public string str_ID = "ID";

	[Translatable]
	public string str_ByConsumption = "By Consumption";

	[Translatable]
	public string str_ByDistanceAndMotohours = "By Distance And Motohours";

	[Translatable]
	public string str_ByMotohours = "By Motohours";

	[Translatable]
	public string str_ByMovingTime = "By Moving Time";

	[Translatable]
	public string str_ByTanks = "By Fuel Level of Tanks ";

	[Translatable]
	public string str_None = "None";

	[Translatable]
	public string str_LitresPerHour = "l/h";

	[Translatable]
	public string str_LitresPer100km = "l/100 km";

	[Translatable]
	public string str_Litres = "l";

	[Translatable]
	public string str_Meters = "m";

	[Translatable]
	public string str_Kilometers = "km";

	[Translatable]
	public string str_Hectares = "ha";

	[Translatable]
	public string str_OneYearShort = "y";

	[Translatable]
	public string str_YearLast1Short = "y";

	[Translatable]
	public string str_YearLast2_4Short = "y";

	[Translatable]
	public string str_YearLast5_0Short = "y";

	[Translatable]
	public string str_MonthsShort = "mon";

	[Translatable]
	public string str_WeeksShort = "w";

	[Translatable]
	public string str_DaysShort = "d";

	[Translatable]
	public string str_HoursShort = "h";

	[Translatable]
	public string str_MinutesShort = "m";

	[Translatable]
	public string str_SecondsShort = "s";

	[Translatable]
	public string str_KBSec = "{0} Kb/sec";

	[Translatable]
	public string str_Remaining = "{0} remaining";

	[Translatable]
	public string StrPropertyTableToolTip = "Property table";

	[Translatable]
	public string StrDeletePropertyToolTip = "Delete property";

	[Translatable]
	public string str_true = "True";

	[Translatable]
	public string str_false = "False";

	[Translatable]
	public string str_OnlineData = "Online data";

	[Translatable]
	public string str_ReportData = "Report data";

	[Translatable]
	public LookUpEditItem[] RepositoryTypeGridColumnItems = new LookUpEditItem[20]
	{
		new LookUpEditItem(PropType.String, "String"),
		new LookUpEditItem(PropType.Number, "Number"),
		new LookUpEditItem(PropType.Date, "Date"),
		new LookUpEditItem(PropType.TareTable, "Tare table"),
		new LookUpEditItem(PropType.Time, "Time"),
		new LookUpEditItem(PropType.Memo, "Memo"),
		new LookUpEditItem(PropType.Color, "Color"),
		new LookUpEditItem(PropType.Bool, "Boolean"),
		new LookUpEditItem(PropType.Radio, "Radio-button"),
		new LookUpEditItem(PropType.Image, "Image"),
		new LookUpEditItem(PropType.File, "File"),
		new LookUpEditItem(PropType.ProgressBar, "Progress bar"),
		new LookUpEditItem(PropType.Combobox, "Combobox"),
		new LookUpEditItem(PropType.FileLink, "Link"),
		new LookUpEditItem(PropType.Device, "Monitoring object"),
		new LookUpEditItem(PropType.GeoFence, "Geo-fence"),
		new LookUpEditItem(PropType.Driver, "Driver"),
		new LookUpEditItem(PropType.Implement, "Implement"),
		new LookUpEditItem(PropType.Task, "Task"),
		new LookUpEditItem(PropType.CheckDays, "Days of the week")
	};

	[Translatable]
	public string str_GF_Count = "Arrivals count";

	[Translatable]
	public string str_GF_ON_dur = "Duration inside";

	[Translatable]
	public string str_GF_OFF_dur = "Duration outside";

	[Translatable]
	public string str_GF_F_ON_time = "Time of first arrival";

	[Translatable]
	public string str_GF_F_OFF_time = "Time of first departure";

	[Translatable]
	public string str_GF_L_ON_time = "Time of last arrival";

	[Translatable]
	public string str_GF_L_OFF_time = "Time of last departure";

	[Translatable]
	public string str_GF_F_ON_L_OFF_dur = "Duration between first arrival and last departure";

	[Translatable]
	public string str_GF_F_OFF_L_ON_dur = "Duration between first departure and last arrival";

	[Translatable]
	public string str_GF_ON_dist = "Distance inside";

	[Translatable]
	public string str_GF_OFF_dist = "Distance outside";

	[Translatable]
	public string str_GF_F_ON_dist = "Distance to first arrival";

	[Translatable]
	public string str_GF_F_OFF_dist = "Distance to first departure";

	[Translatable]
	public string str_GF_L_ON_dist = "Distance to last arrival";

	[Translatable]
	public string str_GF_L_OFF_dist = "Distance to last departure";

	[Translatable]
	public string str_GF_F_ON_L_OFF_dist = "Distance between first arrival and last departure";

	[Translatable]
	public string str_GF_F_OFF_L_ON_dist = "Distance between first departure and last arrival";

	[Translatable]
	public string str_GF_Min_ON_dur = "Minimal duration inside";

	[Translatable]
	public string str_GF_Min_OFF_dur = "Minimal duration outside";

	[Translatable]
	public string str_GF_Min_ON_dist = "Minimal distance inside";

	[Translatable]
	public string str_GF_Min_OFF_dist = "Minimal distance outside";

	[Translatable]
	public string str_GF_Max_ON_dur = "Maximal duration inside";

	[Translatable]
	public string str_GF_Max_OFF_dur = "Maximal duration outside";

	[Translatable]
	public string str_GF_Max_ON_dist = "Maximal distance inside";

	[Translatable]
	public string str_GF_Max_OFF_dist = "Maximal distance outside";

	[Translatable]
	public string str_GF_Mean_ON_dur = "Average duration inside";

	[Translatable]
	public string str_GF_Mean_OFF_dur = "Average duration outside";

	[Translatable]
	public string str_GF_Mean_ON_dist = "Average distance inside";

	[Translatable]
	public string str_GF_Mean_OFF_dist = "Average distance outside";

	[Translatable]
	public string str_GF_F_F_ON_dur = "Duration of first absence";

	[Translatable]
	public string str_GF_F_F_OFF_dur = "Duration of first presence";

	[Translatable]
	public string str_GF_L_ON_L_dur = "Duration of last presence";

	[Translatable]
	public string str_GF_L_OFF_L_dur = "Duration of last absence";

	[Translatable]
	public string str_GF_F_F_ON_dist = "Distance of first absence";

	[Translatable]
	public string str_GF_F_F_OFF_dist = "Distance of first presence";

	[Translatable]
	public string str_GF_L_ON_L_dist = "Distance of last presence";

	[Translatable]
	public string str_GF_L_OFF_L_dist = "Distance of last absence";

	[Translatable]
	public string str_GF_First_ID = "First ID";

	[Translatable]
	public string str_GF_Last_ID = "Last ID";

	[Translatable]
	public string str_GF_First_Addr = "First address";

	[Translatable]
	public string str_GF_Last_Addr = "Last address";

	[Translatable]
	public string str_GF_First_Type = "First type";

	[Translatable]
	public string str_GF_Last_Type = "Last type";

	[Translatable]
	public string str_SmallestGeoFence = "Smallest GeoFence";

	[Translatable]
	public string str_Count = "Count";

	[Translatable]
	public string str_Minimum = "Minimum";

	[Translatable]
	public string str_Maximum = "Maximum";

	[Translatable]
	public string str_Mean = "Average";

	[Translatable]
	public string str_First = "First Value";

	[Translatable]
	public string str_Last = "Last Value";

	[Translatable]
	public string str_Diff = "Difference";

	[Translatable]
	public string str_Range = "Range";

	[Translatable]
	public string str_ON_dur = "Duration of ON state";

	[Translatable]
	public string str_OFF_dur = "Duration of OFF state";

	[Translatable]
	public string str_F_ON_time = "Time of first switch ON";

	[Translatable]
	public string str_F_OFF_time = "Time of first switch OFF";

	[Translatable]
	public string str_L_ON_time = "Time of last switch ON";

	[Translatable]
	public string str_L_OFF_time = "Time of last switch OFF";

	[Translatable]
	public string str_F_ON_L_OFF_dur = "Duration between first switch ON and last switch OFF";

	[Translatable]
	public string str_F_OFF_L_ON_dur = "Duration between first switch OFF and last switch ON";

	[Translatable]
	public string str_ON_dist = "Distance of ON state";

	[Translatable]
	public string str_OFF_dist = "Distance of OFF state";

	[Translatable]
	public string str_F_ON_dist = "Distance to first switch ON";

	[Translatable]
	public string str_F_OFF_dist = "Distance to first switch OFF";

	[Translatable]
	public string str_L_ON_dist = "Distance to last switch ON";

	[Translatable]
	public string str_L_OFF_dist = "Distance to last switch OFF";

	[Translatable]
	public string str_F_ON_L_OFF_dist = "Distance between first switch ON and last switch OFF";

	[Translatable]
	public string str_F_OFF_L_ON_dist = "Distance between first switch OFF and last switch ON";

	[Translatable]
	public string str_Min_ON_dur = "Minimal duration of ON state";

	[Translatable]
	public string str_Min_OFF_dur = "Minimal duration of OFF state";

	[Translatable]
	public string str_Min_ON_dist = "Minimal distance of ON state";

	[Translatable]
	public string str_Min_OFF_dist = "Minimal distance of OFF state";

	[Translatable]
	public string str_Max_ON_dur = "Maximal duration of ON state";

	[Translatable]
	public string str_Max_OFF_dur = "Maximal duration of OFF state";

	[Translatable]
	public string str_Max_ON_dist = "Maximal distance of ON state";

	[Translatable]
	public string str_Max_OFF_dist = "Maximal distance of OFF state";

	[Translatable]
	public string str_Mean_ON_dur = "Average duration of ON state";

	[Translatable]
	public string str_Mean_OFF_dur = "Average duration of OFF state";

	[Translatable]
	public string str_Mean_ON_dist = "Average distance of ON state";

	[Translatable]
	public string str_Mean_OFF_dist = "Average distance if OFF state";

	[Translatable]
	public string str_F_F_ON_dur = "Duration of first OFF state";

	[Translatable]
	public string str_F_F_OFF_dur = "Duration of first ON state";

	[Translatable]
	public string str_L_ON_L_dur = "Duration of last ON state";

	[Translatable]
	public string str_L_OFF_L_dur = "Duration of last OFF state";

	[Translatable]
	public string str_F_F_ON_dist = "Distance of first OFF state";

	[Translatable]
	public string str_F_F_OFF_dist = "Distance of first ON state";

	[Translatable]
	public string str_L_ON_L_dist = "Distance of last ON state";

	[Translatable]
	public string str_L_OFF_L_dist = "Distance of last OFF state";

	[Translatable]
	public string str_MH = "Motohours";

	[Translatable]
	public string str_MH_parks = "Motohours on parks";

	[Translatable]
	public string str_MH_move = "Motohours on move";

	[Translatable]
	public string str_Consump = "Consumption";

	[Translatable]
	public string str_Consump_per100km = "Consumption (l/100 km)";

	[Translatable]
	public string str_Consump_perHour = "Consumption (l/h)";

	[Translatable]
	public string str_Consump_perMH = "Consumption (l/motohour)";

	[Translatable]
	public string str_Consump_parks = "Consumption on parks";

	[Translatable]
	public string str_Consump_parks_perHour = "Consumption on parks (l/h)";

	[Translatable]
	public string str_Consump_parks_perMH = "Consumption on parks (l/motohour)";

	[Translatable]
	public string str_Consump_move = "Consumption on move";

	[Translatable]
	public string str_Consump_move_per100km = "Consumption on move (l/100 km)";

	[Translatable]
	public string str_Consump_move_perHour = "Consumption on move (l/h)";

	[Translatable]
	public string str_Consump_move_perMH = "Consumption on move (l/motohour)";

	[Translatable]
	public string str_Tank_spec = "Specific consumption";

	[Translatable]
	public string str_Tank_limit = "Limit of specific consumption";

	[Translatable]
	public EntryInfo[] arr_EntryInfo = new EntryInfo[84]
	{
		new EntryInfo("Spurious", "Spurious", "Unsupported"),
		new EntryInfo("Spurious", "Spurious", "Deleted entry"),
		new EntryInfo("Spurious", "Spurious", "Unknown"),
		new EntryInfo("Navigation", "Data", "Coordinates"),
		new EntryInfo("Analog data", "Levels", "Analog data"),
		new EntryInfo("Counters", "Counters", "Counters 1-2"),
		new EntryInfo("Counters", "Counters", "Counters 3-4"),
		new EntryInfo("Navigation", "Data", "Motion characteristic"),
		new EntryInfo("Counters", "Counters", "Counters 5-6"),
		new EntryInfo("Identifier", "Data", "Identifier"),
		new EntryInfo("Counters", "Counters", "Counters 7-8"),
		new EntryInfo("RS-485", "Levels", "RS-485 - lls 1-4"),
		new EntryInfo("RS-485", "Levels", "RS-485 - lls 5-8"),
		new EntryInfo("CAN", "Data", "CAN - way"),
		new EntryInfo("CAN", "Levels", "CAN - levels"),
		new EntryInfo("CAN", "Data", "CAN - engine"),
		new EntryInfo("CAN", "Temperature", "CAN - temperature"),
		new EntryInfo("CAN", "Data", "CAN - distance"),
		new EntryInfo("Status", "Data", "Event"),
		new EntryInfo("CAN", "Data", "CAN - axis 1"),
		new EntryInfo("CAN", "Data", "CAN - axis 2"),
		new EntryInfo("CAN", "Data", "CAN - axis 3"),
		new EntryInfo("CAN", "Data", "CAN - axis 4"),
		new EntryInfo("CAN", "Data", "CAN - axis 5"),
		new EntryInfo("CAN", "Data", "CAN - axis 6"),
		new EntryInfo("CAN", "Data", "CAN - axis 7"),
		new EntryInfo("CAN", "Data", "CAN - axis 8"),
		new EntryInfo("CAN", "Data", "CAN - axis 9"),
		new EntryInfo("CAN", "Data", "CAN - axis 10"),
		new EntryInfo("CAN", "Data", "CAN - axis 11"),
		new EntryInfo("CAN", "Data", "CAN - axis 12"),
		new EntryInfo("CAN", "Data", "CAN - axis 13"),
		new EntryInfo("CAN", "Data", "CAN - axis 14"),
		new EntryInfo("CAN", "Data", "CAN - axis 15"),
		new EntryInfo("CAN", "Data", "CAN - axis 16"),
		new EntryInfo("CAN", "Data", "CAN - user data 1"),
		new EntryInfo("CAN", "Data", "CAN - user data 2"),
		new EntryInfo("CAN", "Data", "CAN - user data 3"),
		new EntryInfo("CAN", "Data", "CAN - user data 4"),
		new EntryInfo("1-wire", "Temperature", "1-wire - temperature 1-4"),
		new EntryInfo("1-wire", "Temperature", "1-wire - temperature 5-8"),
		new EntryInfo("RS-485", "Data", "RS-485 - inputs extender"),
		new EntryInfo("RS-485", "Data", "RS-485 - fill amount"),
		new EntryInfo("RS-485", "Data", "RS-485 - fuel rate"),
		new EntryInfo("RS-485", "Data", "RS-485 - fill duration"),
		new EntryInfo("RS-485", "Counters", "RS-485 - passenger traffic"),
		new EntryInfo("RS-485", "Levels", "RS-485 - measuring device"),
		new EntryInfo("CAN", "Data", "CAN - errors"),
		new EntryInfo("CAN", "Data", "CAN - calculated fuel rate"),
		new EntryInfo("CAN", "Data", "CAN - mode"),
		new EntryInfo("CAN", "Data", "CAN - engine, aux"),
		new EntryInfo("Long entry", "Data", "Long entry - header"),
		new EntryInfo("Long entry", "Data", "Long entry - data"),
		new EntryInfo("Palesse", "Data", "Palesse - parameters"),
		new EntryInfo("Palesse", "Data", "Palesse - flags"),
		new EntryInfo("Palesse", "Data", "Palesse - statistic"),
		new EntryInfo("RS-485", "Levels", "RS-485 - lls extended"),
		new EntryInfo("ISOBUS", "Data", "ISOBUS"),
		new EntryInfo("CAN", "Data", "Guard"),
		new EntryInfo("Counters", "Counters", "Heat counter"),
		new EntryInfo("RS-485", "Data", "RS-485 - numerical data"),
		new EntryInfo("Skywave", "Data", "Skywave"),
		new EntryInfo("Status", "Data", "External device status"),
		new EntryInfo("Status", "Data", "Tachograph"),
		new EntryInfo("Status", "Data", "Wheel state"),
		new EntryInfo("RS-485", "Levels", "RS-485 - ls-1 extended"),
		new EntryInfo("RS-485", "Levels", "RS-485 - ls-2 extended"),
		new EntryInfo("RS-485", "Levels", "RS-485 - Struna+"),
		new EntryInfo("Navigation", "Data", "Driving quality"),
		new EntryInfo("RS-485", "Levels", "RS-485 - tilt angle"),
		new EntryInfo("RS-485", "Levels", "Axis load"),
		new EntryInfo("CAN", "Levels", "CAN - trailer weight"),
		new EntryInfo("CAN", "Data", "CAN - doors statuses"),
		new EntryInfo("CAN", "Data", "CAN - vehicle status"),
		new EntryInfo("CAN", "Levels", "CAN - alternate fuel"),
		new EntryInfo("Status", "Data", "FMS Tell Tale Status: FMS1"),
		new EntryInfo("CAN", "Data", "CAN - discrete parameters"),
		new EntryInfo("RS-485", "Levels", "RS-485 - tilt angle 2"),
		new EntryInfo("Named", "Data", "Named param - header"),
		new EntryInfo("Named", "Data", "Named param - data"),
		new EntryInfo("Named", "Data", "Named array - header"),
		new EntryInfo("Named", "Data", "Named array - subhead"),
		new EntryInfo("Named", "Data", "Named array - data"),
		new EntryInfo("CAN", "Levels", "CAN - liters")
	};

	[Translatable]
	public EventDescription[] arr_EventInfo = new EventDescription[32]
	{
		new EventDescription(0, "Unknown event"),
		new EventDescription(3, "GPS reset"),
		new EventDescription(7, "GSM is off"),
		new EventDescription(128, "GPS antenna is connected"),
		new EventDescription(129, "GPS antenna is disconnected"),
		new EventDescription(130, "SC in GPS antenna"),
		new EventDescription(131, "Device tº is normal"),
		new EventDescription(132, "Device tº out of normal range"),
		new EventDescription(133, "Device tº is non-working"),
		new EventDescription(134, "GSM signal jamming"),
		new EventDescription(135, "Hitting the device, acceleration> 2G"),
		new EventDescription(136, "Non-working accelerometer"),
		new EventDescription(137, "Built-in timer correction"),
		new EventDescription(138, "Remote firmware started"),
		new EventDescription(139, "Remote firmware completed"),
		new EventDescription(140, "Remote firmware failed with error"),
		new EventDescription(141, "Working with photos"),
		new EventDescription(142, "Working with WiFi"),
		new EventDescription(143, "Photo was taken"),
		new EventDescription(201, "Skywave alarm"),
		new EventDescription(203, "Skywave version"),
		new EventDescription(204, "Skywave single alarm"),
		new EventDescription(205, "Skywave attention"),
		new EventDescription(206, "Skywave low battery"),
		new EventDescription(207, "Skywave LED alarm"),
		new EventDescription(208, "Skywave poll"),
		new EventDescription(209, "Skywave status engine"),
		new EventDescription(210, "Skywave status object"),
		new EventDescription(211, "Skywave panic button"),
		new EventDescription(220, "SMS begin"),
		new EventDescription(221, "SMS end"),
		new EventDescription(222, "SMS SIM")
	};

	[Translatable]
	public string[] arr_TripSharer = new string[12]
	{
		"Ten-day", "Day", "Shift", "Condition", "Change No->Yes", "Status", "Geo-fence", "Month", "Timespan", "Area",
		"Implement", "Driver"
	};

	[Translatable]
	public string[] arr_DTfltr = new string[12]
	{
		"Ok", "Recover date", "Deleted entry", "Unknown entry", "Duplicated entry", "Invalid CRC", "Invalid time", "By time flag", "By file time", "By power renewal",
		"Back throw", "Forward throw"
	};

	[Translatable]
	public string[] arr_LLfltr = new string[10] { "", "Ok", "Invalid time", "Zero-time", "No signal", "Low signal", "Near error", "Sharp turn", "Acceleration", "Teleportation" };

	[Translatable]
	public string[] arr_Motion = new string[4] { "", "Park", "Move", "Flight" };

	[Translatable]
	public string UpdaterCantCheckUpdates = "Can't check updates";

	[Translatable]
	public string UpdaterNewVersionDetected = "New version detected!";

	[Translatable]
	public string UpdaterSuccessfully = "Successfully upgraded";

	[Translatable]
	public string UpdaterCantFind = "Can't find {0} after updating";

	[Translatable]
	public string UpdaterFileNotAvailable = "Please close all programs that can use files of {0}";

	[Translatable]
	public string UpdaterRestarting = "Restarting...";

	[Translatable]
	public string UpdaterDisabled = "Automatic updates disabled";

	[Translatable]
	public LookUpEditItem[] DataLoaderErrors = new LookUpEditItem[12]
	{
		new LookUpEditItem((object)(ServerErrorCode)0, "No error"),
		new LookUpEditItem((object)(ServerErrorCode)1, "Internal server error, try again"),
		new LookUpEditItem((object)(ServerErrorCode)2, "Request corrupted. Server version too old."),
		new LookUpEditItem((object)(ServerErrorCode)3, "Authentication error. Invalid login or password"),
		new LookUpEditItem((object)(ServerErrorCode)4, "Access denied. You are not authorized to make the requested action."),
		new LookUpEditItem((object)(ServerErrorCode)5, "Request in old format. Change server version in settings."),
		new LookUpEditItem((object)(ServerErrorCode)6, "Request timeout. Check internet connection and try again."),
		new LookUpEditItem((object)(ServerErrorCode)8, "Malformed packet"),
		new LookUpEditItem((object)(ServerErrorCode)9, "Unknown error [9]"),
		new LookUpEditItem((object)(ServerErrorCode)10, "Request length error [10]"),
		new LookUpEditItem((object)(ServerErrorCode)12, "Error while checking device UID."),
		new LookUpEditItem((object)(ServerErrorCode)13, "Unpack error. Server version too old or response with invalid content")
	};

	[Translatable]
	public string SettingsValidator_Parameter = "Parameter";

	[Translatable]
	public string SettingsValidator_TabularParameter = "Tabular parameter";

	[Translatable]
	public string SettingsValidator_TabularCrdIndParameter = "Tabular coordinates-independent parameter";

	[Translatable]
	public string SettingsValidator_TabularOrFinalParameter = "Tabular or final parameter";

	[Translatable]
	public string SettingsValidator_NotExist = "not exist";

	[Translatable]
	public string SettingsValidator_ElementNotExist = "Element not exist";

	[Translatable]
	public string SettingsValidator_Property = "Property";

	[Translatable]
	public string SettingsValidator_Type = "type";

	[Translatable]
	public string SettingsValidator_NeededType = "Needed type";

	[Translatable]
	public string SettingsValidator_EmptyValues = "Empty values";

	[Translatable]
	public string SettingsValidator_Value = "value";

	[Translatable]
	public string SettingsValidator_Or = "or";

	[Translatable]
	public string SettingsValidator_ElementsNotSet = "Elements not set";

	[Translatable]
	public string SettingsValidator_ParameterNotSet = "Parameter not set";

	[Translatable]
	public string SettingsValidator_NotSetPropertyValue = "Not set property value";

	[Translatable]
	public string SettingsValidator_Geofence = "Geofence";

	[Translatable]
	public string SettingsValidator_AreaCaclOption = "Not set area calculation option in parameters designer";

	[Translatable]
	public string SettingsValidator_StartDT = "start date";

	[Translatable]
	public string SettingsValidator_EndDT = "end date";

	[Translatable]
	public string SettingsValidator_OutOfRange = "Out of range";

	[Translatable]
	public string SettingsValidator_InvalidreturnType = "invalid return type";

	[Translatable]
	public string SettingsValidator_NotHasStatuses = "not has statuses";

	[Translatable]
	public string SettingsValidator_Line = "Line";

	[Translatable]
	public string SettingsValidator_NeedNonNegative = "Needed non-negative value";

	[Translatable]
	public string SettingsValidator_NeedPositive = "Needed positive value";

	[Translatable]
	public string SettingsValidator_LargerThan = "Larger than";

	[Translatable]
	public string SettingsValidator_NoAreaSharer = "no trip sharer by area";

	[Translatable]
	public string SettingsValidator_EmptyNullName = "Name empty or null";

	[Translatable]
	public string SettingsValidator_2IdentifierIntersect = "Two identifier intersect";

	[Translatable]
	public string SettingsValidator_Properties = "Properties registry";

	[Translatable]
	public string SettingsValidator_Identifier = "Identifiers";

	[Translatable]
	public string SettingsValidator_Splitter = "Trip splitters";

	[Translatable]
	public string SettingsValidator_BackGroundImage = "Background images";

	[Translatable]
	public string SettingsValidator_ForeGroundImage = "Foreground images";

	[Translatable]
	public string SettingsValidator_BlinkingCursor = "Cursor blinking";

	[Translatable]
	public string SettingsValidator_CursorColouring = "Cursor colouring";

	[Translatable]
	public string SettingsValidator_AlineTrack = "Correction by parameters. Aline track";

	[Translatable]
	public string SettingsValidator_RegisterMovement = "Correction by parameters. Register movement";

	[Translatable]
	public string SettingsValidator_MainTrackColoring = "Track colouring. Main track";

	[Translatable]
	public string SettingsValidator_OnlineTrackColoring = "Track colouring. Online track";

	[Translatable]
	public string SettingsValidator_OnlineTrackCondition = "Track colouring. Online track. Condition";

	[Translatable]
	public string SettingsValidator_Route = "Routes";

	[Translatable]
	public string SettingsValidator_DesignerPrm = "Designer of parameters";

	[Translatable]
	public string SettingsValidator_ParameterCompileError = "Parameter Compilation";

	[Translatable]
	public string SettingsValidator_TaskSettings = "Tasks settings";

	[Translatable]
	public string MonitorValidator_TabularPrmNotFound = "Tabular parameter not found";

	[Translatable]
	public string MonitorValidator_FinalPrmNotFound = "Final parameter not found";

	[Translatable]
	public string MonitorValidator_TabularComparerNotFound = "Tabular comparer not found";

	[Translatable]
	public string MonitorValidator_FinalComparerNotFound = "Final comparer not found";

	[Translatable]
	public string MonitorValidator_ValidatorTabularPrmNotFound = "Validator tabular parameter not found";

	[Translatable]
	public string MonitorValidator_ValidatorTabularComparerNotFound = "Validator tabular comparer not found";

	[Translatable]
	public string MonitorValidator_TabularPrmCompileError = "Tabular parameter compilation error";

	[Translatable]
	public string MonitorValidator_ValidatorTabularPrmCompileError = "Validator tabular parameter compilation error";

	[Translatable]
	public string str_DistanceGPS = "GPS distance";

	public CommonFields()
	{
		int num = -3;
		EntryInfo[] array = arr_EntryInfo;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ID = num++;
		}
		RefillEventInfo();
	}

	public void RefillEventInfo()
	{
		ed = arr_EventInfo;
		gd = arr_EntryInfo.Select((EntryInfo p) => p.Group).ToArray();
		dd = arr_EntryInfo.Select((EntryInfo p) => p.Data).ToArray();
		td = arr_EntryInfo.Select((EntryInfo p) => p.Desc).ToArray();
	}
}
