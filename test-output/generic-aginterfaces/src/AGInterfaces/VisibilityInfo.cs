namespace AGInterfaces;

public sealed class VisibilityInfo
{
	public bool enabled;

	public bool begingroup;

	public object Tag;

	public object Data;

	public string strGroupInfo;

	public string desc { get; set; }

	public string name { get; set; }

	public bool check { get; set; }

	public VisibilityInfo()
	{
		desc = string.Empty;
		name = string.Empty;
		check = false;
		enabled = false;
		begingroup = false;
	}

	public VisibilityInfo(string desc, string name, bool begingroup)
	{
		this.desc = desc;
		this.name = name;
		this.begingroup = begingroup;
		check = false;
		enabled = true;
	}
}
