namespace AGInterfaces;

public sealed class NetworkSetting
{
	public string ProxyHost { get; set; }

	public int ProxyPort { get; set; }

	public ProxyVersion ProxyVersion { get; set; }

	public bool UseSystemProxy { get; set; }

	public bool UseProxy { get; set; }

	public ProxyTarget UseProxyFor { get; set; }

	public static NetworkSetting GetDefault()
	{
		return new NetworkSetting
		{
			UseSystemProxy = true,
			UseProxyFor = (ProxyTarget.DataDown | ProxyTarget.SchemaUpDown | ProxyTarget.MapsDown),
			ProxyVersion = ProxyVersion.HTTP
		};
	}
}
