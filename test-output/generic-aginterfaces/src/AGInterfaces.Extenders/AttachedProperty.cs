using System;
using System.Collections.Generic;

namespace AGInterfaces.Extenders;

public static class AttachedProperty
{
	private static readonly Dictionary<WeakReference, Dictionary<string, object>> Values = new Dictionary<WeakReference, Dictionary<string, object>>();

	private static WeakReference GetKey(object o)
	{
		List<WeakReference> list = new List<WeakReference>();
		try
		{
			foreach (WeakReference key in Values.Keys)
			{
				if (!key.IsAlive)
				{
					list.Add(key);
				}
				else if (key.Target == o)
				{
					return key;
				}
			}
			WeakReference weakReference = new WeakReference(o);
			Values.Add(weakReference, new Dictionary<string, object>());
			return weakReference;
		}
		finally
		{
			foreach (WeakReference item in list)
			{
				Values.Remove(item);
			}
		}
	}

	public static void SetAttachedProperty(this object o, string name, object value)
	{
		WeakReference key = GetKey(o);
		Dictionary<string, object> dictionary = Values[key];
		if (dictionary.ContainsKey(name))
		{
			dictionary[name] = value;
		}
		else
		{
			dictionary.Add(name, value);
		}
	}

	public static object GetAttachedProperty(this object o, string name)
	{
		WeakReference key = GetKey(o);
		if (Values[key].TryGetValue(name, out var value))
		{
			return value;
		}
		return null;
	}

	public static T GetAttachedProperty<T>(this object o, string name, T defaultValue = default(T))
	{
		WeakReference key = GetKey(o);
		if (Values[key].TryGetValue(name, out var value))
		{
			return (T)value;
		}
		return defaultValue;
	}
}
