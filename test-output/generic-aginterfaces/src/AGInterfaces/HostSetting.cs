namespace AGInterfaces;

public sealed class HostSetting
{
	public string Host { get; set; }

	public int Port { get; set; }

	public string Login { get; set; }

	public string Password { get; set; }

	public ServerVersion Version { get; set; }

	public static HostSetting CreateDefault()
	{
		return new HostSetting
		{
			Host = "m.tk-chel.ru",
			Login = "demo",
			Password = "demo",
			Port = 2230
		};
	}

	public override string ToString()
	{
		return Host + "/" + Port + "/" + Login;
	}
}
