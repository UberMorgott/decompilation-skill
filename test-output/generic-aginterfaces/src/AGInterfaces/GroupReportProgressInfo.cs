using System;

namespace AGInterfaces;

public class GroupReportProgressInfo
{
	public IAutoGRAPHShell shellProvider;

	public int completed;

	public int all;

	public Guid guid;

	public GroupReportProgressInfo(IAutoGRAPHShell shellProvider)
	{
		this.shellProvider = shellProvider;
	}

	public GroupReportProgressInfo(GroupReportProgressInfo info, Guid guid)
		: this(info.shellProvider)
	{
		completed = info.completed;
		all = info.all;
		this.guid = guid;
	}

	public int getProgressPosition(int percent, int max = 1000)
	{
		int num = 100 * completed + percent;
		int num2 = 100 * all;
		return max * num / num2;
	}
}
