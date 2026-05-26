using System;
using System.Drawing;

namespace AGInterfaces;

[Serializable]
public class TrackStatuses
{
	public bool onState;

	public string description;

	public Color color;

	public string[] imageNames;

	public Alignment2D imageAlignment;

	public StatusPoint[] statuses;

	public TimeSpan totalDuration;

	public double totalDistance;

	public TrackStatuses()
	{
	}

	public TrackStatuses(bool onState, string description, Color color, string[] imageNames, Alignment2D imageAlignment, StatusPoint[] statuses, TimeSpan totalDuration, double totalDistance)
	{
		this.onState = onState;
		this.description = description;
		this.color = color;
		this.imageNames = imageNames;
		this.imageAlignment = imageAlignment;
		this.statuses = statuses;
		this.totalDuration = totalDuration;
		this.totalDistance = totalDistance;
	}

	public bool Equals(TrackStatuses trackStatuses)
	{
		if (onState == trackStatuses.onState && string.Compare(description, trackStatuses.description) == 0 && color == trackStatuses.color && ArrayEquals(imageNames, trackStatuses.imageNames) && imageAlignment == trackStatuses.imageAlignment)
		{
			return StatusPoint.ArrayEquals(statuses, trackStatuses.statuses);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is TrackStatuses trackStatuses))
		{
			return base.Equals(obj);
		}
		return Equals(trackStatuses);
	}

	public static bool ArrayEquals<T>(T[] arr1, T[] arr2)
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
				if (arr1[i] != null || arr2[i] != null)
				{
					if (arr1[i] == null || arr2[i] == null)
					{
						return false;
					}
					if (!arr1[i].Equals(arr2[i]))
					{
						return false;
					}
				}
			}
		}
		return true;
	}
}
