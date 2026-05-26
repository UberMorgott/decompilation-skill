using System;
using AutoGRAPHShell.Modules.OnlineDeviceProvider;

namespace AGInterfaces;

public sealed class ServerSecurityInfo : IComparable<ServerSecurityInfo>
{
	public bool isOffline;

	public bool isSecuritySupported;

	public bool showSecurity;

	public string roleName;

	public HostEndPoint hostEndPoint;

	public ServerSecurityInfo(bool isOffline, bool isSecuritySupported, bool showSecurity, string roleName, HostEndPoint hostEndPoint)
	{
		this.isOffline = isOffline;
		this.isSecuritySupported = isSecuritySupported;
		this.showSecurity = showSecurity;
		this.roleName = roleName;
		this.hostEndPoint = hostEndPoint;
	}

	public int CompareTo(ServerSecurityInfo other)
	{
		int num = isOffline.CompareTo(other.isOffline);
		if (num == 0)
		{
			num = showSecurity.CompareTo(other.showSecurity);
		}
		return num;
	}
}
