using System;

namespace AGInterfaces;

[Serializable]
public class CustomSharerInfo
{
	public int index;

	public bool createTracksForArea;

	public string description { get; set; }

	public CustomSharerInfo()
	{
	}

	public CustomSharerInfo(string description, int index)
	{
		this.description = description;
		this.index = index;
	}
}
