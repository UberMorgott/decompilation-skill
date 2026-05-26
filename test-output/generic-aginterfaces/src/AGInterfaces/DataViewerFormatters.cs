using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using AGInterfaces.Localization;
using AddressBaseAga2;

namespace AGInterfaces;

public class DataViewerFormatters : IDataViewerFormatters
{
	public delegate string FormatterDelegate(object obj, string format);

	private readonly CommonFields _commonFields;

	private readonly IAddressBaseProvider _addressBaseProvider;

	private readonly Dictionary<RuntimeTypeHandle, FormatterDelegate> _formatters;

	public const string ELAPSE = "[elapse]";

	public const string DEF_TS_DDHHMM = "[ddd\\ ]hh\\:mm";

	public const string DEF_TS_DDH_MM = "[ddd\\ hh\\:]mm";

	public const string DEF_TS_DDHHMMSS = "[ddd\\ ]hh\\:mm\\:ss";

	public const string DEF_TS_DDH_MMSS = "[ddd\\ hh\\:]mm\\:ss";

	public const string DEF_TS_DDHHM_SS = "[ddd\\ hh\\:mm\\:]ss";

	public const string DEF_TS_HHMM = "[hh]\\:mm";

	public const string DEF_TS_H_MM = "[hh\\:]mm";

	public const string DEF_TS_HHMMSS = "[hh]\\:mm\\:ss";

	public const string DEF_TS_H_MMSS = "[hh\\:]mm\\:ss";

	public const string DEF_TS_HHM_SS = "[hh\\:mm\\:]ss";

	public bool SkipEmptyValues;

	private readonly char[] nwLatZones = "NNPPQQRRSSTTUUVVWWXXXYY".ToCharArray();

	private readonly char[] neLatZones = "NNPPQQRRSSTTUUVVWWXXXZZ".ToCharArray();

	private readonly char[] swLatZones = "MMLLKKJJHHGGFFEEDDCCAAA".ToCharArray();

	private readonly char[] seLatZones = "MMLLKKJJHHGGFFEEDDCCBBB".ToCharArray();

	private readonly MapPrjVarsAndConsts mapPrj = new MapPrjVarsAndConsts();

	public static char thin_space = '\u200a';

	public DataViewerFormatters(CommonFields commonFields, IAddressBaseProvider addressBaseProvider)
	{
		_commonFields = commonFields;
		_addressBaseProvider = addressBaseProvider;
		_formatters = new Dictionary<RuntimeTypeHandle, FormatterDelegate>
		{
			{
				typeof(bool).TypeHandle,
				BoolToString
			},
			{
				typeof(byte).TypeHandle,
				ByteToString
			},
			{
				typeof(int).TypeHandle,
				Int32ToString
			},
			{
				typeof(long).TypeHandle,
				Int64ToString
			},
			{
				typeof(double).TypeHandle,
				DoubleToString
			},
			{
				typeof(DateTime).TypeHandle,
				DateTimeToString
			},
			{
				typeof(TimeSpan).TypeHandle,
				TimeSpanToString
			},
			{
				typeof(Coordinates).TypeHandle,
				CoordinatesToString
			},
			{
				typeof(Location).TypeHandle,
				LocationToString
			},
			{
				typeof(LocationAddr).TypeHandle,
				LocationAddrToString
			}
		};
	}

	private string d_mm_mmmmm_string(double val)
	{
		double num = Math.Abs(val);
		return (int)val + "° " + (60.0 * num - (double)(60 * (int)num)).ToString("00.00000") + "'";
	}

	private string d_mm_ss_sss_string(double val)
	{
		double num = Math.Abs(val);
		double num2 = 60.0 * num - (double)(60 * (int)num);
		double num3 = 60.0 * num2 - (double)(60 * (int)num2);
		return (int)val + "° " + ((int)num2).ToString("00") + "' " + num3.ToString("00.000") + "\"";
	}

