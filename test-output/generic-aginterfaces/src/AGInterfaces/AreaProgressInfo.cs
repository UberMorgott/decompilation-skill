using System;

namespace AGInterfaces;

public class AreaProgressInfo
{
	public IAutoGRAPHShell shellProvider;

	public string araName;

	public Guid areaGuid;

	public AreaProgressInfo(IAutoGRAPHShell shellProvider)
	{
		this.shellProvider = shellProvider;
	}

	public AreaProgressInfo(AreaProgressInfo info)
		: this(info?.shellProvider)
	{
	}
}
