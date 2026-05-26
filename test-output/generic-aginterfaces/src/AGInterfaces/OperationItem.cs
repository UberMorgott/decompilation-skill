namespace AGInterfaces;

public class OperationItem
{
	public string Name { get; set; }

	public string Description { get; set; }

	public OperationItem()
	{
	}

	public OperationItem(string name, string description)
	{
		Name = name;
		Description = description;
	}
}
