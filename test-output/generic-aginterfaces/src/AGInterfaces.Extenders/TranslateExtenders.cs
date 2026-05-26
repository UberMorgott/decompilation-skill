using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Xml;
using AGInterfaces.Localization;

namespace AGInterfaces.Extenders;

public static class TranslateExtenders
{
	public static string default_font_str = "(default)";

	public static XmlElement findRightParametersMethod(this XmlElement membersElement, MemberClassInfo memberInfo)
	{
		foreach (XmlNode item in membersElement.SelectNodes(memberInfo.memberType.ToString() + "[@Name='" + memberInfo.Name + "']"))
		{
			if (!(item is XmlElement xmlElement))
			{
				continue;
			}
			bool flag = true;
			MemberClassPrmInfo[] prms = memberInfo.prms;
			foreach (MemberClassPrmInfo memberClassPrmInfo in prms)
			{
				if (!(xmlElement.SelectSingleNode("Parameter[@Name='" + memberClassPrmInfo.Name + "']") is XmlElement))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				return xmlElement;
			}
		}
		return null;
	}

	public static Font TranslateFont(this XmlElement prmsElement, Font baseFont, Font font, string fontName)
	{
		try
		{
			if (prmsElement.SelectSingleNode("Font[@Name='" + fontName + "']") is XmlElement xmlElement)
			{
				string attribute = xmlElement.GetAttribute("Font");
				if (!string.IsNullOrWhiteSpace(attribute) && string.Compare(attribute, default_font_str, ignoreCase: true) != 0)
				{
					font = new Font(attribute, baseFont.Size);
				}
			}
		}
		catch
		{
		}
		return font;
	}

	public static bool getValueAttribute<T>(this XmlElement element, string name, ref T value)
	{
		try
		{
			value = (T)Enum.Parse(typeof(T), element.GetAttribute(name), ignoreCase: true);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static void translateMembers(this XmlElement ownerElement, IEnumerable<MemberClassInfo> members, string membersName)
	{
		if (!(ownerElement.SelectSingleNode(membersName) is XmlElement xmlElement))
		{
			return;
		}
		foreach (MemberClassInfo member in members)
		{
			XmlElement xmlElement2 = ((xmlElement == null) ? null : (member.memberType.HasFlag(MemberTypes.Method) ? xmlElement.findRightParametersMethod(member) : (xmlElement.SelectSingleNode(member.memberType.ToString() + "[@Name='" + member.Name + "']") as XmlElement)));
			if (xmlElement2 == null)
			{
				continue;
			}
			member.translate(xmlElement2);
			if (member.prms != null)
			{
				MemberClassPrmInfo[] prms = member.prms;
				foreach (MemberClassPrmInfo memberClassPrmInfo in prms)
				{
					memberClassPrmInfo.translate(xmlElement2.SelectSingleNode("Parameter[@Name='" + memberClassPrmInfo.Name + "']"));
				}
			}
		}
	}
}
