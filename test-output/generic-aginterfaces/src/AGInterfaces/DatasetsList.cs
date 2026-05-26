using System;
using System.Collections.Generic;

namespace AGInterfaces;

public static class DatasetsList
{
	public static List<ReportDataset> DSlist { get; private set; }

	public static void ClearData()
	{
		foreach (ReportDataset item in DSlist)
		{
			item.Data = Array.CreateInstance(item.DSType, 0);
		}
	}

	static DatasetsList()
	{
		DSlist = new List<ReportDataset>();
		DSlist.Add(new ReportDataset("DS_carList", new DSInfo("", ""), DataTypesEnum.CarListDataset, typeof(CarListDatasetFields)));
		DSlist.Add(new ReportDataset("DS_trip", new DSInfo("trip", ""), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_chp_by_time", new DSInfo("checkpoints", "points_by_time"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_chp_stages", new DSInfo("checkpoints", "stages"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_chp_all", new DSInfo("checkpoints", "points_and_stages"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_zones_by_time", new DSInfo("geozones", "zones_by_time"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_zones_stages", new DSInfo("geozones", "stages"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_zones_all", new DSInfo("geozones", "zones_and_stages"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_streets", new DSInfo("streets", "streets"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_streets_all", new DSInfo("streets", "streets_and_stages"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_platon", new DSInfo("platon", "platon"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_platon_all", new DSInfo("platon", "platon_and_stages"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_parks", new DSInfo("parks", ""), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_moves", new DSInfo("moves", ""), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_parks_and_moves", new DSInfo("parks_and_moves", ""), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_blinds", new DSInfo("blinds", ""), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_power_off", new DSInfo("power_off", ""), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_over_speed", new DSInfo("over_speed", ""), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_tank1", new DSInfo("tank", "num1"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_tank2", new DSInfo("tank", "num2"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_tank3", new DSInfo("tank", "num3"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_tank4", new DSInfo("tank", "num4"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_sensor1", new DSInfo("sensor_on", "num1"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_sensor2", new DSInfo("sensor_on", "num2"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_sensor3", new DSInfo("sensor_on", "num3"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_sensor4", new DSInfo("sensor_on", "num4"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_sensor5", new DSInfo("sensor_on", "num5"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_sensor6", new DSInfo("sensor_on", "num6"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_sensor7", new DSInfo("sensor_on", "num7"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_sensor8", new DSInfo("sensor_on", "num8"), DataTypesEnum.AGDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_fields", new DSInfo("fields", ""), DataTypesEnum.FieldsDataset, typeof(AGListFields)));
		DSlist.Add(new ReportDataset("DS_data_coords", new DSInfo("data_coords", ""), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_fuel1", new DSInfo("data_fuel", "num1"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_fuel2", new DSInfo("data_fuel", "num2"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_fuel3", new DSInfo("data_fuel", "num3"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_fuel4", new DSInfo("data_fuel", "num4"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_level1", new DSInfo("data_level", "num1"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_level2", new DSInfo("data_level", "num2"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_level3", new DSInfo("data_level", "num3"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_level4", new DSInfo("data_level", "num4"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_counters12", new DSInfo("data_counters", "num12"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_counters34", new DSInfo("data_counters", "num34"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_counters56", new DSInfo("data_counters", "num56"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_counters78", new DSInfo("data_counters", "num78"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_1w_thermo14", new DSInfo("data_1w_thermo", "num14"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_1w_thermo58", new DSInfo("data_1w_thermo", "num58"), DataTypesEnum.AGDataset, typeof(AGDataFields)));
		DSlist.Add(new ReportDataset("DS_data_all", new DSInfo("data_all", ""), DataTypesEnum.AGDataset, typeof(AGDataFields)));
	}
}
