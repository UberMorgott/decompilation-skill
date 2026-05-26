using System;
using System.Collections.Generic;

namespace AGInterfaces.ReportDynamicData;

public class StageTypeGenerator
{
	public List<string> StagesNames { get; }

	public DevPrmsType DevPrmsType { get; }

	public Dictionary<string, Type> Parameters { get; }

	public Type ClassType { get; }

	public Dictionary<string, Action<object, object>> SetterList { get; }

	public StageTypeGenerator(DevPrmsType devPrmsType, List<string> stagesNames, Dictionary<string, Type> parameters)
	{
		StagesNames = stagesNames;
		DevPrmsType = devPrmsType;
		Parameters = parameters;
		ClassType = DataTypeBuilder.CompileResultType(typeof(BaseDataRecord), "DynamicDataRecord", parameters);
		BaseDataRecord obj = (BaseDataRecord)Activator.CreateInstance(ClassType);
		SetterList = obj.GetSettersList(parameters);
	}
}
