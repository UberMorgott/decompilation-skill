using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using TKHelpers;

namespace AGInterfaces.Extenders;

public static class StringExtenders
{
	private const string RX_MAIL = "\\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\\Z";

	private static readonly IdnMapping idnMapper;

	private static readonly Regex RX_PARM_REPLACE;

	private static readonly Regex RX_PROPERTY_NAME;

	private static readonly Regex RX_LOGIN;

	private static readonly Regex RX_TELEGRAM_CHATID;

	private static readonly Regex RX_PHONE;

	private static readonly Func<string, string, int> strCmpLogicalHandler;

	private static readonly char[] digits;

	public static bool IsValidEmail(this string strIn)
	{
		if (string.IsNullOrEmpty(strIn))
		{
			return false;
		}
		strIn = strIn.Trim();
		try
		{
			strIn = Regex.Replace(strIn, "(@)(.+)$", IDNDomainMapper, RegexOptions.None);
		}
		catch
		{
			return false;
		}
		if (string.IsNullOrEmpty(strIn))
		{
			return false;
		}
		try
		{
			return Regex.IsMatch(strIn, "\\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\\Z", RegexOptions.IgnoreCase);
		}
		catch
		{
			return false;
		}
	}

	public static bool IsValidPropertyName(this string strIn)
	{
		if (!string.IsNullOrEmpty(strIn))
		{
			return RX_PROPERTY_NAME.Match(strIn.Trim()).Success;
		}
		return false;
	}

	public static bool IsValidParameterReplacement(this string strIn)
	{
		if (!string.IsNullOrEmpty(strIn))
		{
			return RX_PARM_REPLACE.Match(strIn.Trim()).Success;
		}
		return false;
	}

	private static string IDNDomainMapper(Match match)
	{
		string value = match.Groups[2].Value;
		try
		{
			value = idnMapper.GetAscii(value);
		}
		catch (ArgumentException)
		{
			return null;
		}
		return match.Groups[1].Value + value;
	}

	public static bool IsValidJabber(this string strIn)
	{
		if (string.IsNullOrEmpty(strIn))
		{
			return false;
		}
		strIn = strIn.Trim();
		if (!strIn.IsValidEmail())
		{
			return RX_LOGIN.Match(strIn).Success;
		}
		return true;
	}

	public static bool IsValidSkype(this string strIn)
	{
		if (string.IsNullOrEmpty(strIn))
		{
			return false;
		}
		return RX_LOGIN.Match(strIn.Trim()).Success;
	}

	public static bool IsValidPhone(this string strIn)
	{
		if (string.IsNullOrEmpty(strIn))
		{
			return false;
		}
		strIn = new string(strIn.ToCharArray().Where(char.IsDigit).ToArray());
		return RX_PHONE.Match(strIn).Success;
	}

	public static bool IsValidTelegram(this string strIn)
	{
		if (string.IsNullOrEmpty(strIn))
		{
			return false;
		}
		strIn = strIn.Trim();
		if (!RX_PHONE.Match(strIn).Success && !RX_LOGIN.Match(strIn).Success)
		{
			return RX_TELEGRAM_CHATID.Match(strIn).Success;
		}
		return true;
	}

	public static bool IsValidViberBot(this string strIn)
	{
		return strIn.IsValidTelegram();
	}

	public static bool IsValidICQ(this string strIn)
	{
		return strIn.IsValidTelegram();
	}

	public static bool IsValidUrl(this string strIn)
	{
		if (string.IsNullOrEmpty(strIn))
		{
			return false;
		}
		strIn = strIn.Trim();
		return Uri.IsWellFormedUriString(strIn, UriKind.RelativeOrAbsolute);
	}

	public static bool ScanNumber(this string line, int pos, ref int val)
	{
		return line.ScanNumber(ref pos, ref val);
	}

	public static bool ScanNumber(this string line, ref int pos, ref int val)
	{
		if (line == null)
		{
			return false;
		}
		int length = line.Length;
		char c;
		while (pos < length)
		{
			c = line[pos];
			if (char.IsWhiteSpace(c))
			{
				pos++;
				continue;
			}
			if (char.IsDigit(c) || c == '+' || c == '-')
			{
				break;
			}
			return false;
		}
		if (pos >= length)
		{
			return false;
		}
		c = line[pos];
		bool flag = c == '-';
		if (c == '+' || c == '-')
		{
			while (++pos < length)
			{
				c = line[pos];
				if (!char.IsWhiteSpace(c))
				{
					if (char.IsDigit(c))
					{
						break;
					}
					return false;
				}
			}
			if (pos >= length)
			{
				return false;
			}
		}
		val = 0;
		while (pos < length)
		{
			c = line[pos];
			if (!char.IsDigit(c))
			{
				break;
			}
			val *= 10;
			val += c - 48;
			pos++;
		}
		if (flag)
		{
			val = -val;
		}
		return true;
	}

	public static bool ScanNumber(this string line, int pos, ref double val)
	{
		return line.ScanNumber(ref pos, ref val);
	}

	public static bool ScanNumber(this string line, ref int pos, ref double val)
	{
		return line.ScanNumber(ref pos, '.', ref val);
	}

