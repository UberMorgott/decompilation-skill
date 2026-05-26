using System;
using System.Drawing;

namespace AGInterfaces;

public class CarListDatasetFields : BaseDatasetFields
{
	public string GroupName { get; set; }

	public int Sernum { get; set; }

	public string Model { get; set; }

	public string Number { get; set; }

	public string Tel { get; set; }

	public string FileKT { get; set; }

	public string Placement { get; set; }

	public double Lat { get; set; }

	public double Lng { get; set; }

	public double Azimut { get; set; }

	public double Speed { get; set; }

	public string ParkTime { get; set; }

	public string LastTime { get; set; }

	public string LastATime { get; set; }

	public DateTime ParkTimeDT { get; set; }

	public DateTime LastTimeDT { get; set; }

	public DateTime LastATimeDT { get; set; }

	public Image RowImage { get; set; }

	public CarListDatasetFields(string _GroupName, int _Sernum, string _Model, string _Number, string _Tel, string _FileKT, string _Placement, double _Lat, double _Lng, double _Azimut, double _Speed, string _ParkTime, string _LastTime, string _LastATime, DateTime _ParkTimeDT, DateTime _LastTimeDT, DateTime _LastATimeDT, Image _RowImage)
	{
		GroupName = _GroupName;
		Sernum = _Sernum;
		Model = _Model;
		Number = _Number;
		Tel = _Tel;
		FileKT = _FileKT;
		Placement = _Placement;
		Lat = _Lat;
		Lng = _Lng;
		Azimut = _Azimut;
		Speed = _Speed;
		ParkTime = _ParkTime;
		LastTime = _LastTime;
		LastATime = _LastATime;
		ParkTimeDT = _ParkTimeDT;
		LastTimeDT = _LastTimeDT;
		LastATimeDT = _LastATimeDT;
		RowImage = _RowImage;
	}
}
