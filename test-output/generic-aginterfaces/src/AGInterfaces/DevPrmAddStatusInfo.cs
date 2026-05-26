using System;
using Newtonsoft.Json;

namespace AGInterfaces;

public sealed class DevPrmAddStatusInfo : IComparable<DevPrmAddStatusInfo>
{
	public readonly bool actual;

	public readonly string caption;

	public readonly string format;

	public readonly string unit;

	public readonly double min;

	public readonly double max;

	public readonly bool hasStatuses;

	public readonly bool hasStatusesWithImage;

	public readonly bool hasDynamicStatuses;

	public readonly StatusesDefinedType onStatusesDefined;

	[JsonConstructor]
	public DevPrmAddStatusInfo(bool actual, string caption, string format, string unit, double min, double max, bool hasStatuses, bool hasStatusesWithImage, bool hasDynamicStatuses, StatusesDefinedType onStatusesDefined)
	{
		this.actual = actual;
		this.caption = caption;
		this.format = format;
		this.unit = unit;
		this.min = min;
		this.max = max;
		this.hasStatuses = hasStatuses;
		this.hasStatusesWithImage = hasStatusesWithImage;
		this.hasDynamicStatuses = hasDynamicStatuses;
		this.onStatusesDefined = onStatusesDefined;
	}

	public int CompareTo(DevPrmAddStatusInfo other)
	{
		int num = actual.CompareTo(other.actual);
		if (num == 0)
		{
			num = string.Compare(caption, other.caption);
		}
		if (num == 0)
		{
			num = string.Compare(format, other.format);
		}
		if (num == 0)
		{
			num = string.Compare(unit, other.unit);
		}
		if (num == 0)
		{
			num = min.CompareTo(other.min);
		}
		if (num == 0)
		{
			num = max.CompareTo(other.max);
		}
		if (num == 0)
		{
			num = hasStatuses.CompareTo(other.hasStatuses);
		}
		if (num == 0)
		{
			num = hasStatusesWithImage.CompareTo(other.hasStatusesWithImage);
		}
		if (num == 0)
		{
			num = hasDynamicStatuses.CompareTo(other.hasDynamicStatuses);
		}
		if (num == 0)
		{
			num = onStatusesDefined.CompareTo(other.onStatusesDefined);
		}
		return num;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is DevPrmAddStatusInfo other))
		{
			return base.Equals(obj);
		}
		return CompareTo(other) == 0;
	}
}
