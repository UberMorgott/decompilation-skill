using System;
using System.Drawing;
using Newtonsoft.Json;

namespace AGInterfaces;

public sealed class ViewProperties
{
	public object number;

	public TimeZoneInfo timeZoneInfo;

	public string alias;

	public double workZone;

	public bool blinking;

	public Color cursorColor;

	public ImageProperties[] images;

	[JsonConstructor]
	public ViewProperties(TimeZoneInfo timeZoneInfo, object number, string alias, double workZone, bool blinking, Color cursorColor, ImageProperties[] images)
	{
		this.timeZoneInfo = timeZoneInfo;
		this.number = number;
		this.alias = alias;
		this.workZone = workZone;
		this.blinking = blinking;
		this.cursorColor = cursorColor;
		this.images = images;
	}
}
