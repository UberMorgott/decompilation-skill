using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AGInterfaces.Extenders;

public static class OtherExtenders
{
	private sealed class TypeCacheItem
	{
		internal readonly MemberInfo memberInfo;

		internal readonly CustomFilter customFilter;

		private readonly bool isProperty;

		internal TypeCacheItem(MemberInfo mInfo, CustomFilter customFilter)
		{
			memberInfo = mInfo;
			isProperty = memberInfo is PropertyInfo;
			this.customFilter = customFilter;
		}

		internal void SetValue(object classInstance, object value)
		{
			if (isProperty)
			{
				((PropertyInfo)memberInfo).SetValue(classInstance, value, null);
			}
			else
			{
				((FieldInfo)memberInfo).SetValue(classInstance, value);
			}
		}
	}

	public delegate T MapToForEachDelegate<T>(int index, T item);

	private static readonly Dictionary<Type, string> typeShortNames = new Dictionary<Type, string>
	{
		{
			typeof(bool),
			"bool"
		},
		{
			typeof(byte),
			"byte"
		},
		{
			typeof(int),
			"int"
		},
		{
			typeof(long),
			"Int64"
		},
		{
			typeof(double),
			"double"
		},
		{
			typeof(DateTime),
			"DateTime"
		},
		{
			typeof(TimeSpan),
			"TimeSpan"
		},
		{
			typeof(Guid),
			"Guid"
		},
		{
			typeof(Quadro<Guid>),
			"Guid4"
		},
		{
			typeof(string),
			"string"
		},
		{
			typeof(Image),
			"Image"
		},
		{
			typeof(Coordinates),
			"Coordinates"
		},
		{
			typeof(Location),
			"Location"
		}
	};

	private static readonly Type customFilterType = typeof(CustomFilter);

