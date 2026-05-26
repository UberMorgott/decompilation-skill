using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using BINParser;

namespace AGInterfaces;

[Serializable]
public sealed class DataArrays
{
	[NonSerialized]
	public ComputeResult ComputeResult;

	[NonSerialized]
	public string imageName;

	public int imageHue;

	public Image image;

	[NonSerialized]
	public InitComputeInfo computeInfo;

	public DevPrmsGroupInfo[] tabularPrmsGroupInfoArray;

	public DevPrmsGroupInfo[] tripPrmsGroupInfoArray;

	public DevPrmsGroupInfo[] finalPrmsGroupInfoArray;

	public DevPrmsGroupChartInfo[] tabularPrmsGroupChartInfoArray;

	public IDisposable aga2Obj;

	public int[] fileID;

	public int[] oddTripID;

	public int[] evenTripID;

	public Color[] trackColor;

	public Color[] cursorColor;

	public int colorStartPos = -1;

	public int[] cursorImgs;

	public DeviceRecord[] records;

	public int[] gfRawLength;

	public Guid[,] gfRaw;

	public int[,] gfRawType;

	public int gfRawWithoutAreas;

	public int gfRawAreasIndex = -1;

	public int gfRawAreasOffset = -1;

	public bool emptyImplFiltrExists;

	public Guid[][] gfRoutes;

	public Quadro<Guid>[] mobileChP;

	public AGBitArray outOfMobileChP;

	public SortedDictionary<string, DevSwitchPrmStatusInfo[]> valArrays;

	public SortedDictionary<string, int> tickCountDict;

	public AGBitArray trueTypesArray;

	public AGBitArray canSetRecordsArray;

	public byte[] typeID;

	public BitArray intermedArray;

	public Array[] tabular;

	public Array[] trip;

	public Array totalTrip;

	public Array[] final;

	public LoaderInfo[] loadersInfo;

	public LoaderInfo[] routeLoadersInfo;

	public DateTime[] udt;

	[NonSerialized]
	public double[] supply;

	[NonSerialized]
	public InputFlags[] flags;

	[NonSerialized]
	public CrdFiltration[] llf;

	[NonSerialized]
	public int[] routeStatuses;

	public Coordinates[] crds;

	public double[] run;

	public long[] CANDtotalUpper;

	public long[] CANDtotalLower;

	public bool searchStreets;

	public bool searchPlaton;

	public bool searchAddrTabular;

	public bool searchAddrTrip;

	public bool searchAddrFinal;

	public LocationAddr[] address;

	public Guid[] driver;

	public Guid[] implement;

	public BitArray notEmptyimplement;

	public ReportTimeSpan[] noTripSpans;

	public ReportTimeSpan[] blindSpans;

	public ReportTimeSpan[] parkSpans;

	public SharerInfo[] sharersInfo;

	public TripRange[] tripRanges;

	public Array sharerIndexes;

	public SharedTripInfo[] tripsInfo;

	public string[,] sharer;

	public string[] deletedAreaFiles;

	public double[][] tripWeightSumArrays;

	public double[][] tripWeightArrays;

	public Array[][] usingInTripValues;

	public List<int>[] usingInTripUpdatedIndexes;

	public ValInfoLists tabularValLists;

	public ValInfoLists tabularUpdLists;

	public SortedDictionary<string, int> cachedWeightArrays;

	public SortedDictionary<string, int> cachedWeightSumArrays;

	public SortedDictionary<string, Array> cachedLimitArrays;

	public ImplementInfo[] completedAreasByArea;

	public ImplementInfo[] completedAreasByDevice;

	public static int[] createGeoFenceArray(out Guid[,] gfRaw, out int[,] gfRawType, out BitArray[] outOfGF, out GroupOrElementInfo[,] gfRawElem, int itemsCount, List<bool> supportOverlays)
	{
		int num = 0;
		int num2 = supportOverlays?.Count ?? 0;
		int[] array = new int[num2];
		for (int i = 0; i < num2; i++)
		{
			array[i] = ((!supportOverlays[i]) ? 1 : 4);
			num += array[i];
		}
		gfRaw = new Guid[num, itemsCount];
		gfRawType = new int[num, itemsCount];
		outOfGF = new BitArray[num2];
		gfRawElem = new GroupOrElementInfo[num, itemsCount];
		return array;
	}

