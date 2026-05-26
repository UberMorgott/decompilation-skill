using System.Net;
using Socks;

namespace AGInterfaces;

public sealed class NetworkLoginPasswordCredential
{
	public bool WinAuth { get; set; }

	public string Login { get; set; }

	public string Password { get; set; }

	public NetworkLoginPasswordCredential()
	{
		WinAuth = false;
	}

	public NetworkCredential ToNetworkCredential()
	{
		if (!WinAuth)
		{
			return new NetworkCredential(Login, Password);
		}
		return CredentialCache.DefaultNetworkCredentials;
	}

	public ProxyCredential ToProxyCredential()
	{
		return new ProxyCredential(WinAuth, Login, Password);
	}

	public override string ToString()
	{
		if (!WinAuth)
		{
			return "NetworkCredential: " + Login;
		}
		return "<current user>";
	}
}
