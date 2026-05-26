using System;
using System.Drawing;

namespace AGInterfaces;

public static class RegistryPropertyExtenders
{
	public static object correctByType(this PropType type, object Value)
	{
		switch (type)
		{
		case PropType.None:
			Value = null;
			break;
		case PropType.String:
		case PropType.Memo:
			if (!(Value is string))
			{
				Value = ((!(Value is decimal) && !(Value is DateTime)) ? null : Value.ToString());
			}
			break;
		case PropType.Number:
		case PropType.Radio:
		case PropType.ProgressBar:
		{
			if (Value is decimal)
			{
				if ((decimal)Value > 2147483647m)
				{
					Value = Convert.ToDecimal(int.MaxValue);
				}
				else if ((decimal)Value < -2147483648m)
				{
					Value = Convert.ToDecimal(int.MinValue);
				}
				break;
			}
			if (Value is int || Value is double)
			{
				Value = Convert.ToDecimal(Value);
				if ((decimal)Value > 2147483647m)
				{
					Value = Convert.ToDecimal(int.MaxValue);
				}
				else if ((decimal)Value < -2147483648m)
				{
					Value = Convert.ToDecimal(int.MinValue);
				}
				break;
			}
			string text2 = Value?.ToString();
			if (text2 != null && decimal.TryParse(text2, out var result2))
			{
				Value = result2;
				if ((decimal)Value > 2147483647m)
				{
					Value = Convert.ToDecimal(int.MaxValue);
				}
				else if ((decimal)Value < -2147483648m)
				{
					Value = Convert.ToDecimal(int.MinValue);
				}
			}
			else
			{
				Value = 0m;
			}
			break;
		}
		case PropType.Date:
		case PropType.Time:
			if (!(Value is DateTime))
			{
				string text3 = Value?.ToString();
				Value = ((text3 == null || !DateTime.TryParse(text3, out var result3)) ? ((object)DateTime.Today) : ((object)result3));
			}
			break;
		case PropType.Color:
			if (!(Value is int))
			{
				Value = default(Color).ToArgb();
			}
			break;
		case PropType.Bool:
			if (!(Value is bool))
			{
				Value = ((!bool.TryParse(Value?.ToString(), out var result4)) ? ((object)false) : ((object)result4));
			}
			break;
		case PropType.TareTable:
			if (!(Value is TareTable))
			{
				Value = new TareTable();
			}
			break;
		case PropType.Image:
		case PropType.File:
		case PropType.FileLink:
			if (!(Value is RegistryPropertyBinaryData))
			{
				Value = ((!(Value is byte[] content)) ? null : new RegistryPropertyBinaryData
				{
					content = content,
					name = "File"
				});
			}
			break;
		case PropType.Device:
		case PropType.GeoFence:
		case PropType.Driver:
		case PropType.Implement:
		case PropType.Task:
			if (!(Value is Guid))
			{
				string text = Value?.ToString();
				Value = ((text == null || !Guid.TryParse(text, out var result)) ? ((object)Guid.Empty) : ((object)result));
			}
			break;
		case PropType.CheckDays:
			if (Value == null || !Value.GetType().Name.Equals("FDayOfWeek"))
			{
				Value = 0;
			}
			break;
		case PropType.Combobox:
			if (!(Value is string))
			{
				Value = null;
			}
			break;
		}
		return Value;
	}
}
