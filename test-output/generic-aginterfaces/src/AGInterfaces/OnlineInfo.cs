using System;
using System.Collections.Generic;
using System.Linq;

namespace AGInterfaces;

[Serializable]
public sealed class OnlineInfo
{
	public readonly SourceType sourceType;

	public readonly BinFormatType binFormatType;

	public readonly SourceInfo[] sourceInfoArray;

	public readonly SourceInfo[] routeInfoArray;

	public readonly bool stationary;

	public Guid dataBaseGuid;

	public bool outOfOnlinePeriod;

	public bool outOfMemory;

	public bool areaCalcError;

	public FinalFields finalFields;

	public Array[] final;

	public ViewProperties viewProperties;

	public SortedDictionary<string, object> finalParams;

	public SortedDictionary<string, DevSwitchPrmStatusInfo> finalStatuses;

	public SortedDictionary<string, DevPrmAddStatusInfo> finalParamsAddInfo;

	public SortedDictionary<string, object> finalParamsByAlias;

	public SortedDictionary<string, DevSwitchPrmStatusInfo> finalStatusesByAlias;

	public SortedDictionary<string, DevPrmAddStatusInfo> finalParamsAddInfoByAlias;

	public string Addr { get; set; }

	[Obsolete("Для десериализации, Используется в ValidateTest")]
	public OnlineInfo()
	{
	}

	public OnlineInfo(SourceInfo[] sourceInfoArray, SourceInfo[] routeInfoArray, TimeZoneInfo timeZoneInfo, DateTime nextInspDT, InspectionCalcStatus inspCalcStatus, InspectionNotification inspNotification, Dictionary<string, double> techControlValues, bool stationary)
	{
		int num = ((sourceInfoArray != null) ? sourceInfoArray.Length : 0);
		this.sourceInfoArray = new SourceInfo[num];
		for (int i = 0; i < num; i++)
		{
			this.sourceInfoArray[i] = new SourceInfo(sourceInfoArray[i]);
		}
		if (num > 0)
		{
			sourceType = sourceInfoArray[0].sourceType;
			binFormatType = sourceInfoArray[0].binFormatType;
		}
		num = ((routeInfoArray != null) ? routeInfoArray.Length : 0);
		this.routeInfoArray = new SourceInfo[num];
		for (int j = 0; j < num; j++)
		{
			this.routeInfoArray[j] = new SourceInfo(routeInfoArray[j]);
		}
		this.stationary = stationary;
		finalFields = new FinalFields(timeZoneInfo, nextInspDT, inspCalcStatus, inspNotification, techControlValues);
	}

	public OnlineInfo(OnlineInfo onlineInfo)
	{
		sourceInfoArray = new SourceInfo[onlineInfo.sourceInfoArray.Length];
		int i = 0;
		for (int num = onlineInfo.sourceInfoArray.Length; i < num; i++)
		{
			sourceInfoArray[i] = new SourceInfo(onlineInfo.sourceInfoArray[i]);
		}
		routeInfoArray = new SourceInfo[onlineInfo.routeInfoArray.Length];
		int j = 0;
		for (int num2 = onlineInfo.routeInfoArray.Length; j < num2; j++)
		{
			routeInfoArray[j] = new SourceInfo(onlineInfo.routeInfoArray[j]);
		}
		stationary = onlineInfo.stationary;
		sourceType = onlineInfo.sourceType;
		binFormatType = onlineInfo.binFormatType;
		dataBaseGuid = onlineInfo.dataBaseGuid;
		outOfOnlinePeriod = onlineInfo.outOfOnlinePeriod;
		outOfMemory = onlineInfo.outOfMemory;
		areaCalcError = onlineInfo.areaCalcError;
		finalFields = onlineInfo.finalFields;
		final = onlineInfo.final;
		viewProperties = onlineInfo.viewProperties;
		finalParams = onlineInfo.finalParams;
		finalStatuses = onlineInfo.finalStatuses;
		finalParamsAddInfo = onlineInfo.finalParamsAddInfo;
		finalParamsByAlias = onlineInfo.finalParamsByAlias;
		finalStatusesByAlias = onlineInfo.finalStatusesByAlias;
		finalParamsAddInfoByAlias = onlineInfo.finalParamsAddInfoByAlias;
	}

	public bool SourceEquals(SourceInfo[] sourceInfoArray, SourceInfo[] routeInfoArray)
	{
		if (SourceInfo.ArraysEquals(this.sourceInfoArray, sourceInfoArray))
		{
			return SourceInfo.ArraysEquals(this.routeInfoArray, routeInfoArray);
		}
		return false;
	}

	public bool SourcePartEquals(SourceInfo[] sourceInfoArray, SourceInfo[] routeInfoArray)
	{
		if (SourceInfo.ArraysPartEquals(this.sourceInfoArray, sourceInfoArray))
		{
			return SourceInfo.ArraysPartEquals(this.routeInfoArray, routeInfoArray);
		}
		return false;
	}

	public bool Equals(OnlineInfo onlineInfo)
	{
		if (sourceType != onlineInfo.sourceType || binFormatType != onlineInfo.binFormatType || stationary != onlineInfo.stationary || dataBaseGuid != onlineInfo.dataBaseGuid || outOfOnlinePeriod != onlineInfo.outOfOnlinePeriod || outOfMemory != onlineInfo.outOfMemory || areaCalcError != onlineInfo.areaCalcError)
		{
			return false;
		}
		if (!SourceEquals(onlineInfo.sourceInfoArray, onlineInfo.routeInfoArray))
		{
			return false;
		}
		if (finalFields != onlineInfo.finalFields && !finalFields.Equals(onlineInfo.finalFields))
		{
			return false;
		}
		if (finalParams != onlineInfo.finalParams)
		{
			if (finalParams == null || onlineInfo.finalParams == null)
			{
				return false;
			}
			if (!finalParams.SequenceEqual(onlineInfo.finalParams))
			{
				return false;
			}
		}
		if (finalStatuses != onlineInfo.finalStatuses)
		{
			if (finalStatuses == null || onlineInfo.finalStatuses == null)
			{
				return false;
			}
			if (!finalStatuses.SequenceEqual(onlineInfo.finalStatuses))
			{
				return false;
			}
		}
		if (finalParamsAddInfo != onlineInfo.finalParamsAddInfo)
		{
			if (finalParamsAddInfo == null || onlineInfo.finalParamsAddInfo == null)
			{
				return false;
			}
			if (!finalParamsAddInfo.SequenceEqual(onlineInfo.finalParamsAddInfo))
			{
				return false;
			}
		}
		return true;
	}
}
