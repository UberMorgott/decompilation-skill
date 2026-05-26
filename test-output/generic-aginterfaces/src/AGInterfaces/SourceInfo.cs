using System;
using System.Linq;

namespace AGInterfaces;

[Serializable]
public class SourceInfo
{
	public SourceType sourceType;

	public Guid dataBaseGuid;

	public BinFormatType binFormatType;

	public int filesCount;

	public DateTime dateTime;

	public DateTime fileWriteDateTime;

	public string fileName;

	public string fullFileName;

	public long fileLength;

	public int offset;

	public int length;

	public SourceInfo()
	{
	}

	public SourceInfo(SourceType sourceType)
	{
		this.sourceType = sourceType;
	}

	public SourceInfo(SourceInfo sourceInfo)
	{
		CopyFrom(sourceInfo);
	}

	public void CopyFrom(SourceInfo sourceInfo)
	{
		dateTime = sourceInfo.dateTime;
		sourceType = sourceInfo.sourceType;
		binFormatType = sourceInfo.binFormatType;
		filesCount = sourceInfo.filesCount;
		dataBaseGuid = sourceInfo.dataBaseGuid;
		fileWriteDateTime = sourceInfo.fileWriteDateTime;
		fileName = sourceInfo.fileName;
		fullFileName = sourceInfo.fullFileName;
		fileLength = sourceInfo.fileLength;
		offset = sourceInfo.offset;
		length = sourceInfo.length;
	}

	public bool Equals(DateTime dateTime, SourceType sourceType, BinFormatType binFormatType)
	{
		if (this.dateTime == dateTime && this.sourceType == sourceType)
		{
			return this.binFormatType == binFormatType;
		}
		return false;
	}

	public bool SourceEquals(SourceInfo sourceInfo)
	{
		if (CanRestorePosition(sourceInfo) && fileLength == sourceInfo.fileLength)
		{
			return fileWriteDateTime == sourceInfo.fileWriteDateTime;
		}
		return false;
	}

	public bool SourcePartEquals(SourceInfo sourceInfo)
	{
		if (CanRestorePosition(sourceInfo) && fileLength == sourceInfo.fileLength && fileWriteDateTime == sourceInfo.fileWriteDateTime && offset == sourceInfo.offset)
		{
			return length == sourceInfo.length;
		}
		return false;
	}

	private bool CanRestorePosition(SourceInfo sourceInfo)
	{
		if (sourceType == sourceInfo.sourceType && binFormatType == sourceInfo.binFormatType && dataBaseGuid == sourceInfo.dataBaseGuid)
		{
			return fileName == sourceInfo.fileName;
		}
		return false;
	}

	public static bool Equals(SourceInfo sourceInfo1, SourceInfo sourceInfo2)
	{
		if (sourceInfo1 == sourceInfo2)
		{
			return true;
		}
		if (sourceInfo1 == null || sourceInfo2 == null)
		{
			return false;
		}
		return sourceInfo1.SourceEquals(sourceInfo2);
	}

	public static bool ArraysEquals(SourceInfo[] sourceInfoArray1, SourceInfo[] sourceInfoArray2)
	{
		if (sourceInfoArray1 == sourceInfoArray2)
		{
			return true;
		}
		if (sourceInfoArray1 == null || sourceInfoArray2 == null)
		{
			return false;
		}
		if (sourceInfoArray1.Length != sourceInfoArray2.Length)
		{
			return false;
		}
		int i = 0;
		for (int num = sourceInfoArray1.Length; i < num; i++)
		{
			if (!sourceInfoArray1[i].SourceEquals(sourceInfoArray2[i]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool ArraysPartEquals(SourceInfo[] sourceInfoArray1, SourceInfo[] sourceInfoArray2)
	{
		if (sourceInfoArray1 == sourceInfoArray2)
		{
			return true;
		}
		if (sourceInfoArray1 == null || sourceInfoArray2 == null)
		{
			return false;
		}
		SourceInfo[] array = sourceInfoArray1.Where((SourceInfo s) => s.length > 0).ToArray();
		SourceInfo[] array2 = sourceInfoArray2.Where((SourceInfo s) => s.length > 0).ToArray();
		if (array.Length != array2.Length)
		{
			return false;
		}
		int num = 0;
		for (int num2 = array.Length; num < num2; num++)
		{
			if (!array[num].SourcePartEquals(array2[num]))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanRestorePosition(SourceInfo sourceInfo1, SourceInfo sourceInfo2)
	{
		if (sourceInfo1 == sourceInfo2)
		{
			return true;
		}
		if (sourceInfo1 == null || sourceInfo2 == null)
		{
			return false;
		}
		return sourceInfo1.CanRestorePosition(sourceInfo2);
	}

	public override string ToString()
	{
		return string.Format("{0} ({1}): {2}, bytes={3}, offset={4}, length={5} (writeDT: {6})", sourceType, binFormatType, fileName, fileLength, offset, length, fileWriteDateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"));
	}
}
