namespace AGInterfaces.Classes;

public sealed class UserItem
{
	public string Name { get; set; }

	public string Login { get; set; }

	public string ParentLogin { get; set; }

	public string ShortLogin { get; set; }

	public bool Selected { get; set; }

	public int Icon { get; set; }

	public int Devices { get; set; }

	public string Data { get; set; }

	public bool State { get; set; }

	public int? DeviceCount { get; set; }

	public int[] Serials { get; set; }

	private string getParentUser()
	{
		if (string.IsNullOrEmpty(Login))
		{
			return "-";
		}
		int num = Login.IndexOf("@");
		if (num < 0)
		{
			return "-";
		}
		return Login.Substring(num + 1, Login.Length - num - 1);
	}

	private string getShortUser()
	{
		if (string.IsNullOrEmpty(Login))
		{
			return "";
		}
		int num = Login.IndexOf("@");
		if (num < 0)
		{
			return Login;
		}
		return Login.Substring(0, num);
	}

	public UserItem(string user)
	{
		Login = user;
		Name = (user.Contains("@") ? user.Substring(0, user.IndexOf('@')) : user);
		ParentLogin = getParentUser();
		ShortLogin = getShortUser();
		State = true;
	}
}
