using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using AGInterfaces.Classes;

namespace AGInterfaces;

[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
public class ExpressionBase : IDisposable
{
	private readonly IDataViewerFormatters _dataViewerFormatters;

	public readonly IAutoGRAPHShell shellProvider;

	public readonly TimeZoneInfo timeZoneInfo;

	public readonly IPhotoStorage photoStorage;

	public readonly Dictionary<string, object> properties;

	public readonly Dictionary<int, string> gfTypes;

	public readonly Dictionary<int, string> implTypes;

	public readonly Dictionary<Guid, Dictionary<string, object>> elmProperties = new Dictionary<Guid, Dictionary<string, object>>();

	public readonly IElements Devices;

	public readonly IElements GeoFences;

	public readonly IElements Drivers;

	public readonly IElements Implements;

	public readonly IElements Tasks;

	public readonly string format;

	public readonly Dictionary<PeriodicID<string>, GroupOrElementInfo> elmByStrDict = new Dictionary<PeriodicID<string>, GroupOrElementInfo>();

	public readonly Dictionary<PeriodicID<long>, Tuple<GroupOrElementInfo, string>> elmByIDDict = new Dictionary<PeriodicID<long>, Tuple<GroupOrElementInfo, string>>();

	public ElementType autoType;

	protected internal DateTime _UDT;

	public readonly bool FullPhoto;

	[AGInfo("PI", "PI constant.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public const double PI = Math.PI;

	public virtual object ___Result => null;

	public virtual void ___Invoke(Array array, int i, object parms)
	{
	}

	public virtual void setRead()
	{
	}

	public virtual void setTimedRead()
	{
	}

	public ExpressionBase(ExpressionBaseInitInfo initInfo, ParameterInitInfo prmInitInfo, IDataViewerFormatters dataViewerFormatters)
	{
		_dataViewerFormatters = dataViewerFormatters;
		shellProvider = initInfo.shellProvider;
		timeZoneInfo = initInfo.timeZoneInfo;
		photoStorage = initInfo.photoStorage;
		properties = initInfo.properties;
		gfTypes = initInfo.gfTypes;
		implTypes = initInfo.implTypes;
		Devices = initInfo.Devices;
		GeoFences = initInfo.GeoFences;
		Drivers = initInfo.Drivers;
		Implements = initInfo.Implements;
		Tasks = initInfo.Tasks;
		format = prmInitInfo.format;
		FullPhoto = initInfo.FullPhoto;
	}

	private object getPrm(string name, DateTime udt, bool forceSetTimedRead = false)
	{
		if (name == null)
		{
			return null;
		}
		return getPrmValue(properties, name, udt, forceSetTimedRead);
	}

	private object getPrm(string name, Guid guid, DateTime udt, bool forceSetTimedRead = false, ElementType elementType = ElementType.None)
	{
		if (name == null)
		{
			return null;
		}
		if (!elmProperties.TryGetValue(guid, out var value))
		{
			if (elementType == ElementType.None)
			{
				value = shellProvider.PropertiesRegistry.GetPropValueTables(ElementType.Device, guid, allInheritedProps: true);
				if (value == null)
				{
					value = shellProvider.PropertiesRegistry.GetPropValueTables(ElementType.GeoFence, guid, allInheritedProps: true);
					if (value == null)
					{
						value = shellProvider.PropertiesRegistry.GetPropValueTables(ElementType.Driver, guid, allInheritedProps: true);
						if (value == null)
						{
							value = shellProvider.PropertiesRegistry.GetPropValueTables(ElementType.Implement, guid, allInheritedProps: true);
							if (value == null)
							{
								value = shellProvider.PropertiesRegistry.GetPropValueTables(ElementType.Task, guid, allInheritedProps: true);
							}
						}
					}
				}
			}
			else
			{
				value = shellProvider.PropertiesRegistry.GetPropValueTables(elementType, guid, allInheritedProps: true);
			}
			elmProperties.Add(guid, value);
		}
		if (value == null)
		{
			return null;
		}
		return getPrmValue(value, name, udt, forceSetTimedRead);
	}

	private object getPrmValue(Dictionary<string, object> elmProps, string name, DateTime udt, bool forceSetTimedRead)
	{
		if (elmProps.TryGetValue(name, out var value))
		{
			setRead();
		}
		if (!(value is PropertyTable))
		{
			return value;
		}
		if (forceSetTimedRead)
		{
			setTimedRead();
		}
		if (udt == default(DateTime))
		{
			return null;
		}
		PropertyTableItem[] entries = ((PropertyTable)value).Entries;
		if (entries.Length == 0)
		{
			return null;
		}
		int nearestEntryIndex = ((PropertyTable)value).GetNearestEntryIndex(udt);
		if (Nullable.Compare(udt, entries[nearestEntryIndex].startDT) < 0)
		{
			return null;
		}
		if (entries[nearestEntryIndex].endDT.HasValue && Nullable.Compare(udt, entries[nearestEntryIndex].endDT) >= 0)
		{
			return null;
		}
		return entries[nearestEntryIndex].value;
	}

	public bool PrmAnyOf(string name, Guid guid, params int[] values)
	{
		int num = PrmInt(name, int.MinValue, guid, _UDT);
		if (num != int.MinValue)
		{
			return Enumerable.Contains(values, num);
		}
		return false;
	}

	public bool PrmAnyOf(string name, Guid guid, params double[] values)
	{
		double num = PrmDouble(name, double.NaN, guid, _UDT);
		if (!double.IsNaN(num))
		{
			return Enumerable.Contains(values, num);
		}
		return false;
	}

	public bool PrmAnyOf(string name, Guid guid, params string[] values)
	{
		string text = PrmString(name, null, guid, _UDT);
		if (text != null)
		{
			return Enumerable.Contains(values, text);
		}
		return false;
	}

	public bool PrmAnyOf(string name, Quadro<Guid> guid4, params int[] values)
	{
		if (guid4.q1 != Guid.Empty)
		{
			int num = PrmInt(name, int.MinValue, guid4.q1, _UDT);
			if (num != int.MinValue && Enumerable.Contains(values, num))
			{
				return true;
			}
			if (guid4.q2 != Guid.Empty)
			{
				num = PrmInt(name, int.MinValue, guid4.q2, _UDT);
				if (num != int.MinValue && Enumerable.Contains(values, num))
				{
					return true;
				}
				if (guid4.q3 != Guid.Empty)
				{
					num = PrmInt(name, int.MinValue, guid4.q3, _UDT);
					if (num != int.MinValue && Enumerable.Contains(values, num))
					{
						return true;
					}
					if (guid4.q4 != Guid.Empty)
					{
						num = PrmInt(name, int.MinValue, guid4.q4, _UDT);
						if (num != int.MinValue && Enumerable.Contains(values, num))
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public bool PrmAnyOf(string name, Quadro<Guid> guid4, params double[] values)
	{
		if (guid4.q1 != Guid.Empty)
		{
			double num = PrmDouble(name, double.NaN, guid4.q1, _UDT);
			if (!double.IsNaN(num) && Enumerable.Contains(values, num))
			{
				return true;
			}
			if (guid4.q2 != Guid.Empty)
			{
				num = PrmDouble(name, double.NaN, guid4.q2, _UDT);
				if (!double.IsNaN(num) && Enumerable.Contains(values, num))
				{
					return true;
				}
				if (guid4.q3 != Guid.Empty)
				{
					num = PrmDouble(name, double.NaN, guid4.q3, _UDT);
					if (!double.IsNaN(num) && Enumerable.Contains(values, num))
					{
						return true;
					}
					if (guid4.q4 != Guid.Empty)
					{
						num = PrmDouble(name, double.NaN, guid4.q4, _UDT);
						if (!double.IsNaN(num) && Enumerable.Contains(values, num))
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public bool PrmAnyOf(string name, Quadro<Guid> guid4, params string[] values)
	{
		if (guid4.q1 != Guid.Empty)
		{
			string text = PrmString(name, null, guid4.q1, _UDT);
			if (text != null && Enumerable.Contains(values, text))
			{
				return true;
			}
			if (guid4.q2 != Guid.Empty)
			{
				text = PrmString(name, null, guid4.q2, _UDT);
				if (text != null && Enumerable.Contains(values, text))
				{
					return true;
				}
				if (guid4.q3 != Guid.Empty)
				{
					text = PrmString(name, null, guid4.q3, _UDT);
					if (text != null && Enumerable.Contains(values, text))
					{
						return true;
					}
					if (guid4.q4 != Guid.Empty)
					{
						text = PrmString(name, null, guid4.q4, _UDT);
						if (text != null && Enumerable.Contains(values, text))
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	[AGInfo("Boolean property", "Boolean property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Sensor, true, null)]
	public bool PrmBool([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] bool def = false, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt);
		if (prm is decimal)
		{
			return (decimal)prm != 0m;
		}
		if (prm is bool)
		{
			return (bool)prm;
		}
		return def;
	}

	[AGInfo("Boolean property", "Boolean property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public bool PrmBool([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] bool def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		if (prm is decimal)
		{
			return (decimal)prm != 0m;
		}
		if (prm is bool)
		{
			return (bool)prm;
		}
		return def;
	}

	[AGInfo("Integer property", "Integer property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level, true, null)]
	public int PrmInt([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt);
		if (prm is decimal)
		{
			return decimal.ToInt32((decimal)prm);
		}
		if (prm is int)
		{
			return (int)prm;
		}
		if (prm != null && prm.GetType().Name.Equals("FDayOfWeek"))
		{
			return (int)prm;
		}
		return def;
	}

	[AGInfo("Integer property", "Integer property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public int PrmInt([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		if (prm is decimal)
		{
			return decimal.ToInt32((decimal)prm);
		}
		if (prm is int)
		{
			return (int)prm;
		}
		if (prm != null && prm.GetType().Name.Equals("FDayOfWeek"))
		{
			return (int)prm;
		}
		return def;
	}

	public ValuePriorityPair PrmInt([AGPrmInfo("Value property name", double.MinValue, double.MinValue)] string valName, [AGPrmInfo("Priority property name", double.MinValue, double.MinValue)] string priorName, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(valName, udt);
		int value = ((prm is decimal) ? decimal.ToInt32((decimal)prm) : ((!(prm is int)) ? def : ((int)prm)));
		prm = getPrm(priorName, udt);
		int priority = ((prm is decimal) ? decimal.ToInt32((decimal)prm) : ((prm is int) ? ((int)prm) : 0));
		return new ValuePriorityPair(value, priority);
	}

	public ValuePriorityPair PrmInt([AGPrmInfo("Value property name", double.MinValue, double.MinValue)] string valName, [AGPrmInfo("Priority property name", double.MinValue, double.MinValue)] string priorName, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(valName, guid, udt);
		int value = ((prm is decimal) ? decimal.ToInt32((decimal)prm) : ((!(prm is int)) ? def : ((int)prm)));
		prm = getPrm(priorName, guid, udt);
		int priority = ((prm is decimal) ? decimal.ToInt32((decimal)prm) : ((prm is int) ? ((int)prm) : 0));
		return new ValuePriorityPair(value, priority);
	}

	[AGInfo("Real property", "Real property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Level, true, null)]
	public double PrmDouble([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def = 0.0, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt);
		if (prm is decimal)
		{
			return decimal.ToDouble((decimal)prm);
		}
		if (prm is double)
		{
			return (double)prm;
		}
		return def;
	}

	[AGInfo("Real property", "Real property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public double PrmDouble([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		if (prm is decimal)
		{
			return decimal.ToDouble((decimal)prm);
		}
		if (prm is double)
		{
			return (double)prm;
		}
		return def;
	}

	[AGInfo("Date property", "Date and time from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public DateTime PrmDate([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] DateTime def = default(DateTime), [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt);
		if (prm is DateTime)
		{
			return (DateTime)prm;
		}
		return def;
	}

	[AGInfo("Date property", "Date and time from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public DateTime PrmDate([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] DateTime def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		if (prm is DateTime)
		{
			return (DateTime)prm;
		}
		return def;
	}

	[AGInfo("Timespan property", "Timespan property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public TimeSpan PrmTime([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] TimeSpan def = default(TimeSpan), [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt);
		if (prm is TimeSpan)
		{
			return (TimeSpan)prm;
		}
		if (prm is DateTime dateTime)
		{
			return dateTime.TimeOfDay;
		}
		if (prm is decimal)
		{
			return TimeSpan.FromSeconds(decimal.ToDouble((decimal)prm));
		}
		if (prm is double)
		{
			return TimeSpan.FromSeconds((double)prm);
		}
		if (prm is int)
		{
			return TimeSpan.FromSeconds((int)prm);
		}
		return def;
	}

	[AGInfo("Timespan property", "Timespan property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public TimeSpan PrmTime([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] TimeSpan def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		if (prm is TimeSpan)
		{
			return (TimeSpan)prm;
		}
		if (prm is DateTime dateTime)
		{
			return dateTime.TimeOfDay;
		}
		if (prm is decimal)
		{
			return TimeSpan.FromSeconds(decimal.ToDouble((decimal)prm));
		}
		if (prm is double)
		{
			return TimeSpan.FromSeconds((double)prm);
		}
		if (prm is int)
		{
			return TimeSpan.FromSeconds((int)prm);
		}
		return def;
	}

	[AGInfo("Color property", "Color property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public Color PrmColor([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Color def = default(Color), [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt);
		if (prm is int)
		{
			return Color.FromArgb((int)prm);
		}
		return def;
	}

	[AGInfo("Color property", "Color property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public Color PrmColor([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Color def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		if (prm is int)
		{
			return Color.FromArgb((int)prm);
		}
		return def;
	}

	[AGInfo("Guid property", "Guid property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, true, null)]
	public Guid PrmGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def = default(Guid), [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt, forceSetTimedRead: true);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property", "Guid property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public Guid PrmGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt, forceSetTimedRead: true);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property of device", "Guid property of device from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, true, null)]
	public Guid DeviceGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def = default(Guid), [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt, forceSetTimedRead: true);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property of device", "Guid property of device from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public Guid DeviceGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def, [AGPrmInfo("Guid of device", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt, forceSetTimedRead: true, ElementType.Device);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property of geo-fence", "Guid property of geo-fence from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier, true, null)]
	public Guid GeoFenceGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def = default(Guid), [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt, forceSetTimedRead: true);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property of geo-fence", "Guid property of geo-fence from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public Guid GeoFenceGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def, [AGPrmInfo("Guid of geo-fence", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt, forceSetTimedRead: true, ElementType.GeoFence);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property of driver", "Guid property of driver from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier | AGInfoGroupType.Driver, true, null)]
	public Guid DriverGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def = default(Guid), [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt, forceSetTimedRead: true);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property of driver", "Guid property of driver from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public Guid DriverGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def, [AGPrmInfo("Guid of driver", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt, forceSetTimedRead: true, ElementType.Driver);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property of implement", "Guid property of implement from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data | AGInfoGroupType.Identifier | AGInfoGroupType.Implement, true, null)]
	public Guid ImplementGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def = default(Guid), [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt, forceSetTimedRead: true);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("Guid property of implement", "Guid property of implement from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public Guid ImplementGuid([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] Guid def, [AGPrmInfo("Guid of implement", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt, forceSetTimedRead: true, ElementType.Implement);
		if (prm is Guid)
		{
			return (Guid)prm;
		}
		return def;
	}

	[AGInfo("List property to string", "List property from registry to string. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public string PrmListToString([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] string def = null, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		if (getPrm(name, udt, forceSetTimedRead: true) is List<Guid> source && source.Any())
		{
			return source.Select((Guid p) => nameByGUID(p)).Aggregate((string acc, string sel) => acc + ", " + sel);
		}
		return def;
	}

	[AGInfo("Sum of property values", "Sum of property values from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public double PrmDoubleSum([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Value name", double.MinValue, double.MinValue)] string valName, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def = 0.0, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		if (!(getPrm(name, udt, forceSetTimedRead: true) is List<Guid> source))
		{
			return def;
		}
		return (from id in source
			select getGroupOrElementInfo(id) into gi
			where gi != null
			select gi).Sum((GroupOrElementInfo gi) => getDoubleSum(valName, gi, udt));
	}

	private double getDoubleSum(string valName, GroupOrElementInfo gi, DateTime udt)
	{
		return gi.Type switch
		{
			GroupNodeType.Group => gi.Children?.Sum((GroupOrElementInfo ei) => getDoubleSum(valName, ei, udt)) ?? 0.0, 
			GroupNodeType.Element => PrmDouble(valName, 0.0, gi.GUID, udt), 
			_ => 0.0, 
		};
	}

	private GroupOrElementInfo getGroupOrElementInfo(Guid guid)
	{
		if (!Devices.TryGetElement(guid, out var element) && !GeoFences.TryGetElement(guid, out element) && !Drivers.TryGetElement(guid, out element) && !Implements.TryGetElement(guid, out element) && !Tasks.TryGetElement(guid, out element))
		{
			return null;
		}
		return element;
	}

	[AGInfo("String property", "String property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public string PrmString([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] string def = null, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, udt);
		if (prm != null)
		{
			return prm.ToString();
		}
		return def;
	}

	[AGInfo("String property", "String property from registry. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public string PrmString([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] string def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		if (prm != null)
		{
			return prm.ToString();
		}
		return def;
	}

	[AGInfo("Geo-fence type as string", "Geo-fence type from registry as string. Default value will be used if property isn't specified. Element identifier by default is empty (out of geo-fences).", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public string GFTypeString([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		int key = ((prm is decimal) ? decimal.ToInt32((decimal)prm) : def);
		gfTypes.TryGetValue(key, out var value);
		return value;
	}

	[AGInfo("Implement type as string", "Implement type from registry as string. Default value will be used if property isn't specified. Element identifier by default is empty (without implement).", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public string ImplTypeString([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def, [AGPrmInfo("Guid of element", double.MinValue, double.MinValue)] Guid guid, [AGPrmInfo("Date and time", double.MinValue, double.MinValue)] DateTime udt = default(DateTime))
	{
		if (udt == default(DateTime))
		{
			udt = _UDT;
		}
		else if (udt.Kind == DateTimeKind.Local)
		{
			udt = udt.ToUniversalTime();
		}
		object prm = getPrm(name, guid, udt);
		int key = ((prm is decimal) ? decimal.ToInt32((decimal)prm) : def);
		implTypes.TryGetValue(key, out var value);
		return value;
	}

	[AGInfo("String property for multiple elements", "String property from registry for multiple elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public string PrmString([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] string def, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4)
	{
		object obj = ((guid4.q1 != Guid.Empty) ? getPrm(name, guid4.q1, _UDT) : null);
		object obj2 = ((guid4.q2 != Guid.Empty) ? getPrm(name, guid4.q2, _UDT) : null);
		object obj3 = ((guid4.q3 != Guid.Empty) ? getPrm(name, guid4.q3, _UDT) : null);
		object obj4 = ((guid4.q4 != Guid.Empty) ? getPrm(name, guid4.q4, _UDT) : null);
		string text = null;
		string text2 = ((obj != null) ? obj.ToString() : def);
		if (!string.IsNullOrEmpty(text2))
		{
			text += text2;
		}
		text2 = ((obj2 != null) ? obj2.ToString() : def);
		if (!string.IsNullOrEmpty(text2))
		{
			if (text != null)
			{
				text += ", ";
			}
			text += text2;
		}
		text2 = ((obj3 != null) ? obj3.ToString() : def);
		if (!string.IsNullOrEmpty(text2))
		{
			if (text != null)
			{
				text += ", ";
			}
			text += text2;
		}
		text2 = ((obj4 != null) ? obj4.ToString() : def);
		if (!string.IsNullOrEmpty(text2))
		{
			if (text != null)
			{
				text += ", ";
			}
			text += text2;
		}
		return text;
	}

	[AGInfo("Geo-fence type as string for multiple elements", "Geo-fence type from registry as string for multiple element. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public string GFTypeString([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4)
	{
		object obj = ((guid4.q1 != Guid.Empty) ? getPrm(name, guid4.q1, _UDT) : null);
		object obj2 = ((guid4.q2 != Guid.Empty) ? getPrm(name, guid4.q2, _UDT) : null);
		object obj3 = ((guid4.q3 != Guid.Empty) ? getPrm(name, guid4.q3, _UDT) : null);
		object obj4 = ((guid4.q4 != Guid.Empty) ? getPrm(name, guid4.q4, _UDT) : null);
		string text = null;
		int key = ((obj is decimal) ? decimal.ToInt32((decimal)obj) : def);
		if (gfTypes.TryGetValue(key, out var value))
		{
			text += value;
		}
		key = ((obj2 is decimal) ? decimal.ToInt32((decimal)obj2) : def);
		if (gfTypes.TryGetValue(key, out value))
		{
			if (text != null)
			{
				text += ", ";
			}
			text += value;
		}
		key = ((obj3 is decimal) ? decimal.ToInt32((decimal)obj3) : def);
		if (gfTypes.TryGetValue(key, out value))
		{
			if (text != null)
			{
				text += ", ";
			}
			text += value;
		}
		key = ((obj4 is decimal) ? decimal.ToInt32((decimal)obj4) : def);
		if (gfTypes.TryGetValue(key, out value))
		{
			if (text != null)
			{
				text += ", ";
			}
			text += value;
		}
		return text;
	}

	[AGInfo("Minimal boolean property", "Minimal value of boolean property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public bool PrmBoolMin([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] bool def = false)
	{
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q1, _UDT);
			if (prm is decimal && (decimal)prm == 0m)
			{
				return false;
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q2, _UDT);
			if (prm is decimal && (decimal)prm == 0m)
			{
				return false;
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q3, _UDT);
			if (prm is decimal && (decimal)prm == 0m)
			{
				return false;
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q4, _UDT);
			if (prm is decimal && (decimal)prm == 0m)
			{
				return false;
			}
		}
		return def;
	}

	[AGInfo("Maximal boolean property", "Maximal value of boolean property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public bool PrmBoolMax([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] bool def = false)
	{
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q1, _UDT);
			if (prm is decimal && (decimal)prm != 0m)
			{
				return true;
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q2, _UDT);
			if (prm is decimal && (decimal)prm != 0m)
			{
				return true;
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q3, _UDT);
			if (prm is decimal && (decimal)prm != 0m)
			{
				return true;
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q4, _UDT);
			if (prm is decimal && (decimal)prm != 0m)
			{
				return true;
			}
		}
		return def;
	}

	[AGInfo("Minimal integer property", "Minimal value of integer property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public int PrmIntMin([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0)
	{
		int num = 0;
		int num2 = int.MaxValue;
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q1, _UDT);
			if (prm is decimal)
			{
				num++;
				int num3 = decimal.ToInt32((decimal)prm);
				if (num2 > num3)
				{
					num2 = num3;
				}
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q2, _UDT);
			if (prm is decimal)
			{
				num++;
				int num4 = decimal.ToInt32((decimal)prm);
				if (num2 > num4)
				{
					num2 = num4;
				}
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q3, _UDT);
			if (prm is decimal)
			{
				num++;
				int num5 = decimal.ToInt32((decimal)prm);
				if (num2 > num5)
				{
					num2 = num5;
				}
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q4, _UDT);
			if (prm is decimal)
			{
				num++;
				int num6 = decimal.ToInt32((decimal)prm);
				if (num2 > num6)
				{
					num2 = num6;
				}
			}
		}
		if (num > 0)
		{
			return num2;
		}
		return def;
	}

	[AGInfo("Minimal integer property", "Minimal value of integer property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public ValuePriorityPair PrmIntMin([AGPrmInfo("Value property name", double.MinValue, double.MinValue)] string valName, [AGPrmInfo("Priority property name", double.MinValue, double.MinValue)] string priorName, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0)
	{
		int num = 0;
		int num2 = int.MaxValue;
		decimal num3 = decimal.MinValue;
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(valName, guid4.q1, _UDT);
			if (prm is decimal)
			{
				num++;
				int num4 = decimal.ToInt32((decimal)prm);
				prm = getPrm(priorName, guid4.q1, _UDT);
				decimal num5 = ((prm is decimal) ? ((decimal)prm) : 0m);
				if (num5 == num3)
				{
					if (num2 > num4)
					{
						num2 = num4;
					}
				}
				else if (num5 > num3)
				{
					num2 = num4;
					num3 = num5;
				}
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(valName, guid4.q2, _UDT);
			if (prm is decimal)
			{
				num++;
				int num6 = decimal.ToInt32((decimal)prm);
				prm = getPrm(priorName, guid4.q2, _UDT);
				decimal num7 = ((prm is decimal) ? ((decimal)prm) : 0m);
				if (num7 == num3)
				{
					if (num2 > num6)
					{
						num2 = num6;
					}
				}
				else if (num7 > num3)
				{
					num2 = num6;
					num3 = num7;
				}
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(valName, guid4.q3, _UDT);
			if (prm is decimal)
			{
				num++;
				int num8 = decimal.ToInt32((decimal)prm);
				prm = getPrm(priorName, guid4.q3, _UDT);
				decimal num9 = ((prm is decimal) ? ((decimal)prm) : 0m);
				if (num9 == num3)
				{
					if (num2 > num8)
					{
						num2 = num8;
					}
				}
				else if (num9 > num3)
				{
					num2 = num8;
					num3 = num9;
				}
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(valName, guid4.q4, _UDT);
			if (prm is decimal)
			{
				num++;
				int num10 = decimal.ToInt32((decimal)prm);
				prm = getPrm(priorName, guid4.q4, _UDT);
				decimal num11 = ((prm is decimal) ? ((decimal)prm) : 0m);
				if (num11 == num3)
				{
					if (num2 > num10)
					{
						num2 = num10;
					}
				}
				else if (num11 > num3)
				{
					num2 = num10;
					num3 = num11;
				}
			}
		}
		if (num > 0)
		{
			return new ValuePriorityPair(num2, (int)num3);
		}
		return new ValuePriorityPair(def, 0);
	}

	[AGInfo("Maximal integer property", "Maximal value of integer property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public int PrmIntMax([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0)
	{
		int num = 0;
		int num2 = int.MinValue;
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q1, _UDT);
			if (prm is decimal)
			{
				num++;
				int num3 = decimal.ToInt32((decimal)prm);
				if (num2 < num3)
				{
					num2 = num3;
				}
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q2, _UDT);
			if (prm is decimal)
			{
				num++;
				int num4 = decimal.ToInt32((decimal)prm);
				if (num2 < num4)
				{
					num2 = num4;
				}
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q3, _UDT);
			if (prm is decimal)
			{
				num++;
				int num5 = decimal.ToInt32((decimal)prm);
				if (num2 < num5)
				{
					num2 = num5;
				}
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q4, _UDT);
			if (prm is decimal)
			{
				num++;
				int num6 = decimal.ToInt32((decimal)prm);
				if (num2 < num6)
				{
					num2 = num6;
				}
			}
		}
		if (num > 0)
		{
			return num2;
		}
		return def;
	}

	[AGInfo("Maximal integer property", "Maximal value of integer property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public ValuePriorityPair PrmIntMax([AGPrmInfo("Value property name", double.MinValue, double.MinValue)] string valName, [AGPrmInfo("Priority property name", double.MinValue, double.MinValue)] string priorName, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] int def = 0)
	{
		int num = 0;
		int num2 = int.MinValue;
		decimal num3 = decimal.MinValue;
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(valName, guid4.q1, _UDT);
			if (prm is decimal)
			{
				num++;
				int num4 = decimal.ToInt32((decimal)prm);
				prm = getPrm(priorName, guid4.q1, _UDT);
				decimal num5 = ((prm is decimal) ? ((decimal)prm) : 0m);
				if (num5 == num3)
				{
					if (num2 < num4)
					{
						num2 = num4;
					}
				}
				else if (num5 > num3)
				{
					num2 = num4;
					num3 = num5;
				}
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(valName, guid4.q2, _UDT);
			if (prm is decimal)
			{
				num++;
				int num6 = decimal.ToInt32((decimal)prm);
				prm = getPrm(priorName, guid4.q2, _UDT);
				decimal num7 = ((prm is decimal) ? ((decimal)prm) : 0m);
				if (num7 == num3)
				{
					if (num2 < num6)
					{
						num2 = num6;
					}
				}
				else if (num7 > num3)
				{
					num2 = num6;
					num3 = num7;
				}
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(valName, guid4.q3, _UDT);
			if (prm is decimal)
			{
				num++;
				int num8 = decimal.ToInt32((decimal)prm);
				prm = getPrm(priorName, guid4.q3, _UDT);
				decimal num9 = ((prm is decimal) ? ((decimal)prm) : 0m);
				if (num9 == num3)
				{
					if (num2 < num8)
					{
						num2 = num8;
					}
				}
				else if (num9 > num3)
				{
					num2 = num8;
					num3 = num9;
				}
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(valName, guid4.q4, _UDT);
			if (prm is decimal)
			{
				num++;
				int num10 = decimal.ToInt32((decimal)prm);
				prm = getPrm(priorName, guid4.q4, _UDT);
				decimal num11 = ((prm is decimal) ? ((decimal)prm) : 0m);
				if (num11 == num3)
				{
					if (num2 < num10)
					{
						num2 = num10;
					}
				}
				else if (num11 > num3)
				{
					num2 = num10;
					num3 = num11;
				}
			}
		}
		if (num > 0)
		{
			return new ValuePriorityPair(num2, (int)num3);
		}
		return new ValuePriorityPair(def, 0);
	}

	[AGInfo("Minimal real property", "Minimal value of real property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public double PrmDoubleMin([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def = 0.0)
	{
		int num = 0;
		double num2 = double.MaxValue;
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q1, _UDT);
			if (prm is decimal)
			{
				num++;
				double num3 = decimal.ToDouble((decimal)prm);
				if (num2 > num3)
				{
					num2 = num3;
				}
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q2, _UDT);
			if (prm is decimal)
			{
				num++;
				double num4 = decimal.ToDouble((decimal)prm);
				if (num2 > num4)
				{
					num2 = num4;
				}
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q3, _UDT);
			if (prm is decimal)
			{
				num++;
				double num5 = decimal.ToDouble((decimal)prm);
				if (num2 > num5)
				{
					num2 = num5;
				}
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q4, _UDT);
			if (prm is decimal)
			{
				num++;
				double num6 = decimal.ToDouble((decimal)prm);
				if (num2 > num6)
				{
					num2 = num6;
				}
			}
		}
		if (num > 0)
		{
			return num2;
		}
		return def;
	}

	[AGInfo("Maximal real property", "Maximal value of real property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public double PrmDoubleMax([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def = 0.0)
	{
		int num = 0;
		double num2 = double.MinValue;
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q1, _UDT);
			if (prm is decimal)
			{
				num++;
				double num3 = decimal.ToDouble((decimal)prm);
				if (num2 < num3)
				{
					num2 = num3;
				}
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q2, _UDT);
			if (prm is decimal)
			{
				num++;
				double num4 = decimal.ToDouble((decimal)prm);
				if (num2 < num4)
				{
					num2 = num4;
				}
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q3, _UDT);
			if (prm is decimal)
			{
				num++;
				double num5 = decimal.ToDouble((decimal)prm);
				if (num2 < num5)
				{
					num2 = num5;
				}
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q4, _UDT);
			if (prm is decimal)
			{
				num++;
				double num6 = decimal.ToDouble((decimal)prm);
				if (num2 < num6)
				{
					num2 = num6;
				}
			}
		}
		if (num > 0)
		{
			return num2;
		}
		return def;
	}

	[AGInfo("Average real property", "Average value of real property from registry for list of elements. Default value will be used if property isn't specified.", DeviceParameterKind.Flag, AGInfoGroupType.Data, true, null)]
	public double PrmDoubleAver([AGPrmInfo("Property name", double.MinValue, double.MinValue)] string name, [AGPrmInfo("List of elements' Guids", double.MinValue, double.MinValue)] Quadro<Guid> guid4, [AGPrmInfo("Default value", double.MinValue, double.MinValue)] double def = 0.0)
	{
		int num = 0;
		double num2 = 0.0;
		if (guid4.q1 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q1, _UDT);
			if (prm is decimal)
			{
				num++;
				num2 += decimal.ToDouble((decimal)prm);
			}
		}
		if (guid4.q2 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q2, _UDT);
			if (prm is decimal)
			{
				num++;
				num2 += decimal.ToDouble((decimal)prm);
			}
		}
		if (guid4.q3 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q3, _UDT);
			if (prm is decimal)
			{
				num++;
				num2 += decimal.ToDouble((decimal)prm);
			}
		}
		if (guid4.q4 != Guid.Empty)
		{
			object prm = getPrm(name, guid4.q4, _UDT);
			if (prm is decimal)
			{
				num++;
				num2 += decimal.ToDouble((decimal)prm);
			}
		}
		if (num > 0)
		{
			return num2 / (double)num;
		}
		return def;
	}

	[AGInfo("Element GUID by ID", "Element GUID by identificator.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public Guid elementByID([AGPrmInfo("Identificator", double.MinValue, double.MinValue)] long id, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown = false)
	{
		return elementByID(id, skipUnknown, _UDT, elmByIDDict, Devices, GeoFences, Drivers, Implements, format, _dataViewerFormatters, ElementType.None, ref autoType);
	}

	[AGInfo("Geo-fence GUID by ID", "Geo-fence GUID by identificator.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public Guid geoFenceByID([AGPrmInfo("Identificator", double.MinValue, double.MinValue)] long id, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown = true)
	{
		return elementByID(id, skipUnknown, _UDT, elmByIDDict, Devices, GeoFences, Drivers, Implements, format, _dataViewerFormatters, ElementType.GeoFence, ref autoType);
	}

	[AGInfo("Driver GUID by ID", "Driver GUID by identificator.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public Guid driverByID([AGPrmInfo("Identificator", double.MinValue, double.MinValue)] long id, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown = true)
	{
		return elementByID(id, skipUnknown, _UDT, elmByIDDict, Devices, GeoFences, Drivers, Implements, format, _dataViewerFormatters, ElementType.Driver, ref autoType);
	}

	[AGInfo("Implement GUID by ID", "Implement GUID by identificator.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public Guid implementByID([AGPrmInfo("Identificator", double.MinValue, double.MinValue)] long id, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown = true)
	{
		return elementByID(id, skipUnknown, _UDT, elmByIDDict, Devices, GeoFences, Drivers, Implements, format, _dataViewerFormatters, ElementType.Implement, ref autoType);
	}

	public static Guid elementByID(long id, bool skipUnknown, DateTime _UDT, Dictionary<PeriodicID<long>, Tuple<GroupOrElementInfo, string>> elmByIDDict, IElements devices, IElements geoFences, IElements drivers, IElements implements, string format, IDataViewerFormatters _dataViewerFormatters, ElementType elementType, ref ElementType autoType)
	{
		Tuple<GroupOrElementInfo, string> tuple = null;
		foreach (KeyValuePair<PeriodicID<long>, Tuple<GroupOrElementInfo, string>> item in elmByIDDict)
		{
			if (item.Key.id == id && item.Key.HasDT() && item.Key.InsideDT(_UDT))
			{
				tuple = item.Value;
				break;
			}
		}
		if (tuple != null)
		{
			return tuple.Item1.GUID;
		}
		Tuple<GroupOrElementInfo, string> tuple2 = null;
		DateTime? startDT = null;
		DateTime? endDT = null;
		PropertyTable propertyTable = GetPropertyTable(id, elementType, ref autoType, devices.ById, geoFences.ById, drivers.ById, implements.ById);
		if (propertyTable != null)
		{
			PropertyTableItem[] entries = propertyTable.Entries;
			if (entries.Length != 0)
			{
				bool flag = true;
				int nearestEntryIndex = propertyTable.GetNearestEntryIndex(_UDT);
				if (Nullable.Compare(_UDT, entries[nearestEntryIndex].startDT) < 0)
				{
					flag = false;
				}
				if (entries[nearestEntryIndex].endDT.HasValue && Nullable.Compare(_UDT, entries[nearestEntryIndex].endDT) >= 0)
				{
					flag = false;
				}
				if (flag)
				{
					tuple2 = (Tuple<GroupOrElementInfo, string>)entries[nearestEntryIndex].value;
					startDT = entries[nearestEntryIndex].startDT;
					endDT = entries[nearestEntryIndex].endDT;
				}
			}
		}
		if (startDT.HasValue || endDT.HasValue)
		{
			tuple = tuple2;
		}
		if (tuple == null)
		{
			foreach (KeyValuePair<PeriodicID<long>, Tuple<GroupOrElementInfo, string>> item2 in elmByIDDict)
			{
				if (item2.Key.id == id && !item2.Key.HasDT())
				{
					tuple = item2.Value;
					break;
				}
			}
			if (tuple != null)
			{
				return tuple.Item1.GUID;
			}
		}
		tuple = tuple2;
		if (tuple == null)
		{
			if (id == 0 || skipUnknown)
			{
				return Guid.Empty;
			}
			byte[] array = new byte[16];
			Array.Copy(BitConverter.GetBytes(id), array, 8);
			tuple = new Tuple<GroupOrElementInfo, string>(new GroupOrElementInfo
			{
				Type = GroupNodeType.Element,
				GUID = new Guid(array),
				Name = _dataViewerFormatters.ToCustomString(id, format)
			}, null);
		}
		elmByIDDict.Add(new PeriodicID<long>(id, startDT, endDT), tuple);
		return tuple.Item1.GUID;
	}

	[AGInfo("Element ID by GUID", "Element identificator by GUID.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public string IDByGUID([AGPrmInfo("GUID", double.MinValue, double.MinValue)] Guid guid)
	{
		if (!TryGetTable(guid, out var table))
		{
			return string.Empty;
		}
		PropertyTableItem[] entries = table.Entries;
		if (entries.Length == 0)
		{
			return string.Empty;
		}
		DateTime uDT = _UDT;
		int nearestEntryIndex = table.GetNearestEntryIndex(uDT);
		if (Nullable.Compare(uDT, entries[nearestEntryIndex].startDT) < 0)
		{
			return string.Empty;
		}
		if (entries[nearestEntryIndex].endDT.HasValue && Nullable.Compare(uDT, entries[nearestEntryIndex].endDT) >= 0)
		{
			return string.Empty;
		}
		return ((Tuple<GroupOrElementInfo, string>)entries[nearestEntryIndex].value).Item2;
	}

	[AGInfo("Element name by GUID", "Element name by GUID.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public string nameByGUID([AGPrmInfo("GUID", double.MinValue, double.MinValue)] Guid guid)
	{
		if (!TryGetTable(guid, out var table))
		{
			return string.Empty;
		}
		PropertyTableItem[] entries = table.Entries;
		if (entries.Length == 0)
		{
			return string.Empty;
		}
		DateTime uDT = _UDT;
		int nearestEntryIndex = table.GetNearestEntryIndex(uDT);
		if (Nullable.Compare(uDT, entries[nearestEntryIndex].startDT) < 0)
		{
			return string.Empty;
		}
		if (entries[nearestEntryIndex].endDT.HasValue && Nullable.Compare(uDT, entries[nearestEntryIndex].endDT) >= 0)
		{
			return string.Empty;
		}
		return ((Tuple<GroupOrElementInfo, string>)entries[nearestEntryIndex].value).Item1.Name;
	}

	[AGInfo("Element GUID by string", "Element GUID by string identificator.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public Guid elementByStr([AGPrmInfo("String identificator", double.MinValue, double.MinValue)] string str, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown = false)
	{
		return elementByStr(str, skipUnknown, _UDT, elmByStrDict, Devices, GeoFences, Drivers, Implements, format, _dataViewerFormatters, ElementType.None, ref autoType);
	}

	[AGInfo("Geo-fence GUID by string", "Geo-fence GUID by string identificator.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public Guid geoFenceByStr([AGPrmInfo("String identificator", double.MinValue, double.MinValue)] string str, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown = true)
	{
		return elementByStr(str, skipUnknown, _UDT, elmByStrDict, Devices, GeoFences, Drivers, Implements, format, _dataViewerFormatters, ElementType.GeoFence, ref autoType);
	}

	[AGInfo("Driver GUID by string", "Driver GUID by string identificator.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public Guid driverByStr([AGPrmInfo("String identificator", double.MinValue, double.MinValue)] string str, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown = true)
	{
		return elementByStr(str, skipUnknown, _UDT, elmByStrDict, Devices, GeoFences, Drivers, Implements, format, _dataViewerFormatters, ElementType.Driver, ref autoType);
	}

	[AGInfo("Implement GUID by string", "Implement GUID by string identificator.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public Guid implementByStr([AGPrmInfo("String identificator", double.MinValue, double.MinValue)] string str, [AGPrmInfo("Skip unknown", double.MinValue, double.MinValue)] bool skipUnknown = true)
	{
		return elementByStr(str, skipUnknown, _UDT, elmByStrDict, Devices, GeoFences, Drivers, Implements, format, _dataViewerFormatters, ElementType.Implement, ref autoType);
	}

	public static Guid elementByStr(string str, bool skipUnknown, DateTime _UDT, Dictionary<PeriodicID<string>, GroupOrElementInfo> elmByStrDict, IElements devices, IElements geoFences, IElements drivers, IElements implements, string format, IDataViewerFormatters _dataViewerFormatters, ElementType elementType, ref ElementType autoType)
	{
		GroupOrElementInfo groupOrElementInfo = null;
		foreach (KeyValuePair<PeriodicID<string>, GroupOrElementInfo> item in elmByStrDict)
		{
			if (item.Key.id == str && item.Key.HasDT() && item.Key.InsideDT(_UDT))
			{
				groupOrElementInfo = item.Value;
				break;
			}
		}
		if (groupOrElementInfo != null)
		{
			return groupOrElementInfo.GUID;
		}
		GroupOrElementInfo groupOrElementInfo2 = null;
		DateTime? startDT = null;
		DateTime? endDT = null;
		PropertyTable propertyTable = GetPropertyTable(str, elementType, ref autoType, devices.ByString, geoFences.ByString, drivers.ByString, implements.ByString);
		if (propertyTable != null)
		{
			PropertyTableItem[] entries = propertyTable.Entries;
			if (entries.Length != 0)
			{
				bool flag = true;
				int nearestEntryIndex = propertyTable.GetNearestEntryIndex(_UDT);
				if (Nullable.Compare(_UDT, entries[nearestEntryIndex].startDT) < 0)
				{
					flag = false;
				}
				if (entries[nearestEntryIndex].endDT.HasValue && Nullable.Compare(_UDT, entries[nearestEntryIndex].endDT) >= 0)
				{
					flag = false;
				}
				if (flag)
				{
					groupOrElementInfo2 = (GroupOrElementInfo)entries[nearestEntryIndex].value;
					startDT = entries[nearestEntryIndex].startDT;
					endDT = entries[nearestEntryIndex].endDT;
				}
			}
		}
		if (startDT.HasValue || endDT.HasValue)
		{
			groupOrElementInfo = groupOrElementInfo2;
		}
		if (groupOrElementInfo == null)
		{
			foreach (KeyValuePair<PeriodicID<string>, GroupOrElementInfo> item2 in elmByStrDict)
			{
				if (item2.Key.id == str && !item2.Key.HasDT())
				{
					groupOrElementInfo = item2.Value;
					break;
				}
			}
			if (groupOrElementInfo != null)
			{
				return groupOrElementInfo.GUID;
			}
		}
		groupOrElementInfo = groupOrElementInfo2;
		if (groupOrElementInfo == null)
		{
			if (string.IsNullOrEmpty(str) || skipUnknown)
			{
				return Guid.Empty;
			}
			byte[] b = MD5.HashData(Encoding.Default.GetBytes(str));
			groupOrElementInfo = new GroupOrElementInfo
			{
				Type = GroupNodeType.Element,
				GUID = new Guid(b),
				Name = _dataViewerFormatters.ToCustomString(str, format)
			};
		}
		elmByStrDict.Add(new PeriodicID<string>(str, startDT, endDT), groupOrElementInfo);
		return groupOrElementInfo.GUID;
	}

	[AGInfo("Minimum", "Minimum value of integers.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static int minOf([AGPrmInfo("Numbers", double.MinValue, double.MinValue)] params int[] vals)
	{
		return vals.Min();
	}

	[AGInfo("Minimum using priority", "Minimum value of integers using priority.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static int minOf([AGPrmInfo("Numbers", double.MinValue, double.MinValue)] params ValuePriorityPair[] vals)
	{
		int value = vals[0].Value;
		int priority = vals[0].Priority;
		for (int i = 1; i < vals.Length; i++)
		{
			if (vals[i].Priority == priority)
			{
				if (value > vals[i].Value)
				{
					value = vals[i].Value;
				}
			}
			else if (vals[i].Priority > priority)
			{
				value = vals[i].Value;
				priority = vals[i].Priority;
			}
		}
		return value;
	}

	[AGInfo("Maximum", "Maximum value of integers.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static int maxOf([AGPrmInfo("Numbers", double.MinValue, double.MinValue)] params int[] vals)
	{
		return vals.Max();
	}

	[AGInfo("Maximum using priority", "Minimum value of integers using priority.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static int maxOf([AGPrmInfo("Numbers", double.MinValue, double.MinValue)] params ValuePriorityPair[] vals)
	{
		int value = vals[0].Value;
		int priority = vals[0].Priority;
		for (int i = 1; i < vals.Length; i++)
		{
			if (vals[i].Priority == priority)
			{
				if (value < vals[i].Value)
				{
					value = vals[i].Value;
				}
			}
			else if (vals[i].Priority > priority)
			{
				value = vals[i].Value;
				priority = vals[i].Priority;
			}
		}
		return value;
	}

	[AGInfo("Absolute", "Absolute value of integer.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static int abs([AGPrmInfo("Number", double.MinValue, double.MinValue)] int val)
	{
		return Math.Abs(val);
	}

	[AGInfo("Absolute", "Absolute value of real.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static double abs([AGPrmInfo("Number", double.MinValue, double.MinValue)] double val)
	{
		return Math.Abs(val);
	}

	[AGInfo("Square", "Square root.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static double sqrt([AGPrmInfo("Number", 0.0, double.MinValue)] double val)
	{
		return Math.Sqrt(val);
	}

	[AGInfo("Sine", "Sine.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static double sin([AGPrmInfo("Angle in degrees", double.MinValue, double.MinValue)] double val)
	{
		return Math.Sin(val * Math.PI / 180.0);
	}

	[AGInfo("Cosine", "Cosine.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static double cos([AGPrmInfo("Angle in degrees", double.MinValue, double.MinValue)] double val)
	{
		return Math.Cos(val * Math.PI / 180.0);
	}

	[AGInfo("Tangent", "Tangent.", DeviceParameterKind.Flag, AGInfoGroupType.Data, false, null)]
	public static double tan([AGPrmInfo("Angle in degrees", double.MinValue, double.MinValue)] double val)
	{
		return Math.Tan(val * Math.PI / 180.0);
	}

	[AGInfo("Time span from hour, minute, second", "Interval of time in the TimeSpan format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static TimeSpan timespan([AGPrmInfo("Number of hours", double.MinValue, double.MinValue)] int hour, [AGPrmInfo("Number of minutes", double.MinValue, double.MinValue)] int minute, [AGPrmInfo("Number of seconds", double.MinValue, double.MinValue)] int second)
	{
		return TimeSpan.FromSeconds(60 * (60 * hour + minute) + second);
	}

	[AGInfo("Time span from day, hour, minute, second", "Interval of time in the TimeSpan format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static TimeSpan timespan([AGPrmInfo("Number of days", double.MinValue, double.MinValue)] int day, [AGPrmInfo("Number of hours", double.MinValue, double.MinValue)] int hour, [AGPrmInfo("Number of minutes", double.MinValue, double.MinValue)] int minute, [AGPrmInfo("Number of seconds", double.MinValue, double.MinValue)] int second)
	{
		return TimeSpan.FromSeconds(60 * (60 * (24 * day + hour) + minute) + second);
	}

	[AGInfo("Weeks in the TimeSpan format", "Weeks in the TimeSpan format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static TimeSpan weeks([AGPrmInfo("Number of weeks", double.MinValue, double.MinValue)] double val)
	{
		return TimeSpan.FromDays(val * 7.0);
	}

	[AGInfo("Days in the TimeSpan format", "Days in the TimeSpan format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static TimeSpan days([AGPrmInfo("Number of days", double.MinValue, double.MinValue)] double val)
	{
		return TimeSpan.FromDays(val);
	}

	[AGInfo("Hours in the TimeSpan format", "Hours in the TimeSpan format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static TimeSpan hours([AGPrmInfo("Number of hours", double.MinValue, double.MinValue)] double val)
	{
		return TimeSpan.FromHours(val);
	}

	[AGInfo("Minutes in the TimeSpan format", "Minutes in the TimeSpan format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static TimeSpan minutes([AGPrmInfo("Number of minutes", double.MinValue, double.MinValue)] double val)
	{
		return TimeSpan.FromMinutes(val);
	}

	[AGInfo("Seconds in the TimeSpan format", "Seconds in the TimeSpan format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static TimeSpan seconds([AGPrmInfo("Number of seconds", double.MinValue, double.MinValue)] double val)
	{
		return TimeSpan.FromSeconds(val);
	}

	[AGInfo("Elapse", "Elapsed time in easy to visaul control format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public string elapse([AGPrmInfo("Start date and time", double.MinValue, double.MinValue)] DateTime from, [AGPrmInfo("End date and time (usually current)", double.MinValue, double.MinValue)] DateTime to, [AGPrmInfo("Minimal value for displaying start date", double.MinValue, double.MinValue)] TimeSpan limit)
	{
		TimeSpan timeSpan = to - from;
		if (!(timeSpan < limit))
		{
			return from.ToString("d.MM.yy");
		}
		return _dataViewerFormatters.ToCustomString(timeSpan, "[elapse]");
	}

	[AGInfo("Date in DateTime format", "Date in DateTime format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static DateTime date([AGPrmInfo("Year", 0.0, double.MinValue)] int year, [AGPrmInfo("Month", 1.0, 12.0)] int month, [AGPrmInfo("Day", 1.0, 31.0)] int day)
	{
		if (year < 1 || year > 9999)
		{
			return DateTime.MinValue;
		}
		if (month < 1 || month > 12)
		{
			return DateTime.MinValue;
		}
		if (day < 1 || day > DateTime.DaysInMonth(year, month))
		{
			return DateTime.MinValue;
		}
		return new DateTime(year, month, day);
	}

	[AGInfo("Date and time in DateTime format", "Date and time in DateTime format.", DeviceParameterKind.InstTime, AGInfoGroupType.Time, false, null)]
	public static DateTime date([AGPrmInfo("Year", 0.0, double.MinValue)] int year, [AGPrmInfo("Month", 1.0, 12.0)] int month, [AGPrmInfo("Day", 1.0, 31.0)] int day, [AGPrmInfo("Hour", 0.0, 23.0)] int hour, [AGPrmInfo("Minute", 0.0, 59.0)] int minute, [AGPrmInfo("Second", 0.0, 59.0)] int second)
	{
		if (year < 1 || year > 9999)
		{
			return DateTime.MinValue;
		}
		if (month < 1 || month > 12)
		{
			return DateTime.MinValue;
		}
		if (day < 1 || day > DateTime.DaysInMonth(year, month))
		{
			return DateTime.MinValue;
		}
		if (hour < 0 || hour > 23)
		{
			return DateTime.MinValue;
		}
		if (minute < 0 || minute > 59)
		{
			return DateTime.MinValue;
		}
		if (second < 0 || second > 59)
		{
			return DateTime.MinValue;
		}
		return new DateTime(year, month, day, hour, minute, second);
	}

	private bool TryGetTable(Guid id, out PropertyTable table)
	{
		if (!Drivers.TryGetTable(id, out table) && !Implements.TryGetTable(id, out table) && !Tasks.TryGetTable(id, out table) && !Devices.TryGetTable(id, out table) && !GeoFences.TryGetTable(id, out table))
		{
			return false;
		}
		return true;
	}

	private static PropertyTable GetPropertyTable<TId>(TId id, ElementType elementType, ref ElementType autoType, Dictionary<TId, PropertyTable> devices, Dictionary<TId, PropertyTable> geoFences, Dictionary<TId, PropertyTable> drivers, Dictionary<TId, PropertyTable> implements)
	{
		PropertyTable value;
		switch (elementType)
		{
		case ElementType.None:
			if (drivers.TryGetValue(id, out value))
			{
				autoType = ElementType.Driver;
				return value;
			}
			if (implements.TryGetValue(id, out value))
			{
				autoType = ElementType.Implement;
				return value;
			}
			if (geoFences.TryGetValue(id, out value))
			{
				autoType = ElementType.GeoFence;
				return value;
			}
			if (devices.TryGetValue(id, out value))
			{
				autoType = ElementType.Device;
				return value;
			}
			break;
		case ElementType.GeoFence:
			if (geoFences.TryGetValue(id, out value))
			{
				return value;
			}
			break;
		case ElementType.Driver:
			if (drivers.TryGetValue(id, out value))
			{
				return value;
			}
			break;
		case ElementType.Implement:
			if (implements.TryGetValue(id, out value))
			{
				return value;
			}
			break;
		case ElementType.Device:
			if (devices.TryGetValue(id, out value))
			{
				return value;
			}
			break;
		default:
			return null;
		}
		return null;
	}

	public virtual void Dispose()
	{
	}
}
