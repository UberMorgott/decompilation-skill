namespace AGInterfaces;

public class Area
{
	public int Total;

	public int Complete;

	public int Incomplete;

	public int Overlap;

	public int FullOverlap;

	public int MaxOverlapDepth;

	public void Scale(double factor)
	{
		Total = (int)((double)Total * factor);
		Complete = (int)((double)Complete * factor);
		Overlap = (int)((double)Overlap * factor);
		FullOverlap = (int)((double)FullOverlap * factor);
		Incomplete = (int)((double)Incomplete * factor);
	}

	public override string ToString()
	{
		return $"Total={Total}, Complete={Complete}";
	}
}
