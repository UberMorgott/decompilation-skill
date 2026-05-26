using System;
using System.Collections.Generic;
using System.Reflection;

namespace AGInterfaces.Classes;

public static class FieldsEntriesTypes
{
	public static Dictionary<string, int[]> Dictionary;

	public static Dictionary<string, bool> PreparedDictionary;

	static FieldsEntriesTypes()
	{
		Dictionary = new Dictionary<string, int[]>();
		PreparedDictionary = new Dictionary<string, bool>();
		Type typeFromHandle = typeof(TskDependentFields);
		PropertyInfo[] properties = typeFromHandle.GetProperties();
		foreach (PropertyInfo propertyInfo in properties)
		{
			foreach (Attribute customAttribute in propertyInfo.GetCustomAttributes())
			{
				if (customAttribute is AGInfoAttribute aGInfoAttribute)
				{
					if (!Dictionary.ContainsKey(propertyInfo.Name))
					{
						Dictionary.Add(propertyInfo.Name, aGInfoAttribute.EntriesID);
					}
					if (!PreparedDictionary.ContainsKey(propertyInfo.Name))
					{
						PreparedDictionary.Add(propertyInfo.Name, aGInfoAttribute.Prepared);
					}
				}
			}
		}
		MethodInfo[] methods = typeFromHandle.GetMethods();
		foreach (MethodInfo methodInfo in methods)
		{
			foreach (Attribute customAttribute2 in methodInfo.GetCustomAttributes())
			{
				if (customAttribute2 is AGInfoAttribute aGInfoAttribute2)
				{
					if (!Dictionary.ContainsKey(methodInfo.Name))
					{
						Dictionary.Add(methodInfo.Name, aGInfoAttribute2.EntriesID);
					}
					if (!PreparedDictionary.ContainsKey(methodInfo.Name))
					{
						PreparedDictionary.Add(methodInfo.Name, aGInfoAttribute2.Prepared);
					}
				}
			}
		}
	}

	public static HashSet<int> GetTypes(IEnumerable<string> fields, HashSet<string> namedSet)
	{
		HashSet<int> hashSet = null;
		foreach (string field in fields)
		{
			if (Dictionary.TryGetValue(field, out var value) && value != null)
			{
				if (value.Length == 0)
				{
					return new HashSet<int>();
				}
				if (hashSet == null)
				{
					hashSet = new HashSet<int>();
				}
				hashSet.UnionWith(value);
			}
			if (namedSet.Contains(field))
			{
				if (hashSet == null)
				{
					hashSet = new HashSet<int>();
				}
				hashSet.Add(75);
				hashSet.Add(76);
			}
		}
		return hashSet;
	}

	public static bool GetPrepared(IEnumerable<string> fields)
	{
		foreach (string field in fields)
		{
			if (PreparedDictionary.TryGetValue(field, out var value) && value)
			{
				return true;
			}
		}
		return false;
	}
}
