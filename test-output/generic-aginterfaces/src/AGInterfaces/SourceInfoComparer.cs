using System.Collections.Generic;

namespace AGInterfaces;

public sealed class SourceInfoComparer : IComparer<SourceInfo>
{
	public int Compare(SourceInfo x, SourceInfo y)
	{
		int num = x.dateTime.CompareTo(y.dateTime);
		if (num == 0)
		{
			num = x.sourceType.CompareTo(y.sourceType);
			if (num == 0)
			{
				num = x.binFormatType.CompareTo(y.binFormatType);
			}
		}
		return num;
	}
}
