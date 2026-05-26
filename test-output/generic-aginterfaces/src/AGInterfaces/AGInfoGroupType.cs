using System;

namespace AGInterfaces;

[Flags]
public enum AGInfoGroupType
{
	None = 0,
	Data = 1,
	Time = 2,
	Debug = 4,
	Navigation = 8,
	Sensor = 0x10,
	Counter = 0x20,
	Level = 0x40,
	FuelLevel = 0x80,
	Pressure = 0x100,
	Rotation = 0x200,
	Frequency = 0x400,
	Temperature = 0x800,
	CAN = 0x1000,
	RS485 = 0x2000,
	Wire1 = 0x4000,
	Motohours = 0x8000,
	Distance = 0x10000,
	Consumption = 0x20000,
	AutoCistern = 0x40000,
	Identifier = 0x80000,
	Driver = 0x100000,
	Implement = 0x200000,
	All = -1
}
