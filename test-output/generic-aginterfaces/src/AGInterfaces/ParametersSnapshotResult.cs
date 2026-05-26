using System.Collections.Generic;

namespace AGInterfaces;

public class ParametersSnapshotResult
{
	public readonly Dictionary<string, string> Values;

	public readonly Dictionary<string, MonitorParameterRawValue> Raw;

	public static ParametersSnapshotResult Empty => new ParametersSnapshotResult(new Dictionary<string, string>(), new Dictionary<string, MonitorParameterRawValue>());

	public ParametersSnapshotResult(Dictionary<string, string> values, Dictionary<string, MonitorParameterRawValue> raw)
	{
		Values = values;
		Raw = raw;
	}
}
