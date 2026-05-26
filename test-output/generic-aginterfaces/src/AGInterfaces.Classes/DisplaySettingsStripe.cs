using System;

namespace AGInterfaces.Classes;

[Serializable]
public sealed class DisplaySettingsStripe
{
	public readonly string Name;

	public double? ValueFrom { get; set; }

	public double? ValueTo { get; set; }

	public int FillColor { get; set; }

	public int HatchStyle { get; set; }

	public bool ShowLines { get; set; }

	public int Opacity { get; set; }

	public DisplaySettingsStripe()
	{
		Name = Guid.NewGuid().ToString();
	}
}
