using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AGInterfaces;

[Serializable]
[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
public sealed class FinalFields
{
	private bool r;

	private bool actual_r;

	private DateTime _FirstUDT;

	private DateTime _LastUDT;

	private DateTime _LastRouteStatusUDT;

	private int _LastRouteStatus;

	private DateTime _FirstCrdUDT;

	private DateTime _LastCrdUDT;

	private Coordinates _Coords;

	private Coordinates[] _TrailCoords;

	private double _Course = -1.0;

	private double[] _TrailCourse;

	private double _Speed = -1.0;

	private TimeSpan _ParkTime;

	private bool _ParkTimeGreater;

	private TimeSpan _MoveTime;

	private bool _MoveTimeGreater;

	private TimeSpan _FlightTime;

	private bool _FlightTimeGreater;

	public TripInfo OnlineTripInfo;

	private TimeSpan _LoadingTime;

	private TimeSpan _SearchPhotoTime;

	private TimeSpan _DTFilteringTime;

	private TimeSpan _SearchAddrTime;

	private TimeSpan _TabularCompTime;

	private TimeSpan _GFFindingTime;

	private TimeSpan _TripsSharingTime;

	private TimeSpan _TripCompTime;

	private TimeSpan _FinalCompTime;

	private readonly DateTime _NextInspDT;

	private readonly int _InspCalcStatus;

	private readonly int _InspNotification;

	private Location _Location;

	public int EntriesNum;

	public int[] TypifiedEntriesNumArray;

	public int[] TypifiedIntermedEntriesNumArray;

	public readonly TimeZoneInfo TimeZoneInfo;

	private readonly Dictionary<string, double> techControlValues;

	public bool read => r;

	public bool actual_read => actual_r;

	[AGInfo("Current universal time", "Current universal time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime UDT
	{
		get
		{
			setRead();
			setActualRead();
			return DateTime.UtcNow;
		}
	}

	[AGInfo("Сurrent local time", "Сurrent local time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime DT => TimeZoneInfo.ConvertTimeFromUtc(UDT, TimeZoneInfo);

	[AGInfo("First entry universal time", "First entry universal time (for calculation period).", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime FirstUDT
	{
		get
		{
			setRead();
			return _FirstUDT;
		}
		set
		{
			_FirstUDT = value;
		}
	}

	[AGInfo("Last entry universal time", "Last entry universal time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime LastUDT
	{
		get
		{
			setRead();
			setActualRead();
			return _LastUDT;
		}
		set
		{
			_LastUDT = value;
		}
	}

	[AGInfo("First entry local time", "First entry local time (for calculation period).", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime FirstDT
	{
		get
		{
			setRead();
			if (!(_FirstUDT > DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_FirstUDT, TimeZoneInfo);
		}
	}

	[AGInfo("Last entry local time", "Last entry local time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime LastDT
	{
		get
		{
			setRead();
			setActualRead();
			if (!(_LastUDT > DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_LastUDT, TimeZoneInfo);
		}
	}

	[AGInfo("Tracking time", "Tracking time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public TimeSpan TrackingTime
	{
		get
		{
			setRead();
			setActualRead();
			return _LastUDT - _FirstUDT;
		}
	}

	[AGInfo("Valid time", "Valid date and time.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool IsTrueTime
	{
		get
		{
			setRead();
			return LastUDT > DateTime.MinValue;
		}
	}

	[AGInfo("Last route status universal time", "Last route status universal time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime LastRouteStatusUDT
	{
		get
		{
			setRead();
			setActualRead();
			return _LastRouteStatusUDT;
		}
		set
		{
			_LastRouteStatusUDT = value;
		}
	}

	[AGInfo("Last route status local time", "Last route status local time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime LastRouteStatusDT
	{
		get
		{
			setRead();
			setActualRead();
			if (!(_LastRouteStatusUDT > DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_LastRouteStatusUDT, TimeZoneInfo);
		}
	}

	[AGInfo("Last route status", "Last route status.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { })]
	public int LastRouteStatus
	{
		get
		{
			setRead();
			return _LastRouteStatus;
		}
		set
		{
			_LastRouteStatus = value;
		}
	}

	[AGInfo("First coordinate entry universal time", "First coordinate entry universal time (for calculation period).", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public DateTime FirstCrdUDT
	{
		get
		{
			setRead();
			return _FirstCrdUDT;
		}
		set
		{
			_FirstCrdUDT = value;
		}
	}

	[AGInfo("Last coordinate entry universal time", "Last coordinate entry universal time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public DateTime LastCrdUDT
	{
		get
		{
			setRead();
			setActualRead();
			return _LastCrdUDT;
		}
		set
		{
			_LastCrdUDT = value;
		}
	}

	[AGInfo("First coordinate entry local time", "First coordinate entry local time (for calculation period).", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public DateTime FirstCrdDT
	{
		get
		{
			setRead();
			if (!(_FirstCrdUDT > DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_FirstCrdUDT, TimeZoneInfo);
		}
	}

	[AGInfo("Last coordinate entry local time", "Last coordinate entry local time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public DateTime LastCrdDT
	{
		get
		{
			setRead();
			setActualRead();
			if (!(_LastCrdUDT > DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_LastCrdUDT, TimeZoneInfo);
		}
	}

	[AGInfo("Coordinates tracking time", "Coordinates tracking time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public TimeSpan CrdTrackingTime
	{
		get
		{
			setActualRead();
			return LastCrdUDT - FirstCrdUDT;
		}
	}

	[AGInfo("Valid coordinates", "Valid coordinates.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool IsTrueCoords
	{
		get
		{
			setRead();
			return LastCrdUDT > DateTime.MinValue;
		}
	}

	[AGInfo("Is LBS coordinates", "Is location-based service coordinates.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool IsTrueLBSCoords
	{
		get
		{
			setRead();
			if (LastCrdUDT == DateTime.MinValue)
			{
				return Coords != Coordinates.Empty;
			}
			return false;
		}
	}

	[AGInfo("Last coordinates", "Last coordinates (true if IsTrueCoords = true).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public Coordinates Coords
	{
		get
		{
			setRead();
			return _Coords;
		}
		set
		{
			_Coords = value;
		}
	}

	public Coordinates[] TrailCoords
	{
		get
		{
			setRead();
			return _TrailCoords;
		}
		set
		{
			_TrailCoords = value;
		}
	}

	[AGInfo("Last course", "Last course (< 0 for single point).", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public double Course
	{
		get
		{
			setRead();
			return _Course;
		}
		set
		{
			_Course = value;
		}
	}

	public double[] TrailCourse
	{
		get
		{
			setRead();
			return _TrailCourse;
		}
		set
		{
			_TrailCourse = value;
		}
	}

	[AGInfo("Last speed", "Last speed (< 0 for single point).", "km/h", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public double Speed
	{
		get
		{
			setRead();
			return _Speed;
		}
		set
		{
			_Speed = value;
		}
	}

	[AGInfo("Last park time", "Last park time (greater or equal) (true if IsTrueCoords = true).", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public TimeSpan ParkTime
	{
		get
		{
			setRead();
			return _ParkTime;
		}
		set
		{
			_ParkTime = value;
		}
	}

	[AGInfo("Last park time greater", "Last park time greater than ParkTime (true if IsTrueCoords = true).", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public bool ParkTimeGreater
	{
		get
		{
			setRead();
			return _ParkTimeGreater;
		}
		set
		{
			_ParkTimeGreater = value;
		}
	}

	[AGInfo("Last move time", "Last move time (greater or equal) (true if IsTrueCoords = true).", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public TimeSpan MoveTime
	{
		get
		{
			setRead();
			return _MoveTime;
		}
		set
		{
			_MoveTime = value;
		}
	}

	[AGInfo("Last move time greater", "Last move time greater than MoveTime (true if IsTrueCoords = true).", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public bool MoveTimeGreater
	{
		get
		{
			setRead();
			return _MoveTimeGreater;
		}
		set
		{
			_MoveTimeGreater = value;
		}
	}

	[AGInfo("Last flight time", "Last flight time (greater or equal) (true if IsTrueCoords = true).", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public TimeSpan FlightTime
	{
		get
		{
			setRead();
			return _FlightTime;
		}
		set
		{
			_FlightTime = value;
		}
	}

	[AGInfo("Last flight time greater", "Last flight time greater than FlightTime (true if IsTrueCoords = true).", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public bool FlightTimeGreater
	{
		get
		{
			setRead();
			return _FlightTimeGreater;
		}
		set
		{
			_FlightTimeGreater = value;
		}
	}

	[AGInfo("Loading duration", "Bynary files loading duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan LoadingTime
	{
		get
		{
			setRead();
			setActualRead();
			return _LoadingTime;
		}
		set
		{
			_LoadingTime = value;
		}
	}

	[AGInfo("Searching for photo duration", "Searching for photo files duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan SearchPhotoTime
	{
		get
		{
			setRead();
			setActualRead();
			return _SearchPhotoTime;
		}
		set
		{
			_SearchPhotoTime = value;
		}
	}

	[AGInfo("Time filtering duration", "Time filtering duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan DTFilteringTime
	{
		get
		{
			setRead();
			setActualRead();
			return _DTFilteringTime;
		}
		set
		{
			_DTFilteringTime = value;
		}
	}

	[AGInfo("Searching for addresses duration", "Searching for addresses duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan SearchAddrTime
	{
		get
		{
			setRead();
			setActualRead();
			return _SearchAddrTime;
		}
		set
		{
			_SearchAddrTime = value;
		}
	}

	[AGInfo("Tabular computing duration", "Tabular parameters computing duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan TabularCompTime
	{
		get
		{
			setRead();
			setActualRead();
			return _TabularCompTime;
		}
		set
		{
			_TabularCompTime = value;
		}
	}

	[AGInfo("Geo-fence finding duration", "Geo-fence finding duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan GFFindingTime
	{
		get
		{
			setRead();
			setActualRead();
			return _GFFindingTime;
		}
		set
		{
			_GFFindingTime = value;
		}
	}

	[AGInfo("Trips sharing duration", "Trips sharing duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan TripsSharingTime
	{
		get
		{
			setRead();
			setActualRead();
			return _TripsSharingTime;
		}
		set
		{
			_TripsSharingTime = value;
		}
	}

	[AGInfo("Trips computing duration", "Trip parameters computing duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan TripCompTime
	{
		get
		{
			setRead();
			setActualRead();
			return _TripCompTime;
		}
		set
		{
			_TripCompTime = value;
		}
	}

	[AGInfo("Final computing duration", "Final parameters computing duration.", DeviceParameterKind.InstTime, AGInfoGroupType.Time | AGInfoGroupType.Debug, false, new int[] { })]
	public TimeSpan FinalCompTime
	{
		get
		{
			setRead();
			setActualRead();
			return _FinalCompTime;
		}
		set
		{
			_FinalCompTime = value;
		}
	}

	[AGInfo("Next inspection date", "Next inspection date.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public DateTime NextInspDT
	{
		get
		{
			setRead();
			setActualRead();
			if (!(_NextInspDT > DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_NextInspDT, TimeZoneInfo);
		}
	}

	[AGInfo("Inspection calculate status", "Inspection calculate status: 0 - unknown, 1 - not configured, 2 - waiting, 3 - calculating, 4 - completed.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, null)]
	public int InspCalcStatus
	{
		get
		{
			setRead();
			setActualRead();
			return _InspCalcStatus;
		}
	}

	[AGInfo("Inspection notification", "Inspection notification: 0 - none, 1 - soon, 2 - interval exceeded.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, null)]
	public int InspNotification
	{
		get
		{
			setRead();
			setActualRead();
			return _InspNotification;
		}
	}

	[AGInfo("Location", "Location.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public Location Location
	{
		get
		{
			setRead();
			return _Location;
		}
		set
		{
			_Location = value;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void setRead()
	{
		r = true;
	}

	public void resetRead()
	{
		r = false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void setActualRead()
	{
		actual_r = true;
	}

	public void resetActualRead()
	{
		actual_r = false;
	}

	[AGInfo("Tech. control property", "Technical control property. Default value will be used if property isn't specified.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, null)]
	public double TechControlDouble([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def = 0.0)
	{
		if (techControlValues.TryGetValue(name, out var value))
		{
			setRead();
			return value;
		}
		return def;
	}

	public FinalFields()
	{
	}

	public FinalFields(TimeZoneInfo timeZoneInfo, DateTime nextInspDT, InspectionCalcStatus inspCalcStatus, InspectionNotification inspNotification, Dictionary<string, double> techControlValues)
	{
		TimeZoneInfo = timeZoneInfo;
		_NextInspDT = nextInspDT;
		_InspCalcStatus = (int)inspCalcStatus;
		_InspNotification = (int)inspNotification;
		this.techControlValues = techControlValues;
	}

	public bool Equals(FinalFields finalFields)
	{
		if (_LastUDT != finalFields._LastUDT || _LastCrdUDT != finalFields._LastCrdUDT || _ParkTime != finalFields._ParkTime || _ParkTimeGreater != finalFields._ParkTimeGreater || _MoveTime != finalFields._MoveTime || _MoveTimeGreater != finalFields._MoveTimeGreater || _FlightTime != finalFields._FlightTime || _FlightTimeGreater != finalFields._FlightTimeGreater || _LastRouteStatusUDT != finalFields._LastRouteStatusUDT || _LastRouteStatus != finalFields._LastRouteStatus || !_Coords.Equals(finalFields._Coords) || _Course != finalFields._Course || _Speed != finalFields._Speed)
		{
			return false;
		}
		if (!Coordinates.ArrayEquals(_TrailCoords, finalFields._TrailCoords))
		{
			return false;
		}
		if (OnlineTripInfo != finalFields.OnlineTripInfo)
		{
			if (OnlineTripInfo == null || finalFields.OnlineTripInfo == null)
			{
				return false;
			}
			if (!OnlineTripInfo.Equals(finalFields.OnlineTripInfo))
			{
				return false;
			}
		}
		return true;
	}
}
