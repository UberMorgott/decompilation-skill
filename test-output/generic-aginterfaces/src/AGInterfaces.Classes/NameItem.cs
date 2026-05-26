namespace AGInterfaces.Classes;

public class NameItem : INameItem
{
	public int ID { get; set; }

	public string Name { get; set; }

	public NameItem(int _ID, string _Name)
	{
		ID = _ID;
		Name = _Name;
	}
}
