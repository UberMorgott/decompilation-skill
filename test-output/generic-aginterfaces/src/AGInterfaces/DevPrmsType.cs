using System;

namespace AGInterfaces;

[Flags]
public enum DevPrmsType
{
	None = 0,
	TabularCrdInd = 1,
	TabularCrdDep = 2,
	Tabular = 0x13,
	TotalTrip = 4,
	TotalFinal = 8,
	TabularTskDep = 0x10
}