	public string elapse(TimeSpan interval)
	{
		if (interval == TimeSpan.MinValue || interval.TotalDays > 730000.0)
		{
			return "–";
		}
		string text = null;
		if (interval < TimeSpan.Zero)
		{
			interval = -interval;
			text = " " + _commonFields.str_to;
		}
		if (interval.TotalMinutes < 1.0)
		{
			return elapseStr((int)interval.TotalSeconds, interval.Seconds) + " " + _commonFields.str_Secs + text;
		}
		if (interval.TotalHours < 1.0)
		{
			return elapseStr(interval.TotalMinutes, interval.Minutes) + " " + _commonFields.str_Mins + text;
		}
		if (interval.TotalDays < 1.0)
		{
			return elapseStr(interval.TotalHours, interval.Hours) + " " + _commonFields.str_Hours + text;
		}
		if (interval.TotalDays < 7.0)
		{
			return elapseStr(interval.TotalDays, interval.Days) + " " + _commonFields.str_Days + text;
		}
		if (interval.TotalDays < 30.0)
		{
			int partial = interval.Days / 7;
			return elapseStr(interval.TotalDays / 7.0, partial) + " " + _commonFields.str_Weeks + text;
		}
		if (interval.TotalDays < 365.0)
		{
			int num = interval.Days / 30;
			return elapseStr(interval.TotalDays / 30.0, num) + " " + plural(num, _commonFields.str_OneMonth, _commonFields.str_MonthLast1, _commonFields.str_MonthLast2_4, _commonFields.str_MonthLast5_0) + text;
		}
		int num2 = interval.Days / 365;
		if (num2 > 100)
		{
			return "–";
		}
		return elapseStr(interval.TotalDays / 365.0, num2) + " " + plural(num2, _commonFields.str_OneYear, _commonFields.str_YearLast1, _commonFields.str_YearLast2_4, _commonFields.str_YearLast5_0) + text;
	}

	public string elapseShort(TimeSpan interval)
	{
		if (interval == TimeSpan.MinValue || interval.TotalDays > 730000.0)
		{
			return "–";
		}
		string text = null;
		if (interval < TimeSpan.Zero)
		{
			interval = -interval;
			text = " " + _commonFields.str_to;
		}
		if (interval.TotalMinutes < 1.0)
		{
			return elapseStrShort((int)interval.TotalSeconds, interval.Seconds) + thin_space + _commonFields.str_SecondsShort + text;
		}
		if (interval.TotalHours < 1.0)
		{
			return elapseStrShort(interval.TotalMinutes, interval.Minutes) + thin_space + _commonFields.str_MinutesShort + text;
		}
		if (interval.TotalDays < 1.0)
		{
			return elapseStrShort(interval.TotalHours, interval.Hours) + thin_space + _commonFields.str_HoursShort + text;
		}
		if (interval.TotalDays < 7.0)
		{
			return elapseStrShort(interval.TotalDays, interval.Days) + thin_space + _commonFields.str_DaysShort + text;
		}
		if (interval.TotalDays < 30.0)
		{
			int partial = interval.Days / 7;
			return elapseStrShort(interval.TotalDays / 7.0, partial) + thin_space + _commonFields.str_WeeksShort + text;
		}
		if (interval.TotalDays < 365.0)
		{
			int partial2 = interval.Days / 30;
			return elapseStrShort(interval.TotalDays / 30.0, partial2) + thin_space + _commonFields.str_MonthsShort + text;
		}
		int num = interval.Days / 365;
		if (num > 100)
		{
			return "–";
		}
		return elapseStrShort(interval.TotalDays / 365.0, num) + thin_space + plural(num, _commonFields.str_OneYearShort, _commonFields.str_YearLast1Short, _commonFields.str_YearLast2_4Short, _commonFields.str_YearLast5_0Short) + text;
	}

	public string ToCustomString(object obj, string format = null)
	{
		if (format == "[]")
		{
			return string.Empty;
		}
		if (obj is SwitchPrm)
		{
			if (!(format != "[count]"))
			{
				if (((SwitchPrm)obj).status == 0)
				{
					return string.Empty;
				}
				return "[" + ((((SwitchPrm)obj).count + 1) / 2).ToString("n0") + "]";
			}
			obj = ((SwitchPrm)obj).value;
		}
		if (obj == null)
		{
			return string.Empty;
		}
		if (obj is Image)
		{
			return _commonFields.str_Image;
		}
		return GetFormatter(obj.GetType())?.Invoke(obj, format ?? string.Empty) ?? obj.ToString();
	}

	private FormatterDelegate GetFormatter(Type type)
	{
		if (!_formatters.TryGetValue(type.TypeHandle, out var value))
		{
			return null;
		}
		return value;
	}

