using System;
using System.Linq;

namespace AGInterfaces;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ModuleInfoAttribute : Attribute
{
	public string ModuleName { get; }

	public string GroupName { get; }

	public string Description { get; }

	public bool Hidden { get; }

	public string TypeName { get; }

	public ModuleInfoAttribute(string moduleName, string groupName, string description, string typeName = null, bool hidden = false)
	{
		ModuleName = moduleName.Trim();
		GroupName = groupName.Trim();
		Description = description.Trim();
		TypeName = typeName ?? ToKey(moduleName);
		Hidden = hidden;
	}

	private static string ToUpperFirstCase(string source)
	{
		if (!string.IsNullOrEmpty(source))
		{
			return char.ToUpper(source[0]) + source.Substring(1);
		}
		return string.Empty;
	}

	public static string ToKey(string source)
	{
		return (from x in source.Split(' ', '-', ':')
			select ToUpperFirstCase(x)).Aggregate((string acc, string sel) => acc + sel);
	}
}
