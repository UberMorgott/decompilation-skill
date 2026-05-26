using System;
using System.Drawing;
using KML;

namespace AGInterfaces;

public class AgGeoFenceStyle
{
	public Guid ID { get; set; }

	public double LineWidth { get; set; }

	public int LineColor { get; set; }

	public int FillColor { get; set; }

	public AgGeoFenceStyle()
	{
		ID = Guid.NewGuid();
		LineWidth = 1.0;
		LineColor = Color.Green.ToArgb();
		FillColor = -2130771968;
	}

	public AgGeoFenceStyle(Color fillColor, Color lineColor, int lineWidth)
	{
		ID = Guid.NewGuid();
		LineWidth = lineWidth;
		LineColor = lineColor.ToArgb();
		FillColor = fillColor.ToArgb();
	}

	public AgGeoFenceStyle(KMLStyle kmlStyle)
	{
		if (Guid.TryParse(kmlStyle.ID, out var result))
		{
			ID = result;
		}
		else
		{
			ID = Guid.NewGuid();
		}
		LineWidth = kmlStyle.Line?.Width ?? 1.0;
		LineColor = ((KMLStyleLabel)(kmlStyle.Line?)).Color.ToArgb() ?? Color.Green.ToArgb();
		FillColor = ((KMLStyleLabel)(kmlStyle.Fill?)).Color.ToArgb() ?? (-2130771968);
	}

	public KMLStyle GetKMLStyle()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		return new KMLStyle(Color.FromArgb(FillColor), Color.FromArgb(LineColor), LineWidth)
		{
			ID = ID.ToString()
		};
	}

	public bool IsEquals(AgGeoFenceStyle to)
	{
		if (to == null)
		{
			return false;
		}
		if (LineWidth != to.LineWidth || LineColor != to.LineColor || FillColor != to.FillColor)
		{
			return false;
		}
		return true;
	}
}
