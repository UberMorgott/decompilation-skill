using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AGInterfaces;

public class ColorProps
{
	public static readonly Color[] lightColorArray = new Color[16]
	{
		Color.LightCoral,
		Color.PaleGoldenrod,
		Color.GreenYellow,
		Color.LightBlue,
		Color.Orchid,
		Color.Bisque,
		Color.PaleGreen,
		Color.PaleTurquoise,
		Color.DarkOrchid,
		Color.LightSalmon,
		Color.Khaki,
		Color.CornflowerBlue,
		Color.MediumVioletRed,
		Color.SandyBrown,
		Color.CadetBlue,
		Color.Silver
	};

	public static readonly Color[] normalColorArray = new Color[16]
	{
		Color.Red,
		Color.Yellow,
		Color.Lime,
		Color.Aqua,
		Color.Magenta,
		Color.Orange,
		Color.Green,
		Color.Blue,
		Color.Indigo,
		Color.OrangeRed,
		Color.Olive,
		Color.MidnightBlue,
		Color.Maroon,
		Color.SaddleBrown,
		Color.Teal,
		Color.Gray
	};

	public int color { get; set; }

	public double value { get; set; }

	public static int getUniqueColor(IEnumerable<int> colors, Color[] palette)
	{
		int num = 0;
		while (true)
		{
			foreach (Color color in palette)
			{
				int iColor = color.ToArgb();
				if (colors.Count((int s) => s == iColor) <= num)
				{
					return iColor;
				}
			}
			num++;
		}
	}

	public static int getUniqueColor<T>(List<T> colors, Color[] palette) where T : ColorProps
	{
		int num = 0;
		while (true)
		{
			foreach (Color color in palette)
			{
				int iColor = color.ToArgb();
				if (colors.Count((T s) => s.color == iColor) <= num)
				{
					return iColor;
				}
			}
			num++;
		}
	}
}
