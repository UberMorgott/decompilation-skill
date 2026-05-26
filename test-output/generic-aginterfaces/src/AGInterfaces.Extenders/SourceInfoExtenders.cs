using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AGInterfaces.Extenders;

public static class SourceInfoExtenders
{
	private static readonly Regex regex = new Regex("\\A(\\d{3,12})-(\\d{2})(\\d{2})(\\d{2})(-\\S{3,6})?$");

	private static readonly Regex timeRegex = new Regex("-(\\d{2})(\\d{2})(\\d{2})");

	public static void AddSourceInfo(List<SourceInfo> source, string fullFileName, long Length, DateTime LastWriteUTC, int serialNo, BinFormatType binFormatType, SourceType forceType, IAutoGRAPHShell shellProvider)
	{
		GroupCollection groups = regex.Match(Path.GetFileNameWithoutExtension(fullFileName)).Groups;
		if (!int.TryParse(groups[1].Value, out var result) || result != serialNo || !int.TryParse(groups[2].Value, out var result2) || !int.TryParse(groups[3].Value, out var result3) || !int.TryParse(groups[4].Value, out var result4))
		{
			return;
		}
		result2 += 2000;
		int result5 = 0;
		int result6 = 0;
		int result7 = 0;
		SourceType sourceType = SourceType.None;
		string value = groups[5].Value;
		if (forceType == SourceType.None)
		{
			if (string.IsNullOrEmpty(value))
			{
				sourceType = SourceType.GSM;
			}
			else if (string.Compare(value, "-sat", ignoreCase: true) == 0)
			{
				sourceType = SourceType.SAT;
			}
			else if (string.Compare(value, "-sms", ignoreCase: true) == 0)
			{
				sourceType = SourceType.SMS;
			}
			else
			{
				GroupCollection groups2 = timeRegex.Match(value).Groups;
				if (!int.TryParse(groups2[1].Value, out result5) || !int.TryParse(groups2[2].Value, out result6) || !int.TryParse(groups2[3].Value, out result7))
				{
					return;
				}
				sourceType = SourceType.USB;
			}
		}
		else
		{
			if (!string.IsNullOrEmpty(value))
			{
				return;
			}
			sourceType = forceType;
		}
		try
		{
			if (1 <= result2 && result2 <= 9999 && 1 <= result3 && result3 <= 12 && 1 <= result4 && result4 <= DateTime.DaysInMonth(result2, result3) && 0 <= result5 && result5 < 24 && 0 <= result6 && result6 < 60 && 0 <= result7 && result7 < 60)
			{
				source.Add(new SourceInfo
				{
					dateTime = new DateTime(result2, result3, result4, result5, result6, result7),
					sourceType = sourceType,
					binFormatType = binFormatType,
					fileName = Path.GetFileName(fullFileName),
					fullFileName = fullFileName,
					fileLength = Length,
					fileWriteDateTime = LastWriteUTC
				});
			}
		}
		catch (Exception ex)
		{
			shellProvider.Logger.LogError("addSourceInfo:", ex);
		}
	}
}