	private string BoolToString(object obj, string format)
	{
		if (!(format == "[yesno]"))
		{
			if (format == "[onoff]")
			{
				if (!(bool)obj)
				{
					return _commonFields.str_Off;
				}
				return _commonFields.str_On;
			}
			if (string.Compare(format, 0, "[b", 0, 2) == 0 && format.Last() == ']')
			{
				return ToCustomString(((bool)obj) ? 1 : 0, format);
			}
			if (format.Length != 3 || format.First() != '[' || format.Last() != ']')
			{
				return obj.ToString();
			}
			if (!(bool)obj)
			{
				return string.Empty;
			}
			return format[1].ToString();
		}
		if (!(bool)obj)
		{
			return _commonFields.str_No;
		}
		return _commonFields.str_Yes;
	}

	private string ByteToString(object obj, string format)
	{
		return LongToString((byte)obj, format);
	}

	private string Int32ToString(object obj, string format)
	{
		int num = (int)obj;
		if (int.MinValue >= num || num >= int.MaxValue)
		{
			return "–";
		}
		return LongToString(num, format);
	}

	private string Int64ToString(object obj, string format)
	{
		long num = (long)obj;
		if (long.MinValue >= num || num >= long.MaxValue)
		{
			return "–";
		}
		return LongToString(num, format);
	}

	private string LongToString(long val, string format)
	{
		if (SkipEmptyValues && val == 0L)
		{
			return "";
		}
		switch (format)
		{
		case "[ed]":
			return getTypeDesc(_commonFields.ed, val);
		case "[gd]":
			return getTypeDesc(_commonFields.gd, val);
		case "[dd]":
			return getTypeDesc(_commonFields.dd, val);
		case "[td]":
			return getTypeDesc(_commonFields.td, val);
		case "[id]":
			if (val == 0L)
			{
				return string.Empty;
			}
			if ((val & -281474976710656L) == 0L)
			{
				return $"{(val >> 32) & 0xFFFF:X4} {(val >> 16) & 0xFFFF:X4} {val & 0xFFFF:X4}";
			}
			return $"{(val >> 48) & 0xFFFF:X4} {(val >> 32) & 0xFFFF:X4} {(val >> 16) & 0xFFFF:X4} {val & 0xFFFF:X4}";
		case "[card]":
			if (val == 0L)
			{
				return string.Empty;
			}
			return $"{val >> 16}.{val & 0xFFFF:00000}";
		case "[card_no_dot]":
			if (val == 0L)
			{
				return string.Empty;
			}
			return $"{val >> 16}{val & 0xFFFF:00000}";
		case "[dtf]":
			return getFltrDesc(_commonFields.arr_DTfltr, val);
		case "[llf]":
			return getFltrDesc(_commonFields.arr_LLfltr, val);
		case "[motion]":
			return getFltrDesc(_commonFields.arr_Motion, val);
		case "[yesno]":
			if (val == 0L)
			{
				return _commonFields.str_No;
			}
			return _commonFields.str_Yes;
		case "[onoff]":
			if (val == 0L)
			{
				return _commonFields.str_Off;
			}
			return _commonFields.str_On;
		default:
		{
			if (format.Length == 3 && format.First() == '[' && format.Last() == ']')
			{
				if (val == 0L)
				{
					return string.Empty;
				}
				return format[1].ToString();
			}
			if (string.Compare(format, 0, "[b:", 0, 3) == 0 && format.Last() == ']')
			{
				string text = null;
				long num = 1L;
				for (int i = 3; i < format.Length - 1; i++)
				{
					char c = format[i];
					if (char.IsWhiteSpace(c) || char.IsPunctuation(c))
					{
						text += c;
					}
					else if (char.IsLetterOrDigit(c))
					{
						text += (((val & num) != 0L) ? c : '–');
						num <<= 1;
					}
				}
				return text;
			}
			if (string.Compare(format, 0, "[b", 0, 2) == 0 && format.Last() == ']' && int.TryParse(format.Substring(2, format.Length - 3), out var result))
			{
				if (result < 1)
				{
					result = 1;
				}
				else if (result > 64)
				{
					result = 64;
				}
				return Convert.ToString(val, 2).PadLeft(result, '0');
			}
			if (string.Compare(format, 0, "[o", 0, 2) == 0 && format.Last() == ']' && int.TryParse(format.Substring(2, format.Length - 3), out result))
			{
				if (result < 1)
				{
					result = 1;
				}
				else if (result > 64)
				{
					result = 64;
				}
				return Convert.ToString(val, 8).PadLeft(result, '0');
			}
			return val.ToString(format);
		}
		}
	}

