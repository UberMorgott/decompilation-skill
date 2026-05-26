using System;

namespace AGInterfaces;

public class TskDependentFields : CrdDependentFields
{
	private Guid _Task => _prm.task;

	private DateTime _TaskBeginUDT => _prm.taskBeginUDT;

	private DateTime _TaskEndUDT => _prm.taskEndUDT;

	private byte _TaskStatus => _prm.taskStatus;

	private double _TaskPercent => _prm.taskPercent;

	private long _TaskViolations => _prm.taskViolations;

	private Guid _TaskNextGeoFence => _prm.taskNextGeoFence;

	[AGInfo("Task", "Task.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { })]
	public Guid Task
	{
		get
		{
			read(DataType.common);
			return _Task;
		}
	}

	[AGInfo("Task begin local time", "Task begin local time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime TaskBeginDT
	{
		get
		{
			read(DataType.common);
			if (!(_TaskBeginUDT != DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_TaskBeginUDT, timeZoneInfo);
		}
	}

	[AGInfo("Task end local time", "Task end local time.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, new int[] { })]
	public DateTime TaskEndDT
	{
		get
		{
			read(DataType.common);
			if (!(_TaskEndUDT != DateTime.MinValue))
			{
				return DateTime.MinValue;
			}
			return TimeZoneInfo.ConvertTimeFromUtc(_TaskEndUDT, timeZoneInfo);
		}
	}

	[AGInfo("Task status", "Task status: 0 - ready, 1 - wait for start condition, 2 - execution, 3 - wait for end condition, 4 - performed, 5 - not performed.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { })]
	public byte TaskStatus
	{
		get
		{
			read(DataType.common);
			return _TaskStatus;
		}
	}

	[AGInfo("Task percent", "Task execution percent.", "%", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { })]
	public double TaskPercent
	{
		get
		{
			read(DataType.common);
			return _TaskPercent;
		}
	}

	[AGInfo("Task violations", "Task violations.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { })]
	public long TaskViolations
	{
		get
		{
			read(DataType.common);
			return _TaskViolations;
		}
	}

	[AGInfo("Task next geo-fence", "Task next geo-fence.", DeviceParameterKind.InstTime, AGInfoGroupType.Data, false, new int[] { })]
	public Guid TaskNextGeoFence
	{
		get
		{
			read(DataType.common);
			return _TaskNextGeoFence;
		}
	}

	public TskDependentFields(ExpressionBaseInitInfo initInfo, ParameterInitInfo prmInitInfo, IDataViewerFormatters dataViewerFormatters)
		: base(initInfo, prmInitInfo, dataViewerFormatters)
	{
	}

	public override void setRecord(bool setTypedData)
	{
		base.setRecord(setTypedData);
	}

	public override void copyFrom(CrdIndependentFields value)
	{
		base.copyFrom(value);
	}
}
