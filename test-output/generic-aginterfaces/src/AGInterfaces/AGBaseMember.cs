using System;
using System.Reflection;
using AGInterfaces.Extenders;

namespace AGInterfaces;

public class AGBaseMember
{
	public readonly MemberTypes memberType;

	public readonly ReturnType returnType;

	public readonly string name;

	public readonly AGBaseMemberPrm[] prms;

	public readonly string title;

	public readonly string shortDescription;

	public readonly DeviceParameterKind kind;

	public readonly AGInfoGroupType groupType;

	public string[][] description;

	public string units;

	public string fullDescription
	{
		get
		{
			if (description == null || description.Length == 0)
			{
				return null;
			}
			string text = shortDescription + Environment.NewLine + Environment.NewLine;
			int num = 0;
			while (true)
			{
				string[] array = description[num];
				if (array != null && array.Length != 0)
				{
					text += ((array.Length == 1) ? array[0] : $"{array[0]}:{Environment.NewLine}{array[1]}");
				}
				if (++num >= description.Length)
				{
					break;
				}
				text = text + Environment.NewLine + Environment.NewLine;
			}
			return text;
		}
	}

	public AGBaseMember(MemberTypes memberType, ReturnType returnType, string name, AGBaseMemberPrm[] prms, string title, string shortDescription, DeviceParameterKind kind, AGInfoGroupType groupType)
	{
		this.memberType = memberType;
		this.returnType = returnType;
		this.name = name;
		this.prms = prms;
		this.shortDescription = shortDescription;
		this.title = title;
		this.kind = kind;
		this.groupType = groupType;
	}

	protected AGBaseMember(AGBaseMember src)
	{
		memberType = src.memberType;
		returnType = src.returnType;
		name = src.name;
		prms = src.prms;
		title = src.title;
		shortDescription = src.shortDescription;
		kind = src.kind;
		groupType = src.groupType;
		description = src.description;
		units = src.units;
	}

	public override string ToString()
	{
		string text = DevPrmInfo.returnTypes[(int)returnType].typeShortName() + " " + name;
		if (memberType.HasFlag(MemberTypes.Method))
		{
			text += "(";
			if (prms != null)
			{
				int num = 0;
				int num2 = 0;
				AGBaseMemberPrm[] array = prms;
				foreach (AGBaseMemberPrm aGBaseMemberPrm in array)
				{
					if (aGBaseMemberPrm != null)
					{
						if (aGBaseMemberPrm.hasDefault)
						{
							text += " [";
							num2++;
						}
						if (num > 0)
						{
							text += ", ";
						}
						text += aGBaseMemberPrm;
					}
					else
					{
						text += "?";
					}
					num++;
				}
				for (int j = 0; j < num2; j++)
				{
					text += "] ";
				}
			}
			text += ")";
		}
		return text;
	}
}
