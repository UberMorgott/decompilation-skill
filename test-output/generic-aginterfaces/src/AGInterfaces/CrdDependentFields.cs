using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BINParser;

namespace AGInterfaces;

[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
public class CrdDependentFields : CrdIndependentFields
{
	public static TimeSpan _blindTimeSpan = TimeSpan.FromMinutes(15.0);

	private LocationAddr _addrTemp = LocationAddr.Empty;

	private bool usingTempAddr;

	private readonly Func<IDisposable> createAga2Obj;

	private readonly Func<LocationAddr, IDisposable, string> getRegionFromLocation;

	private readonly Func<LocationAddr, IDisposable, string> getCityFromLocation;

	private readonly Dictionary<Quadro<Guid>, int> gfStatuses;

	private IDisposable aga2Obj;

	private double[] _DQSpeedMax = new double[8];

	private double _Lon => _prm.crd.longitude;

	private double _Lat => _prm.crd.latitude;

	private double _Alt => _prm.crd.altitude;

	private double _Ground => _prm.ground;

	[AGInfo("Valid coordinate", "Valid coordinate.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool LL => base.LLF == 1;

	[AGInfo("Longitude", "Longitude, smoothed in proportion to the time.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public double Lon
	{
		get
		{
			read(DataType.coordinates);
			return _Lon;
		}
	}

	[AGInfo("Latitude", "Latitude, smoothed in proportion to the time.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public double Lat
	{
		get
		{
			read(DataType.coordinates);
			return _Lat;
		}
	}

	[AGInfo("Altitude", "Altitude in meters, smoothed in proportion to the time.", "m", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double Alt
	{
		get
		{
			read(DataType.coordinates);
			return _Alt;
		}
	}

	[AGInfo("Ground", "Height of the Earth surface in meters.", "m", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double Ground
	{
		get
		{
			read(DataType.coordinates);
			return _Ground;
		}
	}

	[AGInfo("Signal", "Navigation signal.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool Signal => S > 0;

	[AGInfo("Signal level", "Level of navigation signal from 0 (no) to 7 (maximum).", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public int S
	{
		get
		{
			read(DataType.coordinatesRaw);
			if (!(_NextCrdInt <= _blindTimeSpan))
			{
				return 0;
			}
			return _SRaw;
		}
	}

	private TimeSpan _PrevCrdInt => _prm.prevCrdInt;

	private TimeSpan _NextCrdInt => _prm.nextCrdInt;

	[AGInfo("Previous coordinate interval", "Time span from previous coordinate entry to current or next coordinate entry.", DeviceParameterKind.Accum, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public TimeSpan PrevCrdInt
	{
		get
		{
			read(DataType.coordinates);
			return _PrevCrdInt;
		}
	}

	[AGInfo("Next coordinate interval", "Time span from current or previous coordinate entry to next coordinate entry.", DeviceParameterKind.Flag, AGInfoGroupType.Time | AGInfoGroupType.Navigation, false, new int[] { })]
	public TimeSpan NextCrdInt
	{
		get
		{
			read(DataType.coordinates);
			return _NextCrdInt;
		}
	}

	private double _PrevDist => _prm.prevDist;

	private double _NextDist => _prm.nextDist;

	private double _Distance => _prm.distance;

	[AGInfo("Previous distance", "Distance in meters from previous coordinate entry to current or next coordinate entry.", "m", DeviceParameterKind.Accum, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public double PrevDist
	{
		get
		{
			read(DataType.coordinates);
			return _PrevDist;
		}
	}

	[AGInfo("Next distance", "Distance in meters from current or previous coordinate entry to next coordinate entry.", "m", DeviceParameterKind.Flag, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public double NextDist
	{
		get
		{
			read(DataType.coordinates);
			return _NextDist;
		}
	}

	[AGInfo("Distance", "Smoothed accumulated distance in meters from start of calculation period.", "m", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Distance, true, new int[] { 0 })]
	public double Distance
	{
		get
		{
			read(DataType.coordinates);
			return _Distance;
		}
	}

	private double _PrevCourse => _prm.prevCourse;

	private double _NextCourse => _prm.nextCourse;

	[AGInfo("Previous course", "Course from the North direction clockwise in degrees from previous coordinate entry to current or next coordinate entry.", "°", DeviceParameterKind.Accum, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public double PrevCourse
	{
		get
		{
			read(DataType.coordinates);
			return _PrevCourse;
		}
	}

	[AGInfo("Next course", "Course from the North direction clockwise in degrees from current or previous coordinate entry to next coordinate entry", "°", DeviceParameterKind.Flag, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public double NextCourse
	{
		get
		{
			read(DataType.coordinates);
			return _NextCourse;
		}
	}

	private double _PrevSpeed => _prm.prevSpeed;

	private double _NextSpeed => _prm.nextSpeed;

	[AGInfo("Previous speed", "Average speed in km/h (at stage from previous coordinate entry to current or next coordinate entry).", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double PrevSpeed
	{
		get
		{
			read(DataType.coordinates);
			return _PrevSpeed;
		}
	}

	[AGInfo("Next speed", "Average speed in km/hч (at stage from current or previous coordinate entry to next coordinate entry).", "km/h", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double NextSpeed
	{
		get
		{
			read(DataType.coordinates);
			return _NextSpeed;
		}
	}

	private double _PrevVSpeed => _prm.prevVSpeed;

	private double _NextVSpeed => _prm.nextVSpeed;

	[AGInfo("Previous vertical speed", "Average vertical speed in m/s (at stage from previous coordinate entry to current or next coordinate entry).", "m/s", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double PrevVSpeed
	{
		get
		{
			read(DataType.coordinates);
			return _PrevVSpeed;
		}
	}

	[AGInfo("Next vertical speed", "Average vertical speed in m/s (at stage from current or previous coordinate entry to next coordinate entry).", "m/s", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double NextVSpeed
	{
		get
		{
			read(DataType.coordinates);
			return _NextVSpeed;
		}
	}

	public int _SpeedLimit => _prm.speedLimit;

	[AGInfo("Speed limit", "Speed limit in km/h from vector maps.", "km/h", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public int SpeedLimit
	{
		get
		{
			read(DataType.coordinates);
			return _SpeedLimit;
		}
	}

	public string _Street => _prm.street;

	[AGInfo("Street", "Street.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public string Street
	{
		get
		{
			read(DataType.coordinates);
			return _Street;
		}
	}

	public string _Platon => _prm.platon;

	[AGInfo("Platon", "Platon.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public string Platon
	{
		get
		{
			read(DataType.coordinates);
			return _Platon;
		}
	}

	private bool _CrdReg => _prm.crdReg;

	private byte _Motion => _prm.motion;

	[AGInfo("Coordinates registered", "Current entry is between first and last coordinates entry (can be used for summarize to long period).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool CrdReg
	{
		get
		{
			read(DataType.coordinatesReg);
			return _CrdReg;
		}
	}

	[AGInfo("Motion", "Motion character (at stage from current or previous coordinate entry to next coordinate entry): 0 - transportation, 1 - park, 2 - move, 3 - flight.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation, false, new int[] { })]
	public byte Motion
	{
		get
		{
			read(DataType.common);
			return _Motion;
		}
	}

	[AGInfo("Park", "Park (at stage from current or previous coordinate entry to next coordinate entry).", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool Park => Motion == 1;

	[AGInfo("Move", "Move (at stage from current or previous coordinate entry to next coordinate entry).", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool Move => Motion == 2;

	[AGInfo("Flight", "Flight (at stage from current or previous coordinate entry to next coordinate entry).", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool Flight => Motion == 3;

	[AGInfo("Acceleration", "Acceleration in m/s² from previous coordinate entry to next coordinate entry.", "m/s²", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double Accel
	{
		get
		{
			read(DataType.coordinates);
			if (_PrevCrdInt == TimeSpan.Zero || _NextCrdInt == TimeSpan.Zero)
			{
				return 0.0;
			}
			double num = _PrevDist / _PrevCrdInt.TotalSeconds;
			return (_NextDist / _NextCrdInt.TotalSeconds - num) * 2.0 / (_PrevCrdInt + _NextCrdInt).TotalSeconds;
		}
	}

	[AGInfo("Turn angle", "Turn angle in degrees.", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double TurnAng
	{
		get
		{
			read(DataType.coordinates);
			double num = _NextCourse - _PrevCourse;
			if (num < 0.0)
			{
				num += 360.0;
			}
			if (num > 180.0)
			{
				num -= 360.0;
			}
			return num;
		}
	}

	[AGInfo("Angular velocity", "Angular velocity in degree/s from previous coordinate entry to next coordinate entry.", "°/s", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 0 })]
	public double AngVel
	{
		get
		{
			read(DataType.coordinates);
			TimeSpan timeSpan = _PrevCrdInt + _NextCrdInt;
			if (timeSpan == TimeSpan.Zero)
			{
				return 0.0;
			}
			double num = Math.Abs(_NextCourse - _PrevCourse);
			if (num > 180.0)
			{
				num = 360.0 - num;
			}
			return num * 2.0 / timeSpan.TotalSeconds;
		}
	}

	[AGInfo("Longitude, latitude, altitude", "Coordinates as structure (for UTM).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Coordinates LonLatAlt
	{
		get
		{
			read(DataType.coordinates);
			if (!double.IsNaN(_Lon))
			{
				return new Coordinates(_Lon, _Lat, _Alt);
			}
			return Coordinates.Empty;
		}
	}

	private Quadro<Guid>[] _GF => _prm.GF;

	[AGInfo("Geo-fences 1", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Quadro<Guid> GF1
	{
		get
		{
			read(DataType.coordinates);
			return _GF[0];
		}
	}

	[AGInfo("Geo-fences 2", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Quadro<Guid> GF2
	{
		get
		{
			read(DataType.coordinates);
			return _GF[1];
		}
	}

	[AGInfo("Geo-fences 3", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Quadro<Guid> GF3
	{
		get
		{
			read(DataType.coordinates);
			return _GF[2];
		}
	}

	[AGInfo("Geo-fences 4", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Quadro<Guid> GF4
	{
		get
		{
			read(DataType.coordinates);
			return _GF[3];
		}
	}

	[AGInfo("In geo-fence 1", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool InGF1
	{
		get
		{
			read(DataType.coordinates);
			return _GF[0] != Quadro<Guid>.Empty;
		}
	}

	[AGInfo("In geo-fence 2", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool InGF2
	{
		get
		{
			read(DataType.coordinates);
			return _GF[1] != Quadro<Guid>.Empty;
		}
	}

	[AGInfo("In geo-fence 3", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool InGF3
	{
		get
		{
			read(DataType.coordinates);
			return _GF[2] != Quadro<Guid>.Empty;
		}
	}

	[AGInfo("In geo-fence 4", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool InGF4
	{
		get
		{
			read(DataType.coordinates);
			return _GF[3] != Quadro<Guid>.Empty;
		}
	}

	private bool[] _OutOfGF => _prm.OutOfGF;

	[AGInfo("Out of geo-fences 1", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF1
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfGF[0];
		}
	}

	[AGInfo("Out of geo-fences 2", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF2
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfGF[1];
		}
	}

	[AGInfo("Out of geo-fences 3", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF3
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfGF[2];
		}
	}

	[AGInfo("Out of geo-fences 4", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF4
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfGF[3];
		}
	}

	private Guid[] _GFRoute => _prm.Routes;

	[AGInfo("Route 1", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Guid GF1Route
	{
		get
		{
			read(DataType.coordinates);
			return _GFRoute[0];
		}
	}

	[AGInfo("Route 2", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Guid GF2Route
	{
		get
		{
			read(DataType.coordinates);
			return _GFRoute[1];
		}
	}

	[AGInfo("Route 3", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Guid GF3Route
	{
		get
		{
			read(DataType.coordinates);
			return _GFRoute[2];
		}
	}

	[AGInfo("Route 4", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Guid GF4Route
	{
		get
		{
			read(DataType.coordinates);
			return _GFRoute[3];
		}
	}

	private bool[] _OutOfGFRoute => _prm.OutOfRoutes;

	[AGInfo("Out of routes 1", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF1Routes
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfGFRoute[0];
		}
	}

	[AGInfo("Out of routes 2", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF2Routes
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfGFRoute[1];
		}
	}

	[AGInfo("Out of routes 3", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF3Routes
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfGFRoute[2];
		}
	}

	[AGInfo("Out of routes 4", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF4Routes
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfGFRoute[3];
		}
	}

	private Quadro<Guid> _MCHP => _prm.mchp;

	private bool _OutOfMCHP => _prm.outOfMchp;

	[AGInfo("Mobile checkpoints", "Mobile checkpoints (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Quadro<Guid> MCHP
	{
		get
		{
			read(DataType.coordinates);
			return _MCHP;
		}
	}

	[AGInfo("In mobile checkpoint", "In one mobile checkpoint (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool InMCHP
	{
		get
		{
			read(DataType.coordinates);
			return _MCHP != Quadro<Guid>.Empty;
		}
	}

	[AGInfo("Out of mobile checkpoints", "Out of mobile checkpoints (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfMCHP
	{
		get
		{
			read(DataType.coordinates);
			return _OutOfMCHP;
		}
	}

	private Guid _Area => _prm.Area;

	private Guid _InArea => _prm.InArea;

	[AGInfo("Area", "Area.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Guid Area
	{
		get
		{
			read(DataType.coordinates);
			return _Area;
		}
	}

	[AGInfo("In area", "In area.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Guid InArea
	{
		get
		{
			read(DataType.inArea);
			return _InArea;
		}
	}

	private LocationAddr _Addr
	{
		get
		{
			if (usingTempAddr)
			{
				return _addrTemp;
			}
			return _prm.addr;
		}
	}

	[AGInfo("Distance to address", "Distance in meters to nearest address.", "m", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public double AddrDist
	{
		get
		{
			read(DataType.common);
			return _Addr.distance;
		}
	}

	[AGInfo("Location", "Location.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public Location Location
	{
		get
		{
			read(DataType.common);
			Location empty = Location.Empty;
			if (gfStatuses != null)
			{
				gfStatuses.TryGetValue(_GF[0], out empty.gf1);
				gfStatuses.TryGetValue(_GF[1], out empty.gf2);
				gfStatuses.TryGetValue(_GF[2], out empty.gf3);
				gfStatuses.TryGetValue(_GF[3], out empty.gf4);
			}
			empty.addr = _Addr;
			if (!double.IsNaN(_Lon))
			{
				empty.crds.longitude = _Lon;
				empty.crds.latitude = _Lat;
			}
			empty.crds.altitude = _Alt;
			return empty;
		}
	}

	[AGInfo("Region from location", "Region from location.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public string LocationRegion
	{
		get
		{
			read(DataType.common);
			if (aga2Obj == null)
			{
				aga2Obj = createAga2Obj();
			}
			return getRegionFromLocation(_Addr, aga2Obj);
		}
	}

	[AGInfo("City from location", "City from location.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public string LocationCity
	{
		get
		{
			read(DataType.common);
			if (aga2Obj == null)
			{
				aga2Obj = createAga2Obj();
			}
			return getCityFromLocation(_Addr, aga2Obj);
		}
	}

	[AGInfo("Excess acceleration maximum speed", "Driving quality excess acceleration maximum speed.", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQAccelSpeed
	{
		get
		{
			read(DataType.drivingQualityType);
			return _DQSpeedMax[0];
		}
	}

	[AGInfo("Excess braking maximum speed", "Driving quality excess braking maximum speed.", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQBrakeSpeed
	{
		get
		{
			read((DataType)1365);
			return _DQSpeedMax[1];
		}
	}

	[AGInfo("Emergency braking maximum speed", "Driving quality emergency braking maximum speed.", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQEmBrakeSpeed
	{
		get
		{
			read((DataType)1366);
			return _DQSpeedMax[2];
		}
	}

	[AGInfo("Excess right turn maximum speed", "Driving quality excess right turn maximum speed.", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQRightSpeed
	{
		get
		{
			read((DataType)1367);
			return _DQSpeedMax[3];
		}
	}

	[AGInfo("Excess left turn maximum speed", "Driving quality excess left turn maximum speed.", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQLeftSpeed
	{
		get
		{
			read((DataType)1368);
			return _DQSpeedMax[4];
		}
	}

	[AGInfo("Excess bump maximum speed", "Driving quality excess bump maximum speed.", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQBumpSpeed
	{
		get
		{
			read((DataType)1369);
			return _DQSpeedMax[5];
		}
	}

	[AGInfo("Tilt maximum speed", "Driving quality tilt maximum speed.", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQTiltSpeed
	{
		get
		{
			read((DataType)1370);
			return _DQSpeedMax[6];
		}
	}

	[AGInfo("Overturn maximum speed", "Driving quality overturn maximum speed.", "km/h", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQOverturnSpeed
	{
		get
		{
			read((DataType)1371);
			return _DQSpeedMax[7];
		}
	}

	[AGInfo("Daylight", "Daylight. Zenith: 90° 50' - offical twilight (by default), 96° - civil, 102° - nautical, 106° - astronomical.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool Daylight([AGPrmInfo("Zenith in degrees", 90.0, 108.0)] double zenith = 90.833)
	{
		read(DataType.coordinates);
		if (zenith < 90.0)
		{
			zenith = 90.0;
		}
		else if (zenith > 108.0)
		{
			zenith = 108.0;
		}
		double num = sunHour(sunrise: true, zenith);
		if (num < 0.0)
		{
			return false;
		}
		if (num >= 24.0)
		{
			return true;
		}
		double num2 = sunHour(sunrise: false, zenith);
		if (num > num2)
		{
			DateTime dateTime = base.UDT.Date.AddHours(num);
			if (base.UDT < dateTime)
			{
				num -= 24.0;
			}
			else
			{
				num2 += 24.0;
			}
		}
		if (base.UDT.Date.AddHours(num) <= base.UDT)
		{
			return base.UDT <= base.UDT.Date.AddHours(num2);
		}
		return false;
	}

	private double sunHour(bool sunrise, double zenith)
	{
		if (double.IsNaN(_Lon))
		{
			return -1.0;
		}
		double num = _Lon / 15.0;
		double num2 = (double)base.UDT.DayOfYear + (sunrise ? ((6.0 - num) / 24.0) : ((18.0 - num) / 24.0));
		double num3 = 0.9856 * num2 - 3.289;
		double num4 = num3 + 1.916 * ExpressionBase.sin(num3) + 0.02 * ExpressionBase.sin(2.0 * num3) + 282.634;
		if (num4 > 360.0)
		{
			num4 -= 360.0;
		}
		else if (num4 < 0.0)
		{
			num4 += 360.0;
		}
		double num5 = Math.Atan(0.91764 * ExpressionBase.tan(num4)) * 180.0 / Math.PI;
		if (num5 > 360.0)
		{
			num5 -= 360.0;
		}
		else if (num5 < 0.0)
		{
			num5 += 360.0;
		}
		double num6 = Math.Floor(num4 / 90.0) * 90.0;
		double num7 = Math.Floor(num5 / 90.0) * 90.0;
		num5 += num6 - num7;
		num5 /= 15.0;
		double num8 = 0.39782 * ExpressionBase.sin(num4);
		double num9 = Math.Cos(Math.Asin(num8));
		double num10 = (ExpressionBase.cos(zenith) - num8 * ExpressionBase.sin(_Lat)) / (num9 * ExpressionBase.cos(_Lat));
		if (num10 > 1.0)
		{
			return -1.0;
		}
		if (num10 < -1.0)
		{
			return 24.0;
		}
		double num11 = (sunrise ? (360.0 - Math.Acos(num10) * 180.0 / Math.PI) : (Math.Acos(num10) * 180.0 / Math.PI)) / 15.0 + num5 - 0.06571 * num2 - 6.622 - num;
		if (num11 >= 24.0)
		{
			num11 -= 24.0;
		}
		else if (num11 < 0.0)
		{
			num11 += 24.0;
		}
		return num11;
	}

	[AGInfo("Geo-fences by index", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Quadro<Guid> GF([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return Quadro<Guid>.Empty;
		}
		int num = index - 1;
		read(DataType.coordinates);
		return _GF[num];
	}

	[AGInfo("In geo-fence by index", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool InGF([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return true;
		}
		int num = index - 1;
		read(DataType.coordinates);
		return _GF[num] != Quadro<Guid>.Empty;
	}

	[AGInfo("Out of geo-fences by index", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGF([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return true;
		}
		int num = index - 1;
		read(DataType.coordinates);
		return _OutOfGF[num];
	}

	[AGInfo("Smallest Geo-Fence", "Smallest Geo-fence", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, null)]
	public Guid SmallestGeoFence([AGPrmInfo("Geo-fence guids", double.MinValue, double.MinValue)] Quadro<Guid> val)
	{
		read(DataType.coordinates);
		if (val == Quadro<Guid>.Empty)
		{
			return Guid.Empty;
		}
		return val.Enumerate().MinBy((Guid c) => PrmDouble("Square", double.MaxValue, c));
	}

	[AGInfo("Route by index", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, true, new int[] { 0 })]
	public Guid GFRoute([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return Guid.Empty;
		}
		int num = index - 1;
		read(DataType.coordinates);
		return _GFRoute[num];
	}

	[AGInfo("Out of routes by index", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, true, new int[] { 0 })]
	public bool OutOfGFRoute([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return true;
		}
		int num = index - 1;
		read(DataType.coordinates);
		return _OutOfGFRoute[num];
	}

	[AGInfo("Driver by iButton ID", "iButton ID driver identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Driver, false, new int[] { 6 })]
	public Guid driverIDbyButton([AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		Guid guid = driverByID(_IDButton, skipUnknown);
		if (!skipUnknown || _IDButton == 0L || guid != Guid.Empty)
		{
			read((DataType)62);
		}
		return guid;
	}

	[AGInfo("Implement by iButton ID", "iButton ID implement identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Implement, false, new int[] { 6 })]
	public Guid implementIDbyButton([AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		Guid guid = implementByID(_IDButton, skipUnknown);
		if (!skipUnknown || _IDButton == 0L || guid != Guid.Empty)
		{
			read((DataType)62);
		}
		return guid;
	}

	[AGInfo("Driver by Bluetooth ID", "Bluetooth ID driver identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Driver, false, new int[] { 6 })]
	public Guid driverIDbyBLE([AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		Guid guid = driverByID(_IDBLE, skipUnknown);
		if (!skipUnknown || _IDBLE == 0L || guid != Guid.Empty)
		{
			read((DataType)63);
		}
		return guid;
	}

	[AGInfo("Implement by Bluetooth ID", "Bluetooth ID implement identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Implement, false, new int[] { 6 })]
	public Guid implementIDbyBLE([AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		Guid guid = implementByID(_IDBLE, skipUnknown);
		if (!skipUnknown || _IDBLE == 0L || guid != Guid.Empty)
		{
			read((DataType)63);
		}
		return guid;
	}

	[AGInfo("Driver by CAN ID", "CAN ID driver identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Driver, false, new int[] { 6 })]
	public Guid driverIDbyCAN([AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		Guid guid = driverByID(_IDCAN, skipUnknown);
		if (!skipUnknown || _IDCAN == 0L || guid != Guid.Empty)
		{
			read((DataType)64);
		}
		return guid;
	}

	[AGInfo("Implement by CAN ID", "CAN ID implement identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Implement, false, new int[] { 6 })]
	public Guid implementIDbyCAN([AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		Guid guid = implementByID(_IDCAN, skipUnknown);
		if (!skipUnknown || _IDCAN == 0L || guid != Guid.Empty)
		{
			read((DataType)64);
		}
		return guid;
	}

	public CrdDependentFields(ExpressionBaseInitInfo initInfo, ParameterInitInfo prmInitInfo, IDataViewerFormatters dataViewerFormatters)
		: base(initInfo, prmInitInfo, dataViewerFormatters)
	{
		gfStatuses = initInfo.gfStatuses;
		createAga2Obj = initInfo.createAga2Obj;
		getRegionFromLocation = initInfo.getRegionFromLocation;
		getCityFromLocation = initInfo.getCityFromLocation;
	}

	public void setRecord(LocationAddr addr)
	{
		usingTempAddr = true;
		_addrTemp = addr;
	}

	public override void setRecord(bool setTypedData)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Invalid comparison between Unknown and I4
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		base.setRecord(setTypedData);
		if (_prm.crdRegUpdate)
		{
			write(DataType.coordinatesReg);
		}
		if (_prm.llf == CrdFiltration.Ok)
		{
			write(DataType.coordinates);
		}
		if (_prm.inAreaUpdate)
		{
			write(DataType.inArea);
		}
		if ((int)((DeviceRecordLite)_prm.rec).TypeId == 65)
		{
			DeviceDrivingQualityRecord val = (DeviceDrivingQualityRecord)_prm.rec;
			byte type = val.Type;
			if (type < 8 && (!val.End || _PrevSpeed > _DQSpeedMax[type]))
			{
				_DQSpeedMax[type] = _PrevSpeed;
			}
		}
	}

	public override void copyFrom(CrdIndependentFields value)
	{
		base.copyFrom(value);
		if (value is CrdDependentFields crdDependentFields)
		{
			_addrTemp = crdDependentFields._addrTemp;
			usingTempAddr = crdDependentFields.usingTempAddr;
			for (int i = 0; i < _DQSpeedMax.Length; i++)
			{
				_DQSpeedMax[i] = crdDependentFields._DQSpeedMax[i];
			}
		}
	}

	public override void Dispose()
	{
		if (aga2Obj != null)
		{
			aga2Obj.Dispose();
		}
	}
}
