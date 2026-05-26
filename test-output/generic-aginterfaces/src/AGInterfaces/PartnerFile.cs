using System.Collections.Generic;

namespace AGInterfaces;

public class PartnerFile
{
	public string Name { get; set; } = "";

	public string LogoUrl { get; set; } = "";

	public string[] Addresses { get; set; } = new string[0];

	public string[] Worktimes { get; set; } = new string[0];

	public Dictionary<PartnerContactKind, string[]> Contacts { get; set; } = new Dictionary<PartnerContactKind, string[]>();

	public Dictionary<string, string[]> CustomFields { get; set; } = new Dictionary<string, string[]>();
}
