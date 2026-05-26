using System;

namespace AutoGRAPHShell.Modules.OnlineDeviceProvider;

public sealed class ServerDeviceItem
{
	public readonly Guid ID;

	public readonly string Name;

	public readonly string GroupName;

	public readonly int Serial;

	public readonly string Password;

	public readonly bool SortData;

	public ServerDeviceItem(Guid _ID, int _Serial, string _Name, string _GroupName, string _Password, bool _SortData)
	{
		ID = _ID;
		Name = _Name;
		GroupName = _GroupName;
		Serial = _Serial;
		Password = _Password;
		SortData = _SortData;
	}

	public override int GetHashCode()
	{
		return Serial.GetHashCode();
	}
}
