using System.Reflection;

namespace AGInterfaces.Localization;

[Obfuscation(Exclude = true)]
public sealed class EntryInfo
{
	public int ID;

	[Translatable]
	public string Group { get; set; }

	[Translatable]
	public string Data { get; set; }

	[Translatable]
	public string Desc { get; set; }

	public EntryInfo(string Group, string Data, string Desc)
	{
		this.Group = Group;
		this.Data = Data;
		this.Desc = Desc;
	}
}
