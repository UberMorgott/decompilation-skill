using System;

namespace AGInterfaces;

[Serializable]
public struct Circle(Coordinates centr, double radius)
{
	public Coordinates centr = centr;

	public double radius = radius;

	public static bool ArrayEquals(Circle[] arr1, Circle[] arr2)
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
				if (!arr1[i].centr.Equals(arr2[i].centr) || arr1[i].radius != arr2[i].radius)
				{
					return false;
				}
			}
		}
		return true;
	}
}
