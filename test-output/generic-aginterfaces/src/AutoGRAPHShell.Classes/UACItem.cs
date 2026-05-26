namespace AutoGRAPHShell.Classes;

public class UACItem
{
	public string FileName { get; set; }

	public string Target { get; set; }

	public UACItem(string fileName, string target)
	{
		FileName = fileName;
		Target = target;
	}
}
