using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AGInterfaces;

[Serializable]
[Obfuscation(Exclude = true)]
public class StoreMultiCaption : IEqualityComparer<StoreMultiCaption>
{
	private const string columnNameSeparator = " / ";

	public bool UseIndividualCaption { get; set; }

	public bool UseDefaultSubCaption { get; set; }

	public string IndividualCaption { get; set; }

	public SortedDictionary<string, string> TranslateTable { get; set; }

	public StoreMultiCaption()
	{
	}

	public StoreMultiCaption(StoreMultiCaption mc)
	{
		UseIndividualCaption = mc.UseIndividualCaption;
		UseDefaultSubCaption = mc.UseDefaultSubCaption;
		IndividualCaption = mc.IndividualCaption;
		if (mc.TranslateTable == null || !mc.TranslateTable.Any())
		{
			return;
		}
		TranslateTable = new SortedDictionary<string, string>();
		foreach (KeyValuePair<string, string> item in mc.TranslateTable)
		{
			TranslateTable.Add(item.Key, item.Value);
		}
	}

	public StoreMultiCaption(StoreMultiCaption prevMC, StoreMultiCaption MC)
	{
		UseIndividualCaption = prevMC.UseIndividualCaption | MC.UseIndividualCaption;
		UseDefaultSubCaption = prevMC.UseDefaultSubCaption | MC.UseDefaultSubCaption;
		IndividualCaption = string.Format("{0}{1}{2}", prevMC.IndividualCaption, " / ", MC.IndividualCaption);
		if ((prevMC.TranslateTable == null || !prevMC.TranslateTable.Any()) && (MC.TranslateTable == null || !MC.TranslateTable.Any()))
		{
			return;
		}
		TranslateTable = new SortedDictionary<string, string>();
		SortedSet<string> sortedSet = new SortedSet<string>();
		if (prevMC.TranslateTable != null)
		{
			foreach (string key in prevMC.TranslateTable.Keys)
			{
				sortedSet.Add(key);
			}
		}
		if (MC.TranslateTable != null)
		{
			foreach (string key2 in MC.TranslateTable.Keys)
			{
				sortedSet.Add(key2);
			}
		}
		foreach (string item in sortedSet)
		{
			string value = prevMC.IndividualCaption;
			string value2 = MC.IndividualCaption;
			if (prevMC.TranslateTable != null)
			{
				prevMC.TranslateTable.TryGetValue(item, out value);
			}
			if (MC.TranslateTable != null)
			{
				MC.TranslateTable.TryGetValue(item, out value2);
			}
			TranslateTable.Add(item, string.Format("{0}{1}{2}", value, " / ", value2));
		}
	}

	public string GetLocalizedCaption(string lang, string defLang = null)
	{
		if (TranslateTable == null)
		{
			return IndividualCaption ?? "";
		}
		if (TranslateTable.ContainsKey(lang))
		{
			return TranslateTable[lang];
		}
		if (!string.IsNullOrEmpty(defLang) && TranslateTable.ContainsKey(defLang))
		{
			return TranslateTable[defLang];
		}
		return IndividualCaption ?? "";
	}

	public bool IsEmpty()
	{
		if (!UseIndividualCaption && !UseDefaultSubCaption && string.IsNullOrEmpty(IndividualCaption))
		{
			if (TranslateTable != null)
			{
				return TranslateTable.Count == 0;
			}
			return true;
		}
		return false;
	}

	public override string ToString()
	{
		if (!string.IsNullOrEmpty(IndividualCaption))
		{
			return IndividualCaption;
		}
		if (TranslateTable == null || !TranslateTable.Any())
		{
			return "-";
		}
		return TranslateTable.First().Value;
	}

	public bool Equals(StoreMultiCaption x, StoreMultiCaption y)
	{
		if (x == y)
		{
			return true;
		}
		if (x == null)
		{
			return false;
		}
		if (y == null)
		{
			return false;
		}
		bool flag = x.UseIndividualCaption == y.UseIndividualCaption && x.UseDefaultSubCaption == y.UseDefaultSubCaption && x.IndividualCaption == y.IndividualCaption;
		if (flag)
		{
			if (x.TranslateTable == y.TranslateTable)
			{
				return true;
			}
			if (x.TranslateTable == null)
			{
				return false;
			}
			if (y.TranslateTable == null)
			{
				return false;
			}
			if (x.TranslateTable.Count != y.TranslateTable.Count)
			{
				return false;
			}
			if (x.TranslateTable.Any((KeyValuePair<string, string> pair) => !y.TranslateTable.Contains(pair)))
			{
				return false;
			}
		}
		return flag;
	}

	public int GetHashCode(StoreMultiCaption obj)
	{
		int num = UseIndividualCaption.GetHashCode() ^ UseDefaultSubCaption.GetHashCode();
		if (IndividualCaption != null)
		{
			num ^= IndividualCaption.GetHashCode();
		}
		if (TranslateTable != null)
		{
			foreach (KeyValuePair<string, string> item in TranslateTable)
			{
				num ^= item.Key.GetHashCode();
				if (item.Value != null)
				{
					num ^= item.Value.GetHashCode();
				}
			}
		}
		return num;
	}
}
