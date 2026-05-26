using System;
using System.Reflection;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
[Flags]
public enum SettingsType
{
	None = 0,
	Panels = 1,
	View = 2,
	GroupableCommon = 4,
	GroupableIndividual = 8,
	ElementCommon = 0x10,
	ElementIndividual = 0x20,
	NewElementNodes = 0x40,
	ElementNodeInfo = 0x80,
	ElementImage = 0x100,
	ElementProperties = 0x200,
	ElementNodes = 0x400,
	DeviceSerialNo = 0x800,
	DeviceTimeZone = 0x1000,
	AccessibleDeviceNodes = 0x2000,
	GeoFencesKMLFile = 0x4000,
	ModuleProps = 0x8000,
	ModuleLinks = 0x10000,
	ModuleImages = 0x20000,
	DriverNo = 0x40000,
	SchemeLoaded = 0x80000,
	OpenSetupForm = 0x100000,
	CloseSetupForm = 0x200000
}
