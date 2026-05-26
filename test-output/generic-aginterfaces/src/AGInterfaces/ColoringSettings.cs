using System.Collections.Generic;
using MsgPack.Serialization;

namespace AGInterfaces;

public sealed class ColoringSettings
{
	public ColorDefinedBy definedBy { get; set; }

	public int defaultColor { get; set; }

	[MessagePackRuntimeCollectionItemType]
	public ColorConditionProps[] fewColors { get; set; }

	public string parameter { get; set; }

	public ColorProps[] oneColors { get; set; }

	public HashSet<string> GetUsedPrmsName()
	{
		HashSet<string> hashSet = new HashSet<string>();
		switch (definedBy)
		{
		case ColorDefinedBy.FewParameters:
		{
			if (fewColors == null)
			{
				break;
			}
			ColorConditionProps[] array = fewColors;
			foreach (ColorConditionProps colorConditionProps in array)
			{
				if (!string.IsNullOrEmpty(colorConditionProps?.parameter))
				{
					hashSet.Add(colorConditionProps.parameter);
				}
			}
			break;
		}
		case ColorDefinedBy.SwitchStatuses:
		case ColorDefinedBy.OneParameter:
			if (!string.IsNullOrEmpty(parameter))
			{
				hashSet.Add(parameter);
			}
			break;
		}
		return hashSet;
	}
}