	private string getFltrDesc(string[] d, long id)
	{
		if (0 > id || id >= d.Length)
		{
			return string.Empty;
		}
		return d[id];
	}

	private string getTypeDesc(string[] d, long id)
	{
		long num = id + 3;
		if (0 > num || num >= d.Length)
		{
			return $"{d[0]} - {id}";
		}
		return d[num];
	}

	private string getTypeDesc(EventDescription[] d, long id)
	{
		EventDescription eventDescription = d.FirstOrDefault((EventDescription p) => p.ID == id);
		if (eventDescription == null)
		{
			return $"{d[0].Desc} - {id}";
		}
		return eventDescription.Desc;
	}

	private string DoubleToString(object obj, string format)
	{
		double num = (double)obj;
		if (double.IsNaN(num) || double.IsInfinity(num))
		{
			if (!SkipEmptyValues)
			{
				return "–";
			}
			return "";
		}
		switch (format)
		{
		case "[sd.ddddddd°]":
			return num.ToString("0.0000000°");
		case "[sd° mm.mmmmm']":
			return d_mm_mmmmm_string(num);
		case "[sd° mm' ss.sss\"]":
			return d_mm_ss_sss_string(num);
		case "[ud.ddddddd° lon]":
		{
			if (!(num >= 0.0))
			{
				return (0.0 - num).ToString("0.0000000° ") + _commonFields.str_WLon;
			}
			double num2 = num;
			return num2.ToString("0.0000000° ") + _commonFields.str_ELon;
		}
		case "[ud° mm.mmmmm' lon]":
			if (!(num >= 0.0))
			{
				return d_mm_mmmmm_string(0.0 - num) + " " + _commonFields.str_WLon;
			}
			return d_mm_mmmmm_string(num) + " " + _commonFields.str_ELon;
		case "[ud° mm' ss.sss\" lon]":
			if (!(num >= 0.0))
			{
				return d_mm_ss_sss_string(0.0 - num) + " " + _commonFields.str_WLon;
			}
			return d_mm_ss_sss_string(num) + " " + _commonFields.str_ELon;
		case "[ud.ddddddd° lat]":
		{
			if (!(num >= 0.0))
			{
				return (0.0 - num).ToString("0.0000000° ") + _commonFields.str_SLat;
			}
			double num2 = num;
			return num2.ToString("0.0000000° ") + _commonFields.str_NLat;
		}
		case "[ud° mm.mmmmm' lat]":
			if (!(num >= 0.0))
			{
				return d_mm_mmmmm_string(0.0 - num) + " " + _commonFields.str_SLat;
			}
			return d_mm_mmmmm_string(num) + " " + _commonFields.str_NLat;
		case "[ud° mm' ss.sss\" lat]":
			if (!(num >= 0.0))
			{
				return d_mm_ss_sss_string(0.0 - num) + " " + _commonFields.str_SLat;
			}
			return d_mm_ss_sss_string(num) + " " + _commonFields.str_NLat;
		default:
			if (SkipEmptyValues)
			{
				if (!(Math.Abs(num) < 1E-05))
				{
					return num.ToString(format);
				}
				return "";
			}
			return num.ToString(format);
		}
	}

	private string DateTimeToString(object obj, string format)
	{
		if (!((DateTime)obj != DateTime.MinValue))
		{
			if (!SkipEmptyValues)
			{
				return "–";
			}
			return string.Empty;
		}
		return ((DateTime)obj).ToString(format);
	}

