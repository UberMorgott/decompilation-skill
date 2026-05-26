namespace AGInterfaces;

public class PrefixItem
{
	public static string str_Tabular = "Tabular";

	public static string str_Interval = "Interval";

	public static string str_Final = "Final";

	private readonly PrefixUsingType UsingType;

	public string Name { get; set; }

	public string Description { get; set; }

	public string UsingTypeDesc
	{
		get
		{
			string text = string.Empty;
			if ((UsingType & PrefixUsingType.Tabular) == PrefixUsingType.Tabular)
			{
				text += str_Tabular;
			}
			if ((UsingType & PrefixUsingType.Interval) == PrefixUsingType.Interval)
			{
				text = text + (string.IsNullOrEmpty(text) ? "" : ", ") + str_Interval;
			}
			if ((UsingType & PrefixUsingType.Final) == PrefixUsingType.Final)
			{
				text = text + (string.IsNullOrEmpty(text) ? "" : ", ") + str_Final;
			}
			return text;
		}
	}

	public PrefixItem()
	{
	}

	public PrefixItem(string name, string description, PrefixUsingType usingType)
	{
		Name = name;
		Description = description;
		UsingType = usingType;
	}
}
