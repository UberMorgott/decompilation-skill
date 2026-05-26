using System;
using System.Drawing;
using System.Linq;

namespace AGInterfaces;

[Serializable]
public class DevPrmInfo : DevPrmsGroupInfo
{
	public string alias;

	public DeviceParameterType type;

	public ReturnType returnType;

	public DeviceParameterKind kind;

	public AddValueType addValueType;

	public string unit;

	public string format;

	public string toolTip;

	public int columnWidth;

	public int graphColor;

	public string ordinate;

	public HAlignment hAlignment;

	public TotalValueType totalValueType;

	public ValueRow row;

	public double min;

	public double max;

	public int maxLineDur;

	public StatusesDefinedType onStatusesDefined;

	public string image;

	public DevSwitchPrmStatusInfo[] onStatusesInfo;

	public DevSwitchPrmStatusInfo[] offStatusInfo;

	public int creationIndex;

	public bool byDesigner;

	public bool required;

	public static readonly Type[] returnTypes = new Type[14]
	{
		typeof(bool),
		typeof(byte),
		typeof(int),
		typeof(long),
		typeof(double),
		typeof(DateTime),
		typeof(TimeSpan),
		typeof(Guid),
		typeof(Quadro<Guid>),
		typeof(string),
		typeof(Image),
		typeof(Coordinates),
		typeof(Location),
		typeof(Color)
	};

	public static readonly int[] returnSizes = new int[14]
	{
		1, 1, 4, 8, 8, 8, 8, 16, 64, 4,
		4, 24, 68, 4
	};

	public static readonly ReturnType[] arithmReturnTypes = new ReturnType[13]
	{
		ReturnType.Int64,
		ReturnType.Int64,
		ReturnType.Int64,
		ReturnType.Int64,
		ReturnType.Double,
		ReturnType.TimeSpan,
		ReturnType.TimeSpan,
		ReturnType.Guid,
		ReturnType.Guid4,
		ReturnType.String,
		ReturnType.Image,
		ReturnType.Coordinates,
		ReturnType.Location
	};

	public static readonly ReturnType[,] typesCastTable = new ReturnType[13, 13]
	{
		{
			ReturnType.Boolean,
			ReturnType.Byte,
			ReturnType.Int32,
			ReturnType.Int64,
			ReturnType.Double,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.Byte,
			ReturnType.Byte,
			ReturnType.Int32,
			ReturnType.Int64,
			ReturnType.Double,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.Int32,
			ReturnType.Int32,
			ReturnType.Int32,
			ReturnType.Int64,
			ReturnType.Double,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.Int64,
			ReturnType.Int64,
			ReturnType.Int64,
			ReturnType.Int64,
			ReturnType.Double,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.Double,
			ReturnType.Double,
			ReturnType.Double,
			ReturnType.Double,
			ReturnType.Double,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.DateTime,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.TimeSpan,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Guid,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.Image,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String
		},
		{
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String,
			ReturnType.String
		}
	};

	public DevPrmsGroupInfo parentGroup { get; set; }

	public string param
	{
		get
		{
			if (parentGroup == null)
			{
				return caption;
			}
			return parentGroup.caption + ": " + caption;
		}
	}

	public bool hasStatuses
	{
		get
		{
			if ((onStatusesDefined == StatusesDefinedType.Condition || onStatusesDefined == StatusesDefinedType.Bits) && onStatusesInfo == null)
			{
				return offStatusInfo != null;
			}
			return true;
		}
	}

	public bool hasStatusesWithImage
	{
		get
		{
			switch (onStatusesDefined)
			{
			case StatusesDefinedType.Condition:
			case StatusesDefinedType.Bits:
				if (onStatusesInfo == null || !onStatusesInfo.Any((DevSwitchPrmStatusInfo s) => s.imageNames != null && s.imageNames.Length != 0))
				{
					if (offStatusInfo != null)
					{
						return offStatusInfo.Any((DevSwitchPrmStatusInfo s) => s.imageNames != null && s.imageNames.Length != 0);
					}
					return false;
				}
				return true;
			case StatusesDefinedType.Value:
				if (string.IsNullOrEmpty(image))
				{
					if (offStatusInfo != null)
					{
						return offStatusInfo.Any((DevSwitchPrmStatusInfo s) => s.imageNames != null && s.imageNames.Length != 0);
					}
					return false;
				}
				return true;
			default:
				return true;
			}
		}
	}

	public bool hasDynamicStatuses
	{
		get
		{
			switch (onStatusesDefined)
			{
			case StatusesDefinedType.Value:
				if (returnType != ReturnType.Int32 && returnType != ReturnType.Int64 && returnType != ReturnType.Guid)
				{
					return returnType == ReturnType.String;
				}
				return true;
			case StatusesDefinedType.Bits:
				if (returnType != ReturnType.Byte && returnType != ReturnType.Int32)
				{
					return returnType == ReturnType.Int64;
				}
				return true;
			case StatusesDefinedType.GeoFence:
				if (returnType != ReturnType.Guid)
				{
					return returnType == ReturnType.Guid4;
				}
				return true;
			case StatusesDefinedType.Device:
				if (returnType != ReturnType.Guid)
				{
					return returnType == ReturnType.Guid4;
				}
				return true;
			case StatusesDefinedType.Driver:
			case StatusesDefinedType.Implement:
			case StatusesDefinedType.Route:
			case StatusesDefinedType.Task:
				return returnType == ReturnType.Guid;
			case StatusesDefinedType.GeoFenceType:
			case StatusesDefinedType.StatusType:
			case StatusesDefinedType.ImplementType:
				if (returnType != ReturnType.Int32 && returnType != ReturnType.Int64)
				{
					return returnType == ReturnType.Byte;
				}
				return true;
			default:
				return false;
			}
		}
	}
}
