using System;
using System.Collections;
using System.Collections.Generic;

namespace AGInterfaces.ReportDynamicData;

public class ReportDataStorage : IReadOnlyDictionary<string, ReportDataSet>, IEnumerable<KeyValuePair<string, ReportDataSet>>, IEnumerable, IReadOnlyCollection<KeyValuePair<string, ReportDataSet>>
{
	private readonly Dictionary<string, ReportDataSet> _dataDictionary;

	public ReportTimeSpan timeSpanFromUDT = ReportTimeSpan.Empty;

	public IEnumerable<string> Keys => _dataDictionary.Keys;

	public IEnumerable<ReportDataSet> Values => _dataDictionary.Values;

	public int Count => _dataDictionary.Count;

	public ReportDataSet this[string key] => _dataDictionary[key];

	public ReportDataStorage(Dictionary<string, Dictionary<string, Type>> stagesAndProperties)
	{
		_dataDictionary = new Dictionary<string, ReportDataSet>();
		foreach (KeyValuePair<string, Dictionary<string, Type>> stagesAndProperty in stagesAndProperties)
		{
			_dataDictionary.Add(stagesAndProperty.Key, new ReportDataSet(stagesAndProperty.Key, stagesAndProperty.Value));
		}
	}

	public ReportDataStorage(List<StageTypeGenerator> stageTypeGenerators)
	{
		_dataDictionary = new Dictionary<string, ReportDataSet>();
		foreach (StageTypeGenerator stageTypeGenerator in stageTypeGenerators)
		{
			foreach (string stagesName in stageTypeGenerator.StagesNames)
			{
				_dataDictionary.Add(stagesName, new ReportDataSet(stagesName, stageTypeGenerator));
			}
		}
	}

	IEnumerator<KeyValuePair<string, ReportDataSet>> IEnumerable<KeyValuePair<string, ReportDataSet>>.GetEnumerator()
	{
		return _dataDictionary.GetEnumerator();
	}

	public IEnumerator GetEnumerator()
	{
		return _dataDictionary.GetEnumerator();
	}

	public bool ContainsKey(string key)
	{
		return _dataDictionary.ContainsKey(key);
	}

	public bool TryGetValue(string key, out ReportDataSet value)
	{
		return _dataDictionary.TryGetValue(key, out value);
	}
}
