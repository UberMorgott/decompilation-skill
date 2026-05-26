using System;

namespace AGInterfaces;

public class ImplementInfo
{
	public static readonly ImplementInfo Empty = new ImplementInfo();

	public string MapFileName;

	public string ImgFileName;

	public IUnitPolygon[] trackPolygonsForArea;

	public MainImageProps ImageProps;

	public ElementType Type { get; set; }

	public int ID { get; set; }

	public int ParentID { get; set; }

	public Guid AreaId { get; set; }

	public Guid DeviceId { get; set; }

	public Guid ImplementId { get; set; }

	public string Name { get; set; }

	public string Number { get; set; }

	public double? Total { get; set; }

	public double Complete { get; set; }

	public double? Incomplete { get; set; }

	public double Overlap { get; set; }

	public double FullOverlap { get; set; }

	public int MaxOverlapDepth { get; set; }

	public double CompletePercents { get; set; }

	public double? IncompletePercents { get; set; }

	public double OverlapPercents { get; set; }

	public int ImageIndex { get; set; }

	public ImplementInfo()
	{
	}

	public ImplementInfo(Guid AreaId, AreaInfo areaInfo)
	{
		Type = ElementType.GeoFence;
		this.AreaId = AreaId;
		Total = (double)areaInfo.CommomArea.Total / 10000.0;
		Complete = (double)areaInfo.CommomArea.Complete / 10000.0;
		Incomplete = (double)areaInfo.CommomArea.Incomplete / 10000.0;
		Overlap = (double)areaInfo.CommomArea.Overlap / 10000.0;
		FullOverlap = (double)areaInfo.CommomArea.FullOverlap / 10000.0;
		MaxOverlapDepth = areaInfo.CommomArea.MaxOverlapDepth;
		CompletePercents = (double)areaInfo.CommomArea.Complete * 100.0 / (double)areaInfo.CommomArea.Total;
		IncompletePercents = (double)areaInfo.CommomArea.Incomplete * 100.0 / (double)areaInfo.CommomArea.Total;
		OverlapPercents = (double)areaInfo.CommomArea.Overlap * 100.0 / (double)areaInfo.CommomArea.Total;
	}

	public ImplementInfo(Guid AreaId, DeviceArea deviceInfo)
	{
		Type = ElementType.Device;
		this.AreaId = AreaId;
		DeviceId = deviceInfo.Id;
		Complete = (double)deviceInfo.Area.Complete / 10000.0;
		Overlap = (double)deviceInfo.Area.Overlap / 10000.0;
		FullOverlap = (double)deviceInfo.Area.FullOverlap / 10000.0;
		MaxOverlapDepth = deviceInfo.Area.MaxOverlapDepth;
		CompletePercents = (double)deviceInfo.Area.Complete * 100.0 / (double)deviceInfo.Area.Total;
		OverlapPercents = (double)deviceInfo.Area.Overlap * 100.0 / (double)deviceInfo.Area.Total;
	}

	public ImplementInfo(Guid AreaId, Guid DeviceId, ImplementArea implementInfo)
	{
		Type = ElementType.Implement;
		this.AreaId = AreaId;
		this.DeviceId = DeviceId;
		ImplementId = implementInfo.Id;
		Complete = (double)implementInfo.Area.Complete / 10000.0;
		Overlap = (double)implementInfo.Area.Overlap / 10000.0;
		FullOverlap = (double)implementInfo.Area.FullOverlap / 10000.0;
		MaxOverlapDepth = implementInfo.Area.MaxOverlapDepth;
		CompletePercents = (double)implementInfo.Area.Complete * 100.0 / (double)implementInfo.Area.Total;
		OverlapPercents = (double)implementInfo.Area.Overlap * 100.0 / (double)implementInfo.Area.Total;
	}

	public override string ToString()
	{
		string text = string.Empty;
		switch (Type)
		{
		case ElementType.Device:
			text = "    ";
			break;
		case ElementType.Implement:
			text = "        ";
			break;
		}
		text += $"{Name} [{ID} / {ParentID}]:";
		if (AreaId != Guid.Empty)
		{
			text = text + " / " + AreaId;
		}
		if (DeviceId != Guid.Empty)
		{
			text = text + " / " + DeviceId;
		}
		if (ImplementId != Guid.Empty)
		{
			text = text + " / " + ImplementId;
		}
		return text;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ImplementInfo))
		{
			return false;
		}
		return Equals((ImplementInfo)obj);
	}

	private bool Equals(ImplementInfo info)
	{
		if (Type == info.Type && ID == info.ID && ParentID == info.ParentID && AreaId == info.AreaId && DeviceId == info.DeviceId && ImplementId == info.ImplementId && Name == info.Name && Number == info.Number && Total == info.Total && Complete == info.Complete && Incomplete == info.Incomplete && Overlap == info.Overlap && FullOverlap == info.FullOverlap && MaxOverlapDepth == info.MaxOverlapDepth && CompletePercents == info.CompletePercents && IncompletePercents == info.IncompletePercents && OverlapPercents == info.OverlapPercents && MapFileName == info.MapFileName)
		{
			return ImgFileName == info.ImgFileName;
		}
		return false;
	}
}
