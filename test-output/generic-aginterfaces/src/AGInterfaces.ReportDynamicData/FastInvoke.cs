using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace AGInterfaces.ReportDynamicData;

public static class FastInvoke
{
	public static Dictionary<string, Action<object, object>> GetSettersList<T>(this T obj, Dictionary<string, Type> PropertyDictionary)
	{
		Dictionary<string, Action<object, object>> dictionary = new Dictionary<string, Action<object, object>>(PropertyDictionary.Count);
		foreach (KeyValuePair<string, Type> item in PropertyDictionary)
		{
			string key = item.Key;
			PropertyInfo property = obj.GetType().GetProperty(key);
			dictionary.Add(key, property.GenerateSetterAction());
		}
		return dictionary;
	}

	public static Action<object, object> GenerateSetterAction(this PropertyInfo pi)
	{
		ParameterExpression parameterExpression = Expression.Parameter(typeof(object), "p");
		UnaryExpression instance = Expression.Convert(parameterExpression, pi.DeclaringType);
		ParameterExpression parameterExpression2 = Expression.Parameter(typeof(object), "v");
		UnaryExpression unaryExpression = Expression.Convert(parameterExpression2, pi.PropertyType);
		return Expression.Lambda<Action<object, object>>(Expression.Call(instance, pi.GetSetMethod(), unaryExpression), new ParameterExpression[2] { parameterExpression, parameterExpression2 }).Compile();
	}
}
