namespace AGInterfaces;

public class ReportProgressInfo
{
	public IAutoGRAPHShell shellProvider;

	public int stepCompleted;

	public int stepsAll = 1;

	public ReportOperations completed;

	public ReportOperations all;

	public ReportProgressInfo(IAutoGRAPHShell shellProvider)
	{
		this.shellProvider = shellProvider;
	}

	public ReportProgressInfo(ReportProgressInfo info)
		: this(info.shellProvider)
	{
		stepCompleted = info.stepCompleted;
		stepsAll = info.stepsAll;
		completed = info.completed;
		all = info.all;
	}

	public int getPercent(int percent)
	{
		int num = 100 * stepCompleted + getLocalPercent(percent);
		int num2 = 100 * stepsAll;
		return 100 * num / num2;
	}

	public int getLocalPercent(int percent)
	{
		int num = getOperPercent(completed) + percent;
		int operPercent = getOperPercent(all);
		return 100 * num / operPercent;
	}

	private static int getOperPercent(ReportOperations operations)
	{
		int num = 0;
		if (operations.HasFlag(ReportOperations.FindFiles))
		{
			num += 100;
		}
		if (operations.HasFlag(ReportOperations.LoadFiles))
		{
			num += 100;
		}
		if (operations.HasFlag(ReportOperations.SetTabularValues))
		{
			num += 100;
		}
		if (operations.HasFlag(ReportOperations.ShareTrips))
		{
			num += 100;
		}
		if (operations.HasFlag(ReportOperations.SetTripValues))
		{
			num += 100;
		}
		return num;
	}
}
