using System;
using System.Reflection;
using AGInterfaces;

namespace AutoGRAPHShell.Modules.OnlineDeviceProvider;

[Obfuscation(Exclude = true)]
public class HostEndPoint
{
	[NonSerialized]
	public HostEndPoint[] Secondary;

	public string hostName { get; set; }

	public int port { get; set; }

	public ServerVersion version { get; set; }

	public bool useInWeb { get; set; }

	public HostEndPoint()
	{
	}

	public HostEndPoint(string _hostName, int _port, ServerVersion _version)
	{
		hostName = _hostName;
		port = _port;
		version = _version;
	}

	public override string ToString()
	{
		return hostName + ":" + port;
	}

	public override int GetHashCode()
	{
		return (hostName ?? "").ToLower().GetHashCode() ^ port;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is HostEndPoint hostEndPoint))
		{
			return false;
		}
		if (string.Compare(hostEndPoint.hostName ?? "", hostName ?? "") == 0 && hostEndPoint.port == port)
		{
			return hostEndPoint.version == version;
		}
		return false;
	}
}
