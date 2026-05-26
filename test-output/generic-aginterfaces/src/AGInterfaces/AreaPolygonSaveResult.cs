namespace AGInterfaces;

public class AreaPolygonSaveResult
{
	public readonly string MapFileName;

	public readonly string ImageFileName;

	public AreaPolygonSaveResult(string mapFileName, string imageFileName)
	{
		MapFileName = mapFileName;
		ImageFileName = imageFileName;
	}
}