	private static readonly ConcurrentDictionary<Type, TypeCacheItem[]> cacheTypeInfo = new ConcurrentDictionary<Type, TypeCacheItem[]>();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static string typeShortName(this Type type)
	{
		if (!typeShortNames.TryGetValue(type, out var value))
		{
			return type.Name;
		}
		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DevPrmInfo[] Flatten(this DevPrmsGroupInfo[] parms)
	{
		return parms.OfType<DevPrmInfo>().Concat(parms.Where((DevPrmsGroupInfo p) => p.devPrmInfoArray != null).SelectMany((DevPrmsGroupInfo p) => p.devPrmInfoArray)).ToArray();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static DevPrmInfo[] Flatten(this DevPrmsGroupInfo[] parms, Func<DevPrmInfo, bool> predicate)
	{
		return parms.OfType<DevPrmInfo>().Where(predicate).Concat(parms.Where((DevPrmsGroupInfo p) => p.devPrmInfoArray != null).SelectMany((DevPrmsGroupInfo p) => p.devPrmInfoArray.Where(predicate)))
			.ToArray();
	}

	public static DevPrmInfo Find<T>(this T[] parms, Predicate<DevPrmInfo> predicate) where T : DevPrmsGroupInfo
	{
		foreach (T val in parms)
		{
			if (val is DevPrmInfo devPrmInfo)
			{
				if (predicate(devPrmInfo))
				{
					return devPrmInfo;
				}
			}
			else
			{
				if (val.devPrmInfoArray == null)
				{
					continue;
				}
				DevPrmInfo[] devPrmInfoArray = val.devPrmInfoArray;
				foreach (DevPrmInfo devPrmInfo2 in devPrmInfoArray)
				{
					if (predicate(devPrmInfo2))
					{
						return devPrmInfo2;
					}
				}
			}
		}
		return null;
	}

	private static Dictionary<TypeCacheItem, DevPrmInfo> getMemberMap(Type type, DevPrmInfo[] parms)
	{
		if (!cacheTypeInfo.TryGetValue(type, out var value))
		{
			cacheTypeInfo.TryAdd(type, value = (from p in type.GetFields()
				where p.IsPublic
				select new TypeCacheItem(p, p.GetCustomAttributes(customFilterType, inherit: true).FirstOrDefault() as CustomFilter)).Concat(from p in type.GetProperties()
				where p.GetSetMethod() != null
				select new TypeCacheItem(p, p.GetCustomAttributes(customFilterType, inherit: true).FirstOrDefault() as CustomFilter)).ToArray());
		}
		Dictionary<TypeCacheItem, DevPrmInfo> dictionary = new Dictionary<TypeCacheItem, DevPrmInfo>();
		TypeCacheItem[] array = value;
		foreach (TypeCacheItem typeCacheItem in array)
		{
			string name = typeCacheItem.memberInfo.Name;
			AddValueType valueType = AddValueType.Curr;
			if (typeCacheItem.customFilter != null)
			{
				if (!string.IsNullOrEmpty(typeCacheItem.customFilter.Name))
				{
					name = typeCacheItem.customFilter.Name;
				}
				valueType = typeCacheItem.customFilter.ValueType;
			}
			DevPrmInfo devPrmInfo = parms.FirstOrDefault((DevPrmInfo p) => p.name == name && p.addValueType == valueType);
			if (devPrmInfo != null)
			{
				dictionary.Add(typeCacheItem, devPrmInfo);
			}
		}
		return dictionary;
	}

	public static T[] MapTo<T>(this Array[] data, DevPrmInfo[] parms, MapToForEachDelegate<T> forEachElement = null)
	{
		Type typeFromHandle = typeof(T);
		if (data == null)
		{
			throw new InvalidDataException("MapTo: 'data' parameter is null");
		}
		ConstructorInfo constructor = typeFromHandle.GetConstructor(new Type[0]);
		if (constructor == null)
		{
			throw new InvalidDataException("Type " + typeFromHandle.FullName + " don't have default constructor without parameters");
		}
		if (data.Length == 0)
		{
			return new T[0];
		}
		Dictionary<TypeCacheItem, DevPrmInfo> memberMap = getMemberMap(typeFromHandle, parms);
		int length = data[0].GetLength(0);
		List<T> list = new List<T>();
		for (int i = 0; i < length; i++)
		{
			T val = (T)constructor.Invoke(null);
			foreach (KeyValuePair<TypeCacheItem, DevPrmInfo> item in memberMap)
			{
				item.Key.SetValue(val, data[item.Value.creationIndex].GetValue(i));
			}
			if (forEachElement != null)
			{
				val = forEachElement(i, val);
			}
			if (val != null)
			{
				list.Add(val);
			}
		}
		return list.ToArray();
	}

	public static dynamic[] MapToDynamic(this Array[] data, DevPrmInfo[] parms, params string[] props)
	{
		if (data == null)
		{
			throw new InvalidDataException("MapTo: 'data' parameter is null");
		}
		if (data.Length == 0)
		{
			return new object[0];
		}
		int length = data[0].GetLength(0);
		List<object> list = new List<object>();
		Dictionary<DevPrmInfo, string> dictionary = new Dictionary<DevPrmInfo, string>();
		foreach (string prop in props)
		{
			DevPrmInfo[] array = parms.Where((DevPrmInfo p) => p.name == prop).ToArray();
			if (array.Length == 1)
			{
				dictionary.Add(array[0], prop);
			}
			else if (array.Length > 1)
			{
				DevPrmInfo[] array2 = array;
				foreach (DevPrmInfo devPrmInfo in array2)
				{
					dictionary.Add(devPrmInfo, devPrmInfo.name + devPrmInfo.addValueType);
				}
			}
		}
		for (int num2 = 0; num2 < length; num2++)
		{
			dynamic val = new ExpandoObject();
			IDictionary<string, object> dictionary2 = val as IDictionary<string, object>;
			foreach (KeyValuePair<DevPrmInfo, string> item in dictionary)
			{
				dictionary2.Add(item.Value, data[item.Key.creationIndex].GetValue(num2));
			}
			list.Add(val);
		}
		return list.ToArray();
	}
}
