using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace AGInterfaces;

public sealed class HandlerArrays
{
	public TimeSpan[] interval;

	public DTFiltration[] dtf;

	public BitArray err;

	public readonly int itemsCount;

	public int crdItemsNum;

	public int firstIndex = -1;

	public int lastIndex = -1;

	public HandlerFreqData[] freqData = new HandlerFreqData[10];

	public double[] faAmounts;

	public AGBitArray setAmountByFDRecord;

	public int[] cameraNum;

	public int[][] photoReasonArr = new int[16][];

	public DateTime[][] photoUDTArr = new DateTime[16][];

	public AGBitArray shiftIsOpen;

	public AGBitArray setShiftIsOpen;

	public byte[] naviMotions;

	public string[] naviMapFileNames;

	public Color[] naviMapColors;

	public AGBitArray possibleSpeedError;

	public int[] crdPos;

	public double[] ground;

	public byte[] motion;

	public TimeSpan[] crdInt;

	public double[] dist;

	public double[] course;

	public double[] speed;

	public double[] vSpeed;

	public double[] speedByPrm;

	public int defSpeedLimit;

	public int[] speedLimit;

	public string[] streets;

	public string[] platon;

	public TimeSpan[] durArr;

	public int[,] fiArr;

	public int[] ciArr;

	public int[] siArr;

	public int[] niArr;

	public int[] piArr;

	public BitArray[] outOfGF;

	public BitArray[] outOfGFRoutes;

	public List<StageInfo> parksList;

	public Circles[] debugCircles;

	public Coordinates[] debugLines;

	public Guid[] task;

	public DateTime[] taskBeginUDT;

	public DateTime[] taskEndUDT;

	public byte[] taskStatus;

	public double[] taskPercent;

	public long[] taskViolations;

	public Guid[] taskNextGeoFence;

	public bool fakeData;

	public static int createCrdPosArray(out int[] crdPos, int maxCount, CrdFiltration[] llf)
	{
		crdPos = new int[maxCount + 1];
		int num = 0;
		int num2 = llf.Length;
		for (int i = 0; i < num2; i++)
		{
			if (llf[i] == CrdFiltration.Ok)
			{
				crdPos[num++] = i;
			}
		}
		if (num > 0)
		{
			Array.Resize(ref crdPos, num + 1);
			crdPos[num] = num2;
		}
		else
		{
			crdPos = null;
		}
		return num;
	}

	public HandlerArrays(int itemsCount)
	{
		this.itemsCount = itemsCount;
	}

	public void createCameraNumArrays()
	{
		cameraNum = new int[itemsCount];
	}

	public void createPhotoArrays(int n)
	{
		int[] array = (photoReasonArr[n] = new int[itemsCount]);
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = -1;
		}
		photoUDTArr[n] = new DateTime[itemsCount];
	}

	public void createShiftIsOpenArrays()
	{
		shiftIsOpen = new AGBitArray(itemsCount);
		setShiftIsOpen = new AGBitArray(itemsCount);
		setShiftIsOpen[0] = true;
	}

	public void createNaviMotionsArray()
	{
		naviMotions = new byte[itemsCount];
	}

	public void creatNaviMapFileNameArray()
	{
		naviMapFileNames = new string[itemsCount];
		naviMapColors = new Color[itemsCount];
	}
}
