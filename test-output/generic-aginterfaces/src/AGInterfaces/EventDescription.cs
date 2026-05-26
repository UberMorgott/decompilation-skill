using System.Reflection;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public sealed class EventDescription
{
	public int ID;

	[Translatable]
	public string Desc { get; set; }

	public EventDescription(int ID, string Desc)
	{
		this.ID = ID;
		this.Desc = Desc;
	}
}
