using System;
using MsgPack.Serialization;

namespace AGInterfaces;

public class ReportDataset
{
	public string Name { get; private set; }

	public DSInfo info { get; private set; }

	[MessagePackRuntimeType]
	public object Data { get; set; }

	public DataTypesEnum DataType { get; private set; }

	public Type CANType { get; private set; }

	public Type DSType { get; private set; }

	public ReportDataset(string _Name, DSInfo _dsInfo, DataTypesEnum _Type, Type _DSType)
	{
		DSType = _DSType;
		Name = _Name;
		info = _dsInfo;
		Data = Array.CreateInstance(_DSType, 0);
		DataType = _Type;
	}

	public ReportDataset(string _Name, Type _CANType, DataTypesEnum _Type, Type _DSType)
	{
		DSType = _DSType;
		Name = _Name;
		Data = Array.CreateInstance(_DSType, 0);
		CANType = _CANType;
		DataType = _Type;
	}
}