	private string TimeSpanToString(object obj, string format)
	{
		TimeSpan timeSpan = (TimeSpan)obj;
		if (timeSpan == TimeSpan.MinValue || timeSpan.TotalDays > 730000.0)
		{
			if (!SkipEmptyValues)
			{
				return "–";
			}
			return string.Empty;
		}
		if (SkipEmptyValues && timeSpan == TimeSpan.Zero)
		{
			return string.Empty;
		}
		switch (format)
		{
		case "[elapse]":
			return elapse(timeSpan);
		case "[ddd\\ ]hh\\:mm":
			if (!(-1.0 < timeSpan.TotalDays) || !(timeSpan.TotalDays < 1.0))
			{
				return string.Format("{0} {1} {2}", timeSpan.Days, _commonFields.str_Days, timeSpan.ToString("hh\\:mm"));
			}
			return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + timeSpan.ToString("h\\:mm");
		case "[ddd\\ hh\\:]mm":
			if (!(-1.0 < timeSpan.TotalDays) || !(timeSpan.TotalDays < 1.0))
			{
				return string.Format("{0} {1} {2}", timeSpan.Days, _commonFields.str_Days, timeSpan.ToString("hh\\:mm"));
			}
			if (!(-1.0 < timeSpan.TotalHours) || !(timeSpan.TotalHours < 1.0))
			{
				return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + timeSpan.ToString("h\\:mm");
			}
			return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + Math.Abs(timeSpan.Minutes);
		case "[ddd\\ ]hh\\:mm\\:ss":
			if (!(-1.0 < timeSpan.TotalDays) || !(timeSpan.TotalDays < 1.0))
			{
				return string.Format("{0} {1} {2}", timeSpan.Days, _commonFields.str_Days, timeSpan.ToString("hh\\:mm\\:ss"));
			}
			return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + timeSpan.ToString("h\\:mm\\:ss");
		case "[ddd\\ hh\\:]mm\\:ss":
			if (!(-1.0 < timeSpan.TotalDays) || !(timeSpan.TotalDays < 1.0))
			{
				return string.Format("{0} {1} {2}", timeSpan.Days, _commonFields.str_Days, timeSpan.ToString("hh\\:mm\\:ss"));
			}
			if (!(-1.0 < timeSpan.TotalHours) || !(timeSpan.TotalHours < 1.0))
			{
				return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + timeSpan.ToString("h\\:mm\\:ss");
			}
			return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + timeSpan.ToString("m\\:ss");
		case "[ddd\\ hh\\:mm\\:]ss":
			if (!(-1.0 < timeSpan.TotalDays) || !(timeSpan.TotalDays < 1.0))
			{
				return string.Format("{0} {1} {2}", timeSpan.Days, _commonFields.str_Days, timeSpan.ToString("hh\\:mm\\:ss"));
			}
			if (!(-1.0 < timeSpan.TotalHours) || !(timeSpan.TotalHours < 1.0))
			{
				return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + timeSpan.ToString("h\\:mm\\:ss");
			}
			if (!(-1.0 < timeSpan.TotalMinutes) || !(timeSpan.TotalMinutes < 1.0))
			{
				return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + timeSpan.ToString("m\\:ss");
			}
			return timeSpan.Seconds.ToString();
		case "[hh]\\:mm":
			return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + string.Format("{0}{1}", Math.Abs((int)timeSpan.TotalHours), timeSpan.ToString("\\:mm"));
		case "[hh\\:]mm":
			if (!(-1.0 < timeSpan.TotalHours) || !(timeSpan.TotalHours < 1.0))
			{
				return string.Format("{0}{1}", (int)timeSpan.TotalHours, timeSpan.ToString("\\:mm"));
			}
			return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + Math.Abs(timeSpan.Minutes);
		case "[hh]\\:mm\\:ss":
			return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + string.Format("{0}{1}", Math.Abs((int)timeSpan.TotalHours), timeSpan.ToString("\\:mm\\:ss"));
		case "[hh\\:]mm\\:ss":
			if (!(-1.0 < timeSpan.TotalHours) || !(timeSpan.TotalHours < 1.0))
			{
				return string.Format("{0}{1}", (int)timeSpan.TotalHours, timeSpan.ToString("\\:mm\\:ss"));
			}
			return ((timeSpan >= TimeSpan.Zero) ? "" : "-") + timeSpan.ToString("m\\:ss");
		case "[hh\\:mm\\:]ss":
			if (!(-1.0 < timeSpan.TotalHours) || !(timeSpan.TotalHours < 1.0))
			{
				return string.Format("{0}{1}", (int)timeSpan.TotalHours, timeSpan.ToString("\\:mm\\:ss"));
			}
			if (!(-1.0 < timeSpan.TotalMinutes) || !(timeSpan.TotalMinutes < 1.0))
			{
				return timeSpan.ToString("m\\:ss");
			}
			return timeSpan.Seconds.ToString();
		default:
			if (format.StartsWith("n", ignoreCase: true, CultureInfo.InvariantCulture))
			{
				return timeSpan.TotalHours.ToString(format);
			}
			return timeSpan.ToString(format);
		}
	}

