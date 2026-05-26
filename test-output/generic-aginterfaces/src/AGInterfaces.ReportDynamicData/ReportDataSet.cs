using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AGInterfaces.ReportDynamicData;

public class ReportDataSet
{
	public string Name;

	private readonly Type _type;

	private readonly ConcurrentBag<object> _items;

	private readonly Dictionary<string, Action<object, object>> _setterList;

	private static readonly string[][] tabularDataOrderKeys = new string[2][]
	{
		new string[1] { "trip_Index" },
		new string[1] { "DateTime" }
	};

	private static readonly string[][] tripDataOrderKeys = new string[4][]
	{
		new string[1] { "trip_Index" },
		new string[1] { "stage_Description" },
		new string[1] { "stage_Index" },
		new string[2] { "DateTime_First", "DateTime" }
	};

	private static readonly string[][] finalDataOrderKeys = new string[4][]
	{
		new string[1] { "trip_Index" },
		new string[1] { "stage_Description" },
		new string[1] { "stage_Index" },
		new string[2] { "DateTime_Last", "DateTime" }
	};

	public Dictionary<string, Type> PropertyDictionary { get; }

	public ReportDataSet(string dataSetName, Dictionary<string, Type> propertyDictionary)
	{
		PropertyDictionary = propertyDictionary;
		Name = dataSetName;
		_items = new ConcurrentBag<object>();
		_type = DataTypeBuilder.CompileResultType(typeof(BaseDataRecord), "DynamicDataRecord", propertyDictionary);
		BaseDataRecord obj = (BaseDataRecord)Activator.CreateInstance(_type);
		_setterList = obj.GetSettersList(propertyDictionary);
	}

	public ReportDataSet(string dataSetName, StageTypeGenerator stageTypeGenerator)
	{
		PropertyDictionary = stageTypeGenerator.Parameters;
		Name = dataSetName;
		_items = new ConcurrentBag<object>();
		_type = stageTypeGenerator.ClassType;
		_setterList = stageTypeGenerator.SetterList;
	}

	public BaseDataRecord AddNewRecord()
	{
		object obj = Activator.CreateInstance(_type);
		_items.Add(obj);
		BaseDataRecord obj2 = (BaseDataRecord)obj;
		obj2.setterList = _setterList;
		return obj2;
	}

	public IReadOnlyCollection<object> GetDataList()
	{
		List<object> list = _items.ToList();
		if (list.Any())
		{
			if (PropertyDictionary.ContainsKey("vehicle_Guid"))
			{
				Type type = list.First().GetType();
				PropertyInfo propertyInfoGuid = type.GetProperty("vehicle_Guid");
				if (propertyInfoGuid != null)
				{
					string[][] array = ((PropertyDictionary.ContainsKey("DateTime_First") && type.GetProperty("DateTime_First") != null) ? tripDataOrderKeys : ((!PropertyDictionary.ContainsKey("DateTime_Last") || !(type.GetProperty("DateTime_Last") != null)) ? tabularDataOrderKeys : finalDataOrderKeys));
					IOrderedEnumerable<object> source = list.OrderBy((object p) => GetField(p, propertyInfoGuid));
					string[][] array2 = array;
					foreach (string[] source2 in array2)
					{
						PropertyInfo prop = (from k in source2.Where(PropertyDictionary.ContainsKey)
							select type.GetProperty(k) into p
							where p != null
							select p).FirstOrDefault();
						if (prop != null)
						{
							source = source.ThenBy((object p) => GetField(p, prop));
						}
					}
					list = source.ToList();
				}
			}
			else
			{
				list.Reverse();
			}
		}
		return list;
	}

	private object GetField(object item, PropertyInfo pi)
	{
		return pi.GetValue(item);
	}
}
