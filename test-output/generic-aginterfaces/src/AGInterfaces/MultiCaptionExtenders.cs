using System;
using System.Collections.Generic;

namespace AGInterfaces;

public static class MultiCaptionExtenders
{
	public static string get(this StoreMultiCaption multiCaption, string defaultCaption, string lang)
	{
		if (multiCaption == null || !multiCaption.UseIndividualCaption)
		{
			return defaultCaption;
		}
		if (multiCaption.TranslateTable == null || !multiCaption.TranslateTable.TryGetValue(lang, out var value))
		{
			return multiCaption.IndividualCaption;
		}
		return value;
	}

	public static string get(this StoreMultiCaption multiCaption, string defaultCaption, string defaultSubCaption, string lang)
	{
		if (multiCaption == null || !multiCaption.UseIndividualCaption)
		{
			if (!multiCaption.UseDefaultSubCaption)
			{
				return defaultCaption;
			}
			return defaultSubCaption;
		}
		if (multiCaption.TranslateTable == null || !multiCaption.TranslateTable.TryGetValue(lang, out var value))
		{
			return multiCaption.IndividualCaption;
		}
		return value;
	}

	public static void set(this StoreMultiCaption multiCaption, string defaultCaption, string value, string lang)
	{
		if (value == defaultCaption)
		{
			multiCaption.UseIndividualCaption = false;
		}
		else
		{
			if (string.IsNullOrEmpty(value))
			{
				value = null;
			}
			if (multiCaption.TranslateTable == null || !multiCaption.TranslateTable.ContainsKey(lang))
			{
				multiCaption.IndividualCaption = value;
			}
			else
			{
				multiCaption.TranslateTable[lang] = value;
			}
			multiCaption.UseIndividualCaption = true;
		}
		multiCaption.UseDefaultSubCaption = false;
	}

	public static void set(this StoreMultiCaption multiCaption, string defaultCaption, string value1, string lang1, string value2, string lang2, string value3, string lang3, string value4, string lang4)
	{
		Tuple<string, string>[] array = new Tuple<string, string>[4]
		{
			new Tuple<string, string>(value1, lang1),
			new Tuple<string, string>(value2, lang2),
			new Tuple<string, string>(value3, lang3),
			new Tuple<string, string>(value4, lang4)
		};
		foreach (Tuple<string, string> obj in array)
		{
			string text = obj.Item1;
			string item = obj.Item2;
			if (item == null || text == null)
			{
				continue;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = null;
			}
			if (multiCaption.TranslateTable == null || !multiCaption.TranslateTable.ContainsKey(item))
			{
				if (multiCaption.TranslateTable == null)
				{
					multiCaption.TranslateTable = new SortedDictionary<string, string>();
				}
				multiCaption.TranslateTable.Add(item, text);
			}
			else
			{
				multiCaption.TranslateTable[item] = text;
			}
		}
		multiCaption.UseIndividualCaption = true;
		multiCaption.UseDefaultSubCaption = false;
	}

	public static void set(this StoreMultiCaption multiCaption, string defaultCaption, string defaultSubCaption, string value, string lang)
	{
		if (value == defaultCaption)
		{
			bool useIndividualCaption = (multiCaption.UseDefaultSubCaption = false);
			multiCaption.UseIndividualCaption = useIndividualCaption;
			return;
		}
		if (value == defaultSubCaption)
		{
			multiCaption.UseIndividualCaption = false;
			multiCaption.UseDefaultSubCaption = true;
			return;
		}
		if (string.IsNullOrEmpty(value))
		{
			value = null;
		}
		if (multiCaption.TranslateTable == null || !multiCaption.TranslateTable.ContainsKey(lang))
		{
			multiCaption.IndividualCaption = value;
		}
		else
		{
			multiCaption.TranslateTable[lang] = value;
		}
		multiCaption.UseIndividualCaption = true;
		multiCaption.UseDefaultSubCaption = false;
	}

	public static void set(this StoreMultiCaption multiCaption, string defaultCaption, string defaultSubCaption, string value1, string lang1, string value2, string lang2)
	{
		Tuple<string, string>[] array = new Tuple<string, string>[2]
		{
			new Tuple<string, string>(value1, lang1),
			new Tuple<string, string>(value2, lang2)
		};
		foreach (Tuple<string, string> obj in array)
		{
			string text = obj.Item1;
			string item = obj.Item2;
			if (item == null || text == null)
			{
				continue;
			}
			if (string.IsNullOrEmpty(text))
			{
				text = null;
			}
			if (multiCaption.TranslateTable == null || !multiCaption.TranslateTable.ContainsKey(item))
			{
				if (multiCaption.TranslateTable == null)
				{
					multiCaption.TranslateTable = new SortedDictionary<string, string>();
				}
				multiCaption.TranslateTable.Add(item, text);
			}
			else
			{
				multiCaption.TranslateTable[item] = text;
			}
		}
		multiCaption.UseIndividualCaption = true;
		multiCaption.UseDefaultSubCaption = false;
	}
}
