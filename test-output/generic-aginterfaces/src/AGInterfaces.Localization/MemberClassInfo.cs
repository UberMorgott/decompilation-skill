using System.Reflection;
using System.Xml;

namespace AGInterfaces.Localization;

public sealed class MemberClassInfo
{
	public readonly string Name;

	public string Title;

	public string Description;

	public readonly MemberTypes memberType;

	public readonly AGInfoAttribute attribute;

	public readonly MemberClassPrmInfo[] prms;

	public MemberClassInfo(string name, MemberTypes memberType, AGInfoAttribute attribute, MemberClassPrmInfo[] prms)
	{
		Name = name;
		Title = attribute.Title;
		Description = attribute.Description;
		this.memberType = memberType;
		this.attribute = attribute;
		this.prms = prms;
	}

	public void translate(XmlElement xmlElement)
	{
		string text = xmlElement.GetAttribute("Text");
		if (!string.IsNullOrWhiteSpace(text))
		{
			Title = text;
		}
		string text2 = xmlElement.GetAttribute("ToolTipText");
		if (!string.IsNullOrWhiteSpace(text2))
		{
			Description = text2;
		}
	}
}
