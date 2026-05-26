using System;
using System.Drawing;

namespace AGInterfaces;

[Serializable]
public struct TrackPoint
{
	public Coordinates coordinates;

	public DateTime udt;

	public TrackPointAttrs attrs;

	public Color trackColor;

	public Color cursorColor;

	public int cursorImgs;

	public int implements;

	public double run;

	public double speed;

	public int index;

	public static bool ArrayEquals(TrackPoint[] arr1, TrackPoint[] arr2)
	{
		if (arr1 != arr2)
		{
			if (arr1 == null || arr2 == null)
			{
				return false;
			}
			if (arr1.Length != arr2.Length)
			{
				return false;
			}
			int i = 0;
			for (int num = arr1.Length; i < num; i++)
			{
				if (!arr1[i].coordinates.Equals(arr2[i].coordinates) || arr1[i].udt != arr2[i].udt || arr1[i].attrs != arr2[i].attrs || arr1[i].trackColor != arr2[i].trackColor || arr1[i].cursorColor != arr2[i].cursorColor || arr1[i].cursorImgs != arr2[i].cursorImgs || arr1[i].implements != arr2[i].implements || arr1[i].run != arr2[i].run || arr1[i].speed != arr2[i].speed)
				{
					return false;
				}
			}
		}
		return true;
	}
}
