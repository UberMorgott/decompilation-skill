namespace AGInterfaces;

public class ReadRecordRequest
{
	public int serialNo;

	public string fileName;

	public int entriesOffset;

	public int entriesToRead;

	public bool isSortable;

	public ReadRecordRequest(int serialNo, string fileName, int entriesOffset, int entriesToRead, bool isSortable)
	{
		this.serialNo = serialNo;
		this.fileName = fileName;
		this.entriesOffset = entriesOffset;
		this.entriesToRead = entriesToRead;
		this.isSortable = isSortable;
	}
}
