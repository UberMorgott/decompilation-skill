using System.Reflection;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public enum ElementType
{
	None = 0,
	Module = 1,
	Device = 2,
	GeoFence = 3,
	Driver = 4,
	Implement = 5,
	Task = 6,
	MapLayer = 7,
	Administrative = 100
}
