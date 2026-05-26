using AGInterfaces.Extenders;

namespace AGInterfaces;

public class AGBaseMemberPrm
{
	public readonly ReturnType returnType;

	public readonly string name;

	public readonly bool hasDefault;

	public readonly bool isArray;

	public readonly string title;

	public readonly string rangeInfo;

	public readonly double? minValue;

	public readonly double? maxValue;

	public AGBaseMemberPrm(ReturnType returnType, string name, bool hasDefault, bool isArray, string title, string rangeInfo, double? minValue, double? maxValue)
	{
		this.returnType = returnType;
		this.name = name;
		this.hasDefault = hasDefault;
		this.isArray = isArray;
		this.title = title;
		this.rangeInfo = rangeInfo;
		this.minValue = minValue;
		this.maxValue = maxValue;
	}

	public override string ToString()
	{
		if (!isArray)
		{
			return $"{DevPrmInfo.returnTypes[(int)returnType].typeShortName()} {name}";
		}
		return $"params {DevPrmInfo.returnTypes[(int)returnType].typeShortName()}[] {name}";
	}
}
