using System;
using System.Collections.Generic;
using AGInterfaces.Extenders;

namespace AGInterfaces;

[Serializable]
public class DevSwitchPrmStatusInfo : IComparable<DevSwitchPrmStatusInfo>
{
	public string description;

	public int color;

	public Operator oper;

	public double value;

	public string identifier;

	public StatusesDefinedType statusType;

	[NonSerialized]
	public string[] imageNames;

	[NonSerialized]
	public int[] imageHues;

	[NonSerialized]
	public Quadro<Guid> ID;

	public DevSwitchPrmStatusInfo()
	{
	}

	public DevSwitchPrmStatusInfo(string description, int color, StatusesDefinedType statusType)
	{
		this.description = description;
		this.color = color;
		this.statusType = statusType;
	}

	public DevSwitchPrmStatusInfo(string description, int color, StatusesDefinedType statusType, Operator oper, double value, string imageName, int imageHue)
	{
		this.description = description;
		this.color = color;
		this.oper = oper;
		this.value = value;
		this.statusType = statusType;
		imageNames = new string[1] { imageName };
		imageHues = new int[1] { imageHue };
	}

	public DevSwitchPrmStatusInfo(string description, int color, StatusesDefinedType statusType, List<string> imageNames, List<int> imageHues)
	{
		this.description = description;
		this.color = color;
		this.statusType = statusType;
		this.imageNames = imageNames.ToArray();
		this.imageHues = imageHues.ToArray();
	}

	public int CompareTo(DevSwitchPrmStatusInfo other)
	{
		int num = StringExtenders.NumCompare(description, other.description);
		if (num == 0)
		{
			num = color.CompareTo(other.color);
		}
		if (num == 0)
		{
			num = statusType.CompareTo(other.statusType);
		}
		return num;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is DevSwitchPrmStatusInfo other))
		{
			return base.Equals(obj);
		}
		return CompareTo(other) == 0;
	}
}
