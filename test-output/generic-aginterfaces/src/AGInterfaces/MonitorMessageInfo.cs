using System;
using System.Collections.Generic;

namespace AGInterfaces;

public sealed class MonitorMessageInfo
{
	public readonly DateTime DT;

	public readonly string parameterName;

	public readonly Coordinates Point;

	public readonly string Address;

	public readonly Dictionary<string, string> FormattedValues;

	public readonly Dictionary<string, MonitorParameterRawValue> RawValues;

	public readonly Dictionary<string, string> PrevFormattedValues;

	public readonly Dictionary<string, MonitorParameterRawValue> PrevRawValues;

	public readonly Dictionary<string, MonitorParameterRawValue> TabularRawValues;

	public readonly Dictionary<string, string> TabularFormattedValues;

	public readonly Dictionary<string, object> Properties;

	public readonly TimeSpan? Duration;

	public readonly string MessageSubject;

	public readonly string MessageBody;

	public MonitorMessageInfo(string parmName, DateTime UTCDTEvent, TimeSpan? duration, Coordinates point, string address, ParametersSnapshotResult paramValues, ParametersSnapshotResult paramPrevValues, ParametersSnapshotResult paramTabularValues, Dictionary<string, object> props, string messageSubject, string messageBody)
	{
		parameterName = parmName;
		DT = UTCDTEvent;
		Duration = duration;
		Point = point;
		Address = address;
		Properties = props;
		FormattedValues = paramValues.Values;
		RawValues = paramValues.Raw;
		PrevFormattedValues = paramPrevValues.Values;
		PrevRawValues = paramPrevValues.Raw;
		TabularFormattedValues = paramTabularValues.Values;
		TabularRawValues = paramTabularValues.Raw;
		MessageSubject = messageSubject;
		MessageBody = messageBody;
	}
}
