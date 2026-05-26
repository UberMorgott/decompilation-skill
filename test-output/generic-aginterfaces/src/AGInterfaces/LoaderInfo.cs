using System;
using System.Collections.Generic;
using BINParser;

namespace AGInterfaces;

[Serializable]
public sealed class LoaderInfo
{
	public DateTime dateTime;

	public string fileName;

	public Guid dataBaseGuid;

	public int serialNo;

	public int offset;

	public int length;

	[NonSerialized]
	public List<DeviceRecord> items;

	public DateTime? firstDT;

	public DateTime? lastDT;

	public LoaderInfo()
	{
	}

	public LoaderInfo(int serialNo, SourceInfo sourceInfo, int offset, int length, List<DeviceRecord> items)
	{
		this.serialNo = serialNo;
		if (sourceInfo != null)
		{
			dateTime = sourceInfo.dateTime;
			fileName = sourceInfo.fileName;
			dataBaseGuid = sourceInfo.dataBaseGuid;
		}
		this.offset = offset;
		this.length = length;
		this.items = items;
	}
}
