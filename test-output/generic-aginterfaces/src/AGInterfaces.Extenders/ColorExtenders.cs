using System.Drawing;
using System.Drawing.Imaging;

namespace AGInterfaces.Extenders;

public static class ColorExtenders
{
	public static readonly Color noHue = Color.FromArgb(0);

	public static readonly ColorMatrix emptyColorMatrix = new ColorMatrix();

	public static readonly ColorMatrix bwMatrix = new ColorMatrix(new float[5][]
	{
		new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
		new float[5] { 0.59f, 0.59f, 0.59f, 0f, 0f },
		new float[5] { 0.11f, 0.11f, 0.11f, 0f, 0f },
		new float[5] { 0f, 0f, 0f, 1f, 0f },
		new float[5] { 0.25f, 0.25f, 0.25f, 0f, 1f }
	});

	public static ColorMatrix getColorMatrix(this Color hue)
	{
		if (hue == noHue)
		{
			return emptyColorMatrix;
		}
		float num = (float)(int)hue.R / 255f;
		float num2 = (1f - num) * 0.5f;
		float num3 = (1f - num) * 0.5f;
		float num4 = (float)(int)hue.G / 255f;
		float num5 = (1f - num4) * 0.5f;
		float num6 = (1f - num4) * 0.5f;
		float num7 = (float)(int)hue.B / 255f;
		float num8 = (1f - num7) * 0.5f;
		float num9 = (1f - num7) * 0.5f;
		return new ColorMatrix(new float[5][]
		{
			new float[5] { num, num4, num7, 0f, 0f },
			new float[5] { num2, num5, num8, 0f, 0f },
			new float[5] { num3, num6, num9, 0f, 0f },
			new float[5] { 0f, 0f, 0f, 1f, 0f },
			new float[5] { 0f, 0f, 0f, 0f, 1f }
		});
	}

	public static Color getInverted(this Color c)
	{
		if (30 * c.R + 59 * c.G + 11 * c.B <= 12800)
		{
			return Color.White;
		}
		return Color.Black;
	}

	public static Color GetAverageColor(Color color1, Color color2, float ratio = 0.5f)
	{
		if (ratio < 0f || ratio > 1f)
		{
			ratio = 0.5f;
		}
		float num = ratio;
		float num2 = 1f - ratio;
		return Color.FromArgb((int)((float)(int)color1.A * num + (float)(int)color2.A * num2), (int)((float)(int)color1.R * num + (float)(int)color2.R * num2), (int)((float)(int)color1.G * num + (float)(int)color2.G * num2), (int)((float)(int)color1.B * num + (float)(int)color2.B * num2));
	}

	public static Color mixedColor(this Color c1, Color c2, int p)
	{
		int num = 100 - p;
		return Color.FromArgb((p * c1.R + num * c2.R) / 100, (p * c1.G + num * c2.G) / 100, (p * c1.B + num * c2.B) / 100);
	}

	public static Color normalizedColor(this Color color)
	{
		int num = color.R;
		if (num > color.G)
		{
			num = color.G;
		}
		if (num > color.B)
		{
			num = color.B;
		}
		int num2 = color.R;
		if (num2 < color.G)
		{
			num2 = color.G;
		}
		if (num2 < color.B)
		{
			num2 = color.B;
		}
		int num3 = num2 - num;
		if (num3 <= 0)
		{
			return color;
		}
		return Color.FromArgb((color.R - num) * 255 / num3, (color.G - num) * 255 / num3, (color.B - num) * 255 / num3);
	}

	public static Color darker(this Color from, double coef, int? A = null)
	{
		return Color.FromArgb(A.HasValue ? A.Value : from.A, (int)((double)(int)from.R * coef), (int)((double)(int)from.G * coef), (int)((double)(int)from.B * coef));
	}
}