	private string elapseStr(double total, int partial)
	{
		if (!(total > (double)partial))
		{
			return partial.ToString();
		}
		return "> " + partial;
	}

	private string elapseStrShort(double total, int partial)
	{
		if (!(total > (double)partial))
		{
			return partial.ToString();
		}
		return ">" + thin_space + partial;
	}

	private string plural(int val, string str_one, string str_last1, string str_last2_4, string str_last5_0)
	{
		int num = Math.Abs(val);
		int num2 = num % 10;
		int num3 = num % 100 - num2;
		if (0 >= num2 || num2 >= 5 || num3 == 10)
		{
			return str_last5_0;
		}
		if (num2 != 1)
		{
			return str_last2_4;
		}
		if (num != 1)
		{
			return str_last1;
		}
		return str_one;
	}

	private string CoordinatesToString(object obj, string format)
	{
		Coordinates val = (Coordinates)obj;
		if (format == "[utm]")
		{
			return utm_string(val);
		}
		return val.ToString();
	}

	private string utm_string(Coordinates val)
	{
		double longitude = val.longitude;
		double latitude = val.latitude;
		mapPrj.setVars(latitude, out var cosPhi, out var tanPhi, out var _, out var _, out var _, out var M, out var v);
		int num = (int)((186.0 + longitude) / 6.0);
		double meridian = (double)((num - 31) * 6 + 3) * Math.PI / 180.0;
		mapPrj.getUTMCoords(latitude, cosPhi, tanPhi, M, v, longitude, meridian, out var easting, out var northing);
		char c = ((!(latitude >= 0.0)) ? ((longitude < 0.0) ? swLatZones : seLatZones) : ((longitude < 0.0) ? nwLatZones : neLatZones))[(int)Math.Abs(latitude) >> 2];
		return ("ABYZ".Contains(c) ? $"{c} " : $"{num}{c} ") + $"{easting:# ## ##0} E, ".TrimStart() + $"{northing:# ## ##0} N".TrimStart();
	}

	public string getAddressByID(LocationAddr addr)
	{
		IAddressBaseCollection addressBaseCollection = _addressBaseProvider.GetAddressBaseCollection();
		if (addressBaseCollection == null)
		{
			return string.Empty;
		}
		if (addr.distance < 1000.0)
		{
			return $"{addressBaseCollection.GetAddressByIndexToString(addr.addrID, addr.baseID)} ({addr.distance:n0} {_commonFields.str_Meters})";
		}
		return $"{addressBaseCollection.GetAddressByIndexToString(addr.addrID, addr.baseID)} ({addr.distance / 1000.0:n1} {_commonFields.str_Kilometers})";
	}

	private string LocationToString(object obj, string format)
	{
		Location location = (Location)obj;
		if (location.addr.addrID >= 0)
		{
			return getAddressByID(location.addr);
		}
		return CrdsToString(location.crds);
	}

	private string LocationAddrToString(object obj, string format)
	{
		LocationAddr addr = (LocationAddr)obj;
		if (addr.addrID >= 0)
		{
			return getAddressByID(addr);
		}
		return string.Empty;
	}

	public string CrdsToString(Coordinates crds)
	{
		if (crds.longitude == 0.0 && crds.latitude == 0.0)
		{
			return "–";
		}
		return string.Format("{0}, {1}", (crds.longitude >= 0.0) ? (d_mm_mmmmm_string(crds.longitude) + " " + _commonFields.str_ELon) : (d_mm_mmmmm_string(0.0 - crds.longitude) + " " + _commonFields.str_WLon), (crds.latitude >= 0.0) ? (d_mm_mmmmm_string(crds.latitude) + " " + _commonFields.str_NLat) : (d_mm_mmmmm_string(0.0 - crds.latitude) + " " + _commonFields.str_SLat));
	}
}
