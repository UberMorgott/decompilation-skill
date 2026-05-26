using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AGInterfaces.ReportDynamicData;

public class BaseDataRecord : ICloneable
{
	internal Dictionary<string, Action<object, object>> setterList = new Dictionary<string, Action<object, object>>();

	private static string[] toOrderedPropNames = new string[7] { "vehicle_Guid", "trip_Index", "stage_Description", "stage_Index", "DateTime", "DateTime_First", "DateTime_Last" };

	public object this[string propertyName]
	{
		get
		{
			return GetValue(propertyName);
		}
		set
		{
			SetPropertyValue(propertyName, value);
		}
	}

	public IEnumerable<string> GetPropertiesList()
	{
		return from pi in GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
			select pi.Name;
	}

	public void SetPropertyValue(string propertyName, object value)
	{
		if (value != null && setterList.ContainsKey(propertyName))
		{
			setterList[propertyName]?.Invoke(this, value);
		}
	}

	public object GetValue(string propertyName)
	{
		PropertyInfo property = GetType().GetProperty(propertyName);
		return GetValue(property);
	}

	private object GetValue(PropertyInfo propertyInfo)
	{
		return propertyInfo?.GetValue(this);
	}

	public T GetValue<T>(string propertyName)
	{
		try
		{
			return (T)GetValue(propertyName);
		}
		catch
		{
			return default(T);
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override string ToString()
	{
		return string.Join(", ", from pi in toOrderedPropNames.Select(GetType().GetProperty)
			where pi != null
			select $"{pi.Name}={GetValue(pi)}");
	}
}
