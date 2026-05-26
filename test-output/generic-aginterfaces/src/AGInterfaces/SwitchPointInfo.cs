using System;

namespace AGInterfaces;

public class SwitchPointInfo
{
	public int Index;

	public readonly object Value;

	public readonly object ValuePrev;

	public readonly DateTime DTEvent;

	public readonly DateTime DTCheck;

	public readonly Coordinates Point;

	public readonly string Address;

	public TimeSpan? Duration;

	public readonly ParametersSnapshotResult SnapshotFinal;

	public readonly ParametersSnapshotResult SnapshotTabular;

	public readonly ParametersSnapshotResult SnapshotCurrent;

	public readonly ParametersSnapshotResult SnapshotPrevious;

	public readonly SwitchPointType PointType;

	public SwitchPointInfo(int index, DateTime eventUTC, object valuePrevious, object valueCurrent, Coordinates point, string address, ParametersSnapshotResult snapshotCurrent, ParametersSnapshotResult snapshotPrevious, ParametersSnapshotResult snapshotFinal, ParametersSnapshotResult snapshotTabular, SwitchPointType pointType, TimeSpan? duration = null)
	{
		Index = index;
		DTEvent = eventUTC;
		DTCheck = DateTime.UtcNow;
		Value = valueCurrent;
		ValuePrev = valuePrevious;
		Point = point;
		Address = address;
		SnapshotCurrent = snapshotCurrent;
		SnapshotPrevious = snapshotPrevious;
		SnapshotFinal = snapshotFinal;
		SnapshotTabular = snapshotTabular;
		PointType = pointType;
		Duration = duration;
	}
}
