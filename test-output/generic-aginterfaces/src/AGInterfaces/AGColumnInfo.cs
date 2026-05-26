namespace AGInterfaces;

public sealed class AGColumnInfo
{
	public string fieldName;

	public string caption;

	public HAlignment hAlignment;

	public float width;

	public int rowIndex;

	public int rowCount = 1;

	public DevPrmInfo prmInfo;

	public AGColumnInfo(string fieldName)
	{
		this.fieldName = fieldName;
	}
}
