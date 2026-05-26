namespace AGInterfaces;

public class InspectedObject
{
	public string Name { get; set; }

	public string Description { get; set; }

	public TechControlParameter[] Parameters { get; set; }
}
