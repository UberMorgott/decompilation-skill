using System;

namespace AGInterfaces;

[Serializable]
public struct StatusPoint
{
	public Coordinates coordinates;

	public DateTime start;

	public DateTime end;

	public int index;

	public string caption;

	public static bool ArrayEquals(StatusPoint[] arr1, StatusPoint[] arr2)
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
				if (!arr1[i].coordinates.Equals(arr2[i].coordinates) || arr1[i].start != arr2[i].start || arr1[i].end != arr2[i].end || arr1[i].index != arr2[i].index)
				{
					return false;
				}
			}
		}
		return true;
	}
}
