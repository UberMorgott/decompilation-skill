using System;

namespace AGInterfaces;

[Serializable]
public sealed class SharerInfo : CustomSharerInfo
{
	public int width;

	public int color;

	public bool visible = true;

	public int creationIndex;

	public SharerInfo()
	{
	}

	public SharerInfo(string description, int width, int color, bool visible)
		: base(description, 0)
	{
		this.width = width;
		this.color = color;
		this.visible = visible;
	}

	public override string ToString()
	{
		return base.description;
	}
}