	public static bool ScanNumber(this string line, ref int pos, char ds, ref double val)
	{
		if (line == null)
		{
			return false;
		}
		int length = line.Length;
		char c;
		while (pos < length)
		{
			c = line[pos];
			if (char.IsWhiteSpace(c))
			{
				pos++;
				continue;
			}
			if (char.IsDigit(c) || c == '+' || c == '-' || c == ds)
			{
				break;
			}
			return false;
		}
		if (pos >= length)
		{
			return false;
		}
		c = line[pos];
		bool flag = c == '-';
		if (c == '+' || c == '-')
		{
			while (++pos < length)
			{
				c = line[pos];
				if (!char.IsWhiteSpace(c))
				{
					if (char.IsDigit(c) || c == ds)
					{
						break;
					}
					return false;
				}
			}
			if (pos >= length)
			{
				return false;
			}
		}
		val = 0.0;
		bool flag2 = false;
		bool flag3 = false;
		double num = 1.0;
		while (pos < length)
		{
			c = line[pos];
			if (c == ds)
			{
				if (flag3)
				{
					break;
				}
				flag3 = true;
				pos++;
				continue;
			}
			if (!char.IsDigit(c))
			{
				break;
			}
			flag2 = true;
			val *= 10.0;
			val += c - 48;
			if (flag3)
			{
				num *= 10.0;
			}
			pos++;
		}
		if (!flag2)
		{
			return false;
		}
		val /= num;
		if (flag)
		{
			val = 0.0 - val;
		}
		return true;
	}

	public static bool PassComma(this string line, ref int pos)
	{
		int length = line.Length;
		while (pos < length)
		{
			char c = line[pos];
			if (char.IsWhiteSpace(c))
			{
				pos++;
				continue;
			}
			if (c == ',')
			{
				pos++;
				break;
			}
			return false;
		}
		return pos < length;
	}

	public static bool PassColon(this string line, ref int pos)
	{
		int length = line.Length;
		while (pos < length)
		{
			char c = line[pos];
			if (char.IsWhiteSpace(c))
			{
				pos++;
				continue;
			}
			if (c == ':')
			{
				pos++;
				break;
			}
			return false;
		}
		return pos < length;
	}

	public static bool PassChar(this string line, char ch, ref int pos)
	{
		int length = line.Length;
		while (pos < length)
		{
			char c = line[pos];
			if (char.IsWhiteSpace(c))
			{
				pos++;
				continue;
			}
			if (c == ch)
			{
				pos++;
				break;
			}
			return false;
		}
		return pos < length;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static int NumCompare(string strA, string strB)
	{
		return strCmpLogicalHandler((strA != null) ? strA.Replace(" ", string.Empty) : string.Empty, (strB != null) ? strB.Replace(" ", string.Empty) : string.Empty);
	}

	static StringExtenders()
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		idnMapper = new IdnMapping();
		RX_PARM_REPLACE = new Regex("{(\\S+)\\.(\\S+)\\.(\\S+)}", RegexOptions.None);
		RX_PROPERTY_NAME = new Regex("^\\{\\S+\\}$", RegexOptions.None);
		RX_LOGIN = new Regex("\\A[a-zA-Z0123456789_]{3,32}$", RegexOptions.None);
		RX_TELEGRAM_CHATID = new Regex("\\A@\\d{3,32}$", RegexOptions.None);
		RX_PHONE = new Regex("^[0-9]{5,13}$", RegexOptions.None);
		digits = new char[10] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
		Func<string, string, int> func = (((int)RuntimePlatform.OS != 0) ? new Func<string, string, int>(StrCmpLogical_Alt) : new Func<string, string, int>(StrCmpLogicalW));
		strCmpLogicalHandler = func;
	}

	[DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
	private static extern int StrCmpLogicalW(string psz1, string psz2);

	private static int StrCmpLogical_Alt(string strA, string strB)
	{
		int pos = 0;
		int pos2 = 0;
		int num = ((strA != null && strB != null) ? Math.Min(strA.IndexOfAny(digits), strB.IndexOfAny(digits)) : (-1));
		if (num > 0 && string.Compare(strA, 0, strB, 0, num, ignoreCase: true) == 0)
		{
			pos = (pos2 = num);
		}
		int val = 0;
		bool flag = strA.ScanNumber(ref pos, ref val);
		int val2 = 0;
		bool flag2 = strB.ScanNumber(ref pos2, ref val2);
		if (flag && flag2)
		{
			int num2 = val.CompareTo(val2);
			if (num2 != 0)
			{
				return num2;
			}
		}
		if (flag)
		{
			strA = "0" + strA.Substring(pos);
		}
		if (flag2)
		{
			strB = "0" + strB.Substring(pos2);
		}
		return string.Compare(strA, strB, StringComparison.InvariantCultureIgnoreCase);
	}

	public static bool ComplexTextSearching(this string srcText, string[] findText)
	{
		if (srcText == null)
		{
			return false;
		}
		if (!findText.Any())
		{
			return true;
		}
		srcText = srcText.Replace('Ё', 'Е').Replace('ё', 'е');
		foreach (string value in findText)
		{
			int num = srcText.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);
			if (num < 0)
			{
				return false;
			}
			if (num > 0 && char.IsLetterOrDigit(srcText[num - 1]))
			{
				return false;
			}
		}
		return true;
	}
}
