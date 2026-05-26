using System;
using System.Drawing;

namespace AGInterfaces;

[Serializable]
public class Circles
{
	public Color color;

	public Circle[] circles;

	public Circles()
	{
	}

	public Circles(Color color, Circle[] circles)
	{
		this.color = color;
		this.circles = circles;
	}

	public static bool ArrayEquals(Circles[] arr1, Circles[] arr2)
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
				if (arr1[i].color != arr2[i].color || !Circle.ArrayEquals(arr1[i].circles, arr2[i].circles))
				{
					return false;
				}
			}
		}
		return true;
	}
}
