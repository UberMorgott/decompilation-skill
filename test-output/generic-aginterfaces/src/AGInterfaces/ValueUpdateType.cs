using System;

namespace AGInterfaces;

[Flags]
public enum ValueUpdateType : byte
{
	Unupdated = 0,
	Filtered = 1,
	Updated = 2,
	RaisingEven = 4,
	RaisingOdd = 8,
	LoweringEven = 0xC,
	LoweringOdd = 0x10,
	LvlChngMask = 0x1C,
	EventUpdated = 0x20,
	InclInNext = 0x40,
	InclInPrev = 0x80
}
