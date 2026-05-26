using System.Xml;

namespace AGInterfaces.Localization;

public sealed class MemberClassPrmInfo
{
	public string Name;

	public string Title;

	public AGPrmInfoAttribute Attribute;

	public MemberClassPrmInfo(string Name, AGPrmInfoAttribute Attribute)
	{
		this.Name = Name;
		Title = Attribute.Title;
		this.Attribute = Attribute;
	}

	public void translate(XmlNode xmlNode)
	{
		if (xmlNode is XmlElement xmlElement)
		{
			string attribute = xmlElement.GetAttribute("Text");
			if (!string.IsNullOrWhiteSpace(attribute))
			{
				Title = attribute;
			}
		}
	}
}
