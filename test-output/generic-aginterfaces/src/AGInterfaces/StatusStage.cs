using System;

namespace AGInterfaces;

[Serializable]
public struct StatusStage
{
	public int startPos;

	public int endPos;

	public int startFileID;

	public int endFileID;

	public DateTime startDateTime;

	public DateTime endDateTime;

	public int index;

	public int absIndex;

	public Coordinates coordinates;

	public int pointIndex;

	public string caption;

	public int Status { get; set; }

	public static bool ArrayEquals(StatusStage[] arr1, StatusStage[] arr2)
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
				if (arr1[i].startFileID != arr2[i].startFileID || arr1[i].endFileID != arr2[i].endFileID || arr1[i].startDateTime != arr2[i].startDateTime || arr1[i].endDateTime != arr2[i].endDateTime || arr1[i].index != arr2[i].index || arr1[i].absIndex != arr2[i].absIndex || arr1[i].Status != arr2[i].Status || arr1[i].coordinates != arr2[i].coordinates)
				{
					return false;
				}
			}
		}
		return true;
	}
}
