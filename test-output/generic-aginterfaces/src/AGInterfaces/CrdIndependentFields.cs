using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AGInterfaces.Extenders;
using BINParser;

namespace AGInterfaces;

[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
public class CrdIndependentFields : ExpressionBase
{
	public enum DataType
	{
		typeId = 0,
		common = 1,
		flgAndInp12 = 2,
		inputs36 = 3,
		coordinatesRaw = 4,
		coordinatesReg = 5,
		coordinates = 6,
		analogData = 7,
		cCounters = 8,
		pCounters = 18,
		fCounters = 28,
		aCounters = 38,
		nfCounters = 48,
		motion = 58,
		altitude = 59,
		idType = 60,
		id1w = 61,
		card = 66,
		lls = 82,
		vlls = 90,
		eventId = 98,
		photo = 99,
		devT = 115,
		temperature = 116,
		inputs = 124,
		fillAmount = 125,
		fuelRate = 127,
		fillDuration = 128,
		ptChannel = 129,
		ptInt = 130,
		ptCtgInt = 146,
		mdChannel = 274,
		mdFloat = 275,
		palParamFreq = 291,
		palParamAnlg = 307,
		palFlags = 323,
		palParamStat = 324,
		tlls = 342,
		canWay = 350,
		canLevels = 357,
		canEngine = 364,
		canTemperature = 369,
		canDistance = 375,
		canAxis = 377,
		canUserData = 473,
		canErrors = 474,
		canCalcFuelRate = 481,
		canMode = 484,
		canEngineAux = 487,
		longEntryHeader = 492,
		longEntryIndex = 493,
		longEntryData = 494,
		longEntryDataID = 495,
		longEntryDataFC = 496,
		longEntryDataTGD = 512,
		longEntryDataTGC = 513,
		longEntryDataIMS = 514,
		longEntryDataIMA = 522,
		isobus = 530,
		isobusInt = 531,
		isobusFloat = 547,
		isobusUInt = 563,
		guard = 579,
		numericalData = 580,
		ndCan = 581,
		modbus = 709,
		dispStatus = 837,
		naviStatus = 838,
		tachograph = 840,
		wheelIndex = 841,
		wheelState = 842,
		alls = 1098,
		struna = 1106,
		strunaChType = 1107,
		drivingQuality = 1363,
		drivingQualityType = 1364,
		tkamCH = 1372,
		tkam = 1373,
		tkamTV = 1389,
		tkamRT = 1405,
		wcs = 1421,
		trailerWeight = 1424,
		idlls = 1427,
		vrlls = 1435,
		vehicleStatus = 1443,
		canAlternate = 1447,
		fms1 = 1448,
		discreteParams = 1464,
		discreteParamsData = 1465,
		inArea = 1473,
		canDistanceUpper = 1474,
		canDistanceLower = 1475,
		tkam2CH = 1476,
		tkam2 = 1477,
		namedParamHeader = 1493,
		namedParamData = 1494,
		namedParamUint = 1495,
		namedParamInt = 1496,
		namedParamFloat = 1497,
		namedArrayHeader = 1498,
		namedArraySubhead = 1499,
		namedArrayData = 1500,
		canLiters = 1501,
		next = 1513
	}

	public const int USERDATA1 = 1048577;

	public const int USERDATA2 = 1048578;

	public const int USERDATA3 = 1048579;

	public const int USERDATA4 = 1048580;

	private readonly DateTime summerNormStartDT;

	private readonly DateTime summerNormEndDT;

	public static bool[] passInvTimeDTF;

	public static bool[] passInvTimeTypes;

	public static readonly int setRecordArrayLength;

	public static BitArray eventDataTypes;

	public bool anyChanges;

	public bool curChanges;

	public bool evnChanges;

	private AGBitArray anyChangedDataTypes;

	private AGBitArray curChangedDataTypes;

	public HashSet<int> anyChangedNumericTypes;

	public HashSet<int> curChangedNumericTypes;

	public HashSet<string> anyChangedNames;

	public HashSet<string> curChangedNames;

	private bool pr;

	protected TabularFieldsParams _prm;

	protected internal int _SRaw;

	private bool _IntRcv;

	private bool _PrevPSE;

	private bool _NextPSE;

	private byte _Src;

	private double _LonRaw;

	private double _LatRaw;

	private int _Outputs;

	private int _A0;

	private int _A1;

	private double[] _AVolt = new double[9];

	private int _MainVoltRaw;

	private double _MainVolt;

	private int _ResVoltRaw;

	private double _ResVolt;

	private int _Processor;

	private bool[] _NFC;

	private int[] _C = new int[9];

	private int[] _P = new int[9];

	private double _AltRaw;

	private double _SpeedRaw;

	private double _CourseRaw;

	private int _Sats;

	private int _HDOP;

	private int _IDType;

	private long _ID1W;

	protected long _IDButton;

	protected long _IDBLE;

	protected long _IDCAN;

	private long _IDSN;

	private long[] _Card;

	private readonly int[] _LLS = new int[8];

	private readonly bool[] _VLLS = new bool[8];

	private double _CANSpeed;

	private bool _CANCruise;

	private bool _CANBrake;

	private bool _CANHandbrake;

	private bool _CANCoupling;

	private double _CANGaz;

	private double _CANFtotal;

	private bool _CANFtotalError;

	private readonly double[] _CANL = new double[6];

	private double _CANLAB;

	private int _CANErpmRaw;

	private int _CANDmaint;

	private double _CANEmh;

	private double _CANLOGEmh;

	private int _CANPoil;

	private int _CANTcool;

	private double _CANToil;

	private int _CANTfuel;

	private int _CANPman;

	private int _CANTboost;

	private int _CANPboost;

	private long _CANDtotal;

	private long _CANDdaily;

	private int _EventID;

	private byte[] _AdditionalInfo = new byte[5];

	private int _EventReason;

	private Image[] _PhotoImages = new Image[16];

	private DateTime[] _LoadedPhotosUDT = new DateTime[16];

	private static readonly Size MaxImageSize;

	private double[,] _CANAW;

	private int _UDType;

	private byte[][] _UDArrs;

	private double _DevT;

	private readonly double[] _Temper = new double[8];

	private int _InputsP;

	private int _InputsM;

	private int _IStatus;

	private int _FAID;

	private int _CardID;

	private int _FRChannel;

	private int _FRAddr;

	private int _FRTotal;

	private int _FDDuration;

	private int _FDID;

	private int _PTChannel;

	private int[] _PTMode;

	private int[] _PTStatus;

	private int[] _PTIn;

	private int[,] _PTCtgIn;

	private int[] _PTOut = new int[16];

	private int[,] _PTCtgOut;

	private int[] _PTMDMode;

	private int[] _PTMDStatus;

	private int _MDChannel;

	private int[] _MDMode;

	private int[] _MDStatus;

	private double[] _MDL;

	private int _CANFMI;

	private int _CANSPNv1;

	private int _CANSPNv2;

	private int _CANSPN;

	private bool _CANErrorIsActive;

	private bool _CANLampProtect;

	private bool _CANLampAmber;

	private bool _CANLampRed;

	private bool _CANLampMalfunc;

	private int _CANFlashLampStatus;

	private int _CANOccurCount;

	private double _CANFinstant;

	private double _CANFcalc;

	private double _CANChoker;

	private int _CANTorquePercent;

	private int _CANFrictionPercent;

	private int _CANTorqueMode;

	private int _CANIdlingMode;

	private int _CANKickDownMode;

	private int _CANPTOState;

	private int _CANCruiseState;

	private double _BatteryVolt;

	private double _CANCruiseSpeed;

	private double _CANTair;

	private double _CANPair;

	private double _CANErpm;

	private int _CANEload;

	private int _BatteryAmp;

	private int _LDRawLen;

	private int _LDEnCnt;

	private int _LDType;

	private int _requiredLen;

	private int _requiredEnCnt;

	private List<byte> _longDataList = new List<byte>();

	private byte[] _LDArr = new byte[0];

	private int _LDIndex;

	private double[] _AGFCVolume;

	private int[] _AGFCDuration;

	private long[] _AGFCRefuellerID;

	private long[] _AGFCDriverID;

	private string _LDDriver1 = string.Empty;

	private string _LDDriver2 = string.Empty;

	private string _LDCard1 = string.Empty;

	private string _LDCard2 = string.Empty;

	private int[] _IRMAStatusS = new int[8];

	private int[] _IRMAStatusA = new int[8];

	private int[] emptyFilters = new int[0];

	private static readonly Dictionary<int, int> freqPALPrmDict;

	private static readonly Dictionary<int, int> anlgPALPrmDict;

	private int[] _PALprmF;

	private int[] _PALprmA;

	private long _PALflags;

	private static readonly Dictionary<int, int> statPALprmDict;

	private int[] _PALprmS;

	private readonly int[] _TLLS = new int[8];

	private static readonly Dictionary<int, int> isobusPrmDict;

	private int _ISOBUSType;

	private bool _ISOBUSError;

	private int[] _ISOBUSInt;

	private double[] _ISOBUSFloat;

	private int[] _ISOBUSUInt;

	private GuardState _GuardState;

	private GuardFlags _GuardFlags;

	private int _GuardIndicators;

	private int _NDType;

	private int _NDValue;

	private Dictionary<int, int> _NDDict = new Dictionary<int, int>();

	private int[] _MODBUSInt4321;

	private int[] _MODBUSInt3412;

	private int[] _MODBUSInt2143;

	private int[] _MODBUSInt1234;

	private double[] _MODBUSFloat4321;

	private double[] _MODBUSFloat3412;

	private double[] _MODBUSFloat2143;

	private double[] _MODBUSFloat1234;

	private int[] _NDCANInt4321;

	private readonly byte[] buff = new byte[4];

	private int _DispStatus;

	private int _NaviStatus;

	private bool _NaviIsSet;

	private int _NaviSet;

	private int _NaviGroup;

	private int _NaviSubgroup;

	private int _WorkingState0;

	private int _WorkingState1;

	private int _RelatedState0;

	private int _RelatedState1;

	private int _DriverCard0;

	private int _DriverCard1;

	private int _VehicleMotion;

	private int _VehicleOverspeed;

	private int _VehicleSpeed;

	private double _ShaftRPM;

	private int _MoveDirection;

	private int _WheelAxis;

	private int _WheelIndex;

	private int[,] _WT;

	private double[,] _WP;

	private byte[,] _WA;

	private readonly int[] _FLLS = new int[8];

	private readonly int[] _SLLS = new int[8];

	private readonly int[] _ALLS = new int[8];

	private readonly double[] _VoltLLS = new double[8];

	private readonly int[] _RSSILLS = new int[8];

	private int _StrunaChannel;

	private int _StrunaDataType;

	private int _StrunaRegistry1;

	private int _StrunaRegistry2;

	private int _StrunaRegistry3;

	private int[,] _StrunaReg1Arr;

	private int[,] _StrunaReg2Arr;

	private int[,] _StrunaReg3Arr;

	private bool _DQExtAcc;

	private byte _DQType;

	private bool[] _DQEnd;

	private TimeSpan[] _DQDuration;

	private double[] _DQAccelMax;

	private double[] _DQAccelAver;

	private int _TKAMChannel;

	private byte[] _TKAMOuts;

	private double[] _TKAMAngle;

	private int[] _TKAMTemperature;

	private int[] _TKAMVibration;

	private double[] _TKAMRoulis;

	private double[] _TKAMTangage;

	private double[] _TKAMVoltage;

	private int[] _TKAMRSSI;

	private byte _WCSChannel;

	private bool _WCSIn0;

	private byte _WCSStatusLight;

	private int _WCSWeight;

	private int _WCSFrequency;

	private int _WCSErrorCode;

	private double _TWCouplerLoad;

	private int _TWLoadWeight;

	private int _TWTrailerWeight;

	private int _VSBC1Pressure;

	private int _VSBC2Pressure;

	private int _VSGear;

	private int _VSTotalWeight;

	private double _CANAlternateFtotal;

	private long[] _FMS1Status;

	private long[] _DiscreteParameters;

	private int _ISOBUSGroup;

	private int _ISOBUSSource;

	private int[] _TKAMEventState;

	private long _NPName1;

	private long _NPName2;

	private string _NPName = string.Empty;

	private int _NPType;

	private long _NPUInt;

	private int _NPInt;

	private double _NPFloat;

	protected Dictionary<string, long> _NPUIntDict = new Dictionary<string, long>();

	protected Dictionary<string, int> _NPIntDict = new Dictionary<string, int>();

	protected Dictionary<string, double> _NPFloatDict = new Dictionary<string, double>();

	private readonly int[] _CANLiters = new int[12];

	private static HashSet<int> cycleSentrySet;

	private static HashSet<int> continuousSet;

	[AGInfo("Ignition by DP", "Ignition by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIgnition
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)_DiscreteParameters[0] & 3;
		}
	}

	[AGInfo("Ignition key by DP", "Ignition key by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPKeyIgnition
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 4) & 3;
		}
	}

	[AGInfo("Webasto by DP", "Webasto by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPWebasto
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 6) & 3;
		}
	}

	[AGInfo("Engine by DP", "Engine by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPEngine
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 8) & 3;
		}
	}

	[AGInfo("Engine additional by DP", "Engine additional by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPAddEngine
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 10) & 3;
		}
	}

	[AGInfo("Ready to move by DP", "Ready to move by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPReady
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 12) & 3;
		}
	}

	[AGInfo("Work on gas by DP", "Engine works on gas by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPWorkOnGas
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 14) & 3;
		}
	}

	[AGInfo("Handbrake by DP", "Handbrake by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHandbrake
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 16) & 3;
		}
	}

	[AGInfo("Footbrake by DP", "Footbrake by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFootbrake
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 18) & 3;
		}
	}

	[AGInfo("Clutch by DP", "Clutch by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPClutch
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 20) & 3;
		}
	}

	[AGInfo("Front left door by DP", "Front left door by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontLeftDoor
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 24) & 3;
		}
	}

	[AGInfo("Front right door by DP", "Front right door by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontRightDoor
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 26) & 3;
		}
	}

	[AGInfo("Rear left door by DP", "Rear left door by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearLeftDoor
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 28) & 3;
		}
	}

	[AGInfo("Rear right door by DP", "Rear right door by discrete parameters.", ".", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearRightDoor
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 30) & 3;
		}
	}

	[AGInfo("Trunk by DP", "Trunk by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPTrunk
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 32) & 3;
		}
	}

	[AGInfo("Hood by DP", "Hood by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHood
	{
		get
		{
			read(DataType.discreteParamsData);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[0] >> 34) & 3;
		}
	}

	[AGInfo("Charge wire by DP", "Charge wire by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPChargeWire
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)_DiscreteParameters[1] & 3;
		}
	}

	[AGInfo("Battery charge by DP", "Battery charge by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPBatteryCharge
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 2) & 3;
		}
	}

	[AGInfo("Vehicle closed by DP", "Vehicle closed by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPVehicleClosed
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 8) & 3;
		}
	}

	[AGInfo("Vehicle closed by DP (RC)", "Vehicle closed by discrete parameters (Remote Control).", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPVehicleClosedRC
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 10) & 3;
		}
	}

	[AGInfo("Factory alarm by DP", "Factory alarm by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFactoryAlarm
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 12) & 3;
		}
	}

	[AGInfo("Factory alarm emulation by DP", "Factory alarm emulation by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPEmulFactoryAlarm
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 14) & 3;
		}
	}

	[AGInfo("Close signal by DP (RC)", "Close signal by discrete parameters (Remote Control).", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPCloseSignalRC
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 16) & 3;
		}
	}

	[AGInfo("Open signal by DP (RC)", "Open signal by discrete parameters (Remote Control).", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPOpenSignalRC
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 18) & 3;
		}
	}

	[AGInfo("Restore signal by DP", "Restore signal by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRestoreSignal
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 20) & 3;
		}
	}

	[AGInfo("Trunk open by DP (RC)", "Trunk open by discrete parameters (Remote Control).", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPTrunkOpenRC
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 22) & 3;
		}
	}

	[AGInfo("CAN sleep by DP", "CAN-module sleep by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPCANSleep
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 24) & 3;
		}
	}

	[AGInfo("Close signal by DP (RCx3)", "Close signal by discrete parameters (Remote Control x3).", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPCloseSignalRCx3
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 26) & 3;
		}
	}

	[AGInfo("Transmission P by DP", "Transmission in 'P' by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPTrnsP
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 32) & 3;
		}
	}

	[AGInfo("Transmission R by DP", "Transmission in 'R' by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPTrnsR
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 34) & 3;
		}
	}

	[AGInfo("Transmission N by DP", "Transmission in 'N' by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPTrnsN
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 36) & 3;
		}
	}

	[AGInfo("Transmission D by DP", "Transmission in 'D' by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPTrnsD
	{
		get
		{
			read((DataType)1466);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[1] >> 38) & 3;
		}
	}

	[AGInfo("Park lights by DP", "Park lights by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPParkLights
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)_DiscreteParameters[2] & 3;
		}
	}

	[AGInfo("Low lights by DP", "Low lights by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowLights
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 2) & 3;
		}
	}

	[AGInfo("High lights by DP", "High lights by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHighLights
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 4) & 3;
		}
	}

	[AGInfo("Rear foglights by DP", "Rear foglights by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearFoglights
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 6) & 3;
		}
	}

	[AGInfo("Air condition by DP", "Air condition by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPAirCondition
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 8) & 3;
		}
	}

	[AGInfo("Cruise control by DP", "Cruise control by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPCruiseControl
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 10) & 3;
		}
	}

	[AGInfo("Auto retarder by DP", "Auto retarder by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPAutoRetarder
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 12) & 3;
		}
	}

	[AGInfo("Hand retarder by DP", "Hand retarder by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHandRetarder
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 14) & 3;
		}
	}

	[AGInfo("Driver seat belt by DP", "Driver seat belt by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPDriverBelt
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 16) & 3;
		}
	}

	[AGInfo("Front belt by DP", "Front passenger seat belt by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontBelt
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 18) & 3;
		}
	}

	[AGInfo("Rear left belt by DP", "Rear left passenger seat belt by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearLeftBelt
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 20) & 3;
		}
	}

	[AGInfo("Rear right belt by DP", "Rear right passenger seat belt by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearRightBelt
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 22) & 3;
		}
	}

	[AGInfo("Rear central belt by DP", "Rear central passenger seat belt by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearCentralBelt
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 24) & 3;
		}
	}

	[AGInfo("Front central belt by DP", "Front central passenger seat belt by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontCentralBelt
	{
		get
		{
			read((DataType)1467);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[2] >> 26) & 3;
		}
	}

	[AGInfo("Check indicator by DP", "Check engine indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPICheckEngine
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)_DiscreteParameters[3] & 3;
		}
	}

	[AGInfo("ABS indicator by DP", "ABS indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIABS
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 2) & 3;
		}
	}

	[AGInfo("ESP indicator by DP", "ESP indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIESP
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 4) & 3;
		}
	}

	[AGInfo("ESP by DP", "ESP by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPESP
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 6) & 3;
		}
	}

	[AGInfo("Stop indicator by DP", "Stop indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIStop
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 8) & 3;
		}
	}

	[AGInfo("Oil indicator by DP", "Oil indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIPressLvlOil
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 10) & 3;
		}
	}

	[AGInfo("Coolant indicator by DP", "Coolant indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPPressLvlCoolant
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 12) & 3;
		}
	}

	[AGInfo("Battery indicator by DP", "Battery indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIBattery
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 14) & 3;
		}
	}

	[AGInfo("Handbrake indicator by DP", "Handbrake indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIHandbrake
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 16) & 3;
		}
	}

	[AGInfo("Airbag indicator by DP", "Airbag indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIAirbag
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 18) & 3;
		}
	}

	[AGInfo("EPS indicator by DP", "EPS indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIEPS
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 20) & 3;
		}
	}

	[AGInfo("Warning indicator by DP", "Warning indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIWarning
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 22) & 3;
		}
	}

	[AGInfo("Ext. lights malf. by DP", "External lights malfunction by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPExtLightsMalf
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 24) & 3;
		}
	}

	[AGInfo("Low wheel pressure by DP", "Low wheel pressure by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowPressure
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 26) & 3;
		}
	}

	[AGInfo("Brake pad wear by DP", "Brake pad wear by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPBrakePadWear
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 28) & 3;
		}
	}

	[AGInfo("Low fuel level by DP", "Low fuel level by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowFuelLevel
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 30) & 3;
		}
	}

	[AGInfo("Tech. insp. by DP", "Technical inspection by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPTechInsp
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 32) & 3;
		}
	}

	[AGInfo("Glow plug indicator by DP", "Glow plug indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIGlowPlug
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 34) & 3;
		}
	}

	[AGInfo("FAP indicator by DP", "FAP indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIFAP
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 36) & 3;
		}
	}

	[AGInfo("EPC indicator by DP", "EPC indicator by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPIEPC
	{
		get
		{
			read((DataType)1468);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[3] >> 38) & 3;
		}
	}

	[AGInfo("Hydr. system filter by DP", "Hydraulic system filter clogging by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHydrFltrClogged
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)_DiscreteParameters[4] & 3;
		}
	}

	[AGInfo("Low oil pres. by DP", "Low engine oil pressure by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowOilPressure
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 2) & 3;
		}
	}

	[AGInfo("High oil pres. by DP", "High engine oil pressure by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHighOilPressure
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 4) & 3;
		}
	}

	[AGInfo("Low coolant level by DP", "Low coolant level by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowCoolantLevel
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 6) & 3;
		}
	}

	[AGInfo("Hydr. clogging by DP", "Hydraulic system clogging by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHydrClogged
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 8) & 3;
		}
	}

	[AGInfo("Low hydr. pres. by DP", "Low hydraulic pressure by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowHydrPressure
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 10) & 3;
		}
	}

	[AGInfo("Low hydr. level by DP", "Low hydraulic level by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowHydrLevel
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 12) & 3;
		}
	}

	[AGInfo("High hydr. temper. by DP", "High hydraulic temperature by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHighHydrTemper
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 14) & 3;
		}
	}

	[AGInfo("Hydr. oil over. by DP", "Hydraulic system oil overflow by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPHydrOilOver
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 16) & 3;
		}
	}

	[AGInfo("Air filter by DP", "Air filter clogging by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPAirFltrClogged
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 18) & 3;
		}
	}

	[AGInfo("Fuel filter by DP", "Fuel filter clogging by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFuelFltrClogged
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 20) & 3;
		}
	}

	[AGInfo("Water in fuel by DP", "Water in fuel by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPWaterInFuel
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 22) & 3;
		}
	}

	[AGInfo("Brake system filter by DP", "Brake system filter clogging by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPBrakeFltrClogged
	{
		get
		{
			read((DataType)1469);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[4] >> 24) & 3;
		}
	}

	[AGInfo("Right joystick RIGHT by DP", "Right joystick RIGHT by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRStickRight
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)_DiscreteParameters[5] & 3;
		}
	}

	[AGInfo("Right joystick LEFT by DP", "Right joystick LEFT by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRStickLeft
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 2) & 3;
		}
	}

	[AGInfo("Right joystick FWD by DP", "Right joystick FWD by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRStickFwd
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 4) & 3;
		}
	}

	[AGInfo("Right joystick BACK by DP", "Right joystick BACK by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRStickBack
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 6) & 3;
		}
	}

	[AGInfo("Left joystick RIGHT by DP", "Left joystick RIGHT by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLStickRight
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 8) & 3;
		}
	}

	[AGInfo("Left joystick LEFT by DP", "Left joystick LEFT by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLStickLeft
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 10) & 3;
		}
	}

	[AGInfo("Left joystick FWD by DP", "Left joystick FWD by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLStickFwd
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 12) & 3;
		}
	}

	[AGInfo("Left joystick BACK by DP", "Left joystick BACK by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLStickBack
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 14) & 3;
		}
	}

	[AGInfo("Rear hydr. drv. 1 by DP", "Rear hydraulic drive 1 by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearHydrDrv1
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 16) & 3;
		}
	}

	[AGInfo("Rear hydr. drv. 2 by DP", "Rear hydraulic drive 2 by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearHydrDrv2
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 18) & 3;
		}
	}

	[AGInfo("Rear hydr. drv. 3 by DP", "Rear hydraulic drive 3 by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearHydrDrv3
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 20) & 3;
		}
	}

	[AGInfo("Rear hydr. drv. 4 by DP", "Rear hydraulic drive 4 by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearHydrDrv4
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 22) & 3;
		}
	}

	[AGInfo("Front hydr. drv. 1 by DP", "Front hydraulic drive 1 by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontHydrDrv1
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 24) & 3;
		}
	}

	[AGInfo("Front hydr. drv. 2 by DP", "Front hydraulic drive 2 by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontHydrDrv2
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 26) & 3;
		}
	}

	[AGInfo("Front hydr. drv. 3 by DP", "Front hydraulic drive 3 by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontHydrDrv3
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 28) & 3;
		}
	}

	[AGInfo("Front hydr. drv. 4 by DP", "Front hydraulic drive 4 by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontHydrDrv4
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 30) & 3;
		}
	}

	[AGInfo("Front hitch by DP", "Front three-point hitch system by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontHitch
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 32) & 3;
		}
	}

	[AGInfo("Rear hitch by DP", "Rear three-point hitch system by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearHitch
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 34) & 3;
		}
	}

	[AGInfo("Front PTO by DP", "Front power take-off mechanism by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPFrontPTO
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 36) & 3;
		}
	}

	[AGInfo("Rear PTO by DP", "Rear power take-off mechanism by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPRearPTO
	{
		get
		{
			read((DataType)1470);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[5] >> 38) & 3;
		}
	}

	[AGInfo("Mowing by DP", "Mowing by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPMowing
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)_DiscreteParameters[6] & 3;
		}
	}

	[AGInfo("Threshing by DP", "Threshing by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPThreshing
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 2) & 3;
		}
	}

	[AGInfo("Grain unloading by DP", "Grain unloading by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPGrainUnloading
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 4) & 3;
		}
	}

	[AGInfo("Grain tank 100%  by DP", "Grain tank 100% by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPGrainTank100
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 8) & 3;
		}
	}

	[AGInfo("Grain tank 70% by DP", "Grain tank 70% by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPGrainTank70
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 10) & 3;
		}
	}

	[AGInfo("Grain tank open by DP", "Grain tank is open by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPGrainTankOpen
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 12) & 3;
		}
	}

	[AGInfo("Unloading drv. by DP", "Unloading drive by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPUUnloadingDrv
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 14) & 3;
		}
	}

	[AGInfo("Clean. fan ctrl. by DP", "Cleaning fan control by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPCleaningFanCtrl
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 16) & 3;
		}
	}

	[AGInfo("Thresh. drum ctrl. by DP", "Threshing drum control by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPThreshingDrumCtrl
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 18) & 3;
		}
	}

	[AGInfo("Straw rake by DP", "Straw rake by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPStrawRake
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 20) & 3;
		}
	}

	[AGInfo("Сlearance under drum by DP", "Excess clearance under the threshing drum by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPClearenceUnderDrum
	{
		get
		{
			read((DataType)1471);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[6] >> 22) & 3;
		}
	}

	[AGInfo("Salt sprayer by DP", "Salt/sand sprayer by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPSaltSprayer
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)_DiscreteParameters[7] & 3;
		}
	}

	[AGInfo("Reagent watering by DP", "Reagent watering by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPReagentWatering
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 2) & 3;
		}
	}

	[AGInfo("Conveyor belt by DP", "Conveyor belt by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPConveyorBelt
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 4) & 3;
		}
	}

	[AGInfo("Salt spr. wheel by DP", "Salt spreader drive wheel by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPSprDrvWheel
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 6) & 3;
		}
	}

	[AGInfo("Brush by DP", " by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPBrush
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 8) & 3;
		}
	}

	[AGInfo("Vacuum by DP", "Vacuum cleaner by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPVacuum
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 10) & 3;
		}
	}

	[AGInfo("Water supply by DP", "Water supply by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPWaterSupply
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 12) & 3;
		}
	}

	[AGInfo("Washing device by DP", "Washing device by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPWashingDevice
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 14) & 3;
		}
	}

	[AGInfo("Liquid pump by DP", "Liquid supply pump by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLiquidPump
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 16) & 3;
		}
	}

	[AGInfo("Bunker unloading by DP", "Bunker unloading by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPBunkerUnloading
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 18) & 3;
		}
	}

	[AGInfo("Low salt level by DP", "Low salt/sand level by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowSaltLevel
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 20) & 3;
		}
	}

	[AGInfo("Low water level by DP", "Low water level by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPLowWaterLevel
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 22) & 3;
		}
	}

	[AGInfo("Reagent using by DP", "Reagent using by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPReagentUsing
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 24) & 3;
		}
	}

	[AGInfo("Compressor by DP", "Compressor by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPCompressor
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 26) & 3;
		}
	}

	[AGInfo("Water valve by DP", "Water valve by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPWaterValve
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 28) & 3;
		}
	}

	[AGInfo("Cab at top by DP", "Cab at top by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPCabAtTop
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 32) & 3;
		}
	}

	[AGInfo("Cab at bottom by DP", "Cab at bottom by discrete parameters.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 73 })]
	public int DPCabAtBottom
	{
		get
		{
			read((DataType)1472);
			if (_DiscreteParameters == null)
			{
				return 0;
			}
			return (int)(_DiscreteParameters[7] >> 34) & 3;
		}
	}

	public bool propRead => pr;

	[AGInfo("Image", "Image.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public Image Image
	{
		get
		{
			read(DataType.common);
			return _Image;
		}
	}

	private Image _Image => _prm.image;

	[AGInfo("Device UID", "Device UID.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public int SerialNo
	{
		get
		{
			read(DataType.common);
			return _SerialNo;
		}
	}

	private int _SerialNo => _prm.serialNo;

	[AGInfo("Entry description", "Entry description.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public string EntryDesc
	{
		get
		{
			read(DataType.common);
			return null;
		}
	}

	[AGInfo("Universal raw time", "Universal raw time by receiver (before filtration).", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime UDTRaw
	{
		get
		{
			read(DataType.common);
			return _UDTRaw;
		}
	}

	private DateTime _UDTRaw => ((DeviceRecordLite)_prm.rec).DT;

	private TimeSpan _Duration => _prm.duration;

	private TimeSpan _PrevInt => _prm.prevInt;

	private TimeSpan _NextInt => _prm.nextInt;

	[AGInfo("Universal time", "Universal time (after filtration).", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime UDT
	{
		get
		{
			read(DataType.common);
			return _UDT;
		}
	}

	[AGInfo("Local time", "Local time (after filtration).", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime DT
	{
		get
		{
			read(DataType.common);
			if (!(_UDT != DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_UDT, timeZoneInfo);
		}
	}

	[AGInfo("Duration", "Accumulated duration from start of calculation period.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public TimeSpan Duration
	{
		get
		{
			read(DataType.common);
			return _Duration;
		}
	}

	[AGInfo("Previous interval", "Time interval from the previous entry.", DeviceParameterKind.Accum, AGInfoGroupType.Time, false, new int[] { })]
	public TimeSpan PrevInt
	{
		get
		{
			read(DataType.common);
			return _PrevInt;
		}
	}

	[AGInfo("Next interval", "Time interval to the next entry.", DeviceParameterKind.Flag, AGInfoGroupType.Time, false, new int[] { })]
	public TimeSpan NextInt
	{
		get
		{
			read(DataType.common);
			return _NextInt;
		}
	}

	[AGInfo("Summer norm", "Summer norm period.", DeviceParameterKind.Flag, AGInfoGroupType.Time | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool SummerNorm
	{
		get
		{
			DateTime dT = DT;
			DateTime dateTime = correctedDateTime(dT.Year, summerNormStartDT.Month, summerNormStartDT.Day, summerNormStartDT.Hour, summerNormStartDT.Minute, summerNormStartDT.Second);
			DateTime dateTime2 = correctedDateTime(dT.Year, summerNormEndDT.Month, summerNormEndDT.Day, summerNormEndDT.Hour, summerNormEndDT.Minute, summerNormEndDT.Second);
			if (dateTime > dateTime2)
			{
				if (dT < dateTime)
				{
					dateTime = correctedDateTime(dT.Year - 1, summerNormStartDT.Month, summerNormStartDT.Day, summerNormStartDT.Hour, summerNormStartDT.Minute, summerNormStartDT.Second);
				}
				else if (dT > dateTime2)
				{
					dateTime2 = correctedDateTime(dT.Year + 1, summerNormEndDT.Month, summerNormEndDT.Day, summerNormEndDT.Hour, summerNormEndDT.Minute, summerNormEndDT.Second);
				}
			}
			if (dateTime <= dT)
			{
				return dT < dateTime2;
			}
			return false;
		}
	}

	private InputFlags _FlagsRaw => ((DeviceRecordLite)_prm.rec).Inputs;

	private InputFlags Flags
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			read(DataType.common);
			return _Flags;
		}
	}

	private InputFlags _Flags => _prm.currFlags;

	[AGInfo("Flags by receiver", "Raw flags by receiver.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public int FlagsRaw
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected I4, but got Unknown
			read(DataType.common);
			return (int)_FlagsRaw;
		}
	}

	[AGInfo("Any power", "Any power flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool Power
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Invalid comparison between Unknown and I4
			InputFlags flags = Flags;
			if ((flags & 0xC00) == 0)
			{
				return (flags & 0x4000) != 16384;
			}
			return true;
		}
	}

	[AGInfo("Main power", "Main power flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool B => (Flags & 0x400) == 1024;

	[AGInfo("Backup power", "Backup power flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool R => (Flags & 0x800) == 2048;

	[AGInfo("USB power", "USB power flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool U => (Flags & 0x4000) != 16384;

	[AGInfo("Alarm button", "Alarm button flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool AlarmButton => (Flags & 0x8000) == 32768;

	[AGInfo("Engine by CAN", "Engine working flag by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool CE => (Flags & 0x10000) == 65536;

	[AGInfo("Motion detection", "Motion detection flag.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool M => (Flags & 0x100000) == 1048576;

	[AGInfo("Internal accumulator", "Internal accumulator flag: 0 - battery is missing or discharged, 1 - have the battery voltage.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool IntAcc => (Flags & 0x400000) == 4194304;

	[AGInfo("Data sent 1", "Sending data to server flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool DataSent1 => (Flags & 0x100) == 256;

	[AGInfo("Data sent 2", "Sending data to server flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool DataSent2 => (Flags & 0x200) == 512;

	[AGInfo("Antenna 1", "Navigation receiver antenna connection flag.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool Ant1 => (Flags & 0x1000) == 4096;

	[AGInfo("Antenna 2", "Navigation receiver antenna connection flag.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool Ant2 => (Flags & 0x2000) == 8192;

	[AGInfo("GSM present", "GSM present flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool GSMPresent => (Flags & 0x80000) == 524288;

	[AGInfo("GSM home", "GSM home flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool GSMHome => (Flags & 0x20000) == 131072;

	[AGInfo("Cargo", "Cargo flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool Cargo => (Flags & 0x40000) == 262144;

	[AGInfo("Loading", "Loading flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool Loading => (Flags & 0x40000) == 262144;

	[AGInfo("Inputs", "State of discrete inputs 1 … 9. Input 1 - 0 bit, …, input 9 - 8 bit.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public int Inputs
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected I4, but got Unknown
			InputFlags flags = Flags;
			return (flags & 0xFF) | (((flags & 0x200000) == 2097152) ? 256 : 0);
		}
	}

	[AGInfo("Input 1", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I1 => (Flags & 1) == 1;

	[AGInfo("Input 2", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I2 => (Flags & 2) == 2;

	[AGInfo("Input 3", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I3 => (Flags & 4) == 4;

	[AGInfo("Input 4", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I4 => (Flags & 8) == 8;

	[AGInfo("Input 5", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I5 => (Flags & 0x10) == 16;

	[AGInfo("Input 6", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I6 => (Flags & 0x20) == 32;

	[AGInfo("Input 7", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I7 => (Flags & 0x40) == 64;

	[AGInfo("Input 8", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I8 => (Flags & 0x80) == 128;

	[AGInfo("Input 9 (high resistance)", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I9 => (Flags & 0x200000) == 2097152;

	[AGInfo("Bin-format - Power", "Power flag (for obsolete bin-format).", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool PowerBin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Invalid comparison between Unknown and I4
			read(DataType.flgAndInp12);
			return (_Flags & 0x400) == 1024;
		}
	}

	[AGInfo("Bin-format - Inputs 1-2", "State of discrete inputs 1 … 2 (for obsolete bin-format). Input 1 - 0 bit, input 2 - 1 bit.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public int Inputs12Bin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected I4, but got Unknown
			read(DataType.flgAndInp12);
			return _Flags & 3;
		}
	}

	[AGInfo("Bin-format - Inputs 3-6", "State of discrete inputs 3 … 6 (for obsolete bin-format). Input 3 - 2 bit, …, input 6 - 5 bit.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 0, 4 })]
	public int Inputs36Bin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected I4, but got Unknown
			read(DataType.inputs36);
			return _Flags & 0x3C;
		}
	}

	[AGInfo("Bin-format - Input 1", "State of discrete input (for obsolete bin-format).", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I1Bin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			read(DataType.flgAndInp12);
			return (_Flags & 1) == 1;
		}
	}

	[AGInfo("Bin-format - Input 2", "State of discrete input (for obsolete bin-format).", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I2Bin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			read(DataType.flgAndInp12);
			return (_Flags & 2) == 2;
		}
	}

	[AGInfo("Bin-format - Input 3", "State of discrete input (for obsolete bin-format).", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 0, 4 })]
	public bool I3Bin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			read(DataType.inputs36);
			return (_Flags & 4) == 4;
		}
	}

	[AGInfo("Bin-format - Input 4", "State of discrete input (for obsolete bin-format).", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 0, 4 })]
	public bool I4Bin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Invalid comparison between Unknown and I4
			read(DataType.inputs36);
			return (_Flags & 8) == 8;
		}
	}

	[AGInfo("Bin-format - Input 5", "State of discrete input (for obsolete bin-format).", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 0, 4 })]
	public bool I5Bin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			read(DataType.inputs36);
			return (_Flags & 0x10) == 16;
		}
	}

	[AGInfo("Bin-format - Input 6", "State of discrete input (for obsolete bin-format).", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 0, 4 })]
	public bool I6Bin
	{
		get
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			read(DataType.inputs36);
			return (_Flags & 0x20) == 32;
		}
	}

	[AGInfo("Pulses presence on input 1", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC1 => (Flags & 1) == 1;

	[AGInfo("Pulses presence on input 2", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC2 => (Flags & 2) == 2;

	[AGInfo("Pulses presence on input 3", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC3 => (Flags & 4) == 4;

	[AGInfo("Pulses presence on input 4", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC4 => (Flags & 8) == 8;

	[AGInfo("Pulses presence on input 5", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC5 => (Flags & 0x10) == 16;

	[AGInfo("Pulses presence on input 6", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC6 => (Flags & 0x20) == 32;

	[AGInfo("Pulses presence on input 7", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC7 => (Flags & 0x40) == 64;

	[AGInfo("Pulses presence on input 8", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC8 => (Flags & 0x80) == 128;

	[AGInfo("Pulses presence on input 9", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC9 => (Flags & 0x200000) == 2097152;

	public int _RouteStatus => _prm.routeStatus;

	[AGInfo("Route status", "Route status.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public int RouteStatus
	{
		get
		{
			read(DataType.common);
			return _RouteStatus;
		}
	}

	private int _TypeID => _prm.TypeID;

	private bool _CRC => ((DeviceRecordLite)_prm.rec).CRCValid;

	private int _DTF => (int)_prm.dtf;

	private bool _TRaw => ((DeviceRecordLite)_prm.rec).TimeValid;

	[AGInfo("Type ID", "Entry identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public int TypeID
	{
		get
		{
			read(DataType.typeId);
			return _TypeID;
		}
	}

	[AGInfo("CRC", "CRC valid flag.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { })]
	public bool CRC
	{
		get
		{
			read(DataType.typeId);
			return _CRC;
		}
	}

	[AGInfo("Time filtration", "Date and time filtration code.", DeviceParameterKind.Flag, AGInfoGroupType.Time, false, new int[] { })]
	public int DTF
	{
		get
		{
			read(DataType.common);
			return _DTF;
		}
	}

	[AGInfo("Valid time", "Valid date and time.", DeviceParameterKind.Flag, AGInfoGroupType.Time | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool T => DTF <= 1;

	[AGInfo("Valid time raw", "Valid date and time by receiver.", DeviceParameterKind.Flag, AGInfoGroupType.Time | AGInfoGroupType.Sensor, false, new int[] { })]
	public bool TRaw
	{
		get
		{
			read(DataType.common);
			return _TRaw;
		}
	}

	private int _LLF => (int)_prm.llf;

	[AGInfo("Coordinate filtration", "Coordinate filtration code.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation, false, new int[] { })]
	public int LLF
	{
		get
		{
			read(DataType.common);
			return _LLF;
		}
	}

	public bool fullCalc => _prm != null;

	[AGInfo("Raw signal level", "Level of navigation signal from 0 (no) to 7 (maximum) by receiver.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public int SRaw
	{
		get
		{
			read(DataType.coordinatesRaw);
			return _SRaw;
		}
	}

	[AGInfo("Internal receiver", "Internal receiver working flag.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool IntRcv
	{
		get
		{
			read(DataType.coordinatesRaw);
			return _IntRcv;
		}
	}

	[AGInfo("Possible prev. speed error", "Possible speed calculation error flag (from the previous entry).", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool PrevPSE
	{
		get
		{
			read(DataType.coordinatesRaw);
			return _PrevPSE;
		}
	}

	[AGInfo("Possible next speed error", "Possible speed calculation error flag (to the next entry).", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Sensor, false, new int[] { 0 })]
	public bool NextPSE
	{
		get
		{
			read(DataType.coordinatesRaw);
			return _NextPSE;
		}
	}

	[AGInfo("Source of coordinates", "Source of coordinates: 1 - GPS, 2 - GLONASS, 3 - combined work.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public byte Src
	{
		get
		{
			read(DataType.coordinatesRaw);
			return _Src;
		}
	}

	[AGInfo("Raw longitude", "Raw longitude by receiver.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public double LonRaw
	{
		get
		{
			read(DataType.coordinatesRaw);
			return _LonRaw;
		}
	}

	[AGInfo("Raw latitude", "Raw latitude by receiver.", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation, false, new int[] { 0 })]
	public double LatRaw
	{
		get
		{
			read(DataType.coordinatesRaw);
			return _LatRaw;
		}
	}

	[AGInfo("Output 1", "State of discrete output.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { 1 })]
	public bool Out1
	{
		get
		{
			read(DataType.analogData);
			return (_Outputs & 4) != 0;
		}
	}

	[AGInfo("Output 2", "State of discrete output.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { 1 })]
	public bool Out2
	{
		get
		{
			read(DataType.analogData);
			return (_Outputs & 8) != 0;
		}
	}

	[AGInfo("Analog data 1, ADC", "Analog data voltage in ADC samples.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure, false, new int[] { 1 })]
	public int A1
	{
		get
		{
			read(DataType.analogData);
			return _A0;
		}
	}

	[AGInfo("Analog data 2, ADC", "Analog data voltage in ADC samples.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure, false, new int[] { 1 })]
	public int A2
	{
		get
		{
			read(DataType.analogData);
			return _A1;
		}
	}

	[AGInfo("Analog data 1, voltage", "Analog data 1 voltage.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure, false, new int[] { 1, 2, 3, 5, 7 })]
	public double A1Volt
	{
		get
		{
			read(DataType.aCounters);
			return _AVolt[0];
		}
	}

	[AGInfo("Analog data 2, voltage", "Analog data 2 voltage.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure, false, new int[] { 1, 2, 3, 5, 7 })]
	public double A2Volt
	{
		get
		{
			read((DataType)39);
			return _AVolt[1];
		}
	}

	[AGInfo("Main power raw voltage", "Main power raw voltage in ADC samples.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 1 })]
	public int MainVoltRaw
	{
		get
		{
			read(DataType.analogData);
			return _MainVoltRaw;
		}
	}

	[AGInfo("Main power voltage", "Main power voltage.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 1 })]
	public double MainVolt
	{
		get
		{
			read(DataType.analogData);
			return _MainVolt;
		}
	}

	[AGInfo("Backup power raw voltage", "Backup power raw voltage in ADC samples.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 1 })]
	public int BackupVoltRaw
	{
		get
		{
			read(DataType.analogData);
			return _ResVoltRaw;
		}
	}

	[AGInfo("Backup power voltage", "Backup power voltage.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 1 })]
	public double BackupVolt
	{
		get
		{
			read(DataType.analogData);
			return _ResVolt;
		}
	}

	[AGInfo("Processor load", "Processor load in %.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 1 })]
	public int Processor
	{
		get
		{
			read(DataType.analogData);
			return _Processor;
		}
	}

	private double[] _F => _prm.Freq;

	[AGInfo("Counter value by input 1 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 2 })]
	public bool NFC1
	{
		get
		{
			read(DataType.nfCounters);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[0];
		}
	}

	[AGInfo("Counter value by input 2 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 2 })]
	public bool NFC2
	{
		get
		{
			read((DataType)49);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[1];
		}
	}

	[AGInfo("Counter value by input 3 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 3 })]
	public bool NFC3
	{
		get
		{
			read((DataType)50);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[2];
		}
	}

	[AGInfo("Counter value by input 4 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 3 })]
	public bool NFC4
	{
		get
		{
			read((DataType)51);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[3];
		}
	}

	[AGInfo("Counter value by input 5 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 5 })]
	public bool NFC5
	{
		get
		{
			read((DataType)52);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[4];
		}
	}

	[AGInfo("Counter value by input 6 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 5 })]
	public bool NFC6
	{
		get
		{
			read((DataType)53);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[5];
		}
	}

	[AGInfo("Counter value by input 7 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 7 })]
	public bool NFC7
	{
		get
		{
			read((DataType)54);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[6];
		}
	}

	[AGInfo("Counter value by input 8 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 7 })]
	public bool NFC8
	{
		get
		{
			read((DataType)55);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[7];
		}
	}

	[AGInfo("Counter value by input 9 is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { 57 })]
	public bool NFC9
	{
		get
		{
			read((DataType)56);
			if (_NFC == null)
			{
				return false;
			}
			return _NFC[8];
		}
	}

	[AGInfo("Continuous counter by input 1", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 2 })]
	public int C1
	{
		get
		{
			read(DataType.cCounters);
			return _C[0];
		}
	}

	[AGInfo("Continuous counter by input 2", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 2 })]
	public int C2
	{
		get
		{
			read((DataType)9);
			return _C[1];
		}
	}

	[AGInfo("Continuous counter by input 3", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 3 })]
	public int C3
	{
		get
		{
			read((DataType)10);
			return _C[2];
		}
	}

	[AGInfo("Continuous counter by input 4", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 3 })]
	public int C4
	{
		get
		{
			read((DataType)11);
			return _C[3];
		}
	}

	[AGInfo("Continuous counter by input 5", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 5 })]
	public int C5
	{
		get
		{
			read((DataType)12);
			return _C[4];
		}
	}

	[AGInfo("Continuous counter by input 6", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 5 })]
	public int C6
	{
		get
		{
			read((DataType)13);
			return _C[5];
		}
	}

	[AGInfo("Continuous counter by input 7", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 7 })]
	public int C7
	{
		get
		{
			read((DataType)14);
			return _C[6];
		}
	}

	[AGInfo("Continuous counter by input 8", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 7 })]
	public int C8
	{
		get
		{
			read((DataType)15);
			return _C[7];
		}
	}

	[AGInfo("Continuous counter by input 9", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 57 })]
	public int C9
	{
		get
		{
			read((DataType)16);
			return _C[8];
		}
	}

	[AGInfo("Periodic counter by input 1", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 2 })]
	public int P1
	{
		get
		{
			read(DataType.pCounters);
			return _P[0];
		}
	}

	[AGInfo("Periodic counter by input 2", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 2 })]
	public int P2
	{
		get
		{
			read((DataType)19);
			return _P[1];
		}
	}

	[AGInfo("Periodic counter by input 3", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 3 })]
	public int P3
	{
		get
		{
			read((DataType)20);
			return _P[2];
		}
	}

	[AGInfo("Periodic counter by input 4", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 3 })]
	public int P4
	{
		get
		{
			read((DataType)21);
			return _P[3];
		}
	}

	[AGInfo("Periodic counter by input 5", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 5 })]
	public int P5
	{
		get
		{
			read((DataType)22);
			return _P[4];
		}
	}

	[AGInfo("Periodic counter by input 6", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 5 })]
	public int P6
	{
		get
		{
			read((DataType)23);
			return _P[5];
		}
	}

	[AGInfo("Periodic counter by input 7", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 7 })]
	public int P7
	{
		get
		{
			read((DataType)24);
			return _P[6];
		}
	}

	[AGInfo("Periodic counter by input 8", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 7 })]
	public int P8
	{
		get
		{
			read((DataType)25);
			return _P[7];
		}
	}

	[AGInfo("Periodic counter by input 9", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 57 })]
	public int P9
	{
		get
		{
			read((DataType)26);
			return _P[8];
		}
	}

	[AGInfo("Temperature by periodic counter of input 1", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 2 })]
	public int TP1
	{
		get
		{
			read(DataType.pCounters);
			return _P[0] - 60;
		}
	}

	[AGInfo("Temperature by periodic counter of input 2", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 2 })]
	public int TP2
	{
		get
		{
			read((DataType)19);
			return _P[1] - 60;
		}
	}

	[AGInfo("Temperature by periodic counter of input 3", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 3 })]
	public int TP3
	{
		get
		{
			read((DataType)20);
			return _P[2] - 60;
		}
	}

	[AGInfo("Temperature by periodic counter of input 4", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 3 })]
	public int TP4
	{
		get
		{
			read((DataType)21);
			return _P[3] - 60;
		}
	}

	[AGInfo("Temperature by periodic counter of input 5", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 5 })]
	public int TP5
	{
		get
		{
			read((DataType)22);
			return _P[4] - 60;
		}
	}

	[AGInfo("Temperature by periodic counter of input 6", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 5 })]
	public int TP6
	{
		get
		{
			read((DataType)23);
			return _P[5] - 60;
		}
	}

	[AGInfo("Temperature by periodic counter of input 7", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 7 })]
	public int TP7
	{
		get
		{
			read((DataType)24);
			return _P[6] - 60;
		}
	}

	[AGInfo("Temperature by periodic counter of input 8", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 7 })]
	public int TP8
	{
		get
		{
			read((DataType)25);
			return _P[7] - 60;
		}
	}

	[AGInfo("Temperature by periodic counter of input 9", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 57 })]
	public int TP9
	{
		get
		{
			read((DataType)26);
			return _P[8] - 60;
		}
	}

	[AGInfo("Frequency by input 1", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 2 })]
	public double F1
	{
		get
		{
			read(DataType.fCounters);
			return _F[0];
		}
	}

	[AGInfo("Frequency by input 2", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 2 })]
	public double F2
	{
		get
		{
			read((DataType)29);
			return _F[1];
		}
	}

	[AGInfo("Frequency by input 3", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 3 })]
	public double F3
	{
		get
		{
			read((DataType)30);
			return _F[2];
		}
	}

	[AGInfo("Frequency by input 4", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 3 })]
	public double F4
	{
		get
		{
			read((DataType)31);
			return _F[3];
		}
	}

	[AGInfo("Frequency by input 5", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 5 })]
	public double F5
	{
		get
		{
			read((DataType)32);
			return _F[4];
		}
	}

	[AGInfo("Frequency by input 6", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 5 })]
	public double F6
	{
		get
		{
			read((DataType)33);
			return _F[5];
		}
	}

	[AGInfo("Frequency by input 7", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 7 })]
	public double F7
	{
		get
		{
			read((DataType)34);
			return _F[6];
		}
	}

	[AGInfo("Frequency by input 8", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 7 })]
	public double F8
	{
		get
		{
			read((DataType)35);
			return _F[7];
		}
	}

	[AGInfo("Frequency by input 9", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 57 })]
	public double F9
	{
		get
		{
			read((DataType)36);
			return _F[8];
		}
	}

	[AGInfo("Frequency RPM", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 57 })]
	public double FRPM
	{
		get
		{
			read((DataType)37);
			return _F[9];
		}
	}

	[AGInfo("Raw altitude", "Raw altitude in meters by receiver.", "m", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 4 })]
	public double AltRaw
	{
		get
		{
			read(DataType.altitude);
			return _AltRaw;
		}
	}

	[AGInfo("Speed by receiver", "Instant speed im km/h by receiver.", "km/h", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 4 })]
	public double SpeedRaw
	{
		get
		{
			read(DataType.motion);
			return _SpeedRaw;
		}
	}

	[AGInfo("Course by receiver", "Instant course from the North direction clockwise in degrees by receiver.", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 4 })]
	public double CourseRaw
	{
		get
		{
			read(DataType.motion);
			return _CourseRaw;
		}
	}

	[AGInfo("Satellites number", "Satellites number.", DeviceParameterKind.Flag, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 4 })]
	public int Sats
	{
		get
		{
			read(DataType.motion);
			return _Sats;
		}
	}

	[AGInfo("HDOP", "Level of GPS/GLONASS signal from 0 (minimum) to 50 (maximum).", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level, false, new int[] { 4 })]
	public int HDOP
	{
		get
		{
			read(DataType.motion);
			return _HDOP;
		}
	}

	[AGInfo("ID Type", "ID Type: 0 - iButton; 1, 3 - BLE; 2 - CAN; 14, 15 - MODBUS.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 6 })]
	public int IDType
	{
		get
		{
			read(DataType.idType);
			return _IDType;
		}
	}

	[AGInfo("1-wire ID", "1-wire ID. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier, false, new int[] { 6 })]
	public long ID1W
	{
		get
		{
			read(DataType.id1w);
			return _ID1W;
		}
	}

	[AGInfo("Card ID by 1-wire", "Card ID by 1-wire. 0 if empty. Returns 5 lower bytes of ID1W.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier, false, new int[] { 6 })]
	public long Card1W
	{
		get
		{
			read(DataType.id1w);
			return _ID1W & 0xFFFFFF;
		}
	}

	[AGInfo("iButton ID", "iButton ID. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier, false, new int[] { 6 })]
	public long IDButton
	{
		get
		{
			read((DataType)62);
			return _IDButton;
		}
	}

	[AGInfo("Bluetooth ID", "Bluetooth low energy ID. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier, false, new int[] { 6 })]
	public long IDBLE
	{
		get
		{
			read((DataType)63);
			return _IDBLE;
		}
	}

	[AGInfo("CAN ID", "CAN ID. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier, false, new int[] { 6 })]
	public long IDCAN
	{
		get
		{
			read((DataType)64);
			return _IDCAN;
		}
	}

	[AGInfo("Vehicle ID from BLE", "Vehicle ID from Bluetooth low energy. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier, false, new int[] { 6 })]
	public long IDSN
	{
		get
		{
			read((DataType)65);
			return _IDSN;
		}
	}

	[AGInfo("Level by LLS 1", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 8, 53 })]
	public int LLS1
	{
		get
		{
			read(DataType.lls);
			return _LLS[0];
		}
	}

	[AGInfo("Level by LLS 2", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 8, 53 })]
	public int LLS2
	{
		get
		{
			read((DataType)83);
			return _LLS[1];
		}
	}

	[AGInfo("Level by LLS 3", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 8, 53 })]
	public int LLS3
	{
		get
		{
			read((DataType)84);
			return _LLS[2];
		}
	}

	[AGInfo("Level by LLS 4", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 8, 53 })]
	public int LLS4
	{
		get
		{
			read((DataType)85);
			return _LLS[3];
		}
	}

	[AGInfo("Level by LLS 5", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 9, 53 })]
	public int LLS5
	{
		get
		{
			read((DataType)86);
			return _LLS[4];
		}
	}

	[AGInfo("Level by LLS 6", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 9, 53 })]
	public int LLS6
	{
		get
		{
			read((DataType)87);
			return _LLS[5];
		}
	}

	[AGInfo("Level by LLS 7", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 9, 53 })]
	public int LLS7
	{
		get
		{
			read((DataType)88);
			return _LLS[6];
		}
	}

	[AGInfo("Level by LLS 8", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 9, 53 })]
	public int LLS8
	{
		get
		{
			read((DataType)89);
			return _LLS[7];
		}
	}

	[AGInfo("Speed by CAN", "Instant speed in km/h by CAN bus indication.", "km/h", DeviceParameterKind.InstTime, AGInfoGroupType.Navigation | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public double CANSpeed
	{
		get
		{
			read(DataType.canWay);
			return _CANSpeed;
		}
	}

	[AGInfo("Cruise by CAN", "Cruise control flag by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public bool CANCruise
	{
		get
		{
			read((DataType)351);
			return _CANCruise;
		}
	}

	[AGInfo("Brake by CAN", "Brake pedal pressing flag by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public bool CANBrake
	{
		get
		{
			read((DataType)352);
			return _CANBrake;
		}
	}

	[AGInfo("Handbrake by CAN", "Handbrake flag by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public bool CANHandbrake
	{
		get
		{
			read((DataType)353);
			return _CANHandbrake;
		}
	}

	[AGInfo("Coupling by CAN", "Coupling pedal pressing flag by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public bool CANCoupling
	{
		get
		{
			read((DataType)354);
			return _CANCoupling;
		}
	}

	[AGInfo("Gaz by CAN", "Gas pedal position in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public double CANGaz
	{
		get
		{
			read((DataType)355);
			return _CANGaz;
		}
	}

	[AGInfo("Total fuel consumption by CAN (abs.)", "Total fuel consumption in liters by CAN bus indication (absolute).", "l", DeviceParameterKind.InstDist, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Consumption, false, new int[] { 10 })]
	public double CANFtotalAbs
	{
		get
		{
			read((DataType)356);
			return _CANFtotal;
		}
	}

	[AGInfo("Total fuel consumption by CAN", "Total fuel consumption in liters by CAN bus indication.", "l", DeviceParameterKind.InstDist, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Consumption, false, new int[] { 10 })]
	public double CANFtotal
	{
		get
		{
			read((DataType)356);
			return _CANFtotal;
		}
	}

	[AGInfo("Error of indication total fuel consumption by CAN", "Error of CAN bus indication total fuel consumption.", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public bool CANFtotalError
	{
		get
		{
			read((DataType)356);
			return _CANFtotalError;
		}
	}

	[AGInfo("Level by CAN 1", "Level in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.FuelLevel | AGInfoGroupType.CAN, false, new int[] { 11 })]
	public double CANL1
	{
		get
		{
			read(DataType.canLevels);
			return _CANL[0];
		}
	}

	[AGInfo("Level by CAN 2", "Level in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.FuelLevel | AGInfoGroupType.CAN, false, new int[] { 11 })]
	public double CANL2
	{
		get
		{
			read((DataType)358);
			return _CANL[1];
		}
	}

	[AGInfo("Level by CAN 3", "Level in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.FuelLevel | AGInfoGroupType.CAN, false, new int[] { 11 })]
	public double CANL3
	{
		get
		{
			read((DataType)359);
			return _CANL[2];
		}
	}

	[AGInfo("Level by CAN 4", "Level in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.FuelLevel | AGInfoGroupType.CAN, false, new int[] { 11 })]
	public double CANL4
	{
		get
		{
			read((DataType)360);
			return _CANL[3];
		}
	}

	[AGInfo("Level by CAN 5", "Level in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.FuelLevel | AGInfoGroupType.CAN, false, new int[] { 11 })]
	public double CANL5
	{
		get
		{
			read((DataType)361);
			return _CANL[4];
		}
	}

	[AGInfo("Level by CAN 6", "Level in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.FuelLevel | AGInfoGroupType.CAN, false, new int[] { 11 })]
	public double CANL6
	{
		get
		{
			read((DataType)362);
			return _CANL[5];
		}
	}

	[AGInfo("Level by CAN adBlue", "AdBlue level in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 11 })]
	public double CANLAB
	{
		get
		{
			read((DataType)363);
			return _CANLAB;
		}
	}

	[AGInfo("Engine RPM by CAN (raw)", "Engine rotate per minutes (raw) by CAN bus indication.", "rpm", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Rotation | AGInfoGroupType.CAN, false, new int[] { 12 })]
	public int CANErpmRaw
	{
		get
		{
			read(DataType.canEngine);
			return _CANErpmRaw;
		}
	}

	[AGInfo("Distance to maint by CAN", "Distance to maint in meters by CAN bus indication.", "m", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Distance, false, new int[] { 12 })]
	public int CANDmaint
	{
		get
		{
			read((DataType)365);
			return _CANDmaint;
		}
	}

	[AGInfo("Motohours by CAN (abs.)", "Motohours by CAN bus indication (absolute, with zero value filtration).", "h", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { 12 })]
	public double CANEmhAbs
	{
		get
		{
			read((DataType)366);
			return _CANEmh;
		}
	}

	[AGInfo("Motohours by CAN", "Motohours by CAN bus indication (with zero value filtration).", "h", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { 12 })]
	public double CANEmh
	{
		get
		{
			read((DataType)366);
			return _CANEmh;
		}
	}

	[AGInfo("Motohours by CAN-LOG (abs.)", "Motohours by CAN-LOG bus indication (absolute, without zero value filtration).", "h", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { 12 })]
	public double CANLOGEmhAbs
	{
		get
		{
			read((DataType)367);
			return _CANLOGEmh;
		}
	}

	[AGInfo("Motohours by CAN-LOG", "Motohours by CAN-LOG bus indication (without zero value filtration).", "h", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { 12 })]
	public double CANLOGEmh
	{
		get
		{
			read((DataType)367);
			return _CANLOGEmh;
		}
	}

	[AGInfo("Oil pressure by CAN", "Oil pressure in kPa by CAN bus indication.", "kPa", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Pressure | AGInfoGroupType.CAN, false, new int[] { 12 })]
	public int CANPoil
	{
		get
		{
			read((DataType)368);
			return _CANPoil;
		}
	}

	[AGInfo("Cooler temperature by CAN", "Cooler temperature in °C by CAN bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public int CANTcool
	{
		get
		{
			read(DataType.canTemperature);
			return _CANTcool;
		}
	}

	[AGInfo("Engine oil temperature by CAN", "Engine oil temperature в °C by CAN bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public double CANToil
	{
		get
		{
			read((DataType)370);
			return _CANToil;
		}
	}

	[AGInfo("Fuel temperature by CAN", "Fuel temperature in °C by CAN bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public int CANTfuel
	{
		get
		{
			read((DataType)371);
			return _CANTfuel;
		}
	}

	[AGInfo("Boost temperature by CAN", "Boost air temperature in °C by CAN bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public int CANTboost
	{
		get
		{
			read((DataType)373);
			return _CANTboost;
		}
	}

	[AGInfo("Manometer pressure by CAN", "Manometer pressure in kPa by CAN bus indication.", "kPa", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Pressure | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public int CANPman
	{
		get
		{
			read((DataType)372);
			return _CANPman;
		}
	}

	[AGInfo("Boost pressure by CAN", "Absolute boost pressure in kPa by CAN bus indication.", "kPa", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Pressure | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public int CANPboost
	{
		get
		{
			read((DataType)374);
			return _CANPboost;
		}
	}

	private long _CANDtotalUpper => _prm.CANDtotalUpper;

	private long _CANDtotalLower => _prm.CANDtotalLower;

	[AGInfo("Total distance by CAN (abs.)", "Total distance in meters by CAN bus indication (absolute).", "m", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.CAN | AGInfoGroupType.Distance, false, new int[] { 14 })]
	public long CANDtotalAbs
	{
		get
		{
			read(DataType.canDistance);
			return _CANDtotal;
		}
	}

	[AGInfo("Total distance by CAN UPPER (abs.)", "Total distance UPPER in meters by CAN bus indication (absolute).", "m", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.CAN | AGInfoGroupType.Distance, false, new int[] { 14 })]
	public long CANDtotalUpperAbs
	{
		get
		{
			read(DataType.canDistanceUpper);
			return _CANDtotalUpper;
		}
	}

	[AGInfo("Total distance by CAN LOWER (abs.)", "Total distance LOWER in meters by CAN bus indication (absolute).", "m", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.CAN | AGInfoGroupType.Distance, false, new int[] { 14 })]
	public long CANDtotalLowerAbs
	{
		get
		{
			read(DataType.canDistanceLower);
			return _CANDtotalLower;
		}
	}

	[AGInfo("Total distance by CAN", "Total distance in meters by CAN bus indication.", "m", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.CAN | AGInfoGroupType.Distance, false, new int[] { 14 })]
	public long CANDtotal
	{
		get
		{
			read(DataType.canDistance);
			return _CANDtotal;
		}
	}

	[AGInfo("Daily distance by CAN", "Daily distance in meters by CAN bus indication.", "m", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.CAN | AGInfoGroupType.Distance, false, new int[] { 14 })]
	public long CANDdaily
	{
		get
		{
			read((DataType)376);
			return _CANDdaily;
		}
	}

	private int _CameraNum => _prm.cameraNum;

	[AGInfo("Event ID", "Identifier of event. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 15 })]
	public int EventID
	{
		get
		{
			read(DataType.eventId);
			return _EventID;
		}
	}

	[AGInfo("Camera number", "Camera number.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { })]
	public int CameraNum
	{
		get
		{
			read(DataType.common);
			return _CameraNum;
		}
	}

	[AGInfo("Photo reason", "Photo reason: 0 - by time, 1 - by input, 2 - by request.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, new int[] { 15 })]
	public int PhotoReason => PhotoReasons(getFirstCameraIndex() + 1);

	[AGInfo("Photo universal time", "Photo universal time.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, new int[] { 15 })]
	public DateTime PhotoUDT => PhotoUDTs(getFirstCameraIndex() + 1);

	[AGInfo("Photo local time", "Photo local time.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, new int[] { 15 })]
	public DateTime PhotoDT => PhotoDTs(getFirstCameraIndex() + 1);

	[AGInfo("Photo image", "Photo image.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, new int[] { 15 })]
	public Image PhotoImage => PhotoImages(getFirstCameraIndex() + 1);

	[AGInfo("Device temperature", "Device temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, false, new int[] { 36, 37 })]
	public double DevT
	{
		get
		{
			read(DataType.devT);
			return _DevT;
		}
	}

	[AGInfo("Temperature by 1-wire 1", "Temperature in °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 36 })]
	public double Temper1
	{
		get
		{
			read(DataType.temperature);
			return _Temper[0];
		}
	}

	[AGInfo("Temperature by 1-wire 2", "Temperature in °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 36 })]
	public double Temper2
	{
		get
		{
			read((DataType)117);
			return _Temper[1];
		}
	}

	[AGInfo("Temperature by 1-wire 3", "Temperature in °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 36 })]
	public double Temper3
	{
		get
		{
			read((DataType)118);
			return _Temper[2];
		}
	}

	[AGInfo("Temperature by 1-wire 4", "Temperature in °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 36 })]
	public double Temper4
	{
		get
		{
			read((DataType)119);
			return _Temper[3];
		}
	}

	[AGInfo("Temperature by 1-wire 5", "Temperature in °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 37 })]
	public double Temper5
	{
		get
		{
			read((DataType)120);
			return _Temper[4];
		}
	}

	[AGInfo("Temperature by 1-wire 6", "Temperature in °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 37 })]
	public double Temper6
	{
		get
		{
			read((DataType)121);
			return _Temper[5];
		}
	}

	[AGInfo("Temperature by 1-wire 7", "Temperature in °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 37 })]
	public double Temper7
	{
		get
		{
			read((DataType)122);
			return _Temper[6];
		}
	}

	[AGInfo("Temperature by 1-wire 8", "Temperature in °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 37 })]
	public double Temper8
	{
		get
		{
			read((DataType)123);
			return _Temper[7];
		}
	}

	[AGInfo("Inputs extender - Positive inputs", "State of positive discrete inputs 1 … 8 of inputs extender. Input 1 - 0 bit, …, input 8 - 7 bit.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 38 })]
	public int InputsP
	{
		get
		{
			read(DataType.inputs);
			return _InputsP;
		}
	}

	[AGInfo("Inputs extender - Positive input 1", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP1 => (InputsP & 1) != 0;

	[AGInfo("Inputs extender - Positive input 2", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP2 => (InputsP & 2) != 0;

	[AGInfo("Inputs extender - Positive input 3", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP3 => (InputsP & 4) != 0;

	[AGInfo("Inputs extender - Positive input 4", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP4 => (InputsP & 8) != 0;

	[AGInfo("Inputs extender - Positive input 5", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP5 => (InputsP & 0x10) != 0;

	[AGInfo("Inputs extender - Positive input 6", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP6 => (InputsP & 0x20) != 0;

	[AGInfo("Inputs extender - Positive input 7", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP7 => (InputsP & 0x40) != 0;

	[AGInfo("Inputs extender - Positive input 8", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP8 => (InputsP & 0x80) != 0;

	[AGInfo("Inputs extender - Negative inputs", "State of negative discrete inputs 1 … 8 of inputs extender. Input 1 - 0 bit, …, input 8 - 7 bit.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 38 })]
	public int InputsM
	{
		get
		{
			read(DataType.inputs);
			return _InputsM;
		}
	}

	[AGInfo("Inputs extender - Negative input 1", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM1 => (InputsM & 1) != 0;

	[AGInfo("Inputs extender - Negative input 2", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM2 => (InputsM & 2) != 0;

	[AGInfo("Inputs extender - Negative input 3", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM3 => (InputsM & 4) != 0;

	[AGInfo("Inputs extender - Negative input 4", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM4 => (InputsM & 8) != 0;

	[AGInfo("Inputs extender - Negative input 5", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM5 => (InputsM & 0x10) != 0;

	[AGInfo("Inputs extender - Negative input 6", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM6 => (InputsM & 0x20) != 0;

	[AGInfo("Inputs extender - Negative input 7", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM7 => (InputsM & 0x40) != 0;

	[AGInfo("Inputs extender - Negative input 8", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM8 => (InputsM & 0x80) != 0;

	[AGInfo("Inputs extender - Status", "Inputs extender status.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 38 })]
	public int IStatus
	{
		get
		{
			read(DataType.inputs);
			return _IStatus;
		}
	}

	private double _FAAmount => _prm.faAmount;

	[AGInfo("Filling volume by flowmeter RS-485", "Filling volume by flowmeter RS-485 in liters.", "l", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.RS485 | AGInfoGroupType.AutoCistern, false, new int[] { 39, 41 })]
	public double FAAmount
	{
		get
		{
			read(DataType.fillAmount);
			return _FAAmount;
		}
	}

	[AGInfo("Card ID for amount", "Number of identification cards for fill amount entry. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 39 })]
	public int FAID
	{
		get
		{
			read((DataType)126);
			return _FAID;
		}
	}

	[AGInfo("Card ID by PORT-3/KUSS", "Card ID by PORT-3/KUSS. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485 | AGInfoGroupType.Identifier, false, new int[] { 39, 41 })]
	public int CardID
	{
		get
		{
			read((DataType)126, DataType.fillDuration);
			return _CardID;
		}
	}

	[AGInfo("Flowmeter channel", "Channel of flowmeter.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 40 })]
	public int FRChannel
	{
		get
		{
			read(DataType.fuelRate);
			return _FRChannel;
		}
	}

	[AGInfo("Flowmeter address", "Net address of flowmeter.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 40 })]
	public int FRAddr
	{
		get
		{
			read(DataType.fuelRate);
			return _FRAddr;
		}
	}

	[AGInfo("Total fuel consumption by flowmeter RS-485", "Total fuel consumption in liters for all the work flowmeter RS-485.", "l", DeviceParameterKind.InstDist, AGInfoGroupType.Data | AGInfoGroupType.RS485 | AGInfoGroupType.Consumption, false, new int[] { 40 })]
	public int FRTotal
	{
		get
		{
			read(DataType.fuelRate);
			return _FRTotal;
		}
	}

	[AGInfo("Filling duration by flowmeter RS-485", "Filling duration by flowmeter RS-485 in seconds.", "s", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 41 })]
	public int FDDuration
	{
		get
		{
			read(DataType.fillDuration);
			return _FDDuration;
		}
	}

	[AGInfo("Card ID for filling duration", "Number of identification cards for filling duration entry. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 41 })]
	public int FDID
	{
		get
		{
			read(DataType.fillDuration);
			return _FDID;
		}
	}

	[AGInfo("Passenger traffic channel", "Passenger traffic channel.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 42 })]
	public int PTChannel
	{
		get
		{
			read(DataType.ptChannel);
			return _PTChannel;
		}
	}

	[AGInfo("Measuring device channel", "Measuring device channel.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 43 })]
	public int MDChannel
	{
		get
		{
			read(DataType.mdChannel);
			return _MDChannel;
		}
	}

	[AGInfo("Failure mode identifiers by CAN", "Failure mode identifiers by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public int CANFMI
	{
		get
		{
			read(DataType.canErrors);
			return _CANFMI;
		}
	}

	[AGInfo("Suspect parameter number by CAN (v1)", "Suspect parameter number by CAN bus indication (version 1).", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public int CANSPNv1
	{
		get
		{
			read(DataType.canErrors);
			return _CANSPNv1;
		}
	}

	[AGInfo("Suspect parameter number by CAN (v2)", "Suspect parameter number by CAN bus indication (version 2).", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public int CANSPNv2
	{
		get
		{
			read(DataType.canErrors);
			return _CANSPNv2;
		}
	}

	[AGInfo("Suspect parameter number by CAN (v3, 4)", "Suspect parameter number by CAN bus indication (versions 3, 4).", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public int CANSPN
	{
		get
		{
			read(DataType.canErrors);
			return _CANSPN;
		}
	}

	[AGInfo("Error activity by CAN", "Error activity by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public bool CANErrorIsActive
	{
		get
		{
			read(DataType.canErrors);
			return _CANErrorIsActive;
		}
	}

	[AGInfo("Protect lamp status by CAN", "Protect lamp status by CAN.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public bool CANLampProtect
	{
		get
		{
			read((DataType)475);
			return _CANLampProtect;
		}
	}

	[AGInfo("Amber warning lamp status by CAN", "Amber warning lamp status by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public bool CANLampAmber
	{
		get
		{
			read((DataType)476);
			return _CANLampAmber;
		}
	}

	[AGInfo("Red stop lamp status by CAN", "Red stop lamp status by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public bool CANLampRed
	{
		get
		{
			read((DataType)477);
			return _CANLampRed;
		}
	}

	[AGInfo("Malfunction indicator lamp status by CAN", "Malfunction indicator lamp status by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public bool CANLampMalfunc
	{
		get
		{
			read((DataType)478);
			return _CANLampMalfunc;
		}
	}

	[AGInfo("Flash lamp status by CAN", "Flash lamp status by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public int CANFlashLampStatus
	{
		get
		{
			read((DataType)479);
			return _CANFlashLampStatus;
		}
	}

	[AGInfo("Error occurrence count by CAN", "Error occurrence count by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 44 })]
	public int CANOccurCount
	{
		get
		{
			read((DataType)480);
			return _CANOccurCount;
		}
	}

	[AGInfo("Instant fuel rate by CAN", "Instant fuel rate at the entry moment in l/h by CAN bus indication.", "l/h", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 45 })]
	public double CANFinstant
	{
		get
		{
			read(DataType.canCalcFuelRate);
			return _CANFinstant;
		}
	}

	[AGInfo("Calculated fuel consumption by CAN", "Calculated fule consumption in liters according to instant fuel rate from previous similar entry by CAN bus indication.", "l", DeviceParameterKind.InstDist, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Consumption, false, new int[] { 45 })]
	public double CANFcalc
	{
		get
		{
			read((DataType)482);
			return _CANFcalc;
		}
	}

	[AGInfo("Choker by CAN", "Choker position in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 45 })]
	public double CANChoker
	{
		get
		{
			read((DataType)483);
			return _CANChoker;
		}
	}

	[AGInfo("Torque percentage by CAN", "Current torque percentage by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public int CANTorquePercent
	{
		get
		{
			read(DataType.canMode);
			return _CANTorquePercent;
		}
	}

	[AGInfo("Friction percentage by CAN", "Nominal friction percentage by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public int CANFrictionPercent
	{
		get
		{
			read(DataType.canMode);
			return _CANFrictionPercent;
		}
	}

	[AGInfo("Torque mode by CAN", "Torque mode of engine by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public int CANTorqueMode
	{
		get
		{
			read(DataType.canMode);
			return _CANTorqueMode;
		}
	}

	[AGInfo("Idling mode by CAN", "Idling mode of accelerator by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public int CANIdlingMode
	{
		get
		{
			read(DataType.canMode);
			return _CANIdlingMode;
		}
	}

	[AGInfo("Kick-down mode by CAN", "Kick-down mode of accelerator by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public int CANKickDownMode
	{
		get
		{
			read(DataType.canMode);
			return _CANKickDownMode;
		}
	}

	[AGInfo("PTO state by CAN", "Power take-off box state by CAN bus indication: 0 - off, 1 - hold, 3 - standby, 5 - set, 6 - decelerate, 7 - resume, 8 - accelerate, 10 - speed I, 11 - speed II, 12 - speed III, 13 - speed IV.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public int CANPTOState
	{
		get
		{
			read(DataType.canMode);
			return _CANPTOState;
		}
	}

	[AGInfo("Cruise state by CAN", "Cruise control state by CAN bus indication: 0 - off, 1 - hold, 2 - accelerate, 3 - decelerate, 4 - resume, 5 - set, 6 - override.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public int CANCruiseState
	{
		get
		{
			read(DataType.canMode);
			return _CANCruiseState;
		}
	}

	[AGInfo("Battery voltage by CAN", "Battery voltage in V by CAN bus indication.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public double BatteryVolt
	{
		get
		{
			read((DataType)485);
			return _BatteryVolt;
		}
	}

	[AGInfo("Cruise control speed by CAN", "Cruise control speed in km/h by CAN bus indication.", "km/h", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 46 })]
	public double CANCruiseSpeed
	{
		get
		{
			read((DataType)486);
			return _CANCruiseSpeed;
		}
	}

	[AGInfo("Air temperature by CAN", "Air temperature in °C by CAN bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 47 })]
	public double CANTair
	{
		get
		{
			read(DataType.canEngineAux);
			return _CANTair;
		}
	}

	[AGInfo("Air pressure by CAN", "Air pressur in kPa by CAN bus indication.", "kPa", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Pressure | AGInfoGroupType.CAN, false, new int[] { 47 })]
	public double CANPair
	{
		get
		{
			read((DataType)488);
			return _CANPair;
		}
	}

	[AGInfo("Engine RPM by CAN", "Engine rotate per minutes (precise) by CAN bus indication.", "rpm", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Rotation | AGInfoGroupType.CAN, false, new int[] { 47 })]
	public double CANErpm
	{
		get
		{
			read((DataType)489);
			return _CANErpm;
		}
	}

	[AGInfo("Engine load by CAN", "Engine load in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 47 })]
	public int CANEload
	{
		get
		{
			read((DataType)490);
			return _CANEload;
		}
	}

	[AGInfo("Battery amperage by CAN", "Battery amperage in A by CAN bus indication.", "A", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 47 })]
	public int BatteryAmp
	{
		get
		{
			read((DataType)491);
			return _BatteryAmp;
		}
	}

	[AGInfo("Long data raw length", "Long data length from title in bytes.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48 })]
	public int LDRawLen
	{
		get
		{
			read(DataType.longEntryHeader);
			return _LDRawLen;
		}
	}

	[AGInfo("Long data entries number", "Entries number in long data package.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48 })]
	public int LDEnNum
	{
		get
		{
			read(DataType.longEntryHeader);
			return _LDEnCnt;
		}
	}

	[AGInfo("Long data type", "Long data type.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48 })]
	public int LDType
	{
		get
		{
			read(DataType.longEntryHeader);
			return _LDType;
		}
	}

	[AGInfo("Long data entry index", "Long data entry index.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48, 49 })]
	public int LDIndex
	{
		get
		{
			read(DataType.longEntryIndex);
			return _LDIndex;
		}
	}

	[AGInfo("Long data string", "Long data entry in string format (code page 1251).", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48, 49 })]
	public string LDString
	{
		get
		{
			read(DataType.longEntryData);
			return Encoding.GetEncoding(1251).GetString(_LDArr);
		}
	}

	[AGInfo("Long data (sms)", "Long data (sms)", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48, 49 })]
	public string LDSMS
	{
		get
		{
			read(DataType.longEntryData);
			if (_LDType != 16 || !_LDArr.Any())
			{
				return string.Empty;
			}
			PDUMessage val = PDUMessage.Parse(_LDArr);
			return $"{val.From} -> {val.SMSCAddress} ({val.TimeStamp})" + ((val.ReferenceID > 0) ? $" [{val.ReferenceID} => {val.MessagePartID + 1} of {val.MessageParts}]" : "") + ": " + val.Content.Replace('\n', ' ').Replace('\r', ' ');
		}
	}

	[AGInfo("Driver 1 by tachograph (Shtrih, VDO)", "Driver 1 by tachograph (Shtrih, VDO) from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, false, new int[] { 48, 49 })]
	public string LDDriver1Raw
	{
		get
		{
			read(DataType.longEntryDataTGD);
			return _LDDriver1;
		}
	}

	[AGInfo("Driver 2 by tachograph (Shtrih, VDO)", "Driver 2 by tachograph (Shtrih, VDO) from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, false, new int[] { 48, 49 })]
	public string LDDriver2Raw
	{
		get
		{
			read(DataType.longEntryDataTGD);
			return _LDDriver2;
		}
	}

	[AGInfo("Driver 1 by tachograph", "Driver 1 by tachograph from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, false, new int[] { 48, 49, 60 })]
	public string LDDriver1
	{
		get
		{
			read(DataType.longEntryDataTGD, DataType.tachograph);
			if (_DriverCard0 != 1)
			{
				return string.Empty;
			}
			return _LDDriver1;
		}
	}

	[AGInfo("Driver 2 by tachograph", "Driver 2 by tachograph from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, false, new int[] { 48, 49, 60 })]
	public string LDDriver2
	{
		get
		{
			read(DataType.longEntryDataTGD, DataType.tachograph);
			if (_DriverCard1 != 1)
			{
				return string.Empty;
			}
			return _LDDriver2;
		}
	}

	[AGInfo("Card 1 by tachograph (Shtrih, VDO)", "Card 1 by tachograph (Shtrih, VDO) from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, false, new int[] { 48, 49 })]
	public string LDCard1Raw
	{
		get
		{
			read(DataType.longEntryDataTGC);
			return _LDCard1;
		}
	}

	[AGInfo("Card 2 by tachograph (Shtrih, VDO)", "Card 2 by tachograph (Shtrih, VDO) from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, false, new int[] { 48, 49 })]
	public string LDCard2Raw
	{
		get
		{
			read(DataType.longEntryDataTGC);
			return _LDCard2;
		}
	}

	[AGInfo("Card 1 by tachograph", "Card 1 by tachograph from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, false, new int[] { 48, 49, 60 })]
	public string LDCard1
	{
		get
		{
			read(DataType.longEntryDataTGC, DataType.tachograph);
			if (_DriverCard0 != 1)
			{
				return string.Empty;
			}
			return _LDCard1;
		}
	}

	[AGInfo("Card 2 by tachograph", "Card 2 by tachograph from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, false, new int[] { 48, 49, 60 })]
	public string LDCard2
	{
		get
		{
			read(DataType.longEntryDataTGC, DataType.tachograph);
			if (_DriverCard1 != 1)
			{
				return string.Empty;
			}
			return _LDCard2;
		}
	}

	[AGInfo("Long data hex-string", "Long data entry represented in hex-string.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48, 49 })]
	public string LDHexStr
	{
		get
		{
			read(DataType.longEntryData);
			if (_LDArr.Length == 0)
			{
				return string.Empty;
			}
			return string.Join(" ", _LDArr.Select((byte b) => b.ToString("X2")));
		}
	}

	[AGInfo("Long data length", "Long data length in bytes.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48, 49 })]
	public int LDLen
	{
		get
		{
			read(DataType.longEntryData);
			return _LDArr.Length;
		}
	}

	[AGInfo("Palesse flags", "State of Palesse discrete flags.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 51 })]
	public long PALflags
	{
		get
		{
			read(DataType.palFlags);
			return _PALflags;
		}
	}

	[AGInfo("Palesse distance", "Palesse distance in meters.", "m", DeviceParameterKind.Accum, AGInfoGroupType.Navigation | AGInfoGroupType.Distance, false, new int[] { 52 })]
	public double PALdist => PALprmS(101);

	[AGInfo("Palesse square", "Palesse square in square meters.", "m²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 52 })]
	public double PALsquare => PALprmS(103);

	[AGInfo("Palesse motohours", "Palesse motohours.", "h", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Motohours, false, new int[] { 52 })]
	public double PALEmh => (double)PALprmS(109) / 10.0;

	[AGInfo("Сombine-harvester motohours", "Palesse combine-harvester motohours.", "h", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Motohours, false, new int[] { 52 })]
	public double PALCmh => (double)PALprmS(110) / 10.0;

	[AGInfo("Palesse statistic flags", "State of Palesse statistic discrete flags.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 52 })]
	public int PALstatFlags => PALprmS(43605);

	[AGInfo("Temperature by LLS 1", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS1
	{
		get
		{
			read(DataType.tlls);
			return _TLLS[0];
		}
	}

	[AGInfo("Temperature by LLS 2", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS2
	{
		get
		{
			read((DataType)343);
			return _TLLS[1];
		}
	}

	[AGInfo("Temperature by LLS 3", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS3
	{
		get
		{
			read((DataType)344);
			return _TLLS[2];
		}
	}

	[AGInfo("Temperature by LLS 4", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS4
	{
		get
		{
			read((DataType)345);
			return _TLLS[3];
		}
	}

	[AGInfo("Temperature by LLS 5", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS5
	{
		get
		{
			read((DataType)346);
			return _TLLS[4];
		}
	}

	[AGInfo("Temperature by LLS 6", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS6
	{
		get
		{
			read((DataType)347);
			return _TLLS[5];
		}
	}

	[AGInfo("Temperature by LLS 7", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS7
	{
		get
		{
			read((DataType)348);
			return _TLLS[6];
		}
	}

	[AGInfo("Temperature by LLS 8", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS8
	{
		get
		{
			read((DataType)349);
			return _TLLS[7];
		}
	}

	[AGInfo("ISOBUS type", "ISOBUS type.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 54 })]
	public int ISOBUSType
	{
		get
		{
			read(DataType.isobus);
			return _ISOBUSType;
		}
	}

	[AGInfo("ISOBUS error", "ISOBUS error.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 54 })]
	public bool ISOBUSError
	{
		get
		{
			read(DataType.isobus);
			return _ISOBUSError;
		}
	}

	[AGInfo("Guard state by CAN", "Guard state by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public int GuardState
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected I4, but got Unknown
			read(DataType.guard);
			return (int)_GuardState;
		}
	}

	[AGInfo("Guard flags by CAN", "Guard flags by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public int GuardFlags
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected I4, but got Unknown
			read(DataType.guard);
			return (int)_GuardFlags;
		}
	}

	[AGInfo("Guard indicators by CAN", "Guard indicators by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public int GuardIndicators
	{
		get
		{
			read(DataType.guard);
			return _GuardIndicators;
		}
	}

	[AGInfo("Driver door open by CAN", "Driver door is open by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool DriverDoorOpen => (GuardFlags & 1) == 1;

	[AGInfo("Passenger doors open by CAN", "Any passenger door is open by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool PassengerDoorsOpen => (GuardFlags & 2) == 2;

	[AGInfo("Luggage open by CAN", "Luggage is open by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool LuggageOpen => (GuardFlags & 4) == 4;

	[AGInfo("Hood open by CAN", "Hood is open by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool HoodOpen => (GuardFlags & 8) == 8;

	[AGInfo("Handbrake by CAN", "Handbrake is on by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool Handbrake => (GuardFlags & 0x10) == 16;

	[AGInfo("Footbrake by CAN", "Footbrake is on by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool Footbrake => (GuardFlags & 0x20) == 32;

	[AGInfo("Engine on by CAN", "Engine is on by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { 55 })]
	public bool EngineOn => (GuardFlags & 0x40) == 64;

	[AGInfo("Ignition on by CAN", "Ignition is on by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { 55 })]
	public bool IgnitionOn => (GuardFlags & 0x100) == 256;

	[AGInfo("Immobilizer on by CAN", "Immobilizer is on by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool ImmobilizerOn => (GuardFlags & 0x200) == 512;

	[AGInfo("Closed from trinket by CAN", "Closed from trinket by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool ClosedFromTrinket => (GuardFlags & 0x400) == 1024;

	[AGInfo("Key in ignition by CAN", "Key in ignition by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool KeyInIgnition => (GuardFlags & 0x800) == 2048;

	[AGInfo("Dynamic ignition on by CAN", "Dynamic ignition is on by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { 55 })]
	public bool DynamicIgnition => (GuardFlags & 0x1000) == 4096;

	[AGInfo("Driver belt by CAN", "Driver safety belt indicator by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool DriverBelt => (GuardIndicators & 0x80) != 0;

	[AGInfo("Passenger belt by CAN", "Passenger safety belt indicator by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool PassengerBelt => (GuardIndicators & 0x40) != 0;

	[AGInfo("EPS by CAN", "EPS indicator by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool EPS => (GuardIndicators & 0x800) != 0;

	[AGInfo("Check engine by CAN", "Check engine indicator by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool CheckEngine => (GuardIndicators & 0x10000) != 0;

	[AGInfo("Empty tank by CAN", "Empty tank indicator by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool EmptyTank => (GuardIndicators & 0x400000) != 0;

	[AGInfo("Airbag by CAN", "Airbag indicator by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 55 })]
	public bool Airbag => (GuardIndicators & 0x20000000) != 0;

	[AGInfo("Numerical data type", "Numerical data type.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public int NDType
	{
		get
		{
			read(DataType.numericalData);
			return _NDType;
		}
	}

	[AGInfo("Numerical data value", "Numerical data value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public int NDValue
	{
		get
		{
			read(DataType.numericalData);
			return _NDValue;
		}
	}

	[AGInfo("Numerical data unsigned value", "Numerical data unsigned value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public long NDValueUInt
	{
		get
		{
			read(DataType.numericalData);
			return (uint)_NDValue;
		}
	}

	private bool _NaviShiftIsOpen => _prm.shiftIsOpen;

	private string _NaviMapFileName => _prm.naviMapFileName;

	[AGInfo("Display status", "External display status. 0 if missing.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 59 })]
	public int DispStatus
	{
		get
		{
			read(DataType.dispStatus);
			return _DispStatus;
		}
	}

	[AGInfo("Navigator status", "External navigator status.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 59 })]
	public int NaviStatus
	{
		get
		{
			read(DataType.naviStatus);
			return _NaviStatus;
		}
	}

	[AGInfo("Navigator status switch on/off", "Navigator status switch on/off: 0 - on, 1 - off.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 59 })]
	public bool NaviIsSet
	{
		get
		{
			read(DataType.naviStatus);
			return _NaviIsSet;
		}
	}

	[AGInfo("Navigator status set", "External navigator status set.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 59 })]
	public int NaviSet
	{
		get
		{
			read(DataType.naviStatus);
			return _NaviSet;
		}
	}

	[AGInfo("Navigator status group", "External navigator status group.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 59 })]
	public int NaviGroup
	{
		get
		{
			read(DataType.naviStatus);
			return _NaviGroup;
		}
	}

	[AGInfo("Navigator status subgroup", "External navigator status subgroup.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 59 })]
	public int NaviSubgroup
	{
		get
		{
			read(DataType.naviStatus);
			return _NaviSubgroup;
		}
	}

	[AGInfo("Navigator shift is open", "External navigator shift is open.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Sensor, false, new int[] { 59 })]
	public bool NaviShiftIsOpen
	{
		get
		{
			read((DataType)839);
			return _NaviShiftIsOpen;
		}
	}

	[AGInfo("Map file name", "Map file name.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 0 })]
	public string NaviMapFileName
	{
		get
		{
			read(DataType.coordinatesRaw);
			return _NaviMapFileName;
		}
	}

	[AGInfo("Driver 1 working state", "Driver working state: 0 - rest / sleeping, 1 - driver available / short break, 2 - work (loading, unloading, working in an office), 3 - drive (behind wheel), 6 - error, 7 - not available.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int WorkingState1
	{
		get
		{
			read(DataType.tachograph);
			return _WorkingState0;
		}
	}

	[AGInfo("Driver 2 working state", "Driver working state: 0 - rest / sleeping, 1 - driver available / short break, 2 - work (loading, unloading, working in an office), 3 - drive (behind wheel), 6 - error, 7 - not available.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int WorkingState2
	{
		get
		{
			read(DataType.tachograph);
			return _WorkingState1;
		}
	}

	[AGInfo("Driver 1 related state", "Time related state: 0 - normal / no limits reached, 1 - 15 min before 4½ h, 2 - 4½ h reached, 3 - 15 min before 9 h, 4 - 9 h reached, 5 - 15 min before 16 h (not having 8h rest during the last 24h), 6 - 16 h reached, 13 - other, 14 - error, 15 - not available.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int RelatedState1
	{
		get
		{
			read(DataType.tachograph);
			return _RelatedState0;
		}
	}

	[AGInfo("Driver 2 related state", "Time related state: 0 - normal / no limits reached, 1 - 15 min before 4½ h, 2 - 4½ h reached, 3 - 15 min before 9 h, 4 - 9 h reached, 5 - 15 min before 16 h (not having 8h rest during the last 24h), 6 - 16 h reached, 13 - other, 14 - error, 15 - not available.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int RelatedState2
	{
		get
		{
			read(DataType.tachograph);
			return _RelatedState1;
		}
	}

	[AGInfo("Driver 1 card", "Driver card: 0 - not present, 1 - present, 2 - error, 3 - not available.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int DriverCard1
	{
		get
		{
			read(DataType.tachograph);
			return _DriverCard0;
		}
	}

	[AGInfo("Driver 2 card", "Driver card: 0 - not present, 1 - present, 2 - error, 3 - not available.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int DriverCard2
	{
		get
		{
			read(DataType.tachograph);
			return _DriverCard1;
		}
	}

	[AGInfo("Motion by tachograph", "Vehicle motion by tachograph: 0 - not detected, 1 - detected, 2 - error, 3 - not available.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int VehicleMotion
	{
		get
		{
			read(DataType.tachograph);
			return _VehicleMotion;
		}
	}

	[AGInfo("Overspeed by tachograph", "Vehicle overspeed by tachograph: 0 - not detected, 1 - detected, 2 - error, 3 - not available.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int VehicleOverspeed
	{
		get
		{
			read(DataType.tachograph);
			return _VehicleOverspeed;
		}
	}

	[AGInfo("Speed by tachograph", "Vehicle speed in km/h by tachograph.", "km/h", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 60 })]
	public int VehicleSpeed
	{
		get
		{
			read(DataType.tachograph);
			return _VehicleSpeed;
		}
	}

	[AGInfo("Shaft RPM by tachograph", "Shaft RPM by tachograph.", "rpm", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Rotation, false, new int[] { 60 })]
	public double ShaftRPM
	{
		get
		{
			read(DataType.tachograph);
			return _ShaftRPM;
		}
	}

	[AGInfo("Direction by tachograph", "Vehicle move direction by tachograph: 0 - forward, 1 - reverse, 2 - error, 3 - not available.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 60 })]
	public int MoveDirection
	{
		get
		{
			read(DataType.tachograph);
			return _MoveDirection;
		}
	}

	[AGInfo("Whell axis", "Whell axis.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 61 })]
	public int WheelAxis
	{
		get
		{
			read(DataType.wheelIndex);
			return _WheelAxis + 1;
		}
	}

	[AGInfo("Whell index", "Whell index.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 61 })]
	public int WheelIndex
	{
		get
		{
			read(DataType.wheelIndex);
			return _WheelIndex + 1;
		}
	}

	[AGInfo("Struna+ channel", "Struna+ channel.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public int StrunaChannel
	{
		get
		{
			read(DataType.struna);
			return _StrunaChannel;
		}
	}

	[AGInfo("Struna+ data type", "Struna+ data type.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public int StrunaDataType
	{
		get
		{
			read(DataType.struna);
			return _StrunaDataType;
		}
	}

	[AGInfo("Struna+ registry 1", "Struna+ registry 1.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public int StrunaRegistry1
	{
		get
		{
			read(DataType.struna);
			return _StrunaRegistry1;
		}
	}

	[AGInfo("Struna+ registry 2", "Struna+ registry 2.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public int StrunaRegistry2
	{
		get
		{
			read(DataType.struna);
			return _StrunaRegistry2;
		}
	}

	[AGInfo("Struna+ registry 3", "Struna+ registry 3.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public int StrunaRegistry3
	{
		get
		{
			read(DataType.struna);
			return _StrunaRegistry3;
		}
	}

	[AGInfo("Excess source", "Driving quality excess source: accelerometer internal = 0 / external = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Sensor, false, new int[] { 65 })]
	public bool DQExtAcc
	{
		get
		{
			read(DataType.drivingQuality);
			return _DQExtAcc;
		}
	}

	[AGInfo("Excess type", "Driving quality excess type: 0 - acceleration, 1 - braking, 2 - emergency braking, 3 - right turn, 4 - left turn, 5 - bump, 6 - tilt, 7 - overturn.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public byte DQType
	{
		get
		{
			read(DataType.drivingQuality);
			return _DQType;
		}
	}

	[AGInfo("Excess acceleration start/end", "Driving quality excess acceleration start = 0 / end = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public bool DQAccelEnd
	{
		get
		{
			read(DataType.drivingQualityType);
			bool[] dQEnd = _DQEnd;
			if (dQEnd == null)
			{
				return false;
			}
			return dQEnd[0];
		}
	}

	[AGInfo("Excess acceleration duration", "Driving quality excess acceleration duration.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public TimeSpan DQAccelDuration
	{
		get
		{
			read(DataType.drivingQualityType);
			TimeSpan[] dQDuration = _DQDuration;
			if (dQDuration == null)
			{
				return TimeSpan.Zero;
			}
			return dQDuration[0];
		}
	}

	[AGInfo("Excess acceleration maximum", "Driving quality excess acceleration maximum.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQAccelMax
	{
		get
		{
			read(DataType.drivingQualityType);
			double[] dQAccelMax = _DQAccelMax;
			if (dQAccelMax == null)
			{
				return 0.0;
			}
			return dQAccelMax[0];
		}
	}

	[AGInfo("Excess acceleration average", "Driving quality excess acceleration average.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQAccelAver
	{
		get
		{
			read(DataType.drivingQualityType);
			double[] dQAccelAver = _DQAccelAver;
			if (dQAccelAver == null)
			{
				return 0.0;
			}
			return dQAccelAver[0];
		}
	}

	[AGInfo("Excess braking start/end", "Driving quality excess braking start = 0 / end = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public bool DQBrakeEnd
	{
		get
		{
			read((DataType)1365);
			bool[] dQEnd = _DQEnd;
			if (dQEnd == null)
			{
				return false;
			}
			return dQEnd[1];
		}
	}

	[AGInfo("Excess braking duration", "Driving quality excess braking duration.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public TimeSpan DQBrakeDuration
	{
		get
		{
			read((DataType)1365);
			TimeSpan[] dQDuration = _DQDuration;
			if (dQDuration == null)
			{
				return TimeSpan.Zero;
			}
			return dQDuration[1];
		}
	}

	[AGInfo("Excess braking maximum", "Driving quality excess braking maximum.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQBrakeMax
	{
		get
		{
			read((DataType)1365);
			double[] dQAccelMax = _DQAccelMax;
			if (dQAccelMax == null)
			{
				return 0.0;
			}
			return dQAccelMax[1];
		}
	}

	[AGInfo("Excess braking average", "Driving quality excess braking average.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQBrakeAver
	{
		get
		{
			read((DataType)1365);
			double[] dQAccelAver = _DQAccelAver;
			if (dQAccelAver == null)
			{
				return 0.0;
			}
			return dQAccelAver[1];
		}
	}

	[AGInfo("Emergency braking start/end", "Driving quality emergency braking start = 0 / end = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public bool DQEmBrakeEnd
	{
		get
		{
			read((DataType)1366);
			bool[] dQEnd = _DQEnd;
			if (dQEnd == null)
			{
				return false;
			}
			return dQEnd[2];
		}
	}

	[AGInfo("Emergency braking duration", "Driving quality emergency braking duration.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public TimeSpan DQEmBrakeDuration
	{
		get
		{
			read((DataType)1366);
			TimeSpan[] dQDuration = _DQDuration;
			if (dQDuration == null)
			{
				return TimeSpan.Zero;
			}
			return dQDuration[2];
		}
	}

	[AGInfo("Emergency braking maximum", "Driving quality emergency braking maximum.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQEmBrakeMax
	{
		get
		{
			read((DataType)1366);
			double[] dQAccelMax = _DQAccelMax;
			if (dQAccelMax == null)
			{
				return 0.0;
			}
			return dQAccelMax[2];
		}
	}

	[AGInfo("Emergency braking average", "Driving quality emergency braking average.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQEmBrakeAver
	{
		get
		{
			read((DataType)1366);
			double[] dQAccelAver = _DQAccelAver;
			if (dQAccelAver == null)
			{
				return 0.0;
			}
			return dQAccelAver[2];
		}
	}

	[AGInfo("Excess right turn start/end", "Driving quality excess right turn start = 0 / end = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public bool DQRightEnd
	{
		get
		{
			read((DataType)1367);
			bool[] dQEnd = _DQEnd;
			if (dQEnd == null)
			{
				return false;
			}
			return dQEnd[3];
		}
	}

	[AGInfo("Excess right turn duration", "Driving quality excess right turn duration.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public TimeSpan DQRightDuration
	{
		get
		{
			read((DataType)1367);
			TimeSpan[] dQDuration = _DQDuration;
			if (dQDuration == null)
			{
				return TimeSpan.Zero;
			}
			return dQDuration[3];
		}
	}

	[AGInfo("Excess right turn maximum", "Driving quality excess right turn maximum.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQRightMax
	{
		get
		{
			read((DataType)1367);
			double[] dQAccelMax = _DQAccelMax;
			if (dQAccelMax == null)
			{
				return 0.0;
			}
			return dQAccelMax[3];
		}
	}

	[AGInfo("Excess right turn average", "Driving quality excess right turn average.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQRightAver
	{
		get
		{
			read((DataType)1367);
			double[] dQAccelAver = _DQAccelAver;
			if (dQAccelAver == null)
			{
				return 0.0;
			}
			return dQAccelAver[3];
		}
	}

	[AGInfo("Excess left turn start/end", "Driving quality excess left turn start = 0 / end = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public bool DQLeftEnd
	{
		get
		{
			read((DataType)1368);
			bool[] dQEnd = _DQEnd;
			if (dQEnd == null)
			{
				return false;
			}
			return dQEnd[4];
		}
	}

	[AGInfo("Excess left turn duration", "Driving quality excess left turn duration.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public TimeSpan DQLeftDuration
	{
		get
		{
			read((DataType)1368);
			TimeSpan[] dQDuration = _DQDuration;
			if (dQDuration == null)
			{
				return TimeSpan.Zero;
			}
			return dQDuration[4];
		}
	}

	[AGInfo("Excess left turn maximum", "Driving quality excess left turn maximum.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQLeftMax
	{
		get
		{
			read((DataType)1368);
			double[] dQAccelMax = _DQAccelMax;
			if (dQAccelMax == null)
			{
				return 0.0;
			}
			return dQAccelMax[4];
		}
	}

	[AGInfo("Excess left turn average", "Driving quality excess left turn average.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQLeftAver
	{
		get
		{
			read((DataType)1368);
			double[] dQAccelAver = _DQAccelAver;
			if (dQAccelAver == null)
			{
				return 0.0;
			}
			return dQAccelAver[4];
		}
	}

	[AGInfo("Excess bump start/end", "Driving quality excess bump start = 0 / end = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public bool DQBumpEnd
	{
		get
		{
			read((DataType)1369);
			bool[] dQEnd = _DQEnd;
			if (dQEnd == null)
			{
				return false;
			}
			return dQEnd[5];
		}
	}

	[AGInfo("Excess bump duration", "Driving quality excess bump duration.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public TimeSpan DQBumpDuration
	{
		get
		{
			read((DataType)1369);
			TimeSpan[] dQDuration = _DQDuration;
			if (dQDuration == null)
			{
				return TimeSpan.Zero;
			}
			return dQDuration[5];
		}
	}

	[AGInfo("Excess bump maximum", "Driving quality excess bump maximum.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQBumpMax
	{
		get
		{
			read((DataType)1369);
			double[] dQAccelMax = _DQAccelMax;
			if (dQAccelMax == null)
			{
				return 0.0;
			}
			return dQAccelMax[5];
		}
	}

	[AGInfo("Excess bump average", "Driving quality excess bump average.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQBumpAver
	{
		get
		{
			read((DataType)1369);
			double[] dQAccelAver = _DQAccelAver;
			if (dQAccelAver == null)
			{
				return 0.0;
			}
			return dQAccelAver[5];
		}
	}

	[AGInfo("Tilt start/end", "Driving quality tilt start = 0 / end = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public bool DQTiltEnd
	{
		get
		{
			read((DataType)1370);
			bool[] dQEnd = _DQEnd;
			if (dQEnd == null)
			{
				return false;
			}
			return dQEnd[6];
		}
	}

	[AGInfo("Tilt duration", "Driving quality tilt duration.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public TimeSpan DQTiltDuration
	{
		get
		{
			read((DataType)1370);
			TimeSpan[] dQDuration = _DQDuration;
			if (dQDuration == null)
			{
				return TimeSpan.Zero;
			}
			return dQDuration[6];
		}
	}

	[AGInfo("Tilt maximum", "Driving quality tilt maximum.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQTiltMax
	{
		get
		{
			read((DataType)1370);
			double[] dQAccelMax = _DQAccelMax;
			if (dQAccelMax == null)
			{
				return 0.0;
			}
			return dQAccelMax[6];
		}
	}

	[AGInfo("Tilt average", "Driving quality tilt average.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQTiltAver
	{
		get
		{
			read((DataType)1370);
			double[] dQAccelAver = _DQAccelAver;
			if (dQAccelAver == null)
			{
				return 0.0;
			}
			return dQAccelAver[6];
		}
	}

	[AGInfo("Overturn start/end", "Driving quality overturn start = 0 / end = 1.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public bool DQOverturnEnd
	{
		get
		{
			read((DataType)1371);
			bool[] dQEnd = _DQEnd;
			if (dQEnd == null)
			{
				return false;
			}
			return dQEnd[7];
		}
	}

	[AGInfo("Overturn duration", "Driving quality overturn duration.", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public TimeSpan DQOverturnDuration
	{
		get
		{
			read((DataType)1371);
			TimeSpan[] dQDuration = _DQDuration;
			if (dQDuration == null)
			{
				return TimeSpan.Zero;
			}
			return dQDuration[7];
		}
	}

	[AGInfo("Overturn maximum", "Driving quality overturn maximum.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQOverturnMax
	{
		get
		{
			read((DataType)1371);
			double[] dQAccelMax = _DQAccelMax;
			if (dQAccelMax == null)
			{
				return 0.0;
			}
			return dQAccelMax[7];
		}
	}

	[AGInfo("Overturn average", "Driving quality overturn average.", "m/s²", DeviceParameterKind.Accum, AGInfoGroupType.Data, false, new int[] { 65 })]
	public double DQOverturnAver
	{
		get
		{
			read((DataType)1371);
			double[] dQAccelAver = _DQAccelAver;
			if (dQAccelAver == null)
			{
				return 0.0;
			}
			return dQAccelAver[7];
		}
	}

	[AGInfo("TKAM channel", "Channel of TKAM.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 66, 74 })]
	public int TKAMChannel
	{
		get
		{
			read(DataType.tkamCH, DataType.tkam2CH);
			return _TKAMChannel;
		}
	}

	[AGInfo("WCS channel", "WCS channel.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 67 })]
	public byte WCSChannel
	{
		get
		{
			read(DataType.wcs);
			return _WCSChannel;
		}
	}

	[AGInfo("WCS input 0", "State of discrete WCS input 0.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 67 })]
	public bool WCSIn0
	{
		get
		{
			read(DataType.wcs);
			return _WCSIn0;
		}
	}

	[AGInfo("WCS status light", "WCS status light: 0 - all sections are off; 1 - 1st section is flashing; 2 - 1st section is on, 2nd section is flashing; 3 - 1 and 2 sections are on; 4 - all sections are on.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 67 })]
	public byte WCSStatusLight
	{
		get
		{
			read(DataType.wcs);
			return _WCSStatusLight;
		}
	}

	[AGInfo("WCS weight", "WCS weight.", "kg", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 67 })]
	public int WCSWeight
	{
		get
		{
			read(DataType.wcs);
			return _WCSWeight;
		}
	}

	[AGInfo("WCS frequency", "WCS frequency.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Frequency | AGInfoGroupType.RS485, false, new int[] { 67 })]
	public int WCSFrequency
	{
		get
		{
			read((DataType)1422);
			return _WCSFrequency;
		}
	}

	[AGInfo("WCS error bits", "WCS error bits.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 67 })]
	public int WCSErrorCode
	{
		get
		{
			read((DataType)1423);
			return _WCSErrorCode;
		}
	}

	[AGInfo("Coupler load by CAN", "Coupler load by CAN bus indication.", "kg", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 68 })]
	public double TWCouplerLoad
	{
		get
		{
			read(DataType.trailerWeight);
			return _TWCouplerLoad;
		}
	}

	[AGInfo("Load weight by CAN", "Load weight by CAN bus indication.", "kg", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 68 })]
	public int TWLoadWeight
	{
		get
		{
			read((DataType)1425);
			return _TWLoadWeight;
		}
	}

	[AGInfo("Trailer weight by CAN", "Trailer weight by CAN bus indication.", "kg", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 68 })]
	public int TWTrailerWeight
	{
		get
		{
			read((DataType)1426);
			return _TWTrailerWeight;
		}
	}

	[AGInfo("Braking circuit 1 pressure by CAN", "Pressure in the braking circuit 1 by CAN bus indication.", "kPa", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Pressure | AGInfoGroupType.CAN, false, new int[] { 70 })]
	public int VSBC1Pressure
	{
		get
		{
			read(DataType.vehicleStatus);
			return _VSBC1Pressure;
		}
	}

	[AGInfo("Braking circuit 2 pressure by CAN", "Pressure in the braking circuit 2 by CAN bus indication.", "kPa", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Pressure | AGInfoGroupType.CAN, false, new int[] { 70 })]
	public int VSBC2Pressure
	{
		get
		{
			read((DataType)1444);
			return _VSBC2Pressure;
		}
	}

	[AGInfo("Gear by CAN", "Gear by CAN bus indication.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 70 })]
	public int VSGear
	{
		get
		{
			read((DataType)1445);
			return _VSGear;
		}
	}

	[AGInfo("Total Weight by CAN", "Total Weight by CAN bus indication.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[] { 70 })]
	public int VSTotalWeight
	{
		get
		{
			read((DataType)1446);
			return _VSTotalWeight;
		}
	}

	[AGInfo("Total alternate fuel consumption by CAN (abs.)", "Total alternate fuel consumption in kg by CAN bus indication (absolute).", "kg", DeviceParameterKind.InstDist, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Consumption, false, new int[] { 71 })]
	public double CANAlternateFtotalAbs
	{
		get
		{
			read(DataType.canAlternate);
			return _CANAlternateFtotal;
		}
	}

	[AGInfo("Total alternate fuel consumption by CAN", "Total alternate fuel consumption in kg by CAN bus indication.", "kg", DeviceParameterKind.InstDist, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Consumption, false, new int[] { 71 })]
	public double CANAlternateFtotal
	{
		get
		{
			read(DataType.canAlternate);
			return _CANAlternateFtotal;
		}
	}

	[AGInfo("Isobus group", "Isobus group.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 73 })]
	public int ISOBUSGroup
	{
		get
		{
			read(DataType.discreteParams);
			return _ISOBUSGroup;
		}
	}

	[AGInfo("Isobus source", "Isobus source.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 73 })]
	public int ISOBUSSource
	{
		get
		{
			read(DataType.discreteParams);
			return _ISOBUSSource;
		}
	}

	[AGInfo("Named parameter name", "Named parameter name.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { 75, 76 })]
	public string NPName
	{
		get
		{
			read(DataType.namedParamData);
			return _NPName;
		}
	}

	[AGInfo("Named parameter data type", "Named parameter data type.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { 75, 76 })]
	public int NPType
	{
		get
		{
			read(DataType.namedParamData);
			return _NPType;
		}
	}

	[AGInfo("Named parameter unsigned integer", "Named parameter unsigned integer.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { 75, 76 })]
	public long NPUInt
	{
		get
		{
			read(DataType.namedParamUint);
			return _NPUInt;
		}
	}

	[AGInfo("Named parameter signed integer", "Named parameter signed integer.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { 75, 76 })]
	public int NPInt
	{
		get
		{
			read(DataType.namedParamInt);
			return _NPInt;
		}
	}

	[AGInfo("Named parameter float value", "Named parameter float value.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { 75, 76 })]
	public double NPFloat
	{
		get
		{
			read(DataType.namedParamFloat);
			return _NPFloat;
		}
	}

	[AGInfo("Door state by iQFreeze", "Door state by iQFreeze indication: false - close, true - open.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public bool IQFdr => CANBrake;

	[AGInfo("Motohours by iQFreeze", "Motohours by iQFreeze indication.", "h", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.CAN | AGInfoGroupType.Motohours, false, new int[] { 12 })]
	public double IQFhm => CANEmh;

	[AGInfo("Refrigerator temperature by iQFreeze", "Refrigerator temperature in °C by iQFreeze indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public double IQFmt => CANToil;

	[AGInfo("Set temperature by iQFreeze", "Set temperature in °C by iQFreeze indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public double IQFsp => CANTfuel;

	[AGInfo("Cooler temperature by iQFreeze", "Cooler temperature in °C by iQFreeze indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 13 })]
	public double IQFafzt => CANTcool;

	[AGInfo("Air temperature by iQFreeze", "Air temperature in °C by iQFreeze indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.CAN, false, new int[] { 47 })]
	public double IQFambt => CANTair;

	[AGInfo("Engine RPM by iQFreeze", "Engine rotate per minutes by iQFreeze indication.", "rpm", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Rotation | AGInfoGroupType.CAN, false, new int[] { 47 })]
	public double IQFrpm => CANErpm;

	[AGInfo("Compr. config. by iQFreeze", "Compressor configuration by iQFreeze indication.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 45 })]
	public int IQFconf => (int)CANFinstant;

	[AGInfo("State of iQFreeze", "State of iQFreeze.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 10 })]
	public int IQFstate => (int)CANSpeed;

	[AGInfo("Voltage by iQFreeze", "Voltage by iQFreeze.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 52 })]
	public double IQFbatv => BatteryVolt;

	[AGInfo("Amperage by iQFreeze", "Amperage by iQFreeze.", "A", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 52 })]
	public double IQFbata => BatteryAmp;

	[AGInfo("IQFrefsn", "IQFrefsn.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 48, 49 })]
	public string IQFrefsn => LDStr(0, 20, null, 65534);

	[AGInfo("IQFtrsn", "IQFtrsn.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 48, 49 })]
	public string IQFtrsn => LDStr(20, 13, null, 65534);

	[AGInfo("IQFsn", "IQFsn.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 48, 49 })]
	public string IQFsn => LDStr(33, 15, null, 65534);

	[AGInfo("IQFver", "IQFver.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 48, 49 })]
	public string IQFver => LDStr(48, 10, null, 65534);

	[AGInfo("IQFbtname", "IQFbtname.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 48, 49 })]
	public string IQFbtname => LDStr(58, 32, null, 65534);

	[AGInfo("IQFno_connect", "IQFno_connect.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.CAN, false, new int[] { 48, 49 })]
	public bool IQFno_connect => LDBits(720, 1, 0, 65534) == 0;

	[AGInfo("IQFalcount", "IQFalcount.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 48, 49 })]
	public int IQFalcount => LDByte(94, 1, 65534);

	[AGInfo("IQFcycleSentry", "IQFcycleSentry.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 45 })]
	public bool IQFcycleSentry => cycleSentrySet.Contains((int)CANFinstant);

	[AGInfo("IQFcontinuous", "IQFcontinuous.", DeviceParameterKind.Flag, AGInfoGroupType.CAN, false, new int[] { 45 })]
	public bool IQFcontinuous => continuousSet.Contains((int)CANFinstant);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_DQEnd()
	{
		_DQEnd = new bool[8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_DQDuration()
	{
		_DQDuration = new TimeSpan[8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_DQAccelMax()
	{
		_DQAccelMax = new double[8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_DQAccelAver()
	{
		_DQAccelAver = new double[8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_ISOBUSInt()
	{
		_ISOBUSInt = new int[isobusPrmDict.Count];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_ISOBUSFloat()
	{
		_ISOBUSFloat = new double[isobusPrmDict.Count];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_ISOBUSUInt()
	{
		_ISOBUSUInt = new int[isobusPrmDict.Count];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_NFC()
	{
		_NFC = new bool[9];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_IRMAStatusS()
	{
		_IRMAStatusS = new int[8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_IRMAStatusA()
	{
		_IRMAStatusA = new int[8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PALprmA()
	{
		_PALprmA = new int[anlgPALPrmDict.Count];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PALprmF()
	{
		_PALprmF = new int[freqPALPrmDict.Count];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PALprmS()
	{
		_PALprmS = new int[statPALprmDict.Count];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_FMS1Status()
	{
		_FMS1Status = new long[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_DiscreteParameters()
	{
		_DiscreteParameters = new long[8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMOuts()
	{
		_TKAMOuts = new byte[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMAngle()
	{
		_TKAMAngle = new double[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMTemperature()
	{
		_TKAMTemperature = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMEventState()
	{
		_TKAMEventState = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMVibration()
	{
		_TKAMVibration = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMRoulis()
	{
		_TKAMRoulis = new double[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMTangage()
	{
		_TKAMTangage = new double[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMVoltage()
	{
		_TKAMVoltage = new double[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_TKAMRSSI()
	{
		_TKAMRSSI = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_MDL()
	{
		_MDL = new double[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_MDStatus()
	{
		_MDStatus = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_MDMode()
	{
		_MDMode = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PTMDMode()
	{
		_PTMDMode = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PTMDStatus()
	{
		_PTMDStatus = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PTOut()
	{
		_PTOut = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PTIn()
	{
		_PTIn = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PTMode()
	{
		_PTMode = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PTStatus()
	{
		_PTStatus = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_AGFCVolume()
	{
		_AGFCVolume = new double[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_AGFCDuration()
	{
		_AGFCDuration = new int[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_AGFCRefuellerID()
	{
		_AGFCRefuellerID = new long[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_AGFCDriverID()
	{
		_AGFCDriverID = new long[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_Card()
	{
		_Card = new long[16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_MODBUSInt()
	{
		_MODBUSInt4321 = new int[128];
		_MODBUSInt3412 = new int[128];
		_MODBUSInt2143 = new int[128];
		_MODBUSInt1234 = new int[128];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_MODBUSFloat()
	{
		_MODBUSFloat4321 = new double[128];
		_MODBUSFloat3412 = new double[128];
		_MODBUSFloat2143 = new double[128];
		_MODBUSFloat1234 = new double[128];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_NDCANInt4321()
	{
		_NDCANInt4321 = new int[128];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_CANAW()
	{
		_CANAW = new double[16, 6];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_UDArrs()
	{
		_UDArrs = new byte[4][]
		{
			new byte[6],
			new byte[6],
			new byte[6],
			new byte[6]
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PTCtgIn()
	{
		_PTCtgIn = new int[16, 8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_PTCtgOut()
	{
		_PTCtgOut = new int[16, 8];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_WT()
	{
		_WT = new int[16, 16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_WP()
	{
		_WP = new double[16, 16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_WA()
	{
		_WA = new byte[16, 16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_StrunaReg1Arr()
	{
		_StrunaReg1Arr = new int[16, 16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_StrunaReg2Arr()
	{
		_StrunaReg2Arr = new int[16, 16];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void Initialize_StrunaReg3Arr()
	{
		_StrunaReg3Arr = new int[16, 16];
	}

	static CrdIndependentFields()
	{
		passInvTimeDTF = new bool[12]
		{
			true, true, false, false, false, false, true, true, true, true,
			false, false
		};
		passInvTimeTypes = new bool[81]
		{
			false, false, true, true, false, true, false, true, false, false,
			false, false, false, false, false, true, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, true, false, false, false, false, false, true, true,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, false, false, false, false, false,
			false, false, false, false, false, true, true, true, true, true,
			false
		};
		setRecordArrayLength = passInvTimeTypes.Length;
		MaxImageSize = new Size(16, 16);
		freqPALPrmDict = new Dictionary<int, int>
		{
			{ 0, 0 },
			{ 1, 1 },
			{ 2, 2 },
			{ 3, 3 },
			{ 4, 4 },
			{ 5, 5 },
			{ 7, 6 },
			{ 9, 7 },
			{ 10, 8 },
			{ 11, 9 },
			{ 13, 10 },
			{ 14, 11 },
			{ 15, 12 },
			{ 255, 13 }
		};
		anlgPALPrmDict = new Dictionary<int, int>
		{
			{ 0, 0 },
			{ 1, 1 },
			{ 2, 2 },
			{ 3, 3 },
			{ 5, 4 },
			{ 6, 5 },
			{ 7, 6 },
			{ 8, 7 },
			{ 9, 8 },
			{ 10, 9 }
		};
		statPALprmDict = new Dictionary<int, int>
		{
			{ 1, 0 },
			{ 2, 1 },
			{ 3, 2 },
			{ 4, 3 },
			{ 5, 4 },
			{ 6, 5 },
			{ 7, 6 },
			{ 8, 7 },
			{ 9, 8 },
			{ 10, 9 },
			{ 100, 10 },
			{ 101, 11 },
			{ 102, 12 },
			{ 103, 13 },
			{ 109, 14 },
			{ 110, 15 },
			{ 43605, 16 },
			{ 65535, 17 }
		};
		isobusPrmDict = new Dictionary<int, int>
		{
			{ 90, 0 },
			{ 99, 1 },
			{ 100, 2 },
			{ 101, 3 },
			{ 116, 4 },
			{ 119, 5 },
			{ 151, 6 },
			{ 65000, 7 },
			{ 65001, 8 }
		};
		cycleSentrySet = new HashSet<int>(new int[12]
		{
			1, 2, 5, 6, 9, 10, 13, 14, 17, 18,
			21, 22
		});
		continuousSet = new HashSet<int>(new int[16]
		{
			3, 4, 7, 8, 11, 12, 15, 16, 19, 20,
			23, 24, 25, 26, 27, 28
		});
		eventDataTypes = new BitArray(1513);
		eventDataTypes[60] = true;
		for (int i = 0; i < 5; i++)
		{
			eventDataTypes[61 + i] = true;
		}
		for (int j = 0; j < 16; j++)
		{
			eventDataTypes[66 + j] = true;
		}
		eventDataTypes[98] = true;
		eventDataTypes[126] = true;
		for (int k = 0; k < 16; k++)
		{
			eventDataTypes[99 + k] = true;
		}
		eventDataTypes[492] = true;
		eventDataTypes[493] = true;
		eventDataTypes[494] = true;
		eventDataTypes[512] = true;
		eventDataTypes[513] = true;
		for (int l = 0; l < 8; l++)
		{
			eventDataTypes[1427 + l] = true;
			eventDataTypes[514 + l] = true;
			eventDataTypes[522 + l] = true;
		}
		for (int m = 0; m < 16; m++)
		{
			eventDataTypes[496 + m] = true;
		}
		for (int n = 0; n < 8; n++)
		{
			eventDataTypes[1364 + n] = true;
		}
		eventDataTypes[1493] = true;
		eventDataTypes[1494] = true;
		eventDataTypes[1495] = true;
		eventDataTypes[1496] = true;
		eventDataTypes[1497] = true;
		eventDataTypes[1498] = true;
		eventDataTypes[1499] = true;
		eventDataTypes[1500] = true;
	}

	public CrdIndependentFields(ExpressionBaseInitInfo initInfo, ParameterInitInfo prmInitInfo, IDataViewerFormatters dataViewerFormatters)
		: base(initInfo, prmInitInfo, dataViewerFormatters)
	{
		summerNormStartDT = initInfo.summerNorm.startDateTime;
		summerNormEndDT = initInfo.summerNorm.endDateTime;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void read(DataType dataType)
	{
		anyChanges |= anyChangedDataTypes[(int)dataType];
		curChanges |= curChangedDataTypes[(int)dataType];
		evnChanges |= anyChangedDataTypes[(int)dataType] & eventDataTypes[(int)dataType];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void read(int numericType)
	{
		anyChanges |= anyChangedNumericTypes.Contains(numericType);
		curChanges |= curChangedNumericTypes.Contains(numericType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void read(string name)
	{
		anyChanges |= anyChangedNames.Contains(name);
		bool flag = curChangedNames.Contains(name);
		curChanges |= flag;
		evnChanges |= flag;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void read(DataType dataType1, DataType dataType2)
	{
		anyChanges |= anyChangedDataTypes[(int)dataType1] | anyChangedDataTypes[(int)dataType2];
		curChanges |= curChangedDataTypes[(int)dataType1] | curChangedDataTypes[(int)dataType2];
		evnChanges |= (anyChangedDataTypes[(int)dataType1] & eventDataTypes[(int)dataType1]) | (anyChangedDataTypes[(int)dataType2] & eventDataTypes[(int)dataType2]);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void write(DataType dataType)
	{
		AGBitArray aGBitArray = anyChangedDataTypes;
		bool value = (curChangedDataTypes[(int)dataType] = true);
		aGBitArray[(int)dataType] = value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void write(int numericType)
	{
		anyChangedNumericTypes.Add(numericType);
		curChangedNumericTypes.Add(numericType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void write(string name)
	{
		anyChangedNames.Add(name);
		curChangedNames.Add(name);
	}

	public void copyModifiedFlags(CrdIndependentFields f)
	{
		if (f != null)
		{
			anyChangedDataTypes = new AGBitArray(f.anyChangedDataTypes);
			curChangedDataTypes = new AGBitArray(f.curChangedDataTypes);
			anyChangedNumericTypes = new HashSet<int>(f.anyChangedNumericTypes);
			curChangedNumericTypes = new HashSet<int>(f.curChangedNumericTypes);
			anyChangedNames = new HashSet<string>(f.anyChangedNames);
			curChangedNames = new HashSet<string>(f.curChangedNames);
		}
		else
		{
			anyChangedDataTypes = new AGBitArray(1513);
			curChangedDataTypes = new AGBitArray(1513);
			anyChangedNumericTypes = new HashSet<int>();
			curChangedNumericTypes = new HashSet<int>();
			anyChangedNames = new HashSet<string>();
			curChangedNames = new HashSet<string>();
		}
	}

	public void fillUnupdatedValues(CrdIndependentFields prev)
	{
		if (prev != null)
		{
			if (!anyChangedDataTypes[356] && prev.anyChangedDataTypes[356])
			{
				_CANFtotal = prev._CANFtotal;
			}
			if (!anyChangedDataTypes[366] && prev.anyChangedDataTypes[366])
			{
				_CANEmh = prev._CANEmh;
			}
			if (!anyChangedDataTypes[367] && prev.anyChangedDataTypes[367])
			{
				_CANLOGEmh = prev._CANLOGEmh;
			}
			if (!anyChangedDataTypes[375] && prev.anyChangedDataTypes[375])
			{
				_CANDtotal = prev._CANDtotal;
			}
			if (!anyChangedDataTypes[376] && prev.anyChangedDataTypes[376])
			{
				_CANDdaily = prev._CANDdaily;
			}
			if (!anyChangedDataTypes[1447] && prev.anyChangedDataTypes[1447])
			{
				_CANAlternateFtotal = prev._CANAlternateFtotal;
			}
		}
	}

	public void combineModifiedFlags(CrdIndependentFields f)
	{
		if (f == null)
		{
			return;
		}
		anyChanges |= f.anyChanges;
		curChanges |= f.curChanges;
		evnChanges |= f.evnChanges;
		anyChangedDataTypes.Or(f.anyChangedDataTypes);
		curChangedDataTypes.Or(f.curChangedDataTypes);
		foreach (int anyChangedNumericType in f.anyChangedNumericTypes)
		{
			anyChangedNumericTypes.Add(anyChangedNumericType);
		}
		foreach (int curChangedNumericType in f.curChangedNumericTypes)
		{
			curChangedNumericTypes.Add(curChangedNumericType);
		}
		foreach (string anyChangedName in f.anyChangedNames)
		{
			anyChangedNames.Add(anyChangedName);
		}
		foreach (string curChangedName in f.curChangedNames)
		{
			curChangedNames.Add(curChangedName);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void resetModifiedFlags()
	{
		anyChanges = (curChanges = (evnChanges = false));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void resetCurChangedDataTypes()
	{
		curChangedDataTypes.SetFalse();
		curChangedNumericTypes.Clear();
		curChangedNames.Clear();
	}

	public override void setRead()
	{
		pr = true;
	}

	public override void setTimedRead()
	{
		read(DataType.common);
	}

	private DateTime correctedDateTime(int year, int month, int day, int hour, int minute, int second)
	{
		if (day > DateTime.DaysInMonth(year, month))
		{
			month++;
			day = 1;
			hour = (minute = (second = 0));
		}
		return new DateTime(year, month, day, hour, minute, second);
	}

	[AGInfo("Input by index", "State of discrete input.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool I([AGPrmInfo("Index of input", 1.0, 9.0)] int index)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (1 <= index && index <= 8)
		{
			InputFlags val = (InputFlags)(1 << index - 1);
			return (InputFlags)(Flags & val) == val;
		}
		if (index != 9)
		{
			return false;
		}
		return (Flags & 0x200000) == 2097152;
	}

	[AGInfo("Pulses presence on input by index", "Pulses presence on input.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.Motohours, false, new int[] { })]
	public bool IC([AGPrmInfo("Index of input", 1.0, 9.0)] int index)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (1 <= index && index <= 8)
		{
			InputFlags val = (InputFlags)(1 << index - 1);
			return (InputFlags)(Flags & val) == val;
		}
		if (index != 9)
		{
			return false;
		}
		return (Flags & 0x200000) == 2097152;
	}

	public virtual void setIndexAndUDT(int index, int dri)
	{
		_prm.SetIndex(index, dri);
		_UDT = _prm.udt;
	}

	public virtual void setRecord(bool setTypedData)
	{
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		int cameraNum = _prm.cameraNum;
		if (cameraNum != 0)
		{
			int num = 0;
			int num2 = 1;
			while (num < 16)
			{
				if ((cameraNum & num2) != 0)
				{
					write((DataType)(99 + num));
				}
				num++;
				num2 <<= 1;
			}
		}
		if (_prm.setShiftIsOpen)
		{
			write((DataType)839);
		}
		if (_prm.setAmountByFDRecord)
		{
			write(DataType.fillAmount);
		}
		write(DataType.typeId);
		if (_prm.trueType)
		{
			if (_prm.canSetRecord)
			{
				if (_TypeID == 0)
				{
					_PrevPSE = _prm.prevPSE;
					_NextPSE = _prm.nextPSE;
				}
				if (setTypedData)
				{
					switch (_TypeID)
					{
					case 0:
						setMainRecord();
						break;
					case 1:
						setAnalogRecord();
						break;
					case 2:
						setCounterRecord();
						break;
					case 3:
						setCounterRecord();
						break;
					case 4:
						setMotionRecord();
						break;
					case 5:
						setCounterRecord();
						break;
					case 6:
						setID1WRecord();
						break;
					case 7:
						setCounterRecord();
						break;
					case 8:
						setLLSRecord();
						break;
					case 9:
						setLLSRecord();
						break;
					case 10:
						setCANWayRecord();
						break;
					case 11:
						setCANLevelsRecord();
						break;
					case 12:
						setCANEngineRecord();
						break;
					case 13:
						setCANTemperatureRecord();
						break;
					case 14:
						setCANDistanceRecord();
						break;
					case 15:
						setEventIDRecord();
						break;
					case 16:
						setCANAxisRecord();
						break;
					case 17:
						setCANAxisRecord();
						break;
					case 18:
						setCANAxisRecord();
						break;
					case 19:
						setCANAxisRecord();
						break;
					case 20:
						setCANAxisRecord();
						break;
					case 21:
						setCANAxisRecord();
						break;
					case 22:
						setCANAxisRecord();
						break;
					case 23:
						setCANAxisRecord();
						break;
					case 24:
						setCANAxisRecord();
						break;
					case 25:
						setCANAxisRecord();
						break;
					case 26:
						setCANAxisRecord();
						break;
					case 27:
						setCANAxisRecord();
						break;
					case 28:
						setCANAxisRecord();
						break;
					case 29:
						setCANAxisRecord();
						break;
					case 30:
						setCANAxisRecord();
						break;
					case 31:
						setCANAxisRecord();
						break;
					case 32:
						setCANUserData();
						break;
					case 33:
						setCANUserData();
						break;
					case 34:
						setCANUserData();
						break;
					case 35:
						setCANUserData();
						break;
					case 36:
						setTempSensorRecord();
						break;
					case 37:
						setTempSensorRecord();
						break;
					case 38:
						setInputExtRecord();
						break;
					case 39:
						setFillAmountRecord();
						break;
					case 40:
						setFuelRateRecord();
						break;
					case 41:
						setFillDurationRecord();
						break;
					case 42:
						setRS485_PeopleCountRecord();
						break;
					case 43:
						setRS485_WeightCountRecord();
						break;
					case 44:
						setCANErrorRecord();
						break;
					case 45:
						setCANCalcFuelRateRecord();
						break;
					case 46:
						setCANModeRecord();
						break;
					case 47:
						setCANEngineAuxRecord();
						break;
					case 48:
						setLongEntryHeader();
						break;
					case 49:
						setLongEntryData();
						break;
					case 50:
						setPALParametersRecord();
						break;
					case 51:
						setPALFlagsRecord();
						break;
					case 52:
						setPALStatisticRecord();
						break;
					case 53:
						setLLSExtRecord();
						break;
					case 54:
						setISOBUSRecord();
						break;
					case 55:
						setGuardRecord();
						break;
					case 56:
						setNull();
						break;
					case 57:
						setNumericalDataRecord();
						break;
					case 58:
						setNull();
						break;
					case 59:
						setDisplayStatusRecord();
						break;
					case 60:
						setTachographRecord();
						break;
					case 61:
						setWheelStateRecord();
						break;
					case 62:
						setLS1ExtRecord();
						break;
					case 63:
						setLS2ExtRecord();
						break;
					case 64:
						setStrunaDataRecord();
						break;
					case 65:
						setDrivingQualityDataRecord();
						break;
					case 66:
						setTKAMRecord();
						break;
					case 67:
						setWCSRecord();
						break;
					case 68:
						setTrailerWeightRecord();
						break;
					case 69:
						setDoorsStatusesRecord();
						break;
					case 70:
						setVehicleStatusdRecord();
						break;
					case 71:
						setAlternateFuelRecord();
						break;
					case 72:
						setFMS1Record();
						break;
					case 73:
						setDiscreteParametersRecord();
						break;
					case 74:
						setTKAM2Record();
						break;
					case 75:
						setNamedParamHeader();
						break;
					case 76:
						setNamedParamData();
						break;
					case 77:
						setNamedArrayHeader();
						break;
					case 78:
						setNamedArraySubhead();
						break;
					case 79:
						setNamedArrayData();
						break;
					case 80:
						setCANLitersRecord();
						break;
					}
				}
			}
			write(DataType.common);
			switch (_TypeID)
			{
			case 0:
				write(DataType.flgAndInp12);
				write(DataType.inputs36);
				break;
			case 4:
				write(DataType.inputs36);
				break;
			default:
				write(DataType.flgAndInp12);
				break;
			}
		}
		if ((_prm.prevFlags & 0xFF) == (_prm.currFlags & 0xFF))
		{
			return;
		}
		int num3 = 0;
		int num4 = 1;
		while (num3 < 8)
		{
			if ((long)(((ulong)_prm.currFlags & (ulong)num4) - ((ulong)_prm.prevFlags & (ulong)num4)) > 0L && !curChangedDataTypes[8 + num3])
			{
				_C[num3] = 0;
				write((DataType)(8 + num3));
			}
			num3++;
			num4 <<= 1;
		}
	}

	private void setNull()
	{
	}

	private void setMainRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected I4, but got Unknown
		DeviceMainRecord val = (DeviceMainRecord)_prm.rec;
		_SRaw = ((((DeviceRecordLite)val).HDOP > 0) ? (8 - ((DeviceRecordLite)val).HDOP) : ((DeviceRecordLite)val).HDOP);
		_IntRcv = val.IsCoordInternal;
		_Src = (byte)(int)val.Source;
		_LonRaw = val.Longitude;
		_LatRaw = val.Latitude;
		write(DataType.coordinatesRaw);
	}

	[AGInfo("Output by index", "State of discrete output.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { 1 })]
	public bool Out([AGPrmInfo("Index of output", 1.0, 4.0)] int index)
	{
		if (index < 1 || index > 4)
		{
			return false;
		}
		bool result = index switch
		{
			1 => (_Outputs & 4) != 0, 
			2 => (_Outputs & 8) != 0, 
			3 => (_Outputs & 1) != 0, 
			4 => (_Outputs & 2) != 0, 
			_ => false, 
		};
		read(DataType.analogData);
		return result;
	}

	[AGInfo("Analog data by index, ADC", "Analog data voltage in ADC samples.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure, false, new int[] { 1 })]
	public int A([AGPrmInfo("Index of input", 1.0, 2.0)] int index)
	{
		if (index < 1 || index > 2)
		{
			return 0;
		}
		read(DataType.analogData);
		if (index != 1)
		{
			return _A1;
		}
		return _A0;
	}

	[AGInfo("Analog data by index, voltage", "Analog data voltage. 0 if input isn't in voltage mode.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure, true, new int[] { 1, 2, 3, 5, 7 })]
	public double AVolt([AGPrmInfo("Index of input", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0.0;
		}
		int num = index - 1;
		read((DataType)(38 + num));
		return _AVolt[num];
	}

	private void setAnalogRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		DeviceAnalogRecord val = (DeviceAnalogRecord)_prm.rec;
		AnalogItem[] values = val.Values;
		for (int i = 0; i < values.Length; i++)
		{
			AnalogItem val2 = values[i];
			int num = val2.ID - 1;
			if (num == 0)
			{
				_A0 = val2.Raw;
			}
			else
			{
				_A1 = val2.Raw;
			}
			if (val2.Value.HasValue && !_prm.CounterAsVoltage[num])
			{
				_AVolt[num] = val2.Value.Value;
				write((DataType)(38 + num));
			}
		}
		_Outputs = val.Outputs;
		_MainVoltRaw = val.InternalVoltageRaw;
		_MainVolt = val.InternalVoltage;
		_ResVoltRaw = val.ReservVoltageRaw;
		_ResVolt = val.ReservVoltage;
		_Processor = val.Processor;
		write(DataType.analogData);
	}

	[AGInfo("Counter value by input by index is valid", "Counter value by input is valid. 0 if it's the first counter entry after power on.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor, false, new int[] { 2, 3, 5, 7, 57 })]
	public bool NFC([AGPrmInfo("Index of input", 1.0, 9.0)] int index)
	{
		if (index < 1 || index > 9)
		{
			return false;
		}
		int num = index - 1;
		read((DataType)(48 + num));
		if (_NFC == null)
		{
			return false;
		}
		return _NFC[num];
	}

	[AGInfo("Continuous counter by index", "Number of front edges counted from previous similar entry. 0 if input isn't in continuous counter mode.", DeviceParameterKind.Accum, AGInfoGroupType.Counter | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 2, 3, 5, 7, 57 })]
	public int C([AGPrmInfo("Index of input", 1.0, 9.0)] int index)
	{
		if (index < 1 || index > 9)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(8 + num));
		return _C[num];
	}

	[AGInfo("Periodic counter by input by index", "Pulse number by input in last completed pulse packet. 0 if input isn't in periodic counter mode.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel, true, new int[] { 2, 3, 5, 7, 57 })]
	public int P([AGPrmInfo("Index of input", 1.0, 9.0)] int index)
	{
		if (index < 1 || index > 9)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(18 + num));
		return _P[num];
	}

	[AGInfo("Temperature by periodic counter of input by index", "Temperature in в °C by periodic counter of input.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, true, new int[] { 2, 3, 5, 7, 57 })]
	public int TP([AGPrmInfo("Index of input", 1.0, 9.0)] int index)
	{
		if (index < 1 || index > 9)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(18 + num));
		return _P[num] - 60;
	}

	[AGInfo("Frequency by input by index", "Average frequency in Hz by input for period of time from previous similar entry. 0 if input isn't in frequency mode.", "Hz", DeviceParameterKind.Accum, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency, true, new int[] { 2, 3, 5, 7, 57 })]
	public double F([AGPrmInfo("Index of input", 1.0, 9.0)] int index)
	{
		if (index < 1 || index > 9)
		{
			return 0.0;
		}
		int num = index - 1;
		read((DataType)(28 + num));
		return _F[num];
	}

	private void setCounterRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Invalid comparison between Unknown and I4
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Invalid comparison between Unknown and I4
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Invalid comparison between Unknown and I4
		CounterItem[] values = ((DeviceCounterRecord)_prm.rec).Values;
		foreach (CounterItem val in values)
		{
			int num = val.ID - 1;
			if (_NFC == null)
			{
				Initialize_NFC();
			}
			_NFC[num] = val.IsntFirst;
			write((DataType)(48 + num));
			if ((int)val.CounterType == 0 || (int)val.CounterType == 1)
			{
				_C[num] = val.Value;
				write((DataType)(8 + num));
			}
			if ((int)val.CounterType == 0 || (int)val.CounterType == 2)
			{
				_P[num] = val.Value >> 1;
				write((DataType)(18 + num));
			}
			if ((int)val.CounterType == 0 || (int)val.CounterType == 3)
			{
				write((DataType)(28 + num));
			}
			if ((int)val.CounterType == 4)
			{
				_AVolt[num] = (double)val.Value / 1000.0;
				write((DataType)(38 + num));
			}
		}
	}

	private void setMotionRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceMotionRecord val = (DeviceMotionRecord)_prm.rec;
		_SpeedRaw = val.SpeedRaw;
		_CourseRaw = val.AzimuthRaw;
		_Sats = val.Sattelites;
		_HDOP = val.xHDOP;
		write(DataType.motion);
		if (val.Altitude.HasValue)
		{
			_AltRaw = val.Altitude.Value;
			write(DataType.altitude);
		}
	}

	[AGInfo("Driver by 1-wire ID", "1-wire driver identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Driver, false, new int[] { 6 })]
	public Guid driverIDby1W([AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		Guid guid = driverByID(_ID1W, skipUnknown);
		if (!skipUnknown || _ID1W == 0L || guid != Guid.Empty)
		{
			read(DataType.id1w);
		}
		return guid;
	}

	[AGInfo("Implement by 1-wire ID", "1-wire implement identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Implement, false, new int[] { 6 })]
	public Guid implementIDby1W([AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		Guid guid = implementByID(_ID1W, skipUnknown);
		if (!skipUnknown || _ID1W == 0L || guid != Guid.Empty)
		{
			read(DataType.id1w);
		}
		return guid;
	}

	[AGInfo("Card ID by card reader", "Card ID by card reader. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485 | AGInfoGroupType.Identifier, false, new int[] { 6 })]
	public long Card([AGPrmInfo("Index of card reader", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0L;
		}
		int num = index - 1;
		read((DataType)(66 + num));
		if (_Card == null)
		{
			return 0L;
		}
		return _Card[num];
	}

	[AGInfo("Driver by Card by index", "Card driver identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Driver, false, new int[] { 6 })]
	public Guid driverIDbyCard([AGPrmInfo("Index of card reader", 1.0, 16.0)] int index, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		if (index < 1 || index > 16)
		{
			return Guid.Empty;
		}
		int num = index - 1;
		long num2 = 0L;
		if (_Card != null)
		{
			num2 = _Card[num];
		}
		Guid guid = driverByID(num2, skipUnknown);
		if (!skipUnknown || num2 == 0L || guid != Guid.Empty)
		{
			read((DataType)(66 + num));
		}
		return guid;
	}

	[AGInfo("Implement by Card by index", "Card implement identifier.", DeviceParameterKind.Flag, AGInfoGroupType.Wire1 | AGInfoGroupType.Identifier | AGInfoGroupType.Implement, false, new int[] { 6 })]
	public Guid implementIDbyCard([AGPrmInfo("Index of card reader", 1.0, 16.0)] int index, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		if (index < 1 || index > 16)
		{
			return Guid.Empty;
		}
		int num = index - 1;
		long num2 = 0L;
		if (_Card != null)
		{
			num2 = _Card[num];
		}
		Guid guid = implementByID(num2, skipUnknown);
		if (!skipUnknown || num2 == 0L || guid != Guid.Empty)
		{
			read((DataType)(66 + num));
		}
		return guid;
	}

	private void setID1WRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceIButtonRecord val = (DeviceIButtonRecord)_prm.rec;
		_IDType = val.Type;
		write(DataType.idType);
		switch (val.Type)
		{
		case 0:
			_IDButton = (_ID1W = val.Value);
			write(DataType.id1w);
			write((DataType)62);
			break;
		case 1:
			_IDBLE = (_ID1W = val.Value);
			write(DataType.id1w);
			write((DataType)63);
			break;
		case 2:
			_IDCAN = (_ID1W = val.Value);
			write(DataType.id1w);
			write((DataType)64);
			break;
		case 3:
			_IDSN = (_ID1W = val.Value);
			write(DataType.id1w);
			write((DataType)65);
			break;
		case 13:
		case 14:
		case 15:
		{
			if (_Card == null)
			{
				Initialize_Card();
			}
			int num = val.Number - 1;
			_Card[num] = val.Value;
			write((DataType)(66 + num));
			break;
		}
		}
	}

	[AGInfo("Level by LLS by index", "LLS level in ADC samples from 0 to 4095.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 8, 9, 53 })]
	public int LLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(82 + num));
		return _LLS[num];
	}

	[AGInfo("Driver by LLS by index", "LLS driver identifier.", DeviceParameterKind.Flag, AGInfoGroupType.RS485 | AGInfoGroupType.Identifier | AGInfoGroupType.Driver, false, new int[] { 8, 9, 53 })]
	public Guid driverIDbyLLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		if (index < 1 || index > 8)
		{
			return Guid.Empty;
		}
		int num = index - 1;
		Guid guid = driverByID(_LLS[num], skipUnknown);
		if (!skipUnknown || _LLS[num] == 0 || guid != Guid.Empty)
		{
			read((DataType)(1427 + num));
		}
		return guid;
	}

	[AGInfo("Implement by LLS by index", "LLS implement identifier.", DeviceParameterKind.Flag, AGInfoGroupType.RS485 | AGInfoGroupType.Identifier | AGInfoGroupType.Implement, false, new int[] { 8, 9, 53 })]
	public Guid implementIDbyLLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown)
	{
		if (index < 1 || index > 8)
		{
			return Guid.Empty;
		}
		int num = index - 1;
		Guid guid = implementByID(_LLS[num], skipUnknown);
		if (!skipUnknown || _LLS[num] == 0 || guid != Guid.Empty)
		{
			read((DataType)(1427 + num));
		}
		return guid;
	}

	[AGInfo("LLS level by index is valid", "LLS level is valid.", DeviceParameterKind.InstTime, AGInfoGroupType.Sensor | AGInfoGroupType.RS485, false, new int[] { 8, 9, 53, 62 })]
	public bool VLLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return false;
		}
		int num = index - 1;
		read((DataType)(90 + num));
		return _VLLS[num];
	}

	private void setLLSRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		LLSItem[] values = ((DeviceLLSRecord)_prm.rec).Values;
		foreach (LLSItem val in values)
		{
			int num = val.ID - 1;
			_VLLS[num] = val.Valid;
			write((DataType)(90 + num));
			if (val.Valid)
			{
				_LLS[num] = val.Value;
				write((DataType)(82 + num));
				write((DataType)(1427 + num));
			}
		}
	}

	private void setCANWayRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCAN1Record val = (DeviceCAN1Record)_prm.rec;
		if (val.CANSpeed.HasValue)
		{
			_CANSpeed = val.CANSpeed.Value;
			write(DataType.canWay);
		}
		if (val.Cruise.HasValue)
		{
			_CANCruise = val.Cruise.Value;
			write((DataType)351);
		}
		if (val.PedalStop.HasValue)
		{
			_CANBrake = val.PedalStop.Value;
			write((DataType)352);
		}
		if (val.PedalParking.HasValue)
		{
			_CANHandbrake = val.PedalParking.Value;
			write((DataType)353);
		}
		if (val.PedalCoupling.HasValue)
		{
			_CANCoupling = val.PedalCoupling.Value;
			write((DataType)354);
		}
		if (val.PedalEngine.HasValue)
		{
			_CANGaz = val.PedalEngine.Value;
			write((DataType)355);
		}
		if (val.Fuel.HasValue)
		{
			_CANFtotal = val.Fuel.Value;
			_CANFtotalError = val.FuelError;
			write((DataType)356);
		}
	}

	[AGInfo("Level by CAN by index", "Level in % by CAN bus indication.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.FuelLevel | AGInfoGroupType.CAN, false, new int[] { 11 })]
	public double CANL([AGPrmInfo("Index of level", 1.0, 6.0)] int index)
	{
		if (index < 1 || index > 6)
		{
			return 0.0;
		}
		int num = index - 1;
		read((DataType)(357 + num));
		return _CANL[num];
	}

	private void setCANLevelsRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		DeviceCAN2Record val = (DeviceCAN2Record)_prm.rec;
		CAN2Item[] values = val.Values;
		foreach (CAN2Item val2 in values)
		{
			if (val2.Valid)
			{
				int num = val2.TankID - 1;
				_CANL[num] = val2.Value;
				write((DataType)(357 + num));
			}
		}
		if (val.AdBlue.HasValue)
		{
			_CANLAB = val.AdBlue.Value;
			write((DataType)363);
		}
	}

	private void setCANEngineRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCAN3Record val = (DeviceCAN3Record)_prm.rec;
		if (val.EngineRotate.HasValue)
		{
			_CANErpmRaw = val.EngineRotate.Value;
			write(DataType.canEngine);
		}
		if (val.DistanceTO.HasValue)
		{
			_CANDmaint = val.DistanceTO.Value * 1000;
			write((DataType)365);
		}
		if (val.MotoHours.HasValue)
		{
			if (val.MotoHours.Value > 0.0)
			{
				_CANEmh = val.MotoHours.Value;
				write((DataType)366);
			}
			_CANLOGEmh = val.MotoHours.Value;
			write((DataType)367);
		}
		if (val.OilPressure.HasValue)
		{
			_CANPoil = val.OilPressure.Value;
			write((DataType)368);
		}
	}

	private void setCANTemperatureRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCAN4Record val = (DeviceCAN4Record)_prm.rec;
		if (val.FreezeLiquidTemp.HasValue)
		{
			_CANTcool = val.FreezeLiquidTemp.Value;
			write(DataType.canTemperature);
		}
		if (val.OilEngineTemp.HasValue)
		{
			_CANToil = val.OilEngineTemp.Value;
			write((DataType)370);
		}
		if (val.FuelTemp.HasValue)
		{
			_CANTfuel = val.FuelTemp.Value;
			write((DataType)371);
		}
		if (val.ManomPressure.HasValue)
		{
			_CANPman = val.ManomPressure.Value;
			write((DataType)372);
		}
		if (val.SuperchargeT.HasValue)
		{
			_CANTboost = val.SuperchargeT.Value;
			write((DataType)373);
		}
		if (val.SuperchargePressure.HasValue)
		{
			_CANPboost = val.SuperchargePressure.Value;
			write((DataType)374);
		}
	}

	private void setCANDistanceRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCAN5Record val = (DeviceCAN5Record)_prm.rec;
		if (val.TotalDistance.HasValue && val.TotalDistance.Value > 0)
		{
			_CANDtotal = val.TotalDistance.Value;
			write(DataType.canDistance);
		}
		if (val.TodayDistance.HasValue)
		{
			_CANDdaily = val.TodayDistance.Value;
			write((DataType)376);
		}
		if (_CANDtotalUpper > 0)
		{
			write(DataType.canDistanceUpper);
		}
		if (_CANDtotalLower > 0)
		{
			write(DataType.canDistanceLower);
		}
	}

	private int _PhotoReason(int n)
	{
		return _prm.photoReason(n);
	}

	private DateTime _PhotoUDT(int n)
	{
		return _prm.photoUDT(n);
	}

	private int getFirstCameraIndex()
	{
		int cameraNum = _prm.cameraNum;
		if (cameraNum != 0)
		{
			int num = 0;
			int num2 = 1;
			while (num < 16)
			{
				if ((cameraNum & num2) != 0)
				{
					return num;
				}
				num++;
				num2 <<= 1;
			}
		}
		return -1;
	}

	[AGInfo("Photo reason", "Photo reason by camera number: 0 - by time, 1 - by input, 2 - by request.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, new int[] { 15 })]
	public int PhotoReasons([AGPrmInfo("Camera number", 1.0, 16.0)] int cameraNum)
	{
		if (1 <= cameraNum && cameraNum <= 16)
		{
			int n = cameraNum - 1;
			read(DataType.common);
			return _PhotoReason(n);
		}
		return 0;
	}

	[AGInfo("Photo universal time", "Photo universal time by camera number.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, new int[] { 15 })]
	public DateTime PhotoUDTs([AGPrmInfo("Camera number", 1.0, 16.0)] int cameraNum)
	{
		if (1 <= cameraNum && cameraNum <= 16)
		{
			int num = cameraNum - 1;
			read((DataType)(99 + num));
			return _PhotoUDT(num);
		}
		return DateTime.MinValue;
	}

	[AGInfo("Photo local times", "Photo local time by camera number.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, new int[] { 15 })]
	public DateTime PhotoDTs([AGPrmInfo("Camera number", 1.0, 16.0)] int cameraNum)
	{
		if (1 <= cameraNum && cameraNum <= 16)
		{
			int num = cameraNum - 1;
			read((DataType)(99 + num));
			return TimeZoneInfo.ConvertTimeFromUtc(_PhotoUDT(num), timeZoneInfo);
		}
		return DateTime.MinValue;
	}

	[AGInfo("Photo images", "Photo images by camera number.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, new int[] { 15 })]
	public Image PhotoImages([AGPrmInfo("Camera number", 1.0, 16.0)] int cameraNum)
	{
		if (1 <= cameraNum && cameraNum <= 16)
		{
			int num = cameraNum - 1;
			if ((_CameraNum & (1 << num)) != 0 && _LoadedPhotosUDT[num] != _PhotoUDT(num))
			{
				_PhotoImages[num] = null;
				if (photoStorage != null)
				{
					byte[] photo = photoStorage.GetPhoto(_SerialNo, _PhotoUDT(num), cameraNum);
					if (photo != null)
					{
						Image image = Image.FromStream(new MemoryStream(photo));
						_PhotoImages[num] = (FullPhoto ? image : image.Inscribe(MaxImageSize));
					}
				}
				_LoadedPhotosUDT[num] = _PhotoUDT(num);
			}
			read((DataType)(99 + num));
			return _PhotoImages[num];
		}
		return null;
	}

	private void setEventIDRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected I4, but got Unknown
		DeviceEventRecord val = (DeviceEventRecord)_prm.rec;
		_EventID = (int)val.EventId;
		_AdditionalInfo[0] = val.AdditionalInfo[0];
		_AdditionalInfo[1] = val.AdditionalInfo[1];
		_AdditionalInfo[2] = val.AdditionalInfo[2];
		_AdditionalInfo[3] = val.AdditionalInfo[3];
		_AdditionalInfo[4] = val.AdditionalInfo[4];
		_EventReason = val.EventReason;
		write(DataType.eventId);
	}

	[AGInfo("Event entry Byte", "Getting the byte from event entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 15 })]
	public byte EventByte([AGPrmInfo("Index of byte", 0.0, 5.0)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid event ID", double.MinValue, double.MinValue)] int id = -1)
	{
		if ((id >= 0 && _EventID != id) || index < 0 || index > 5)
		{
			return (byte)def;
		}
		read(DataType.eventId);
		return index switch
		{
			0 => _AdditionalInfo[0], 
			1 => _AdditionalInfo[1], 
			2 => (byte)_EventReason, 
			3 => _AdditionalInfo[2], 
			4 => _AdditionalInfo[3], 
			5 => _AdditionalInfo[4], 
			_ => (byte)def, 
		};
	}

	[AGInfo("Wheel load by CAN", "Wheel load in tn by CAN bus indication.", "t", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.CAN, false, new int[]
	{
		16, 17, 18, 19, 20, 21, 22, 23, 24, 25,
		26, 27, 28, 29, 30, 31
	})]
	public double CANAW([AGPrmInfo("Index of axis", 1.0, 16.0)] int axis, [AGPrmInfo("Index of wheel", 1.0, 6.0)] int wheel)
	{
		if (1 <= axis && axis <= 16 && 1 <= wheel && wheel <= 6)
		{
			axis--;
			wheel--;
			read((DataType)(377 + axis * 6 + wheel));
			if (_CANAW == null)
			{
				return 0.0;
			}
			return _CANAW[axis, wheel];
		}
		return 0.0;
	}

	private void setCANAxisRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		int shaft = ((DeviceCAN6Record)_prm.rec).Shaft;
		int num = shaft * 6;
		WheelPressureItem[] values = ((DeviceCAN6Record)_prm.rec).Values;
		for (int i = 0; i < values.Length; i++)
		{
			WheelPressureItem val = values[i];
			if (val.Value.HasValue)
			{
				if (_CANAW == null)
				{
					Initialize_CANAW();
				}
				int num2 = val.ID - 1;
				_CANAW[shaft, num2] = val.Value.Value;
				write((DataType)(377 + num + num2));
			}
		}
	}

	private void setCANUserData()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected I4, but got Unknown
		if (_UDArrs == null)
		{
			Initialize_UDArrs();
		}
		DeviceCAN7Record val = (DeviceCAN7Record)_prm.rec;
		int num = ((DeviceRecordLite)val).TypeId - 32;
		_UDType = num + 1;
		Array.Copy(val.Data, _UDArrs[num], 6);
		write(DataType.canUserData);
	}

	[AGInfo("Temperature by 1-wire by index", "Temperature in в °C by 1-wire bus indication.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.Wire1, false, new int[] { 36, 37 })]
	public double Temper([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0.0;
		}
		int num = index - 1;
		read((DataType)(116 + num));
		return _Temper[num];
	}

	private void setTempSensorRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceTempSensorRecord val = (DeviceTempSensorRecord)_prm.rec;
		if (val.DeviceTemp.HasValue)
		{
			_DevT = val.DeviceTemp.Value;
			write(DataType.devT);
		}
		SensorTempItem[] values = val.Values;
		foreach (SensorTempItem val2 in values)
		{
			if (val2.Value.HasValue)
			{
				int num = val2.ID - 1;
				_Temper[num] = val2.Value.Value;
				write((DataType)(116 + num));
			}
		}
	}

	[AGInfo("Inputs extender - Positive input by index", "State of positive discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IP([AGPrmInfo("Index of input", 1.0, 8.0)] int index)
	{
		if (1 > index || index > 8)
		{
			return false;
		}
		return (InputsP & (1 << index - 1)) != 0;
	}

	[AGInfo("Inputs extender - Negative input by index", "State of negative discrete input of inputs extender.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 38 })]
	public bool IM([AGPrmInfo("Index of input", 1.0, 8.0)] int index)
	{
		if (1 > index || index > 8)
		{
			return false;
		}
		return (InputsM & (1 << index - 1)) != 0;
	}

	private void setInputExtRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceInputExtRecord val = (DeviceInputExtRecord)_prm.rec;
		_InputsP = val.MaskPlus;
		_InputsM = val.MaskMinus;
		_IStatus = val.MaskStatus;
		write(DataType.inputs);
	}

	private void setFillAmountRecord()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_FAID = (_CardID = ((RS485_FuelTankRecord)_prm.rec).CardID);
		write((DataType)126);
	}

	private void setFuelRateRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		RS485_FuelOffRecord val = (RS485_FuelOffRecord)_prm.rec;
		_FRChannel = val.DRT;
		_FRAddr = val.Address;
		_FRTotal = val.TotalFuelOff;
		write(DataType.fuelRate);
	}

	private void setFillDurationRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		RS485_FuelDurationRecord val = (RS485_FuelDurationRecord)_prm.rec;
		_FDDuration = val.Duration;
		_FDID = (_CardID = val.CardID);
		write(DataType.fillDuration);
	}

	[AGInfo("Passenger traffic mode by index", "Passenger traffic mode by index.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 42 })]
	public int PTMode([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(130 + num));
		if (_PTMode == null)
		{
			return 0;
		}
		return _PTMode[num];
	}

	[AGInfo("Passenger traffic status by index", "Passenger traffic status by index.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 42 })]
	public int PTStatus([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(130 + num));
		if (_PTStatus == null)
		{
			return 0;
		}
		return _PTStatus[num];
	}

	[AGInfo("Passenger category by index", "Passenger category by index.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 42 })]
	public int PTCategory([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(130 + num));
		if (_PTStatus == null)
		{
			return 0;
		}
		return (_PTStatus[num] >> 5) & 7;
	}

	[AGInfo("Passenger in by index", "Passenger in by index.", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 42 })]
	public int PTIn([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(130 + num));
		if (_PTIn == null)
		{
			return 0;
		}
		return _PTIn[num];
	}

	[AGInfo("Passenger in by index", "Passenger in of category by index.", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 42 })]
	public int PTCtgIn([AGPrmInfo("Index of door", 1.0, 16.0)] int index, [AGPrmInfo("Category", 1.0, 8.0)] int category)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		if (category < 1 || category > 8)
		{
			return 0;
		}
		int num = index - 1;
		int num2 = category - 1;
		read((DataType)(146 + num * 8 + num2));
		if (_PTCtgIn == null)
		{
			return 0;
		}
		return _PTCtgIn[num, num2];
	}

	[AGInfo("Passenger out by index", "Passenger out by index.", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 42 })]
	public int PTOut([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(130 + num));
		if (_PTOut == null)
		{
			return 0;
		}
		return _PTOut[num];
	}

	[AGInfo("Passenger out by index", "Passenger out of category by index.", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 42 })]
	public int PTCtgOut([AGPrmInfo("Index of door", 1.0, 16.0)] int index, [AGPrmInfo("Category", 1.0, 8.0)] int category)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		if (category < 1 || category > 8)
		{
			return 0;
		}
		int num = index - 1;
		int num2 = category - 1;
		read((DataType)(146 + num * 8 + num2));
		if (_PTCtgOut == null)
		{
			return 0;
		}
		return _PTCtgOut[num, num2];
	}

	[AGInfo("PT and MD mode by index", "Common mode of passenger traffic and measuring device by index.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 42, 43 })]
	public int PTMDMode([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(130 + num), (DataType)(275 + num));
		if (_PTMDMode == null)
		{
			return 0;
		}
		return _PTMDMode[num];
	}

	[AGInfo("PT and MD status by index", "Common status of passenger traffic and measuring device by index.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 42, 43 })]
	public int PTMDStatus([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(130 + num), (DataType)(275 + num));
		if (_PTMDStatus == null)
		{
			return 0;
		}
		return _PTMDStatus[num];
	}

	private void setRS485_PeopleCountRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		RS485_PeopleCountRecord val = (RS485_PeopleCountRecord)_prm.rec;
		_PTChannel = val.Channel;
		write(DataType.ptChannel);
		if (_PTMode == null)
		{
			Initialize_PTMode();
		}
		if (_PTStatus == null)
		{
			Initialize_PTStatus();
		}
		if (_PTIn == null)
		{
			Initialize_PTIn();
		}
		if (_PTOut == null)
		{
			Initialize_PTOut();
		}
		if (_PTMDStatus == null)
		{
			Initialize_PTMDStatus();
		}
		if (_PTMDMode == null)
		{
			Initialize_PTMDMode();
		}
		_PTMode[val.Channel] = (_PTMDMode[val.Channel] = val.Mode);
		_PTStatus[val.Channel] = (_PTMDStatus[val.Channel] = val.Status);
		_PTIn[val.Channel] = val.CountIn;
		_PTOut[val.Channel] = val.CountOut;
		write((DataType)(130 + val.Channel));
		int num = (val.Status >> 5) & 7;
		if (_PTCtgIn == null)
		{
			Initialize_PTCtgIn();
		}
		_PTCtgIn[val.Channel, num] = val.CountIn;
		if (_PTCtgOut == null)
		{
			Initialize_PTCtgOut();
		}
		_PTCtgOut[val.Channel, num] = val.CountOut;
		write((DataType)(146 + val.Channel * 8 + num));
	}

	[AGInfo("Measuring device mode by index", "Measuring device mode by index.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 43 })]
	public int MDMode([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(275 + num));
		if (_MDMode == null)
		{
			return 0;
		}
		return _MDMode[num];
	}

	[AGInfo("Measuring device status by index", "Measuring device status by index.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 43 })]
	public int MDStatus([AGPrmInfo("Index of door", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(275 + num));
		if (_MDStatus == null)
		{
			return 0;
		}
		return _MDStatus[num];
	}

	[AGInfo("Weight / level by index", "Weight / level.", "kg", DeviceParameterKind.Flag, AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 43 })]
	public double MDL([AGPrmInfo("Index of input", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0.0;
		}
		int num = index - 1;
		read((DataType)(275 + num));
		if (_MDL == null)
		{
			return 0.0;
		}
		return _MDL[num];
	}

	private void setRS485_WeightCountRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		RS485_WeightCountRecord val = (RS485_WeightCountRecord)_prm.rec;
		_MDChannel = val.Channel;
		write(DataType.mdChannel);
		if (_PTMDMode == null)
		{
			Initialize_PTMDMode();
		}
		if (_PTMDStatus == null)
		{
			Initialize_PTMDStatus();
		}
		if (_MDMode == null)
		{
			Initialize_MDMode();
		}
		if (_MDStatus == null)
		{
			Initialize_MDStatus();
		}
		if (_MDL == null)
		{
			Initialize_MDL();
		}
		_MDMode[val.Channel] = (_PTMDMode[val.Channel] = val.Mode);
		_MDStatus[val.Channel] = (_PTMDStatus[val.Channel] = val.Status);
		_MDL[val.Channel] = val.Weight;
		write((DataType)(275 + val.Channel));
	}

	private void setCANErrorRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCANErrorRecord val = (DeviceCANErrorRecord)_prm.rec;
		_CANFMI = val.FMI;
		_CANSPNv1 = val.SPNv1;
		_CANSPNv2 = val.SPNv2;
		_CANSPN = val.SPN;
		_CANErrorIsActive = val.ErrorIsActive;
		write(DataType.canErrors);
		if (val.LampProtect.HasValue)
		{
			_CANLampProtect = val.LampProtect.Value;
			write((DataType)475);
		}
		if (val.LampAmber.HasValue)
		{
			_CANLampAmber = val.LampAmber.Value;
			write((DataType)476);
		}
		if (val.LampRed.HasValue)
		{
			_CANLampRed = val.LampRed.Value;
			write((DataType)477);
		}
		if (val.LampMalfunc.HasValue)
		{
			_CANLampMalfunc = val.LampMalfunc.Value;
			write((DataType)478);
		}
		if (val.FlashLampStatus.HasValue)
		{
			_CANFlashLampStatus = val.FlashLampStatus.Value;
			write((DataType)479);
		}
		if (val.OccurCount.HasValue)
		{
			_CANOccurCount = val.OccurCount.Value;
			write((DataType)480);
		}
	}

	private void setCANCalcFuelRateRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCANFuelRecord val = (DeviceCANFuelRecord)_prm.rec;
		if (val.ConsInstant.HasValue)
		{
			_CANFinstant = val.ConsInstant.Value;
			write(DataType.canCalcFuelRate);
		}
		if (val.ConsAccum.HasValue)
		{
			_CANFcalc = val.ConsAccum.Value;
			write((DataType)482);
		}
		if (val.Choker.HasValue)
		{
			_CANChoker = val.Choker.Value;
			write((DataType)483);
		}
	}

	private void setCANModeRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected I4, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected I4, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected I4, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected I4, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected I4, but got Unknown
		DeviceCANModeRecord val = (DeviceCANModeRecord)_prm.rec;
		_CANTorquePercent = val.TorquePercent;
		_CANFrictionPercent = val.FrictionPercent;
		_CANTorqueMode = (int)val.ModeTorque;
		_CANIdlingMode = (int)val.ModeIdle;
		_CANKickDownMode = (int)val.ModeKickdown;
		_CANPTOState = (int)val.PTOStatus;
		_CANCruiseState = (int)val.CruiseState;
		write(DataType.canMode);
		if (val.BatteryVoltage.HasValue)
		{
			_BatteryVolt = val.BatteryVoltage.Value;
			write((DataType)485);
		}
		if (val.CruiseSpeed.HasValue)
		{
			_CANCruiseSpeed = val.CruiseSpeed.Value;
			write((DataType)486);
		}
	}

	private void setCANEngineAuxRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCANAdditionalRecord val = (DeviceCANAdditionalRecord)_prm.rec;
		if (val.AirTemp.HasValue)
		{
			_CANTair = val.AirTemp.Value;
			write(DataType.canEngineAux);
		}
		if (val.AirPressure.HasValue)
		{
			_CANPair = val.AirPressure.Value;
			write((DataType)488);
		}
		if (val.Rotation.HasValue)
		{
			_CANErpm = val.Rotation.Value;
			write((DataType)489);
		}
		if (val.MotorLoading.HasValue)
		{
			_CANEload = val.MotorLoading.Value;
			write((DataType)490);
		}
		if (val.Amperage.HasValue)
		{
			_BatteryAmp = val.Amperage.Value;
			write((DataType)491);
		}
	}

	private void setLongEntryHeader()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceLongRecordHeader val = (DeviceLongRecordHeader)_prm.rec;
		_LDRawLen = val.TotalLength;
		_LDEnCnt = val.RecordCount;
		_LDType = val.DataType;
		write(DataType.longEntryHeader);
		if ((_LDRawLen + 5) / 6 == _LDEnCnt)
		{
			_requiredLen = _LDRawLen;
			_requiredEnCnt = _LDEnCnt;
		}
		else
		{
			_requiredLen = (_requiredEnCnt = 0);
		}
		_LDArr = new byte[0];
		_longDataList.Clear();
	}

	private void setLongEntryData()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceLongRecordBody val = (DeviceLongRecordBody)_prm.rec;
		_LDIndex = val.RecordIndex;
		write(DataType.longEntryIndex);
		if (_requiredEnCnt == 0)
		{
			return;
		}
		if (_longDataList.Count / 6 == val.RecordIndex)
		{
			for (int i = 0; i < 6; i++)
			{
				_longDataList.Add(val.Data[i]);
				if (_longDataList.Count < _requiredLen)
				{
					continue;
				}
				_requiredLen = (_requiredEnCnt = 0);
				_LDArr = _longDataList.ToArray();
				_longDataList.Clear();
				write(DataType.longEntryData);
				switch (_LDType)
				{
				case 83:
					write(DataType.longEntryDataID);
					break;
				case 209:
				{
					if (_LDArr.Length != 25)
					{
						break;
					}
					int num5 = _LDArr[0] - 208;
					if (num5 >= 0 && num5 <= 15)
					{
						if (_AGFCRefuellerID == null)
						{
							Initialize_AGFCRefuellerID();
						}
						if (_AGFCDriverID == null)
						{
							Initialize_AGFCDriverID();
						}
						if (_AGFCVolume == null)
						{
							Initialize_AGFCVolume();
						}
						if (_AGFCDuration == null)
						{
							Initialize_AGFCDuration();
						}
						_AGFCVolume[num5] = (double)BitConverter.ToInt32(_LDArr, 1) / 1000.0;
						_AGFCDuration[num5] = BitConverter.ToInt32(_LDArr, 5);
						_AGFCRefuellerID[num5] = BitConverter.ToInt64(_LDArr, 9);
						_AGFCDriverID[num5] = BitConverter.ToInt64(_LDArr, 17);
						write((DataType)(496 + num5));
					}
					break;
				}
				case 65131:
				{
					int pos2 = 0;
					_LDDriver1 = parseLDDriver(_LDArr, ref pos2);
					_LDDriver2 = parseLDDriver(_LDArr, ref pos2);
					write(DataType.longEntryDataTGD);
					break;
				}
				case 65262:
				{
					int pos = 0;
					_LDCard1 = parseLDDriver(_LDArr, ref pos);
					_LDCard2 = parseLDDriver(_LDArr, ref pos);
					write(DataType.longEntryDataTGC);
					break;
				}
				case 4096:
				{
					if (_LDArr.Length < 5 || _LDArr[1] != 83 || _LDArr[3] != 71)
					{
						break;
					}
					byte b = _LDArr[0];
					if (0 > b || b > 7)
					{
						break;
					}
					byte b2 = _LDArr[4];
					switch (_LDArr[2])
					{
					case 48:
						if (5 + b2 <= _LDArr.Length)
						{
							if (_IRMAStatusS == null)
							{
								Initialize_IRMAStatusS();
							}
							_IRMAStatusS[b] = 0;
							int num3 = 0;
							int num4 = 5;
							while (num3 < b2)
							{
								_IRMAStatusS[b] |= 1 << _LDArr[num4] - 1;
								num3++;
								num4++;
							}
							write((DataType)(514 + b));
						}
						break;
					case 49:
						if (5 + b2 * 3 <= _LDArr.Length)
						{
							if (_IRMAStatusA == null)
							{
								Initialize_IRMAStatusA();
							}
							_IRMAStatusA[b] = 0;
							int num = 0;
							int num2 = 5;
							while (num < b2)
							{
								_IRMAStatusA[b] |= 1 << _LDArr[num2 + 2] - 1;
								num++;
								num2 += 3;
							}
							write((DataType)(522 + b));
						}
						break;
					}
					break;
				}
				}
				break;
			}
		}
		else
		{
			_requiredLen = (_requiredEnCnt = 0);
			_LDArr = new byte[0];
			_longDataList.Clear();
		}
	}

	private string parseLDDriver(byte[] arr, ref int pos)
	{
		byte[] array = new byte[arr.Length - pos];
		int num = 0;
		bool flag = false;
		while (pos < arr.Length)
		{
			switch (arr[pos])
			{
			case 0:
			case 1:
			case 5:
			case 32:
			case byte.MaxValue:
				flag = num > 0;
				break;
			default:
				if (flag)
				{
					array[num++] = 32;
					flag = false;
				}
				array[num++] = arr[pos];
				break;
			case 42:
				break;
			}
			if (arr[pos] == 42)
			{
				pos++;
				break;
			}
			pos++;
		}
		for (int i = 0; i < num; i++)
		{
			if (array[i] < 32)
			{
				return string.Empty;
			}
		}
		return Encoding.GetEncoding("ISO-8859-5").GetString(array, 0, num);
	}

	[AGInfo("Filling volume by flowmeter AGFC", "Filling volume by flowmeter AGFC index in liters.", "l", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.RS485 | AGInfoGroupType.AutoCistern, false, new int[] { 48, 49 })]
	public double AGFCVolume([AGPrmInfo("Index of flowmeter", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0.0;
		}
		int num = index - 1;
		read((DataType)(496 + num));
		if (_AGFCVolume == null)
		{
			return 0.0;
		}
		return _AGFCVolume[num];
	}

	[AGInfo("Filling duration by flowmeter AGFC", "Fill duration by flowmeter AGFC index in seconds.", "s", DeviceParameterKind.Accum, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 48, 49 })]
	public int AGFCDuration([AGPrmInfo("Index of flowmeter", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(496 + num));
		if (_AGFCDuration == null)
		{
			return 0;
		}
		return _AGFCDuration[num];
	}

	[AGInfo("Refueller ID by flowmeter AGFC", "Refueller ID by flowmeter AGFC index. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485 | AGInfoGroupType.Identifier, false, new int[] { 48, 49 })]
	public long AGFCRefuellerID([AGPrmInfo("Index of flowmeter", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0L;
		}
		int num = index - 1;
		read((DataType)(496 + num));
		if (_AGFCRefuellerID == null)
		{
			return 0L;
		}
		return _AGFCRefuellerID[num];
	}

	[AGInfo("Driver ID by flowmeter AGFC", "Driver ID by flowmeter AGFC index. 0 if empty.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485 | AGInfoGroupType.Identifier, false, new int[] { 48, 49 })]
	public long AGFCDriverID([AGPrmInfo("Index of flowmeter", 1.0, 16.0)] int index)
	{
		if (index < 1 || index > 16)
		{
			return 0L;
		}
		int num = index - 1;
		read((DataType)(496 + num));
		if (_AGFCDriverID == null)
		{
			return 0L;
		}
		return _AGFCDriverID[num];
	}

	[AGInfo("IRMA sensor status", "IRMA MATRIX sensor status by door.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48, 49 })]
	public int IRMAStatusS([AGPrmInfo("Index of door", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(514 + num));
		if (_IRMAStatusS == null)
		{
			return 0;
		}
		return _IRMAStatusS[num];
	}

	[AGInfo("IRMA function area status", "IRMA MATRIX function area status by door.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 48, 49 })]
	public int IRMAStatusFA([AGPrmInfo("Index of door", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(522 + num));
		if (_IRMAStatusA == null)
		{
			return 0;
		}
		return _IRMAStatusA[num];
	}

	private bool ldFiltration(int type, out DataType dataType, out byte[] arr, params int[] filters)
	{
		int num;
		if (1048577 <= type && type <= 1048580)
		{
			if (_UDArrs == null)
			{
				Initialize_UDArrs();
			}
			type -= 1048576;
			dataType = DataType.canUserData;
			num = _UDType;
			arr = _UDArrs[type - 1];
		}
		else
		{
			dataType = DataType.longEntryData;
			num = _LDType;
			arr = _LDArr;
		}
		if (type >= 0 && type != num)
		{
			return true;
		}
		int num2 = 0;
		for (int i = 1; i < filters.Length; i += 2)
		{
			int num3 = filters[num2];
			if (num3 < 0 || num3 >= arr.Length)
			{
				return true;
			}
			if (arr[num3] != (byte)filters[i])
			{
				return true;
			}
			num2 += 2;
		}
		return false;
	}

	[AGInfo("Long data String", "Getting the string from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public string LDStr([AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1)
	{
		if (ldFiltration(type, out var dataType, out var arr, emptyFilters))
		{
			return null;
		}
		read(dataType);
		return Encoding.GetEncoding(1251).GetString(arr);
	}

	[AGInfo("Long data String", "Getting the string from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public string LDStr([AGPrmInfo("Index of the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Number of chars", 0.0, double.MinValue)] int len, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] string def = null, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index > arr.Length - len)
		{
			return def;
		}
		read(dataType);
		if (len < 0)
		{
			len = 0;
		}
		for (int i = 0; i < len; i++)
		{
			if (arr[index + i] == 0)
			{
				len = i;
				break;
			}
		}
		return Encoding.GetEncoding(1251).GetString(arr, index, len);
	}

	[AGInfo("Long data Byte", "Getting the byte from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public byte LDByte([AGPrmInfo("Index of byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index >= arr.Length)
		{
			return (byte)def;
		}
		read(dataType);
		return arr[index];
	}

	[AGInfo("Long data Byte (variable)", "Getting the byte from long data entry (variable-length).", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public byte LDByteV([AGPrmInfo("Index of byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0)
		{
			return (byte)def;
		}
		read(dataType);
		if (index >= arr.Length)
		{
			return (byte)def;
		}
		return arr[index];
	}

	[AGInfo("Long data SByte", "Getting the signed byte from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public int LDSByte([AGPrmInfo("Index of byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index >= arr.Length)
		{
			return def;
		}
		read(dataType);
		return (sbyte)arr[index];
	}

	[AGInfo("Long data SByte (variable)", "Getting the signed byte from long data entry (variable-length).", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public int LDSByteV([AGPrmInfo("Index of byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0)
		{
			return def;
		}
		read(dataType);
		if (index >= arr.Length)
		{
			return def;
		}
		return (sbyte)arr[index];
	}

	[AGInfo("Long data Byte from BCD", "Getting the byte from BCD format from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public byte LDByteBCD([AGPrmInfo("Index of byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index >= arr.Length)
		{
			return (byte)def;
		}
		int num = arr[index] & 0xF;
		if (num > 9)
		{
			return (byte)def;
		}
		int num2 = arr[index] >> 4;
		if (num2 > 9)
		{
			return (byte)def;
		}
		read(dataType);
		return (byte)(10 * num2 + num);
	}

	[AGInfo("Long data Int32", "Getting the integer 32-bit number from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public int LDInt32([AGPrmInfo("Index of the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index > arr.Length - 4)
		{
			return def;
		}
		read(dataType);
		return BitConverter.ToInt32(arr, index);
	}

	[AGInfo("Long data bits", "Getting the bits field from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public int LDBits([AGPrmInfo("Index of the first bit from the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Number of bits", 0.0, 31.0)] int len, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index > arr.Length * 8 - len)
		{
			return def;
		}
		read(dataType);
		int num = 0;
		if (len < 0)
		{
			len = 0;
		}
		else if (len > 31)
		{
			len = 31;
		}
		int num2 = index + len;
		int num3 = 1;
		while (index < num2)
		{
			if ((arr[index / 8] & (1 << index % 8)) != 0)
			{
				num |= num3;
			}
			index++;
			num3 <<= 1;
		}
		return num;
	}

	[AGInfo("Long data bits (variable)", "Getting the bits field from long data entry (variable-length).", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public int LDBitsV([AGPrmInfo("Index of the first bit from the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Number of bits", 0.0, 31.0)] int len, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0)
		{
			return def;
		}
		read(dataType);
		int num = def;
		if (len < 0)
		{
			len = 0;
		}
		else if (len > 31)
		{
			len = 31;
		}
		int num2 = index + len;
		int num3 = 1;
		while (index < num2)
		{
			int num4 = index / 8;
			if (num4 >= arr.Length)
			{
				break;
			}
			if ((arr[num4] & (1 << index % 8)) != 0)
			{
				num |= num3;
			}
			index++;
			num3 <<= 1;
		}
		return num;
	}

	[AGInfo("Long data Int16", "Getting the integer 16-bit number from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public int LDInt16([AGPrmInfo("Index of the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index > arr.Length - 2)
		{
			return def;
		}
		read(dataType);
		return BitConverter.ToInt16(arr, index);
	}

	[AGInfo("Long data UInt16", "Getting the unsigned integer 16-bit number from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public int LDUInt16([AGPrmInfo("Index of the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index > arr.Length - 2)
		{
			return def;
		}
		read(dataType);
		return BitConverter.ToUInt16(arr, index);
	}

	[AGInfo("Long data Int64", "Getting the integer 64-bit number from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public long LDInt64([AGPrmInfo("Index of the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] long def = 0L, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index > arr.Length - 8)
		{
			return def;
		}
		read(dataType);
		return BitConverter.ToInt64(arr, index);
	}

	[AGInfo("Long data ID", "Getting the identifier by RS-485 from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485 | AGInfoGroupType.Identifier, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public long LDID([AGPrmInfo("Index", 1.0, 4.0)] int index)
	{
		int i = 0;
		int num = 0;
		int num2;
		for (; i < _LDArr.Length; i += num2)
		{
			num2 = _LDArr[i++];
			if (i + num2 > _LDArr.Length)
			{
				break;
			}
			if (num2 == 8 && ++num == index)
			{
				read(DataType.longEntryDataID);
				return BitConverter.ToInt64(_LDArr, i);
			}
		}
		return 0L;
	}

	[AGInfo("Long data Float", "Getting the float number from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public double LDFloat([AGPrmInfo("Index of the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def = 0.0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index > arr.Length - 4)
		{
			return def;
		}
		read(dataType);
		return BitConverter.ToSingle(arr, index);
	}

	[AGInfo("Long data Double", "Getting the real number from long data entry.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 32, 33, 34, 35, 48, 49 })]
	public double LDDouble([AGPrmInfo("Index of the first byte", 0.0, double.MinValue)] int index, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def = 0.0, [AGPrmInfo("Valid type", double.MinValue, double.MinValue)] int type = -1, [AGPrmInfo("Additional filters (pos1, val1, pos2, val2, ...)", double.MinValue, double.MinValue)] params int[] filters)
	{
		if (ldFiltration(type, out var dataType, out var arr, filters) || index < 0 || index > arr.Length - 8)
		{
			return def;
		}
		read(dataType);
		return BitConverter.ToDouble(arr, index);
	}

	[AGInfo("Palesse frequency", "Palesse frequency value by channel.", DeviceParameterKind.Accum, AGInfoGroupType.Level, false, new int[] { 50 })]
	public int PALprmF([AGPrmInfo("Channel", 0.0, 255.0)] int channel)
	{
		if (!freqPALPrmDict.TryGetValue(channel, out var value))
		{
			return 0;
		}
		read((DataType)(291 + value));
		if (_PALprmF == null)
		{
			return 0;
		}
		return _PALprmF[value];
	}

	[AGInfo("Palesse analog", "Palesse analog value by channel.", DeviceParameterKind.InstTime, AGInfoGroupType.Level, false, new int[] { 50 })]
	public int PALprmA([AGPrmInfo("Channel", 0.0, 255.0)] int channel)
	{
		if (!anlgPALPrmDict.TryGetValue(channel, out var value))
		{
			return 0;
		}
		read((DataType)(307 + value));
		if (_PALprmA == null)
		{
			return 0;
		}
		return _PALprmA[value];
	}

	private void setPALParametersRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCANParameterRecord val = (DeviceCANParameterRecord)_prm.rec;
		for (int i = 0; i < 2; i++)
		{
			int value;
			switch (val.DataType)
			{
			case 1:
				if (freqPALPrmDict.TryGetValue(val.Channel[i], out value))
				{
					if (_PALprmF == null)
					{
						Initialize_PALprmF();
					}
					_PALprmF[value] = val.Value[i];
					write((DataType)(291 + value));
				}
				break;
			case 2:
				if (anlgPALPrmDict.TryGetValue(val.Channel[i], out value))
				{
					if (_PALprmA == null)
					{
						Initialize_PALprmA();
					}
					_PALprmA[value] = val.Value[i];
					write((DataType)(307 + value));
				}
				break;
			}
		}
	}

	private void setPALFlagsRecord()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_PALflags = ((DeviceCANFlagsRecord)_prm.rec).Value;
		write(DataType.palFlags);
	}

	[AGInfo("Palesse statistic", "Palesse statistic value by channel.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Navigation | AGInfoGroupType.Level | AGInfoGroupType.Pressure | AGInfoGroupType.Rotation | AGInfoGroupType.Temperature | AGInfoGroupType.Distance, false, new int[] { 52 })]
	public int PALprmS([AGPrmInfo("Channel", 1.0, 65535.0)] int channel)
	{
		if (!statPALprmDict.TryGetValue(channel, out var value))
		{
			return 0;
		}
		read((DataType)(324 + value));
		if (_PALprmS == null)
		{
			return 0;
		}
		return _PALprmS[value];
	}

	private void setPALStatisticRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceCANStatRecord val = (DeviceCANStatRecord)_prm.rec;
		if (statPALprmDict.TryGetValue(val.DataID, out var value))
		{
			if (_PALprmS == null)
			{
				Initialize_PALprmS();
			}
			_PALprmS[value] = val.Value;
			write((DataType)(324 + value));
		}
	}

	[AGInfo("Temperature by LLS by index", "LLS temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 53 })]
	public int TLLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(342 + num));
		return _TLLS[num];
	}

	private void setLLSExtRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		LLSExtItem[] values = ((DeviceLLSExtRecord)_prm.rec).Values;
		foreach (LLSExtItem val in values)
		{
			int num = val.SensorID - 1;
			_VLLS[num] = val.Valid;
			write((DataType)(90 + num));
			if (val.Valid)
			{
				_LLS[num] = val.Value;
				write((DataType)(82 + num));
				write((DataType)(1427 + num));
				_TLLS[num] = val.Temp;
				write((DataType)(342 + num));
			}
		}
	}

	[AGInfo("ISOBUS signed integer", "ISOBUS signed integer value by type.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 54 })]
	public int ISOBUSInt([AGPrmInfo("Type", 0.0, 65535.0)] int type)
	{
		if (!isobusPrmDict.TryGetValue(type, out var value))
		{
			return 0;
		}
		read((DataType)(531 + value));
		if (_ISOBUSInt == null)
		{
			return 0;
		}
		return _ISOBUSInt[value];
	}

	[AGInfo("ISOBUS float", "ISOBUS float value by type.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 54 })]
	public double ISOBUSFloat([AGPrmInfo("Type", 0.0, 65535.0)] int type)
	{
		if (!isobusPrmDict.TryGetValue(type, out var value))
		{
			return 0.0;
		}
		read((DataType)(547 + value));
		if (_ISOBUSFloat == null)
		{
			return 0.0;
		}
		return _ISOBUSFloat[value];
	}

	[AGInfo("ISOBUS unsigned integer", "ISOBUS unsigned integer value by type.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 54 })]
	public int ISOBUSUInt([AGPrmInfo("Type", 0.0, 65535.0)] int type)
	{
		if (!isobusPrmDict.TryGetValue(type, out var value))
		{
			return 0;
		}
		read((DataType)(563 + value));
		if (_ISOBUSUInt == null)
		{
			return 0;
		}
		return _ISOBUSUInt[value];
	}

	private void setISOBUSRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected I4, but got Unknown
		DeviceISOBUSRecord val = (DeviceISOBUSRecord)_prm.rec;
		_ISOBUSType = val.ParmType;
		_ISOBUSError = (int)val.DataType == 3;
		write(DataType.isobus);
		ISOBUSDataType dataType = val.DataType;
		int value;
		switch ((int)dataType)
		{
		case 0:
			if (isobusPrmDict.TryGetValue(val.ParmType, out value))
			{
				if (_ISOBUSInt == null)
				{
					Initialize_ISOBUSInt();
				}
				_ISOBUSInt[value] = (int)val.Value;
				write((DataType)(531 + value));
			}
			break;
		case 1:
			if (isobusPrmDict.TryGetValue(val.ParmType, out value))
			{
				if (_ISOBUSFloat == null)
				{
					Initialize_ISOBUSFloat();
				}
				_ISOBUSFloat[value] = BitConverter.ToSingle(BitConverter.GetBytes(val.Value), 0);
				write((DataType)(547 + value));
			}
			break;
		case 2:
			if (isobusPrmDict.TryGetValue(val.ParmType, out value))
			{
				if (_ISOBUSUInt == null)
				{
					Initialize_ISOBUSUInt();
				}
				_ISOBUSUInt[value] = (int)val.Value;
				write((DataType)(563 + value));
			}
			break;
		}
	}

	private void setGuardRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		DeviceGuardRecord val = (DeviceGuardRecord)_prm.rec;
		_GuardState = val.State;
		_GuardFlags = val.Flags;
		_GuardIndicators = (int)val.Errors;
		write(DataType.guard);
	}

	[AGInfo("Numerical data signed integer by type", "Numerical data signed integer by type.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public int NDInt([AGPrmInfo("Type", double.MinValue, double.MinValue)] int type)
	{
		read(type);
		if (!_NDDict.TryGetValue(type, out var value))
		{
			return 0;
		}
		return value;
	}

	[AGInfo("Numerical data unsigned integer by type", "Numerical data unsigned integer by type.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public long NDUInt([AGPrmInfo("Type", double.MinValue, double.MinValue)] int type)
	{
		read(type);
		int value;
		return (uint)(_NDDict.TryGetValue(type, out value) ? value : 0);
	}

	[AGInfo("Numerical data float value by type", "Numerical data float value by type.", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public double NDFloat([AGPrmInfo("Type", double.MinValue, double.MinValue)] int type)
	{
		read(type);
		int value;
		return _NDDict.TryGetValue(type, out value) ? BitConverter.ToSingle(BitConverter.GetBytes(value), 0) : 0f;
	}

	[AGInfo("CAN custom signed integer by type", "CAN custom signed integer value by type.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency | AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public int NDCANIntByType([AGPrmInfo("Type", 0.0, 127.0)] int type)
	{
		return NDCANInt(type + 1);
	}

	[AGInfo("CAN custom signed integer", "CAN custom signed integer value by index of sensor.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency | AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public int NDCANInt([AGPrmInfo("Index of sensor", 1.0, 128.0)] int index)
	{
		if (index < 1 || index > 128)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(581 + num));
		if (_NDCANInt4321 == null)
		{
			return 0;
		}
		return _NDCANInt4321[num];
	}

	[AGInfo("MODBUS signed integer", "MODBUS signed integer value by index of sensor. Order of bytes: 0 - direct, 1 - swap bytes of words, 2 - swap words, 3 - reverse.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency | AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public int MODBUSInt([AGPrmInfo("Index of sensor", 1.0, 128.0)] int index, [AGPrmInfo("Order of bytes", 0.0, 3.0)] int order)
	{
		if (index < 1 || index > 128)
		{
			return 0;
		}
		if (order < 0 || order > 3)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(709 + num));
		if (_MODBUSInt4321 == null)
		{
			return 0;
		}
		return order switch
		{
			0 => _MODBUSInt4321[num], 
			1 => _MODBUSInt3412[num], 
			2 => _MODBUSInt2143[num], 
			_ => _MODBUSInt1234[num], 
		};
	}

	[AGInfo("MODBUS float", "MODBUS float value by index of sensor. Order of bytes: 0 - direct, 1 - swap bytes of words, 2 - swap words, 3 - reverse.", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure | AGInfoGroupType.Rotation | AGInfoGroupType.Frequency | AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 57 })]
	public double MODBUSFloat([AGPrmInfo("Index of sensor", 1.0, 128.0)] int index, [AGPrmInfo("Order of bytes", 0.0, 3.0)] int order)
	{
		if (index < 1 || index > 128)
		{
			return 0.0;
		}
		if (order < 0 || order > 3)
		{
			return 0.0;
		}
		int num = index - 1;
		read((DataType)(709 + num));
		if (_MODBUSFloat4321 == null)
		{
			return 0.0;
		}
		return order switch
		{
			0 => _MODBUSFloat4321[num], 
			1 => _MODBUSFloat3412[num], 
			2 => _MODBUSFloat2143[num], 
			_ => _MODBUSFloat1234[num], 
		};
	}

	private void setNumericalDataRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected I4, but got Unknown
		DeviceNumericParamRecord val = (DeviceNumericParamRecord)_prm.rec;
		_NDType = (int)val.ParamType;
		_NDDict[_NDType] = (_NDValue = (int)val.ParamValue);
		write(DataType.numericalData);
		write(_NDType);
		if (_NDType == 256)
		{
			write((DataType)37);
		}
		else if (_NDType == 257)
		{
			_C[8] = _NDValue;
			write((DataType)16);
			_P[8] = _NDValue >> 1;
			write((DataType)26);
			write((DataType)36);
		}
		else if (16449536 <= _NDType && _NDType <= 16449663)
		{
			int num = _NDType - 16449536;
			if (_NDCANInt4321 == null)
			{
				Initialize_NDCANInt4321();
			}
			_NDCANInt4321[num] = _NDValue;
			write((DataType)(581 + num));
		}
		else if (16646144 <= _NDType && _NDType <= 16646271)
		{
			if (_MODBUSInt4321 == null)
			{
				Initialize_MODBUSInt();
			}
			if (_MODBUSFloat4321 == null)
			{
				Initialize_MODBUSFloat();
			}
			int num2 = _NDType - 16646144;
			buff[0] = (byte)_NDValue;
			buff[1] = (byte)(_NDValue >> 8);
			buff[2] = (byte)(_NDValue >> 16);
			buff[3] = (byte)(_NDValue >> 24);
			_MODBUSInt4321[num2] = _NDValue;
			_MODBUSFloat4321[num2] = BitConverter.ToSingle(buff, 0);
			byte b = buff[0];
			buff[0] = buff[2];
			buff[2] = b;
			byte b2 = buff[1];
			buff[1] = buff[3];
			buff[3] = b2;
			_MODBUSInt2143[num2] = BitConverter.ToInt32(buff, 0);
			_MODBUSFloat2143[num2] = BitConverter.ToSingle(buff, 0);
			b = buff[0];
			buff[0] = buff[3];
			buff[3] = b;
			b2 = buff[1];
			buff[1] = buff[2];
			buff[2] = b2;
			_MODBUSInt3412[num2] = BitConverter.ToInt32(buff, 0);
			_MODBUSFloat3412[num2] = BitConverter.ToSingle(buff, 0);
			b = buff[0];
			buff[0] = buff[2];
			buff[2] = b;
			b2 = buff[1];
			buff[1] = buff[3];
			buff[3] = b2;
			_MODBUSInt1234[num2] = BitConverter.ToInt32(buff, 0);
			_MODBUSFloat1234[num2] = BitConverter.ToSingle(buff, 0);
			write((DataType)(709 + num2));
		}
	}

	private void setDisplayStatusRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		long value = ((DeviceDisplayStatusRecord)_prm.rec).Value;
		switch (value & 0xFF)
		{
		case 17L:
			_DispStatus = (int)((value >> 8) & 0xFF);
			write(DataType.dispStatus);
			break;
		case 18L:
		case 19L:
			_NaviIsSet = (value & 1) == 0;
			_NaviSet = (int)((value >> 8) & 0xFFFF);
			_NaviStatus = (int)((value >> 24) & 0xFFFF);
			_NaviGroup = (int)(value >> 48);
			_NaviSubgroup = (int)((value >> 40) & 0xFF);
			write(DataType.naviStatus);
			break;
		}
	}

	private void setTachographRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected I4, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected I4, but got Unknown
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected I4, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Expected I4, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected I4, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected I4, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected I4, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected I4, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected I4, but got Unknown
		DeviceTachographRecord val = (DeviceTachographRecord)_prm.rec;
		_WorkingState0 = (int)val.Driver1WorkingState;
		_WorkingState1 = (int)val.Driver2WorkingState;
		_RelatedState0 = (int)val.Driver1TimeRelatedState;
		_RelatedState1 = (int)val.Driver2TimeRelatedState;
		_DriverCard0 = (int)val.Driver1Card;
		_DriverCard1 = (int)val.Driver2Card;
		_VehicleMotion = (int)val.VehicleMotion;
		_VehicleOverspeed = (int)val.VehicleOverspeed;
		_VehicleSpeed = val.VehicleSpeed;
		_ShaftRPM = val.ShaftSpeed;
		_MoveDirection = (int)val.DirectionIndicator;
		write(DataType.tachograph);
	}

	[AGInfo("Wheel temperature", "Wheel temperature in °C.", "°C", DeviceParameterKind.InstExp, AGInfoGroupType.Temperature, false, new int[] { 61 })]
	public int WT([AGPrmInfo("Index of axis", 1.0, 16.0)] int axis, [AGPrmInfo("Index of wheel", 1.0, 16.0)] int wheel)
	{
		if (1 <= axis && axis <= 16 && 1 <= wheel && wheel <= 16)
		{
			axis--;
			wheel--;
			read((DataType)(842 + axis * 16 + wheel));
			if (_WT == null)
			{
				return 0;
			}
			return _WT[axis, wheel];
		}
		return 0;
	}

	[AGInfo("Wheel pressure", "Wheel pressure in kPa.", "kPa", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Pressure, false, new int[] { 61 })]
	public double WP([AGPrmInfo("Index of axis", 1.0, 16.0)] int axis, [AGPrmInfo("Index of wheel", 1.0, 16.0)] int wheel)
	{
		if (1 <= axis && axis <= 16 && 1 <= wheel && wheel <= 16)
		{
			axis--;
			wheel--;
			read((DataType)(842 + axis * 16 + wheel));
			if (_WP == null)
			{
				return 0.0;
			}
			return _WP[axis, wheel];
		}
		return 0.0;
	}

	[AGInfo("Wheel alerts", "Wheel alerts: Bits: 0 - low pressure, 1 - high pressure, 2 - high temperature, 3 - sensor battery low, 5 - high air escaping, 6 - low air escaping, 7 - loss of communication with the sensor.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, new int[] { 61 })]
	public byte WA([AGPrmInfo("Index of axis", 1.0, 16.0)] int axis, [AGPrmInfo("Index of wheel", 1.0, 16.0)] int wheel)
	{
		if (1 <= axis && axis <= 16 && 1 <= wheel && wheel <= 16)
		{
			axis--;
			wheel--;
			read((DataType)(842 + axis * 16 + wheel));
			if (_WA == null)
			{
				return 0;
			}
			return _WA[axis, wheel];
		}
		return 0;
	}

	private void setWheelStateRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		DeviceWheelPressureRecord val = (DeviceWheelPressureRecord)_prm.rec;
		_WheelAxis = val.Axle;
		_WheelIndex = val.Wheel;
		write(DataType.wheelIndex);
		if (_WheelAxis < 16 && _WheelIndex < 16 && val.Pressure.HasValue)
		{
			if (_WT == null)
			{
				Initialize_WT();
			}
			if (_WP == null)
			{
				Initialize_WP();
			}
			if (_WA == null)
			{
				Initialize_WA();
			}
			_WT[_WheelAxis, _WheelIndex] = (val.Temperature.HasValue ? val.Temperature.Value : 0);
			_WP[_WheelAxis, _WheelIndex] = val.Pressure.Value * 100.0;
			_WA[_WheelAxis, _WheelIndex] = (byte)val.Alerts;
			write((DataType)(842 + _WheelAxis * 16 + _WheelIndex));
		}
	}

	[AGInfo("Forward angle of LLS by index", "Forward angle in ° by index.", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 62 })]
	public int FLLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(1098 + num));
		return _FLLS[num];
	}

	[AGInfo("Side angle of LLS by index", "Side angle in ° by index.", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 62 })]
	public int SLLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(1098 + num));
		return _SLLS[num];
	}

	[AGInfo("Angle from vertial of LLS by index", "Angle from vertial in ° by index.", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 62 })]
	public int ALLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(1098 + num));
		return _ALLS[num];
	}

	private void setLS1ExtRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		LS1ExtItem[] values = ((DeviceLS1ExtRecord)_prm.rec).Values;
		foreach (LS1ExtItem val in values)
		{
			int num = val.SensorID - 1;
			_VLLS[num] = val.Valid;
			write((DataType)(90 + num));
			if (val.Valid)
			{
				_FLLS[num] = val.ForwardAngle;
				_SLLS[num] = val.SideAngle;
				_ALLS[num] = val.VerticalAngle;
				write((DataType)(1098 + num));
			}
		}
	}

	[AGInfo("Battary voltage of LLS by index", "Battary voltage in volts by index.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 63 })]
	public double VoltLLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0.0;
		}
		int num = index - 1;
		read((DataType)(1435 + num));
		return _VoltLLS[num];
	}

	[AGInfo("RSSI of LLS by index", "RSSI in dBm by index.", "dBm", DeviceParameterKind.InstTime, AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 63 })]
	public int RSSILLS([AGPrmInfo("Index of sensor", 1.0, 8.0)] int index)
	{
		if (index < 1 || index > 8)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(1435 + num));
		return _RSSILLS[num];
	}

	private void setLS2ExtRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		LS2ExtItem[] values = ((DeviceLS2ExtRecord)_prm.rec).Values;
		foreach (LS2ExtItem val in values)
		{
			int num = val.SensorID - 1;
			_VLLS[num] = val.Valid;
			write((DataType)(90 + num));
			if (val.Valid)
			{
				_VoltLLS[num] = val.Voltage;
				_RSSILLS[num] = val.RSSI;
				write((DataType)(1435 + num));
			}
		}
	}

	[AGInfo("Struna+ registry 1", "Struna+ registry 1.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure | AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public int Struna1([AGPrmInfo("Channel", 0.0, 15.0)] int channel, [AGPrmInfo("Data type", 0.0, 15.0)] int dataType)
	{
		if (0 <= channel && channel < 16 && 0 <= dataType && dataType < 16)
		{
			read((DataType)(1107 + channel * 16 + dataType));
			if (_StrunaReg1Arr == null)
			{
				return 0;
			}
			return _StrunaReg1Arr[channel, dataType];
		}
		return 0;
	}

	[AGInfo("Struna+ registry 2", "Struna+ registry 2.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure | AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public int Struna2([AGPrmInfo("Channel", 0.0, 15.0)] int channel, [AGPrmInfo("Data type", 0.0, 15.0)] int dataType)
	{
		if (0 <= channel && channel < 16 && 0 <= dataType && dataType < 16)
		{
			read((DataType)(1107 + channel * 16 + dataType));
			if (_StrunaReg2Arr == null)
			{
				return 0;
			}
			return _StrunaReg2Arr[channel, dataType];
		}
		return 0;
	}

	[AGInfo("Struna+ float", "Struna+ float.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.FuelLevel | AGInfoGroupType.Pressure | AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public double StrunaF([AGPrmInfo("Channel", 0.0, 15.0)] int channel, [AGPrmInfo("Data type", 0.0, 15.0)] int dataType)
	{
		if (0 <= channel && channel < 16 && 0 <= dataType && dataType < 16)
		{
			read((DataType)(1107 + channel * 16 + dataType));
			uint num = 0u;
			if (_StrunaReg1Arr != null)
			{
				num = (uint)_StrunaReg1Arr[channel, dataType];
			}
			uint num2 = 0u;
			if (_StrunaReg2Arr != null)
			{
				num2 = (uint)_StrunaReg2Arr[channel, dataType];
			}
			num = (num >> 8) | ((num & 0xFF) << 8);
			num2 = (num2 >> 8) | ((num2 & 0xFF) << 8);
			return BitConverter.ToSingle(BitConverter.GetBytes((num2 << 16) | num), 0);
		}
		return 0.0;
	}

	[AGInfo("Struna+ status", "Struna+ status.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 64 })]
	public int StrunaS([AGPrmInfo("Channel", 0.0, 15.0)] int channel, [AGPrmInfo("Data type", 0.0, 15.0)] int dataType)
	{
		if (0 <= channel && channel < 16 && 0 <= dataType && dataType < 16)
		{
			read((DataType)(1107 + channel * 16 + dataType));
			if (_StrunaReg3Arr == null)
			{
				return 0;
			}
			return _StrunaReg3Arr[channel, dataType];
		}
		return 0;
	}

	private void setStrunaDataRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceStrunaParamRecord val = (DeviceStrunaParamRecord)_prm.rec;
		_StrunaChannel = val.Channel;
		_StrunaDataType = val.DataType;
		_StrunaRegistry1 = val.Registry1;
		_StrunaRegistry2 = val.Registry2;
		_StrunaRegistry3 = val.Registry3;
		write(DataType.struna);
		if (_StrunaReg1Arr == null)
		{
			Initialize_StrunaReg1Arr();
		}
		if (_StrunaReg2Arr == null)
		{
			Initialize_StrunaReg2Arr();
		}
		if (_StrunaReg3Arr == null)
		{
			Initialize_StrunaReg3Arr();
		}
		_StrunaReg1Arr[_StrunaChannel, _StrunaDataType] = _StrunaRegistry1;
		_StrunaReg2Arr[_StrunaChannel, _StrunaDataType] = _StrunaRegistry2;
		_StrunaReg3Arr[_StrunaChannel, _StrunaDataType] = _StrunaRegistry3;
		write((DataType)(1107 + _StrunaChannel * 16 + _StrunaDataType));
	}

	private void setDrivingQualityDataRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceDrivingQualityRecord val = (DeviceDrivingQualityRecord)_prm.rec;
		_DQExtAcc = val.IsExternal;
		_DQType = val.Type;
		write(DataType.drivingQuality);
		if (_DQType < 8)
		{
			if (_DQEnd == null)
			{
				Initialize_DQEnd();
			}
			if (_DQDuration == null)
			{
				Initialize_DQDuration();
			}
			if (_DQAccelMax == null)
			{
				Initialize_DQAccelMax();
			}
			if (_DQAccelAver == null)
			{
				Initialize_DQAccelAver();
			}
			_DQEnd[_DQType] = val.End;
			_DQDuration[_DQType] = val.Duration;
			_DQAccelMax[_DQType] = Math.Abs(val.AccelMax);
			_DQAccelAver[_DQType] = Math.Abs(val.AccelAver);
			write((DataType)(1364 + _DQType));
		}
	}

	[AGInfo("Output 1 TKAM", "Output TKAM by channel.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 66 })]
	public bool TKAMOut1([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return false;
		}
		int num = channel - 1;
		read((DataType)(1373 + num));
		if (_TKAMOuts == null)
		{
			return false;
		}
		return (_TKAMOuts[num] & 1) != 0;
	}

	[AGInfo("Output 2 TKAM", "Output TKAM by channel.", DeviceParameterKind.Flag, AGInfoGroupType.Sensor | AGInfoGroupType.RS485 | AGInfoGroupType.Motohours, false, new int[] { 66 })]
	public bool TKAMOut2([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return false;
		}
		int num = channel - 1;
		read((DataType)(1373 + num));
		if (_TKAMOuts == null)
		{
			return false;
		}
		return (_TKAMOuts[num] & 2) != 0;
	}

	[AGInfo("Tilt angle TKAM", "Tilt angle TKAM by channel.", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 66, 74 })]
	public double TKAMAngle([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return 0.0;
		}
		int num = channel - 1;
		read((DataType)(1373 + num), (DataType)(1477 + num));
		if (_TKAMAngle == null)
		{
			return 0.0;
		}
		return _TKAMAngle[num];
	}

	[AGInfo("Temperature TKAM", "Temperature TKAM by channel.", "°C", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Temperature | AGInfoGroupType.RS485, false, new int[] { 66, 74 })]
	public int TKAMTemperature([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return 0;
		}
		int num = channel - 1;
		read((DataType)(1389 + num), (DataType)(1477 + num));
		if (_TKAMTemperature == null)
		{
			return 0;
		}
		return _TKAMTemperature[num];
	}

	[AGInfo("Vibration TKAM", "Vibration TKAM by channel.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 66 })]
	public int TKAMVibration([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return 0;
		}
		int num = channel - 1;
		read((DataType)(1389 + num));
		if (_TKAMVibration == null)
		{
			return 0;
		}
		return _TKAMVibration[num];
	}

	[AGInfo("Roulis angle TKAM", "Roulis angle TKAM by channel.", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 66 })]
	public double TKAMRoulis([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return 0.0;
		}
		int num = channel - 1;
		read((DataType)(1405 + num));
		if (_TKAMRoulis == null)
		{
			return 0.0;
		}
		return _TKAMRoulis[num];
	}

	[AGInfo("Tangage angle TKAM", "Tangage angle TKAM by channel.", "°", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 66 })]
	public double TKAMTangage([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return 0.0;
		}
		int num = channel - 1;
		read((DataType)(1405 + num));
		if (_TKAMTangage == null)
		{
			return 0.0;
		}
		return _TKAMTangage[num];
	}

	[AGInfo("Battery voltage TKAM", "Battery voltage TKAM by channel.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 66, 74 })]
	public double TKAMVoltage([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return 0.0;
		}
		int num = channel - 1;
		read((DataType)(1405 + num), (DataType)(1477 + num));
		if (_TKAMVoltage == null)
		{
			return 0.0;
		}
		return _TKAMVoltage[num];
	}

	[AGInfo("RSSI TKAM", "RSSI in dBm TKAM by channel.", "V", DeviceParameterKind.InstTime, AGInfoGroupType.Data | AGInfoGroupType.Level | AGInfoGroupType.RS485, false, new int[] { 66, 74 })]
	public int TKAMRSSI([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return 0;
		}
		int num = channel - 1;
		read((DataType)(1405 + num), (DataType)(1477 + num));
		if (_TKAMRSSI == null)
		{
			return 0;
		}
		return _TKAMRSSI[num];
	}

	private void setTKAMRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceTKAMRecord val = (DeviceTKAMRecord)_prm.rec;
		int channel = val.Channel;
		_TKAMChannel = channel + 1;
		write(DataType.tkamCH);
		if (_TKAMOuts == null)
		{
			Initialize_TKAMOuts();
		}
		if (_TKAMAngle == null)
		{
			Initialize_TKAMAngle();
		}
		_TKAMOuts[channel] = val.Outs;
		_TKAMAngle[channel] = val.Angle;
		write((DataType)(1373 + channel));
		if (val.Temperature.HasValue)
		{
			if (_TKAMTemperature == null)
			{
				Initialize_TKAMTemperature();
			}
			_TKAMTemperature[channel] = val.Temperature.Value;
			write((DataType)(1389 + channel));
		}
		if (val.Vibration.HasValue)
		{
			if (_TKAMVibration == null)
			{
				Initialize_TKAMVibration();
			}
			_TKAMVibration[channel] = val.Vibration.Value;
			write((DataType)(1389 + channel));
		}
		if (val.Roulis.HasValue)
		{
			if (_TKAMRoulis == null)
			{
				Initialize_TKAMRoulis();
			}
			_TKAMRoulis[channel] = val.Roulis.Value;
			write((DataType)(1405 + channel));
		}
		if (val.Tangage.HasValue)
		{
			if (_TKAMTangage == null)
			{
				Initialize_TKAMTangage();
			}
			_TKAMTangage[channel] = val.Tangage.Value;
			write((DataType)(1405 + channel));
		}
		if (val.Voltage.HasValue)
		{
			if (_TKAMVoltage == null)
			{
				Initialize_TKAMVoltage();
			}
			_TKAMVoltage[channel] = val.Voltage.Value;
			write((DataType)(1405 + channel));
		}
		if (val.RSSI.HasValue)
		{
			if (_TKAMRSSI == null)
			{
				Initialize_TKAMRSSI();
			}
			_TKAMRSSI[channel] = val.RSSI.Value;
			write((DataType)(1405 + channel));
		}
	}

	private void setWCSRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceWCSRecord val = (DeviceWCSRecord)_prm.rec;
		_WCSChannel = val.Channel;
		_WCSIn0 = val.In0;
		_WCSStatusLight = val.StatusLight;
		_WCSWeight = val.Weight;
		write(DataType.wcs);
		if (val.Frequency.HasValue)
		{
			_WCSFrequency = val.Frequency.Value;
			write((DataType)1422);
		}
		if (val.ErrorCode.HasValue)
		{
			_WCSErrorCode = val.ErrorCode.Value;
			write((DataType)1423);
		}
	}

	private void setTrailerWeightRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceTrailerWeightRecord val = (DeviceTrailerWeightRecord)_prm.rec;
		if (val.CouplerLoad.HasValue)
		{
			_TWCouplerLoad = val.CouplerLoad.Value;
			write(DataType.trailerWeight);
		}
		if (val.LoadWeight.HasValue)
		{
			_TWLoadWeight = val.LoadWeight.Value;
			write((DataType)1425);
		}
		if (val.TrailerWeight.HasValue)
		{
			_TWTrailerWeight = val.TrailerWeight.Value;
			write((DataType)1426);
		}
	}

	private void setDoorsStatusesRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		_ = (DeviceDoorsStatusesRecord)_prm.rec;
	}

	private void setVehicleStatusdRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceVehicleStatusRecord val = (DeviceVehicleStatusRecord)_prm.rec;
		if (val.VSBC1Pressure.HasValue)
		{
			_VSBC1Pressure = (int)val.VSBC1Pressure.Value;
			write(DataType.vehicleStatus);
		}
		if (val.VSBC2Pressure.HasValue)
		{
			_VSBC2Pressure = (int)val.VSBC2Pressure.Value;
			write((DataType)1444);
		}
		if (val.VSGear.HasValue)
		{
			_VSGear = val.VSGear.Value;
			write((DataType)1445);
		}
		if (val.VSTotalWeight.HasValue)
		{
			_VSTotalWeight = (int)val.VSTotalWeight.Value;
			write((DataType)1446);
		}
	}

	private void setAlternateFuelRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceAlternateFuelRecord val = (DeviceAlternateFuelRecord)_prm.rec;
		if (val.AFConsump.HasValue)
		{
			_CANAlternateFtotal = val.AFConsump.Value;
			write(DataType.canAlternate);
		}
	}

	[AGInfo("Status FMS1", "Status FMS1.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level, false, new int[] { 72 })]
	public int FMS1Status([AGPrmInfo("Block ID", 1.0, 16.0)] int BlockID, [AGPrmInfo("Status ID", 1.0, 15.0)] int StatusID)
	{
		if (BlockID < 1 || BlockID > 16)
		{
			return 0;
		}
		if (StatusID < 1 || StatusID > 15)
		{
			return 0;
		}
		int num = BlockID - 1;
		int num2 = StatusID - 1;
		read((DataType)(1448 + num));
		if (_FMS1Status == null)
		{
			return 0;
		}
		return (int)((_FMS1Status[num] >> 3 * num2) & 7);
	}

	private void setFMS1Record()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (_FMS1Status == null)
		{
			Initialize_FMS1Status();
		}
		DeviceFMS1Record val = (DeviceFMS1Record)_prm.rec;
		int iD = val.ID;
		write((DataType)(1448 + val.ID));
		_FMS1Status[iD] = 0L;
		int num = 0;
		int num2 = 0;
		while (num < val.Statuses.Length)
		{
			_FMS1Status[iD] |= (long)((ulong)val.Statuses[num] << num2);
			num++;
			num2 += 3;
		}
	}

	private void setDiscreteParametersRecord()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected I4, but got Unknown
		if (_DiscreteParameters == null)
		{
			Initialize_DiscreteParameters();
		}
		DeviceDiscreteParamsRecord val = (DeviceDiscreteParamsRecord)_prm.rec;
		_ISOBUSGroup = val.GroupNumber;
		_ISOBUSSource = (int)val.Source;
		write(DataType.discreteParams);
		int num = val.GroupNumber - 1;
		if (0 <= num && num < _DiscreteParameters.Length)
		{
			_DiscreteParameters[num] = val.Data;
			write((DataType)(1465 + num));
		}
	}

	[AGInfo("Event state TKAM", "Event state TKAM by channel.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.RS485, false, new int[] { 74 })]
	public int TKAMEventState([AGPrmInfo("Channel", 1.0, 16.0)] int channel)
	{
		if (channel < 1 || channel > 16)
		{
			return 0;
		}
		int num = channel - 1;
		read((DataType)(1477 + num));
		if (_TKAMEventState == null)
		{
			return 0;
		}
		return _TKAMEventState[num];
	}

	private void setTKAM2Record()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceTKAM2Record val = (DeviceTKAM2Record)_prm.rec;
		int channel = val.Channel;
		_TKAMChannel = channel + 1;
		write(DataType.tkam2CH);
		if (_TKAMAngle == null)
		{
			Initialize_TKAMAngle();
		}
		if (_TKAMTemperature == null)
		{
			Initialize_TKAMTemperature();
		}
		if (_TKAMEventState == null)
		{
			Initialize_TKAMEventState();
		}
		if (_TKAMVoltage == null)
		{
			Initialize_TKAMVoltage();
		}
		if (_TKAMRSSI == null)
		{
			Initialize_TKAMRSSI();
		}
		_TKAMAngle[channel] = val.Angle;
		_TKAMTemperature[channel] = val.Temperature;
		_TKAMEventState[channel] = val.EventState;
		_TKAMVoltage[channel] = val.Voltage;
		_TKAMRSSI[channel] = val.RSSI;
		write((DataType)(1477 + channel));
	}

	private void setNamedParamHeader()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceNamedParamHeader val = (DeviceNamedParamHeader)_prm.rec;
		_NPName1 = val.Name1;
		_NPName2 = val.Name2;
		write(DataType.namedParamHeader);
	}

	private void setNamedParamData()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		DeviceNamedParamData val = (DeviceNamedParamData)_prm.rec;
		if (_NPName1 != 0L)
		{
			_NPName = $"{(char)(_NPName1 & 0xFF)}{(char)((_NPName1 >> 8) & 0xFF)}{(char)((_NPName1 >> 16) & 0xFF)}{(char)(_NPName2 & 0xFF)}{(char)((_NPName2 >> 8) & 0xFF)}{(char)((_NPName2 >> 16) & 0xFF)}{(char)(_NPName2 >> 24)}{(char)(val.Name3 & 0xFF)}{(char)((val.Name3 >> 8) & 0xFF)}";
		}
		_NPName1 = 0L;
		_NPName2 = 0L;
		_NPType = (int)val.DataType;
		write(DataType.namedParamData);
		switch (_NPType)
		{
		case 1:
			_NPUIntDict[_NPName] = (_NPUInt = val.Value);
			write(DataType.namedParamUint);
			write(_NPName + "+long");
			break;
		case 2:
			_NPIntDict[_NPName] = (_NPInt = (int)val.Value);
			write(DataType.namedParamInt);
			write(_NPName + "+int");
			break;
		case 3:
			_NPFloatDict[_NPName] = (_NPFloat = BitConverter.ToSingle(BitConverter.GetBytes(val.Value), 0));
			write(DataType.namedParamFloat);
			write(_NPName + "+double");
			break;
		}
	}

	private void setNamedArrayHeader()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		_ = (DeviceNamedArrayHeader)_prm.rec;
		write(DataType.namedArrayHeader);
	}

	private void setNamedArraySubhead()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		_ = (DeviceNamedArraySubhead)_prm.rec;
		write(DataType.namedArraySubhead);
	}

	private void setNamedArrayData()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		_ = (DeviceNamedArrayData)_prm.rec;
		write(DataType.namedArrayData);
	}

	[AGInfo("Liters by CAN by index", "Liters by CAN bus indication.", "l", DeviceParameterKind.InstTime, AGInfoGroupType.FuelLevel | AGInfoGroupType.CAN, false, new int[] { 80 })]
	public int CANLiters([AGPrmInfo("Index of level", 1.0, 12.0)] int index)
	{
		if (index < 1 || index > 12)
		{
			return 0;
		}
		int num = index - 1;
		read((DataType)(1501 + num));
		return _CANLiters[num];
	}

	private void setCANLitersRecord()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		CANLitersItem[] values = ((DeviceCANLitersRecord)_prm.rec).Values;
		foreach (CANLitersItem val in values)
		{
			if (val.Valid)
			{
				int num = val.TankID - 1;
				_CANLiters[num] = val.Value;
				write((DataType)(1501 + num));
			}
		}
	}

	public void SetTabularFieldsParams(TabularFieldsParams prm)
	{
		_prm = prm;
	}

	public virtual void copyFrom(CrdIndependentFields value)
	{
		//IL_0af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		if (value == null)
		{
			return;
		}
		_prm = value._prm;
		_UDT = value._UDT;
		_SRaw = value._SRaw;
		_IntRcv = value._IntRcv;
		_PrevPSE = value._PrevPSE;
		_NextPSE = value._NextPSE;
		_Src = value._Src;
		_LonRaw = value._LonRaw;
		_LatRaw = value._LatRaw;
		_Outputs = value._Outputs;
		_A0 = value._A0;
		_A1 = value._A1;
		_MainVoltRaw = value._MainVoltRaw;
		_MainVolt = value._MainVolt;
		_ResVoltRaw = value._ResVoltRaw;
		_ResVolt = value._ResVolt;
		_Processor = value._Processor;
		for (int i = 0; i < 9; i++)
		{
			_AVolt[i] = value._AVolt[i];
		}
		if (value._NFC != null)
		{
			if (_NFC == null)
			{
				Initialize_NFC();
			}
			Array.Copy(value._NFC, _NFC, 9);
		}
		for (int j = 0; j < 9; j++)
		{
			_C[j] = value._C[j];
			_P[j] = value._P[j];
		}
		_AltRaw = value._AltRaw;
		_SpeedRaw = value._SpeedRaw;
		_CourseRaw = value._CourseRaw;
		_Sats = value._Sats;
		_HDOP = value._HDOP;
		_ID1W = value._ID1W;
		_IDButton = value._IDButton;
		_IDBLE = value._IDBLE;
		_IDCAN = value._IDCAN;
		_IDSN = value._IDSN;
		if (value._Card != null)
		{
			if (_Card == null)
			{
				Initialize_Card();
			}
			Array.Copy(value._Card, _Card, 16);
		}
		Array.Copy(value._LLS, _LLS, 8);
		Array.Copy(value._VLLS, _VLLS, 8);
		_CANSpeed = value._CANSpeed;
		_CANCruise = value._CANCruise;
		_CANBrake = value._CANBrake;
		_CANHandbrake = value._CANHandbrake;
		_CANCoupling = value._CANCoupling;
		_CANGaz = value._CANGaz;
		_CANFtotal = value._CANFtotal;
		Array.Copy(value._CANL, _CANL, 6);
		_CANLAB = value._CANLAB;
		_CANErpmRaw = value._CANErpmRaw;
		_CANDmaint = value._CANDmaint;
		_CANEmh = value._CANEmh;
		_CANLOGEmh = value._CANLOGEmh;
		_CANPoil = value._CANPoil;
		_CANTcool = value._CANTcool;
		_CANToil = value._CANToil;
		_CANTfuel = value._CANTfuel;
		_CANPman = value._CANPman;
		_CANTboost = value._CANTboost;
		_CANPboost = value._CANPboost;
		_CANDtotal = value._CANDtotal;
		_CANDdaily = value._CANDdaily;
		_EventID = value._EventID;
		_AdditionalInfo[0] = value._AdditionalInfo[0];
		_AdditionalInfo[1] = value._AdditionalInfo[1];
		_AdditionalInfo[2] = value._AdditionalInfo[2];
		_AdditionalInfo[3] = value._AdditionalInfo[3];
		_AdditionalInfo[4] = value._AdditionalInfo[4];
		_EventReason = value._EventReason;
		if (value._CANAW != null)
		{
			if (_CANAW == null)
			{
				Initialize_CANAW();
			}
			for (int k = 0; k < 16; k++)
			{
				for (int l = 0; l < 6; l++)
				{
					_CANAW[k, l] = value._CANAW[k, l];
				}
			}
		}
		_UDType = value._UDType;
		if (value._UDArrs != null)
		{
			if (_UDArrs == null)
			{
				Initialize_UDArrs();
			}
			for (int m = 0; m < 4; m++)
			{
				Array.Copy(value._UDArrs[m], _UDArrs[m], 6);
			}
		}
		_DevT = value._DevT;
		Array.Copy(value._Temper, _Temper, 8);
		_InputsP = value._InputsP;
		_InputsM = value._InputsM;
		_IStatus = value._IStatus;
		_FAID = value._FAID;
		_CardID = value._CardID;
		_FRChannel = value._FRChannel;
		_FRAddr = value._FRAddr;
		_FRTotal = value._FRTotal;
		_FDDuration = value._FDDuration;
		_FDID = value._FDID;
		_PTChannel = value._PTChannel;
		if (value._PTMode != null)
		{
			if (_PTMode == null)
			{
				Initialize_PTMode();
			}
			Array.Copy(value._PTMode, _PTMode, 16);
		}
		if (value._PTStatus != null)
		{
			if (_PTStatus == null)
			{
				Initialize_PTStatus();
			}
			Array.Copy(value._PTStatus, _PTStatus, 16);
		}
		if (value._PTIn != null)
		{
			if (_PTIn == null)
			{
				Initialize_PTIn();
			}
			Array.Copy(value._PTIn, _PTIn, 16);
		}
		if (value._PTOut != null)
		{
			if (_PTOut == null)
			{
				Initialize_PTOut();
			}
			Array.Copy(value._PTOut, _PTOut, 16);
		}
		if (value._PTMDStatus != null)
		{
			if (_PTMDStatus == null)
			{
				Initialize_PTMDStatus();
			}
			Array.Copy(value._PTMDStatus, _PTMDStatus, 16);
		}
		if (value._PTMDMode != null)
		{
			if (_PTMDMode == null)
			{
				Initialize_PTMDMode();
			}
			Array.Copy(value._PTMDMode, _PTMDMode, 16);
		}
		if (value._PTCtgIn != null)
		{
			if (_PTCtgIn == null)
			{
				Initialize_PTCtgIn();
			}
			for (int n = 0; n < 16; n++)
			{
				for (int num = 0; num < 8; num++)
				{
					_PTCtgIn[n, num] = value._PTCtgIn[n, num];
				}
			}
		}
		if (value._PTCtgOut != null)
		{
			if (_PTCtgOut == null)
			{
				Initialize_PTCtgOut();
			}
			for (int num2 = 0; num2 < 16; num2++)
			{
				for (int num3 = 0; num3 < 8; num3++)
				{
					_PTCtgOut[num2, num3] = value._PTCtgOut[num2, num3];
				}
			}
		}
		_MDChannel = value._MDChannel;
		if (value._MDMode != null)
		{
			if (_MDMode == null)
			{
				Initialize_MDMode();
			}
			Array.Copy(value._MDMode, _MDMode, 16);
		}
		if (value._MDStatus != null)
		{
			if (_MDStatus == null)
			{
				Initialize_MDStatus();
			}
			Array.Copy(value._MDStatus, _MDStatus, 16);
		}
		if (value._MDL != null)
		{
			if (_MDL == null)
			{
				Initialize_MDL();
			}
			Array.Copy(value._MDL, _MDL, 16);
		}
		_CANFMI = value._CANFMI;
		_CANSPNv1 = value._CANSPNv1;
		_CANSPNv2 = value._CANSPNv2;
		_CANSPN = value._CANSPN;
		_CANErrorIsActive = value._CANErrorIsActive;
		_CANLampProtect = value._CANLampProtect;
		_CANLampAmber = value._CANLampAmber;
		_CANLampRed = value._CANLampRed;
		_CANLampMalfunc = value._CANLampMalfunc;
		_CANFlashLampStatus = value._CANFlashLampStatus;
		_CANOccurCount = value._CANOccurCount;
		_CANFinstant = value._CANFinstant;
		_CANFcalc = value._CANFcalc;
		_CANChoker = value._CANChoker;
		_CANTorquePercent = value._CANTorquePercent;
		_CANTorqueMode = value._CANTorqueMode;
		_CANIdlingMode = value._CANIdlingMode;
		_CANKickDownMode = value._CANKickDownMode;
		_BatteryVolt = value._BatteryVolt;
		_CANTair = value._CANTair;
		_CANPair = value._CANPair;
		_CANErpm = value._CANErpm;
		_CANEload = value._CANEload;
		_BatteryAmp = value._BatteryAmp;
		_LDRawLen = value._LDRawLen;
		_LDEnCnt = value._LDEnCnt;
		_LDType = value._LDType;
		_requiredLen = value._requiredLen;
		_requiredEnCnt = value._requiredEnCnt;
		_LDArr = new byte[value._LDArr.Length];
		Array.Copy(value._LDArr, _LDArr, value._LDArr.Length);
		_LDIndex = value._LDIndex;
		if (value._AGFCRefuellerID != null)
		{
			if (_AGFCRefuellerID == null)
			{
				Initialize_AGFCRefuellerID();
			}
			Array.Copy(value._AGFCRefuellerID, _AGFCRefuellerID, 16);
		}
		if (value._AGFCDriverID != null)
		{
			if (_AGFCDriverID == null)
			{
				Initialize_AGFCDriverID();
			}
			Array.Copy(value._AGFCDriverID, _AGFCDriverID, 16);
		}
		if (value._AGFCVolume != null)
		{
			if (_AGFCVolume == null)
			{
				Initialize_AGFCVolume();
			}
			Array.Copy(value._AGFCVolume, _AGFCVolume, 16);
		}
		if (value._AGFCDuration != null)
		{
			if (_AGFCDuration == null)
			{
				Initialize_AGFCDuration();
			}
			Array.Copy(value._AGFCDuration, _AGFCDuration, 16);
		}
		if (value._IRMAStatusS != null)
		{
			if (_IRMAStatusS == null)
			{
				Initialize_IRMAStatusS();
			}
			Array.Copy(value._IRMAStatusS, _IRMAStatusS, 8);
		}
		if (value._IRMAStatusA != null)
		{
			if (_IRMAStatusA == null)
			{
				Initialize_IRMAStatusA();
			}
			Array.Copy(value._IRMAStatusA, _IRMAStatusA, 8);
		}
		_LDDriver1 = value._LDDriver1;
		_LDDriver2 = value._LDDriver2;
		_LDCard1 = value._LDCard1;
		_LDCard2 = value._LDCard2;
		if (value._PALprmF != null)
		{
			if (_PALprmF == null)
			{
				Initialize_PALprmF();
			}
			Array.Copy(value._PALprmF, _PALprmF, freqPALPrmDict.Count);
		}
		if (value._PALprmA != null)
		{
			if (_PALprmA == null)
			{
				Initialize_PALprmA();
			}
			Array.Copy(value._PALprmA, _PALprmA, anlgPALPrmDict.Count);
		}
		_PALflags = value._PALflags;
		if (value._PALprmS != null)
		{
			if (_PALprmS == null)
			{
				Initialize_PALprmS();
			}
			Array.Copy(value._PALprmS, _PALprmS, statPALprmDict.Count);
		}
		Array.Copy(value._TLLS, _TLLS, 8);
		_ISOBUSType = value._ISOBUSType;
		_ISOBUSError = value._ISOBUSError;
		if (value._ISOBUSInt != null)
		{
			if (_ISOBUSInt == null)
			{
				Initialize_ISOBUSInt();
			}
			Array.Copy(value._ISOBUSInt, _ISOBUSInt, isobusPrmDict.Count);
		}
		if (value._ISOBUSFloat != null)
		{
			if (_ISOBUSFloat == null)
			{
				Initialize_ISOBUSFloat();
			}
			Array.Copy(value._ISOBUSFloat, _ISOBUSFloat, isobusPrmDict.Count);
		}
		if (value._ISOBUSUInt != null)
		{
			if (_ISOBUSUInt == null)
			{
				Initialize_ISOBUSUInt();
			}
			Array.Copy(value._ISOBUSUInt, _ISOBUSUInt, isobusPrmDict.Count);
		}
		_GuardState = value._GuardState;
		_GuardFlags = value._GuardFlags;
		_GuardIndicators = value._GuardIndicators;
		_NDType = value._NDType;
		_NDValue = value._NDValue;
		foreach (KeyValuePair<int, int> item in value._NDDict)
		{
			_NDDict.Add(item.Key, item.Value);
		}
		if (value._MODBUSInt4321 != null)
		{
			if (_MODBUSInt4321 == null)
			{
				Initialize_MODBUSInt();
			}
			Array.Copy(value._MODBUSInt4321, _MODBUSInt4321, 128);
			Array.Copy(value._MODBUSInt3412, _MODBUSInt3412, 128);
			Array.Copy(value._MODBUSInt2143, _MODBUSInt2143, 128);
			Array.Copy(value._MODBUSInt1234, _MODBUSInt1234, 128);
		}
		if (value._MODBUSFloat4321 != null)
		{
			if (_MODBUSFloat4321 == null)
			{
				Initialize_MODBUSFloat();
			}
			Array.Copy(value._MODBUSFloat4321, _MODBUSFloat4321, 128);
			Array.Copy(value._MODBUSFloat3412, _MODBUSFloat3412, 128);
			Array.Copy(value._MODBUSFloat2143, _MODBUSFloat2143, 128);
			Array.Copy(value._MODBUSFloat1234, _MODBUSFloat1234, 128);
		}
		if (value._NDCANInt4321 != null)
		{
			if (_NDCANInt4321 == null)
			{
				Initialize_NDCANInt4321();
			}
			Array.Copy(value._NDCANInt4321, _NDCANInt4321, 128);
		}
		_DispStatus = value._DispStatus;
		_NaviStatus = value._NaviStatus;
		_NaviIsSet = value._NaviIsSet;
		_NaviSet = value._NaviSet;
		_NaviStatus = value._NaviStatus;
		_NaviGroup = value._NaviGroup;
		_NaviSubgroup = value._NaviSubgroup;
		_WorkingState0 = value._WorkingState0;
		_WorkingState1 = value._WorkingState1;
		_RelatedState0 = value._RelatedState0;
		_RelatedState1 = value._RelatedState1;
		_DriverCard0 = value._DriverCard0;
		_DriverCard1 = value._DriverCard1;
		_VehicleMotion = value._VehicleMotion;
		_VehicleOverspeed = value._VehicleOverspeed;
		_VehicleSpeed = value._VehicleSpeed;
		_ShaftRPM = value._ShaftRPM;
		_MoveDirection = value._MoveDirection;
		if (value._WT != null)
		{
			if (_WT == null)
			{
				Initialize_WT();
			}
			for (int num4 = 0; num4 < 16; num4++)
			{
				for (int num5 = 0; num5 < 16; num5++)
				{
					_WT[num4, num5] = value._WT[num4, num5];
				}
			}
		}
		if (value._WP != null)
		{
			if (_WP == null)
			{
				Initialize_WP();
			}
			for (int num6 = 0; num6 < 16; num6++)
			{
				for (int num7 = 0; num7 < 16; num7++)
				{
					_WP[num6, num7] = value._WP[num6, num7];
				}
			}
		}
		if (value._WA != null)
		{
			if (_WA == null)
			{
				Initialize_WA();
			}
			for (int num8 = 0; num8 < 16; num8++)
			{
				for (int num9 = 0; num9 < 16; num9++)
				{
					_WA[num8, num9] = value._WA[num8, num9];
				}
			}
		}
		for (int num10 = 0; num10 < 8; num10++)
		{
			_FLLS[num10] = value._FLLS[num10];
			_SLLS[num10] = value._SLLS[num10];
			_ALLS[num10] = value._ALLS[num10];
		}
		for (int num11 = 0; num11 < 8; num11++)
		{
			_VoltLLS[num11] = value._VoltLLS[num11];
			_RSSILLS[num11] = value._RSSILLS[num11];
		}
		_StrunaChannel = value._StrunaChannel;
		_StrunaDataType = value._StrunaDataType;
		_StrunaRegistry1 = value._StrunaRegistry1;
		_StrunaRegistry2 = value._StrunaRegistry2;
		_StrunaRegistry3 = value._StrunaRegistry3;
		if (value._StrunaReg1Arr != null)
		{
			if (_StrunaReg1Arr == null)
			{
				Initialize_StrunaReg1Arr();
			}
			int num12 = 0;
			for (int num13 = 16; num12 < num13; num12++)
			{
				Array.Copy(value._StrunaReg1Arr, _StrunaReg1Arr, 16);
			}
		}
		if (value._StrunaReg2Arr != null)
		{
			if (_StrunaReg2Arr == null)
			{
				Initialize_StrunaReg2Arr();
			}
			for (int num14 = 0; num14 < 16; num14++)
			{
				Array.Copy(value._StrunaReg2Arr, _StrunaReg2Arr, 16);
			}
		}
		if (value._StrunaReg3Arr != null)
		{
			if (_StrunaReg3Arr == null)
			{
				Initialize_StrunaReg3Arr();
			}
			for (int num15 = 0; num15 < 16; num15++)
			{
				Array.Copy(value._StrunaReg3Arr, _StrunaReg3Arr, 16);
			}
		}
		_DQExtAcc = value._DQExtAcc;
		_DQType = value._DQType;
		if (value._DQEnd != null)
		{
			if (_DQEnd == null)
			{
				Initialize_DQEnd();
			}
			Array.Copy(value._DQEnd, _DQEnd, 8);
		}
		if (value._DQDuration != null)
		{
			if (_DQDuration == null)
			{
				Initialize_DQDuration();
			}
			Array.Copy(value._DQDuration, _DQDuration, 8);
		}
		if (value._DQAccelMax != null)
		{
			if (_DQAccelMax == null)
			{
				Initialize_DQAccelMax();
			}
			Array.Copy(value._DQAccelMax, _DQAccelMax, 8);
		}
		if (value._DQAccelAver != null)
		{
			if (_DQAccelAver == null)
			{
				Initialize_DQAccelAver();
			}
			Array.Copy(value._DQAccelAver, _DQAccelAver, 8);
		}
		if (value._TKAMOuts != null)
		{
			if (_TKAMOuts == null)
			{
				Initialize_TKAMOuts();
			}
			Array.Copy(value._TKAMOuts, _TKAMOuts, 16);
		}
		if (value._TKAMAngle != null)
		{
			if (_TKAMAngle == null)
			{
				Initialize_TKAMAngle();
			}
			Array.Copy(value._TKAMAngle, _TKAMAngle, 16);
		}
		if (value._TKAMTemperature != null)
		{
			if (_TKAMTemperature == null)
			{
				Initialize_TKAMTemperature();
			}
			Array.Copy(value._TKAMTemperature, _TKAMTemperature, 16);
		}
		if (value._TKAMEventState != null)
		{
			if (_TKAMEventState == null)
			{
				Initialize_TKAMEventState();
			}
			Array.Copy(value._TKAMEventState, _TKAMEventState, 16);
		}
		if (value._TKAMVibration != null)
		{
			if (_TKAMVibration == null)
			{
				Initialize_TKAMVibration();
			}
			Array.Copy(value._TKAMVibration, _TKAMVibration, 16);
		}
		if (value._TKAMRoulis != null)
		{
			if (_TKAMRoulis == null)
			{
				Initialize_TKAMRoulis();
			}
			Array.Copy(value._TKAMRoulis, _TKAMRoulis, 16);
		}
		if (value._TKAMTangage != null)
		{
			if (_TKAMTangage == null)
			{
				Initialize_TKAMTangage();
			}
			Array.Copy(value._TKAMTangage, _TKAMTangage, 16);
		}
		if (value._TKAMVoltage != null)
		{
			if (_TKAMVoltage == null)
			{
				Initialize_TKAMVoltage();
			}
			Array.Copy(value._TKAMVoltage, _TKAMVoltage, 16);
		}
		if (value._TKAMRSSI != null)
		{
			if (_TKAMRSSI == null)
			{
				Initialize_TKAMRSSI();
			}
			Array.Copy(value._TKAMRSSI, _TKAMRSSI, 16);
		}
		_TWCouplerLoad = value._TWCouplerLoad;
		_TWLoadWeight = value._TWLoadWeight;
		_TWTrailerWeight = value._TWTrailerWeight;
		_VSBC1Pressure = value._VSBC1Pressure;
		_VSBC2Pressure = value._VSBC2Pressure;
		_VSGear = value._VSGear;
		_VSTotalWeight = value._VSTotalWeight;
		_CANAlternateFtotal = value._CANAlternateFtotal;
		if (value._FMS1Status != null)
		{
			if (_FMS1Status == null)
			{
				Initialize_FMS1Status();
			}
			Array.Copy(value._FMS1Status, _FMS1Status, 16);
		}
		_ISOBUSGroup = value._ISOBUSGroup;
		_ISOBUSSource = value._ISOBUSSource;
		if (value._DiscreteParameters != null)
		{
			if (_DiscreteParameters == null)
			{
				Initialize_DiscreteParameters();
			}
			Array.Copy(value._DiscreteParameters, _DiscreteParameters, 8);
		}
		_NPName1 = value._NPName1;
		_NPName2 = value._NPName2;
		_NPName = value._NPName;
		_NPType = value._NPType;
		_NPUInt = value._NPUInt;
		_NPInt = value._NPInt;
		_NPFloat = value._NPFloat;
		foreach (KeyValuePair<string, long> item2 in value._NPUIntDict)
		{
			_NPUIntDict.Add(item2.Key, item2.Value);
		}
		foreach (KeyValuePair<string, int> item3 in value._NPIntDict)
		{
			_NPIntDict.Add(item3.Key, item3.Value);
		}
		foreach (KeyValuePair<string, double> item4 in value._NPFloatDict)
		{
			_NPFloatDict.Add(item4.Key, item4.Value);
		}
		Array.Copy(value._CANLiters, _CANLiters, 12);
	}

	[AGInfo("IQFalpr", "IQFalpr.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.CAN, false, new int[] { 48, 49 })]
	public int IQFalpr([AGPrmInfo("Seriousness", 1.0, 3.0)] int seriousness)
	{
		if (1 > seriousness || seriousness > 3)
		{
			return 0;
		}
		return LDByte(94 + seriousness, 1, 65534);
	}
}
