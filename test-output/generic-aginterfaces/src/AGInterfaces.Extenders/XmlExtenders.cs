using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace AGInterfaces.Extenders;

public static class XmlExtenders
{
	internal static int getXmlNodeIndex(this XmlNode node)
	{
		int num = 0;
		if (node.ParentNode != null)
		{
			XmlNodeList childNodes = node.ParentNode.ChildNodes;
			for (int i = 0; i < childNodes.Count; i++)
			{
				if (childNodes[i] == node)
				{
					num++;
					break;
				}
				if (childNodes[i].Name == node.Name)
				{
					num++;
				}
			}
		}
		return num;
	}

	public static string getElementXPath(this XmlElement element)
	{
		StringBuilder stringBuilder = new StringBuilder(4096);
		XmlNodeList xmlNodeList = element.SelectNodes("ancestor-or-self::node()");
		int num = 1;
		int count = xmlNodeList.Count;
		while (true)
		{
			stringBuilder.Append(xmlNodeList[num].Name);
			XmlAttribute attributeNode = ((XmlElement)xmlNodeList[num]).GetAttributeNode("Name");
			if (attributeNode != null)
			{
				stringBuilder.AppendFormat("[@Name='{0}']", attributeNode.Value);
			}
			else
			{
				int xmlNodeIndex = xmlNodeList[num].getXmlNodeIndex();
				if (xmlNodeIndex > 1)
				{
					stringBuilder.AppendFormat("[{0}]", xmlNodeIndex);
				}
			}
			if (++num >= count)
			{
				break;
			}
			stringBuilder.Append('/');
		}
		return stringBuilder.ToString();
	}

	public static string getAttributeXPath(this XmlAttribute attribute)
	{
		StringBuilder stringBuilder = new StringBuilder(4096);
		XmlNodeList xmlNodeList = attribute.SelectNodes("ancestor-or-self::node()");
		int i = 1;
		for (int num = xmlNodeList.Count - 1; i < num; i++)
		{
			stringBuilder.Append(xmlNodeList[i].Name);
			XmlAttribute attributeNode = ((XmlElement)xmlNodeList[i]).GetAttributeNode("Name");
			if (attributeNode != null)
			{
				stringBuilder.AppendFormat("[@Name='{0}']", attributeNode.Value);
			}
			else
			{
				int xmlNodeIndex = xmlNodeList[i].getXmlNodeIndex();
				if (xmlNodeIndex > 1)
				{
					stringBuilder.AppendFormat("[{0}]", xmlNodeIndex);
				}
			}
			stringBuilder.Append('/');
		}
		stringBuilder.AppendFormat("@{0}", attribute.Name);
		return stringBuilder.ToString();
	}

	internal static void gatherLanguageStrings(this XmlNode node, SortedDictionary<string, string[]> target)
	{
		XmlAttribute xmlAttribute = node.Attributes["Name"];
		XmlAttribute xmlAttribute2 = node.Attributes["Text"];
		if (node.Name == "LookUpEdit" || node.Name == "RepositoryItemLookUpEdit")
		{
			XmlNode xmlNode = node.SelectSingleNode("LookUpColumnInfoCollection");
			if (xmlNode != null)
			{
				target.Add(xmlAttribute.Value, xmlNode.SelectNodes("LookUpColumnInfo").Cast<XmlNode>().Select(delegate(XmlNode p)
				{
					XmlAttribute xmlAttribute3 = p.Attributes["Text"];
					return (xmlAttribute3 == null) ? "" : xmlAttribute3.Value;
				})
					.ToArray());
			}
			return;
		}
		string name = node.Name;
		XmlNodeList xmlNodeList = node.SelectNodes(name);
		if (xmlNodeList.Count > 0 && xmlNodeList[0].Attributes["Name"] == null && xmlNodeList[0].Attributes["Text"] == null)
		{
			return;
		}
		if (node.Name == "String")
		{
			if (xmlAttribute2 == null)
			{
				target.Add(xmlAttribute.Value, node.SelectNodes("String").Cast<XmlNode>().Select(delegate(XmlNode p)
				{
					XmlAttribute xmlAttribute3 = p.Attributes["Text"];
					return (xmlAttribute3 != null) ? xmlAttribute3.Value : "";
				})
					.ToArray());
			}
			else
			{
				target.Add(xmlAttribute.Value, new string[1] { xmlAttribute2.Value });
			}
			return;
		}
		if (xmlAttribute == null)
		{
			if (xmlAttribute2 != null)
			{
				target.Add(node.Name, new string[1] { xmlAttribute2.Value });
			}
		}
		else if (xmlAttribute2 != null)
		{
			target.Add(xmlAttribute.Value, new string[1] { xmlAttribute2.Value });
		}
		foreach (XmlNode item in node.ChildNodes.Cast<XmlNode>())
		{
			item.gatherLanguageStrings(target);
		}
	}
}
