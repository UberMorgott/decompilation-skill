namespace AGInterfaces;

public class ReportStreetsMap
{
	public string Street;

	public int StartIndex;

	public int EndIndex;

	public ReportStreetsMap(string Street, int StartIndex, int EndIndex)
	{
		this.Street = Street;
		this.StartIndex = StartIndex;
		this.EndIndex = EndIndex;
	}

	public override string ToString()
	{
		return Street;
	}
}