	public void clear()
	{
		fileID = null;
		oddTripID = null;
		evenTripID = null;
		trackColor = null;
		cursorColor = null;
		colorStartPos = -1;
		cursorImgs = null;
		records = null;
		valArrays = null;
		tickCountDict = null;
		trueTypesArray = null;
		canSetRecordsArray = null;
		typeID = null;
		intermedArray = null;
		tabular = null;
		trip = null;
		totalTrip = null;
		final = null;
		loadersInfo = null;
		routeLoadersInfo = null;
		udt = null;
		supply = null;
		flags = null;
		llf = null;
		crds = null;
		run = null;
		CANDtotalUpper = null;
		CANDtotalLower = null;
		searchAddrTabular = false;
		searchAddrTrip = false;
		searchAddrFinal = false;
		address = null;
		driver = null;
		implement = null;
		sharersInfo = null;
		tripRanges = null;
		sharerIndexes = null;
		tripsInfo = null;
		sharer = null;
		deletedAreaFiles = null;
		gfRawLength = null;
		gfRaw = null;
		gfRawType = null;
		gfRawWithoutAreas = 0;
		gfRawAreasIndex = -1;
		gfRawAreasOffset = -1;
		emptyImplFiltrExists = false;
		tripWeightSumArrays = null;
		tripWeightArrays = null;
		usingInTripValues = null;
		usingInTripUpdatedIndexes = null;
		tabularValLists = null;
		tabularUpdLists = null;
		cachedWeightSumArrays = null;
		cachedWeightArrays = null;
		cachedLimitArrays = null;
	}

	public static void copy(DataArrays source, DataArrays dest)
	{
		if (source != null)
		{
			dest.imageName = source.imageName;
			dest.imageHue = source.imageHue;
			dest.computeInfo = source.computeInfo;
			dest.tabularPrmsGroupInfoArray = source.tabularPrmsGroupInfoArray;
			dest.tripPrmsGroupInfoArray = source.tripPrmsGroupInfoArray;
			dest.finalPrmsGroupInfoArray = source.finalPrmsGroupInfoArray;
			dest.tabularPrmsGroupChartInfoArray = source.tabularPrmsGroupChartInfoArray;
			dest.fileID = source.fileID;
			dest.oddTripID = source.oddTripID;
			dest.evenTripID = source.evenTripID;
			dest.trackColor = source.trackColor;
			dest.cursorColor = source.cursorColor;
			dest.colorStartPos = source.colorStartPos;
			dest.cursorImgs = source.cursorImgs;
			dest.records = source.records;
			dest.valArrays = source.valArrays;
			dest.tickCountDict = source.tickCountDict;
			dest.trueTypesArray = source.trueTypesArray;
			dest.canSetRecordsArray = source.canSetRecordsArray;
			dest.typeID = source.typeID;
			dest.intermedArray = source.intermedArray;
			dest.tabular = source.tabular;
			dest.trip = source.trip;
			dest.totalTrip = source.totalTrip;
			dest.final = source.final;
			dest.loadersInfo = source.loadersInfo;
			dest.routeLoadersInfo = source.routeLoadersInfo;
			dest.udt = source.udt;
			dest.supply = source.supply;
			dest.flags = source.flags;
			dest.llf = source.llf;
			dest.crds = source.crds;
			dest.run = source.run;
			dest.CANDtotalUpper = source.CANDtotalUpper;
			dest.CANDtotalLower = source.CANDtotalLower;
			dest.searchAddrTabular = source.searchAddrTabular;
			dest.searchAddrTrip = source.searchAddrTrip;
			dest.searchAddrFinal = source.searchAddrFinal;
			dest.address = source.address;
			dest.driver = source.driver;
			dest.implement = source.implement;
			dest.sharersInfo = source.sharersInfo;
			dest.tripRanges = source.tripRanges;
			dest.sharerIndexes = source.sharerIndexes;
			dest.tripsInfo = source.tripsInfo;
			dest.sharer = source.sharer;
			dest.deletedAreaFiles = source.deletedAreaFiles;
			dest.gfRawLength = source.gfRawLength;
			dest.gfRaw = source.gfRaw;
			dest.gfRawType = source.gfRawType;
			dest.gfRawWithoutAreas = source.gfRawWithoutAreas;
			dest.gfRawAreasIndex = source.gfRawAreasIndex;
			dest.gfRawAreasOffset = source.gfRawAreasOffset;
			dest.emptyImplFiltrExists = source.emptyImplFiltrExists;
			dest.tripWeightSumArrays = source.tripWeightSumArrays;
			dest.tripWeightArrays = source.tripWeightArrays;
			dest.usingInTripValues = source.usingInTripValues;
			dest.usingInTripUpdatedIndexes = source.usingInTripUpdatedIndexes;
			dest.tabularValLists = source.tabularValLists;
			dest.tabularUpdLists = source.tabularUpdLists;
			dest.cachedWeightSumArrays = source.cachedWeightSumArrays;
			dest.cachedWeightArrays = source.cachedWeightArrays;
			dest.cachedLimitArrays = source.cachedLimitArrays;
		}
		else
		{
			dest.imageName = null;
			dest.imageHue = 0;
			dest.computeInfo = null;
			dest.tabularPrmsGroupInfoArray = null;
			dest.tripPrmsGroupInfoArray = null;
			dest.finalPrmsGroupInfoArray = null;
			dest.tabularPrmsGroupChartInfoArray = null;
			dest.clear();
		}
	}
}
