using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading;
using BINParser;

namespace AGInterfaces;

[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
public class TripFields : ExpressionBase
{
	private bool r;

	public bool prevAreaInfoSetted;

	public bool prevOutOfMemory;

	public AreaInfo areaInfo;

	public Area deviceAreas;

	private readonly CancellationToken cancellationToken;

	private readonly IProcessedAreaCalculation areaCalculation;

	private readonly ReportProgress areaProgress;

	private readonly AreaProgressInfo areaProgressInfo;

	private readonly Func<IDisposable> createAga2Obj;

	private readonly Func<LocationAddr, IDisposable, string> getRegionFromLocation;

	private readonly Func<LocationAddr, IDisposable, string> getCityFromLocation;

	private readonly Dictionary<Quadro<Guid>, int> gfStatuses;

	private IDisposable aga2Obj;

	private TripFieldsParams _fieldsParams;

	public bool read => r;

	private Image _Image => _fieldsParams.image;

	[AGInfo("Image", "Image.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public Image Image
	{
		get
		{
			setRead();
			return _Image;
		}
	}

	[AGInfo("Universal time", "Universal time (after filtration).", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime UDT
	{
		get
		{
			setRead();
			return _UDT;
		}
	}

	[AGInfo("Local time", "Local time (after filtration).", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime DT => TimeZoneInfo.ConvertTimeFromUtc(UDT, timeZoneInfo);

	private InputFlags _Flags => _fieldsParams.flags;

	[AGInfo("Flags", "Flags.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public int Flags
	{
		get
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected I4, but got Unknown
			setRead();
			return (int)_Flags;
		}
	}

	private Guid _Implement => _fieldsParams.implement;

	[AGInfo("Implement", "Implement.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public Guid Implement
	{
		get
		{
			setRead();
			return _Implement;
		}
	}

	private double _Lon => _fieldsParams.crd.longitude;

	private double _Lat => _fieldsParams.crd.latitude;

	private double _Alt => _fieldsParams.crd.altitude;

	[AGInfo("Longitude", "Longitude, smoothed in proportion to the time.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public double Lon
	{
		get
		{
			setRead();
			return _Lon;
		}
	}

	[AGInfo("Latitude", "Latitude, smoothed in proportion to the time.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public double Lat
	{
		get
		{
			setRead();
			return _Lat;
		}
	}

	[AGInfo("Altitude", "Altitude in meters, smoothed in proportion to the time.", "m", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public double Alt
	{
		get
		{
			setRead();
			return _Alt;
		}
	}

	private double _Distance => _fieldsParams.distance;

	[AGInfo("Distance", "Smoothed accumulated distance in meters from start of calculation period.", "m", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public double Distance
	{
		get
		{
			setRead();
			return _Distance;
		}
	}

	private Quadro<Guid>[] _GF => _fieldsParams.gf;

	[AGInfo("Geo-fences 1", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Quadro<Guid> GF1
	{
		get
		{
			setRead();
			return _GF[0];
		}
	}

	[AGInfo("Geo-fences 2", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Quadro<Guid> GF2
	{
		get
		{
			setRead();
			return _GF[1];
		}
	}

	[AGInfo("Geo-fences 3", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Quadro<Guid> GF3
	{
		get
		{
			setRead();
			return _GF[2];
		}
	}

	[AGInfo("Geo-fences 4", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Quadro<Guid> GF4
	{
		get
		{
			setRead();
			return _GF[3];
		}
	}

	[AGInfo("In geo-fence 1", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool InGF1
	{
		get
		{
			setRead();
			return _GF[0] != Quadro<Guid>.Empty;
		}
	}

	[AGInfo("In geo-fence 2", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool InGF2
	{
		get
		{
			setRead();
			return _GF[1] != Quadro<Guid>.Empty;
		}
	}

	[AGInfo("In geo-fence 3", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool InGF3
	{
		get
		{
			setRead();
			return _GF[2] != Quadro<Guid>.Empty;
		}
	}

	[AGInfo("In geo-fence 4", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool InGF4
	{
		get
		{
			setRead();
			return _GF[3] != Quadro<Guid>.Empty;
		}
	}

	[AGInfo("Out of geo-fences 1", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF1
	{
		get
		{
			setRead();
			return _GF[0] == Quadro<Guid>.Empty;
		}
	}

	[AGInfo("Out of geo-fences 2", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF2
	{
		get
		{
			setRead();
			return _GF[1] == Quadro<Guid>.Empty;
		}
	}

	[AGInfo("Out of geo-fences 3", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF3
	{
		get
		{
			setRead();
			return _GF[2] == Quadro<Guid>.Empty;
		}
	}

	[AGInfo("Out of geo-fences 4", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF4
	{
		get
		{
			setRead();
			return _GF[3] == Quadro<Guid>.Empty;
		}
	}

	private Guid[] _GFRoute => _fieldsParams.routes;

	[AGInfo("Route 1", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Guid GF1Route
	{
		get
		{
			setRead();
			return _GFRoute[0];
		}
	}

	[AGInfo("Route 2", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Guid GF2Route
	{
		get
		{
			setRead();
			return _GFRoute[1];
		}
	}

	[AGInfo("Route 3", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Guid GF3Route
	{
		get
		{
			setRead();
			return _GFRoute[2];
		}
	}

	[AGInfo("Route 4", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Guid GF4Route
	{
		get
		{
			setRead();
			return _GFRoute[3];
		}
	}

	[AGInfo("Out of routes 1", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF1Routes
	{
		get
		{
			setRead();
			return _GFRoute[0] == Guid.Empty;
		}
	}

	[AGInfo("Out of routes 2", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF2Routes
	{
		get
		{
			setRead();
			return _GFRoute[1] == Guid.Empty;
		}
	}

	[AGInfo("Out of routes 3", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF3Routes
	{
		get
		{
			setRead();
			return _GFRoute[2] == Guid.Empty;
		}
	}

	[AGInfo("Out of routes 4", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF4Routes
	{
		get
		{
			setRead();
			return _GFRoute[3] == Guid.Empty;
		}
	}

	private Quadro<Guid> _MCHP => _fieldsParams.mchp;

	[AGInfo("Mobile checkpoints", "Mobile checkpoints (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Quadro<Guid> MCHP
	{
		get
		{
			setRead();
			return _MCHP;
		}
	}

	private Guid _Area => _fieldsParams.area;

	private int _id => _fieldsParams.id;

	private int _count => _fieldsParams.count;

	private DeviceTrack[] _deviceTracks => _fieldsParams.deviceTracks;

	private ImageFormat _imageFormat => _fieldsParams.imageFormat;

	[AGInfo("Area", "Area.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Guid Area
	{
		get
		{
			setRead();
			return _Area;
		}
	}

	public double TotalArea
	{
		get
		{
			setRead();
			getAreaInfo();
			return deviceAreas?.Total ?? 0;
		}
	}

	public double ComplArea
	{
		get
		{
			setRead();
			getAreaInfo();
			return deviceAreas?.Complete ?? 0;
		}
	}

	public double OverArea
	{
		get
		{
			setRead();
			getAreaInfo();
			return deviceAreas?.Overlap ?? 0;
		}
	}

	public double FullOverArea
	{
		get
		{
			setRead();
			getAreaInfo();
			return deviceAreas?.FullOverlap ?? 0;
		}
	}

	public double UncomplArea
	{
		get
		{
			setRead();
			getAreaInfo();
			return deviceAreas?.Incomplete ?? 0;
		}
	}

	public int OverCntrMax
	{
		get
		{
			setRead();
			getAreaInfo();
			return deviceAreas?.MaxOverlapDepth ?? 0;
		}
	}

	private LocationAddr _Addr => _fieldsParams.addr;

	[AGInfo("Distance to address", "Distance in meters to nearest address.", "m", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public double AddrDist
	{
		get
		{
			setRead();
			return _Addr.distance;
		}
	}

	[AGInfo("Location", "Location.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public Location Location
	{
		get
		{
			setRead();
			Location empty = Location.Empty;
			if (gfStatuses != null)
			{
				gfStatuses.TryGetValue(_GF[0], out empty.gf1);
				gfStatuses.TryGetValue(_GF[1], out empty.gf2);
				gfStatuses.TryGetValue(_GF[2], out empty.gf3);
				gfStatuses.TryGetValue(_GF[3], out empty.gf4);
			}
			empty.addr = _Addr;
			empty.crds.longitude = _Lon;
			empty.crds.latitude = _Lat;
			empty.crds.altitude = _Alt;
			return empty;
		}
	}

	[AGInfo("Region from location", "Region from location.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { })]
	public string LocationRegion
	{
		get
		{
			setRead();
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
			setRead();
			if (aga2Obj == null)
			{
				aga2Obj = createAga2Obj();
			}
			return getCityFromLocation(_Addr, aga2Obj);
		}
	}

	public override void setRead()
	{
		r = true;
	}

	public void resetRead()
	{
		r = false;
	}

	[AGInfo("Geo-fences by index", "Geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Quadro<Guid> GF([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return Quadro<Guid>.Empty;
		}
		int num = index - 1;
		setRead();
		return _GF[num];
	}

	[AGInfo("In geo-fence by index", "In one geo-fence (at least, from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool InGF([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return true;
		}
		int num = index - 1;
		setRead();
		return _GF[num] != Quadro<Guid>.Empty;
	}

	[AGInfo("Out of geo-fences by index", "Out of geo-fences (from 0 to 3 overlays).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGF([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return true;
		}
		int num = index - 1;
		setRead();
		return _GF[num] == Quadro<Guid>.Empty;
	}

	[AGInfo("Route by index", "Route by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public Guid GFRoute([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return Guid.Empty;
		}
		int num = index - 1;
		setRead();
		return _GF[num];
	}

	[AGInfo("Out of routes by index", "Out of routes by geo-fences.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool OutOfGFRoute([AGPrmInfo("Index of geo-fence node", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return true;
		}
		int num = index - 1;
		setRead();
		return _GFRoute[num] == Guid.Empty;
	}

	private void getAreaInfo()
	{
		if (areaCalculation == null || _deviceTracks == null || !_deviceTracks.Any() || prevAreaInfoSetted || prevOutOfMemory)
		{
			return;
		}
		AreaProgressInfo _areaProgressInfo = new AreaProgressInfo(areaProgressInfo);
		try
		{
			areaInfo = areaCalculation.CalculateArea(_Area, _deviceTracks, areaMode: false, _imageFormat, cancellationToken, delegate(int percent, object userState)
			{
				if (areaProgress != null)
				{
					areaProgress((100 * _id + percent) / _count, _areaProgressInfo);
				}
			});
		}
		catch (OutOfMemoryException)
		{
			prevOutOfMemory = true;
		}
		if (areaInfo.DeviceAreas != null)
		{
			Guid id = _deviceTracks[0].DeviceId;
			DeviceArea deviceArea = areaInfo.DeviceAreas.FirstOrDefault((DeviceArea p) => p.Id == id);
			if (deviceArea != null)
			{
				deviceAreas = deviceArea.Area;
			}
			else
			{
				deviceAreas = new Area
				{
					Total = areaInfo.CommomArea.Total,
					Incomplete = areaInfo.CommomArea.Incomplete
				};
			}
		}
		if (areaProgress != null)
		{
			areaProgress((100 * _id + 100) / _count, _areaProgressInfo);
		}
		prevAreaInfoSetted = true;
	}

	[AGInfo("Stage summary value", "Stage summary value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, null)]
	public double StageSumDouble([AGPrmInfo("Stage name", double.MinValue, double.MinValue)] string stageName, [AGPrmInfo("Parameter name", double.MinValue, double.MinValue)] string prmName, [AGPrmInfo("Status", double.MinValue, double.MinValue)] int status = -1)
	{
		return stageTotalDouble(stageName, prmName, TotalValueType.Sum, AddValueType.Diff, status);
	}

	[AGInfo("Stage minimal value", "Stage minimal value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, null)]
	public double StageMinDouble([AGPrmInfo("Stage name", double.MinValue, double.MinValue)] string stageName, [AGPrmInfo("Parameter name", double.MinValue, double.MinValue)] string prmName, [AGPrmInfo("Status", double.MinValue, double.MinValue)] int status = -1)
	{
		return stageTotalDouble(stageName, prmName, TotalValueType.Min, AddValueType.Diff, status);
	}

	[AGInfo("Stage maximal value", "Stage maximal value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, null)]
	public double StageMaxDouble([AGPrmInfo("Stage name", double.MinValue, double.MinValue)] string stageName, [AGPrmInfo("Parameter name", double.MinValue, double.MinValue)] string prmName, [AGPrmInfo("Status", double.MinValue, double.MinValue)] int status = -1)
	{
		return stageTotalDouble(stageName, prmName, TotalValueType.Max, AddValueType.Diff, status);
	}

	[AGInfo("Stage average value", "Stage average value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, null)]
	public double StageAverDouble([AGPrmInfo("Stage name", double.MinValue, double.MinValue)] string stageName, [AGPrmInfo("Parameter name", double.MinValue, double.MinValue)] string prmName, [AGPrmInfo("Status", double.MinValue, double.MinValue)] int status = -1)
	{
		return stageTotalDouble(stageName, prmName, TotalValueType.Mean, AddValueType.Diff, status);
	}

	[AGInfo("Stage first value", "Stage first value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, null)]
	public double StageFirstDouble([AGPrmInfo("Stage name", double.MinValue, double.MinValue)] string stageName, [AGPrmInfo("Parameter name", double.MinValue, double.MinValue)] string prmName, [AGPrmInfo("Status", double.MinValue, double.MinValue)] int status = -1)
	{
		return stageTotalDouble(stageName, prmName, TotalValueType.First, AddValueType.First, status);
	}

	[AGInfo("Stage last value", "Stage last value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, null)]
	public double StageLastDouble([AGPrmInfo("Stage name", double.MinValue, double.MinValue)] string stageName, [AGPrmInfo("Parameter name", double.MinValue, double.MinValue)] string prmName, [AGPrmInfo("Status", double.MinValue, double.MinValue)] int status = -1)
	{
		return stageTotalDouble(stageName, prmName, TotalValueType.Last, AddValueType.Last, status);
	}

	private double stageTotalDouble(string stageName, string prmName, TotalValueType type, AddValueType addType, int status)
	{
		if (_fieldsParams._tripStages == null)
		{
			return 0.0;
		}
		string[] array = stageName.Split('.', StringSplitOptions.TrimEntries);
		if (array.Length != 2 || array[0] != "u")
		{
			return 0.0;
		}
		if (!_fieldsParams._tripStages.TryGetValue(array[1], out var value))
		{
			return 0.0;
		}
		if (value.stagesInfoArray == null)
		{
			return 0.0;
		}
		string[] array2 = prmName.Split('.', StringSplitOptions.TrimEntries);
		if (array2.Length != 2 || array2[0] != "u")
		{
			return 0.0;
		}
		DevPrmsGroupInfo grpInfo;
		DevPrmInfo prmInfoByName = DevPrmsGroupInfo.getPrmInfoByName(string.Empty, array2[1], AddValueType.Curr, _fieldsParams._tripPrmsGroupInfoArray, out grpInfo);
		if (prmInfoByName == null || prmInfoByName.returnType != ReturnType.Double)
		{
			prmInfoByName = DevPrmsGroupInfo.getPrmInfoByName(string.Empty, array2[1], addType, _fieldsParams._tripPrmsGroupInfoArray, out grpInfo);
			if (prmInfoByName == null || prmInfoByName.returnType != ReturnType.Double)
			{
				return 0.0;
			}
		}
		_ = prmInfoByName.returnType;
		double[] array3 = (double[])value.stage[prmInfoByName.creationIndex];
		_ = array3.Length;
		StatusStage[] stagesInfoArray = value.stagesInfoArray;
		double num = 0.0;
		switch (type)
		{
		case TotalValueType.Sum:
		{
			if (status < 0)
			{
				for (int num14 = 0; num14 < stagesInfoArray.Length; num14++)
				{
					if (stagesInfoArray[num14].Status > 0)
					{
						double num15 = array3[stagesInfoArray[num14].absIndex];
						if (!double.IsNaN(num15) && !double.IsInfinity(num15))
						{
							num += num15;
						}
					}
				}
				break;
			}
			for (int num16 = 0; num16 < stagesInfoArray.Length; num16++)
			{
				if (stagesInfoArray[num16].Status == status)
				{
					double num17 = array3[stagesInfoArray[num16].absIndex];
					if (!double.IsNaN(num17) && !double.IsInfinity(num17))
					{
						num += num17;
					}
				}
			}
			break;
		}
		case TotalValueType.Min:
		{
			if (status < 0)
			{
				int num18;
				for (num18 = 0; num18 < stagesInfoArray.Length; num18++)
				{
					if (stagesInfoArray[num18].Status > 0)
					{
						double num19 = array3[stagesInfoArray[num18].absIndex];
						if (!double.IsNaN(num19) && !double.IsInfinity(num19))
						{
							num = num19;
							num18++;
							break;
						}
					}
				}
				for (; num18 < stagesInfoArray.Length; num18++)
				{
					if (stagesInfoArray[num18].Status > 0)
					{
						double num20 = array3[stagesInfoArray[num18].absIndex];
						if (!double.IsNaN(num20) && !double.IsInfinity(num20) && num20.CompareTo(num) < 0)
						{
							num = num20;
						}
					}
				}
				break;
			}
			int num21;
			for (num21 = 0; num21 < stagesInfoArray.Length; num21++)
			{
				if (stagesInfoArray[num21].Status == status)
				{
					double num22 = array3[stagesInfoArray[num21].absIndex];
					if (!double.IsNaN(num22) && !double.IsInfinity(num22))
					{
						num = num22;
						num21++;
						break;
					}
				}
			}
			for (; num21 < stagesInfoArray.Length; num21++)
			{
				if (stagesInfoArray[num21].Status == status)
				{
					double num23 = array3[stagesInfoArray[num21].absIndex];
					if (!double.IsNaN(num23) && !double.IsInfinity(num23) && num23.CompareTo(num) < 0)
					{
						num = num23;
					}
				}
			}
			break;
		}
		case TotalValueType.Max:
		{
			if (status < 0)
			{
				int num8;
				for (num8 = 0; num8 < stagesInfoArray.Length; num8++)
				{
					if (stagesInfoArray[num8].Status > 0)
					{
						double num9 = array3[stagesInfoArray[num8].absIndex];
						if (!double.IsNaN(num9) && !double.IsInfinity(num9))
						{
							num = num9;
							num8++;
							break;
						}
					}
				}
				for (; num8 < stagesInfoArray.Length; num8++)
				{
					if (stagesInfoArray[num8].Status > 0)
					{
						double num10 = array3[stagesInfoArray[num8].absIndex];
						if (!double.IsNaN(num10) && !double.IsInfinity(num10) && num10.CompareTo(num) > 0)
						{
							num = num10;
						}
					}
				}
				break;
			}
			int num11;
			for (num11 = 0; num11 < stagesInfoArray.Length; num11++)
			{
				if (stagesInfoArray[num11].Status == status)
				{
					double num12 = array3[stagesInfoArray[num11].absIndex];
					if (!double.IsNaN(num12) && !double.IsInfinity(num12))
					{
						num = num12;
						num11++;
						break;
					}
				}
			}
			for (; num11 < stagesInfoArray.Length; num11++)
			{
				if (stagesInfoArray[num11].Status == status)
				{
					double num13 = array3[stagesInfoArray[num11].absIndex];
					if (!double.IsNaN(num13) && !double.IsInfinity(num13) && num13.CompareTo(num) > 0)
					{
						num = num13;
					}
				}
			}
			break;
		}
		case TotalValueType.Mean:
		{
			double num3 = 0.0;
			if (status < 0)
			{
				for (int num4 = 0; num4 < stagesInfoArray.Length; num4++)
				{
					if (stagesInfoArray[num4].Status > 0)
					{
						double num5 = array3[stagesInfoArray[num4].absIndex];
						if (!double.IsNaN(num5) && !double.IsInfinity(num5))
						{
							num += num5;
							num3 += 1.0;
						}
					}
				}
			}
			else
			{
				for (int num6 = 0; num6 < stagesInfoArray.Length; num6++)
				{
					if (stagesInfoArray[num6].Status == status)
					{
						double num7 = array3[stagesInfoArray[num6].absIndex];
						if (!double.IsNaN(num7) && !double.IsInfinity(num7))
						{
							num += num7;
							num3 += 1.0;
						}
					}
				}
			}
			num /= num3;
			break;
		}
		case TotalValueType.First:
		{
			int num2 = ((status < 0) ? Array.FindIndex(stagesInfoArray, (StatusStage p) => p.Status > 0) : Array.FindIndex(stagesInfoArray, (StatusStage p) => p.Status == status));
			if (num2 < 0)
			{
				return 0.0;
			}
			num = array3[stagesInfoArray[num2].absIndex];
			break;
		}
		case TotalValueType.Last:
		{
			int num2 = ((status < 0) ? Array.FindLastIndex(stagesInfoArray, (StatusStage p) => p.Status > 0) : Array.FindLastIndex(stagesInfoArray, (StatusStage p) => p.Status == status));
			if (num2 < 0)
			{
				return 0.0;
			}
			num = array3[stagesInfoArray[num2].absIndex];
			break;
		}
		default:
			return 0.0;
		}
		setRead();
		return num;
	}

	public TripFields(ExpressionBaseInitInfo initInfo, ParameterInitInfo prmInitInfo, IDataViewerFormatters dataViewerFormatters)
		: base(initInfo, prmInitInfo, dataViewerFormatters)
	{
		gfStatuses = initInfo.gfStatuses;
		createAga2Obj = initInfo.createAga2Obj;
		getRegionFromLocation = initInfo.getRegionFromLocation;
		getCityFromLocation = initInfo.getCityFromLocation;
		cancellationToken = initInfo.cancellationToken;
		areaCalculation = initInfo.AreaCalculation;
		areaProgress = initInfo.AreaProgress;
		areaProgressInfo = initInfo.AreaProgressInfo;
	}

	public void setRecord(TripFieldsParams fieldsParams)
	{
		_fieldsParams = fieldsParams;
		_UDT = _fieldsParams.udt;
	}

	public override void Dispose()
	{
		if (aga2Obj != null)
		{
			aga2Obj.Dispose();
		}
	}
}
