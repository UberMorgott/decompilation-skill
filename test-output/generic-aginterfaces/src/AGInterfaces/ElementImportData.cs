using System.Collections.Generic;
using AGInterfaces.Wlp;
using Newtonsoft.Json.Linq;

namespace AGInterfaces;

public class ElementImportData
{
	public string Description;

	public string Value;

	public string Unit;

	public string Type;

	public double MinFillingVolume;

	public double MinTheftVolume;

	public List<(string key, int delimiter)> OperatorKeys = new List<(string, int)>();

	public readonly int id;

	public ElementImportData(string Value)
	{
		this.Value = Value;
	}

	public ElementImportData(string Description, string Value)
	{
		this.Description = Description;
		this.Value = Value;
	}

	public ElementImportData(Profile profile)
	{
		Description = profile.n;
		Value = profile.v;
	}

	public ElementImportData(Field field)
	{
		Description = field.n;
		Value = field.v;
	}

	public ElementImportData(JObject afield)
	{
		Description = afield.SelectToken("n")?.ToString() ?? string.Empty;
		Value = afield.SelectToken("v")?.ToString() ?? string.Empty;
		Unit = afield.SelectToken("m")?.ToString() ?? string.Empty;
		Type = afield.SelectToken("t")?.ToString() ?? string.Empty;
	}

	public ElementImportData(Sensor sensor, LevelParams prms, bool isTare)
	{
		id = sensor.id;
		Type = sensor.t;
		if (isTare)
		{
			Description = sensor.n + " " + sensor.p;
			Value = sensor.d;
			return;
		}
		Description = sensor.n;
		Value = sensor.p;
		Unit = sensor.m;
		List<Tbl> tbl = sensor.tbl;
		if (tbl != null && tbl.Count >= 2 && tbl[0].a == 0.0 && tbl[1].a == 0.0)
		{
			if (tbl[0].b == 1.0 && tbl[1].b == 0.0)
			{
				Value = Value + "<" + tbl[1].x;
			}
			else if (tbl[0].b == 0.0 && tbl[1].b == 1.0)
			{
				Value = Value + ">=" + tbl[1].x;
			}
		}
		MinFillingVolume = (prms?.fuel_params?.minFillingVolume).GetValueOrDefault();
		MinTheftVolume = (prms?.fuel_params?.minTheftVolume).GetValueOrDefault();
	}

	public override string ToString()
	{
		return $"{id}: \"{Description}\" {Value}, {Unit}";
	}
}
