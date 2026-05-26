using System.Collections.Generic;

namespace AGInterfaces.ReportDynamicData;

public class NeedParamsForReport
{
	public readonly HashSet<string> NeededPrmsNameForReport;

	public readonly HashSet<string> NeededPrmsAliasForReport;

	public readonly bool NeededColorAndImagePrm;

	public NeedParamsForReport(HashSet<string> neededPrmsNameForReport, HashSet<string> neededPrmsAliasForReport, bool neededColorAndImagePrm)
	{
		NeededPrmsNameForReport = neededPrmsNameForReport;
		NeededPrmsAliasForReport = neededPrmsAliasForReport;
		NeededColorAndImagePrm = neededColorAndImagePrm;
	}

	public NeedParamsForReport(NeedParamsForReport source)
	{
		NeededPrmsNameForReport = new HashSet<string>(source.NeededPrmsNameForReport);
		NeededPrmsAliasForReport = new HashSet<string>(source.NeededPrmsAliasForReport);
		NeededColorAndImagePrm = source.NeededColorAndImagePrm;
	}

	public HashSet<string> GetCopyPrmsName()
	{
		return new HashSet<string>(NeededPrmsNameForReport);
	}

	public static NeedParamsForReport GetCopy(NeedParamsForReport source)
	{
		if (source == null)
		{
			return null;
		}
		return new NeedParamsForReport(source);
	}
}
