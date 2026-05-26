using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using BINParser;

namespace AGInterfaces;

public class TabularFieldsParams
{
	private int i;

	private int _dri;

	private readonly int _serialNo;

	private readonly DataArrays _arrays;

	private readonly HandlerArrays _harrays;

	private readonly List<DeviceRecord> _deviceRecords;

	private readonly bool _setWithCrds;

	private readonly bool _setWithTask;

	private readonly bool _taskNotNull;

	private readonly int _defSpeedLimit;

	private readonly int _freqNum;

	private readonly bool[] _counterAsVoltage;

	private readonly int _firstCrdPos;

	private readonly int _lastCrdPos;

	private readonly int _firstCrdPos1;

	private readonly int _lastCrdPos1;

	private readonly int _maxPos;

	private double[] _freq;

	private int indexLastUpdateFreq = -1;

	private readonly Coordinates _defCoordinates = new Coordinates(double.NaN, double.NaN, 0.0);

	private int indexLastUpdateGF = -1;

	private Quadro<Guid>[] _gf;

	private bool[] _outOfGF;

	private Guid[] _routes;

	private bool[] _outOfRoutes;

	private Guid _area;

	private Guid _inArea;

	private int ci
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0;
			}
			return _harrays.ciArr[i];
		}
	}

	private int si
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0;
			}
			return _harrays.siArr[i];
		}
	}

	private int ni
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0;
			}
			return _harrays.niArr[i];
		}
	}

	private int pi
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0;
			}
			return _harrays.piArr[i];
		}
	}

	public int serialNo
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _serialNo;
		}
	}

	public int TypeID
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected I4, but got Unknown
			return (int)((DeviceRecordLite)rec).TypeId;
		}
	}

	public DeviceRecord rec
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _deviceRecords[_dri];
		}
	}

	public bool trueType
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.trueTypesArray[i];
		}
	}

	public bool canSetRecord
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.canSetRecordsArray[i];
		}
	}

	public Image image
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.image;
		}
	}

	public DateTime udt
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.udt[i];
		}
	}

	public TimeSpan duration
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.durArr[i];
		}
	}

	public TimeSpan prevInt
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.interval[i];
		}
	}

	public TimeSpan nextInt
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.interval[i + 1];
		}
	}

	public DTFiltration dtf
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.dtf[i];
		}
	}

	public InputFlags prevFlags
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.flags[(i > 0) ? (i - 1) : i];
		}
	}

	public InputFlags currFlags
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.flags[i];
		}
	}

	public double[] Freq
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (indexLastUpdateFreq != this.i)
			{
				for (int i = 0; i < _freqNum; i++)
				{
					_freq[i] = _harrays.freqData[i].freq[_harrays.fiArr[this.i, i]];
				}
				indexLastUpdateFreq = this.i;
			}
			return _freq;
		}
	}

	public bool[] CounterAsVoltage
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _counterAsVoltage;
		}
	}

	public double faAmount
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.faAmounts[i];
		}
	}

	public bool setAmountByFDRecord
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.setAmountByFDRecord[i];
		}
	}

	public bool prevPSE
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.possibleSpeedError[i];
		}
	}

	public bool nextPSE
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.possibleSpeedError[i + 1];
		}
	}

	public int cameraNum
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			int[] array = _harrays.cameraNum;
			if (array == null)
			{
				return 0;
			}
			return array[i];
		}
	}

	public bool shiftIsOpen
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.shiftIsOpen?[i] ?? false;
		}
	}

	public bool setShiftIsOpen
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _harrays.setShiftIsOpen?[i] ?? false;
		}
	}

	public string naviMapFileName
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			string[] naviMapFileNames = _harrays.naviMapFileNames;
			if (naviMapFileNames == null)
			{
				return null;
			}
			return naviMapFileNames[i];
		}
	}

	public int routeStatus
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			int[] routeStatuses = _arrays.routeStatuses;
			if (routeStatuses == null)
			{
				return 0;
			}
			return routeStatuses[i];
		}
	}

	public CrdFiltration llf
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_arrays.llf == null)
			{
				return CrdFiltration.None;
			}
			return _arrays.llf[i];
		}
	}

	public Coordinates crd
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return _defCoordinates;
			}
			return _arrays.crds[i];
		}
	}

	public double ground
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.ground[ci];
		}
	}

	public TimeSpan prevCrdInt
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return TimeSpan.Zero;
			}
			return _harrays.crdInt[pi];
		}
	}

	public TimeSpan nextCrdInt
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return TimeSpan.Zero;
			}
			return _harrays.crdInt[ni];
		}
	}

	public double prevDist
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.dist[pi];
		}
	}

	public double nextDist
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.dist[ni];
		}
	}

	public double distance
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _arrays.run[i];
		}
	}

	public long CANDtotalUpper
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			DataArrays arrays = _arrays;
			long? obj;
			if (arrays == null)
			{
				obj = null;
			}
			else
			{
				long[] cANDtotalUpper = arrays.CANDtotalUpper;
				obj = ((cANDtotalUpper != null) ? new long?(cANDtotalUpper[i]) : ((long?)null));
			}
			long? num = obj;
			return num.GetValueOrDefault();
		}
	}

	public long CANDtotalLower
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			DataArrays arrays = _arrays;
			long? obj;
			if (arrays == null)
			{
				obj = null;
			}
			else
			{
				long[] cANDtotalLower = arrays.CANDtotalLower;
				obj = ((cANDtotalLower != null) ? new long?(cANDtotalLower[i]) : ((long?)null));
			}
			long? num = obj;
			return num.GetValueOrDefault();
		}
	}

	public double prevCourse
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.course[si];
		}
	}

	public double nextCourse
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.course[ci];
		}
	}

	public double prevSpeed
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.speed[si];
		}
	}

	public double nextSpeed
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.speed[ci];
		}
	}

	public double prevVSpeed
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.vSpeed[si];
		}
	}

	public double nextVSpeed
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0.0;
			}
			return _harrays.vSpeed[ci];
		}
	}

	public int speedLimit
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			int[] array = _harrays.speedLimit;
			if (array == null)
			{
				return _defSpeedLimit;
			}
			return array[ci];
		}
	}

	public string street
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			string[] streets = _harrays.streets;
			if (streets == null)
			{
				return null;
			}
			return streets[ci];
		}
	}

	public string platon
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			string[] array = _harrays.platon;
			if (array == null)
			{
				return null;
			}
			return array[ci];
		}
	}

	public bool crdReg
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_setWithCrds)
			{
				if (_firstCrdPos <= i)
				{
					return i <= _lastCrdPos;
				}
				return false;
			}
			return false;
		}
	}

	public bool crdRegUpdate
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_setWithCrds)
			{
				if (i != _firstCrdPos1 && i != _firstCrdPos && i != _lastCrdPos1 && i != _lastCrdPos)
				{
					return i == _maxPos;
				}
				return true;
			}
			return false;
		}
	}

	public bool inAreaUpdate
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_arrays.gfRawAreasOffset >= 0)
			{
				if (llf != CrdFiltration.Ok)
				{
					if (i < _maxPos)
					{
						return _arrays.gfRaw[_arrays.gfRawAreasOffset, i + 1] != _area;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	public byte motion
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithCrds)
			{
				return 0;
			}
			return _harrays.motion[i];
		}
	}

	public Quadro<Guid>[] GF
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (indexLastUpdateGF != i)
			{
				setGFAndRoutesArrays(i);
				indexLastUpdateGF = i;
			}
			return _gf;
		}
	}

	public bool[] OutOfGF
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (indexLastUpdateGF != i)
			{
				setGFAndRoutesArrays(i);
				indexLastUpdateGF = i;
			}
			return _outOfGF;
		}
	}

	public Guid[] Routes
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (indexLastUpdateGF != i)
			{
				setGFAndRoutesArrays(i);
				indexLastUpdateGF = i;
			}
			return _routes;
		}
	}

	public bool[] OutOfRoutes
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (indexLastUpdateGF != i)
			{
				setGFAndRoutesArrays(i);
				indexLastUpdateGF = i;
			}
			return _outOfRoutes;
		}
	}

	public Quadro<Guid> mchp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			Quadro<Guid>[] mobileChP = _arrays.mobileChP;
			if (mobileChP == null)
			{
				return Quadro<Guid>.Empty;
			}
			return mobileChP[i];
		}
	}

	public bool outOfMchp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.outOfMobileChP?[i] ?? true;
		}
	}

	public Guid Area
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (indexLastUpdateGF != i)
			{
				setGFAndRoutesArrays(i);
				indexLastUpdateGF = i;
			}
			return _area;
		}
	}

	public Guid InArea
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (indexLastUpdateGF != i)
			{
				setGFAndRoutesArrays(i);
				indexLastUpdateGF = i;
			}
			return _inArea;
		}
	}

	public LocationAddr addr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_arrays.address == null)
			{
				return LocationAddr.Empty;
			}
			return _arrays.address[i];
		}
	}

	public Guid task
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithTask || !_taskNotNull)
			{
				return default(Guid);
			}
			return _harrays.task[i];
		}
	}

	public DateTime taskBeginUDT
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithTask || !_taskNotNull)
			{
				return default(DateTime);
			}
			return _harrays.taskBeginUDT[i];
		}
	}

	public DateTime taskEndUDT
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithTask || !_taskNotNull)
			{
				return default(DateTime);
			}
			return _harrays.taskEndUDT[i];
		}
	}

	public byte taskStatus
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithTask || !_taskNotNull)
			{
				return 0;
			}
			return _harrays.taskStatus[i];
		}
	}

	public double taskPercent
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithTask || !_taskNotNull)
			{
				return 0.0;
			}
			return _harrays.taskPercent[i];
		}
	}

	public long taskViolations
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithTask || !_taskNotNull)
			{
				return 0L;
			}
			return _harrays.taskViolations[i];
		}
	}

	public Guid taskNextGeoFence
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (!_setWithTask || !_taskNotNull)
			{
				return default(Guid);
			}
			return _harrays.taskNextGeoFence[i];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int photoReason(int n)
	{
		int[] obj = _harrays.photoReasonArr[n];
		if (obj == null)
		{
			return 0;
		}
		return obj[i];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public DateTime photoUDT(int n)
	{
		DateTime[] obj = _harrays.photoUDTArr[n];
		if (obj == null)
		{
			return DateTime.MinValue;
		}
		return obj[i];
	}

	public TabularFieldsParams(DataArrays arrays, HandlerArrays harrays, LoaderInfo loaderInfo, CrdIndependentFields instance)
	{
		_arrays = arrays;
		_harrays = harrays;
		_deviceRecords = loaderInfo.items;
		_serialNo = loaderInfo.serialNo;
		_setWithCrds = instance is CrdDependentFields && harrays.crdPos != null;
		_setWithTask = instance is TskDependentFields;
		_defSpeedLimit = harrays.defSpeedLimit;
		_freqNum = harrays.freqData.Length;
		_taskNotNull = harrays.task != null;
		_firstCrdPos = -1;
		_lastCrdPos = -1;
		_firstCrdPos1 = -1;
		_lastCrdPos1 = -1;
		_maxPos = harrays.itemsCount - 1;
		_gf = new Quadro<Guid>[4];
		_outOfGF = new bool[4];
		_routes = new Guid[4];
		_outOfRoutes = new bool[4];
		_freq = new double[_freqNum];
		_counterAsVoltage = new bool[_freqNum];
		for (int i = 0; i < _freqNum; i++)
		{
			_counterAsVoltage[i] = _harrays.freqData[i].counterAsVoltage;
		}
		if (_harrays.crdPos != null)
		{
			_firstCrdPos = _harrays.crdPos[0];
			_lastCrdPos = _harrays.crdPos[harrays.crdItemsNum - 1];
			_firstCrdPos1 = _firstCrdPos - 1;
			_lastCrdPos1 = _lastCrdPos - 1;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetIndex(int i, int dri)
	{
		this.i = i;
		_dri = dri;
	}

	private void setGFAndRoutesArrays(int i)
	{
		if (_arrays.gfRawLength == null)
		{
			return;
		}
		int num = 0;
		for (int j = 0; j < _arrays.gfRawWithoutAreas; j++)
		{
			int num2 = _arrays.gfRawLength[j];
			if (num2 != 0)
			{
				_gf[j].q1 = _arrays.gfRaw[num, i];
				if (num2 == 4)
				{
					_gf[j].q2 = _arrays.gfRaw[num + 1, i];
					_gf[j].q3 = _arrays.gfRaw[num + 2, i];
					_gf[j].q4 = _arrays.gfRaw[num + 3, i];
				}
				_outOfGF[j] = _harrays.outOfGF[j] == null || _harrays.outOfGF[j][i];
				if (_arrays.gfRoutes[j] != null)
				{
					_routes[j] = _arrays.gfRoutes[j][i];
					_outOfRoutes[j] = _harrays.outOfGFRoutes[j][i];
				}
				else
				{
					_outOfRoutes[j] = true;
				}
				num += num2;
			}
		}
		if (_arrays.gfRawWithoutAreas < _arrays.gfRawLength.Length)
		{
			_area = _arrays.gfRaw[num, i];
			_inArea = ((i >= _maxPos || _arrays.gfRaw[num, i + 1] == _area) ? _area : Guid.Empty);
		}
	}
}
