using System;

namespace AGInterfaces;

[Flags]
public enum ReportOperations
{
	None = 0,
	Start = 1,
	FindFiles = 2,
	LoadFiles = 4,
	SetTabularValues = 8,
	ShareTrips = 0x10,
	SetTripValues = 0x20
}
