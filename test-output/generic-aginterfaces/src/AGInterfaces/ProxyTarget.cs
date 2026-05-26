using System;

namespace AGInterfaces;

[Flags]
public enum ProxyTarget
{
	None = 0,
	DataDown = 1,
	SchemaUpDown = 2,
	MapsDown = 4,
	Messages = 8,
	AutoUpdate = 0x10,
	RemoteSupport = 0x20
}
