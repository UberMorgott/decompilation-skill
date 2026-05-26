using System;
using System.Collections.Generic;

namespace AGInterfaces;

public class ExpressionBaseInternal
{
	public virtual AGBitArray ReadStatus()
	{
		return null;
	}

	public virtual void SetValues(Array[] tabularArrays, List<int> tabularOffsets, int tabularIndex, Array[] totalArrays = null, List<int> totalOffsets = null, int totalIndex = 0)
	{
	}
}
