using System.Drawing;

namespace AGInterfaces.Extenders;

public static class RectangleHelper
{
	public static Rectangle GetCenteredRectangleUnscale(Rectangle baseRect, int w, int h)
	{
		Size baseSize = new Size(w, h);
		if (w > baseRect.Width || h > baseRect.Height)
		{
			baseSize = GetInscribedSize(baseRect.Size, baseSize);
		}
		return new Rectangle(baseRect.X + (baseRect.Width - baseSize.Width >> 1), baseRect.Y + (baseRect.Height - baseSize.Height >> 1), baseSize.Width, baseSize.Height);
	}

	public static Rectangle GetCenteredSquare(Rectangle baseRect)
	{
		Rectangle result = baseRect;
		if (result.Width > result.Height)
		{
			result.X = result.X + (result.Width >> 1) - (result.Height >> 1);
			result.Width = result.Height;
		}
		else if (result.Width < result.Height)
		{
			result.Y = result.Y + (result.Height >> 1) - (result.Width >> 1);
			result.Height = result.Width;
		}
		return result;
	}

	public static Rectangle GetCenteredRectangle(Rectangle baseRect, int w, int h)
	{
		return GetCenteredRectangle(baseRect, new Rectangle(0, 0, w, h));
	}

	public static Rectangle GetCenteredRectangle(Rectangle baseRect, Rectangle imageRect)
	{
		if (imageRect.Height == imageRect.Width)
		{
			return GetCenteredSquare(baseRect);
		}
		float num = CalcReductionFactor(baseRect.Width, baseRect.Height, imageRect.Width, imageRect.Height);
		imageRect.Width = (int)((float)imageRect.Width * num);
		imageRect.Height = (int)((float)imageRect.Height * num);
		imageRect.X = baseRect.X + (baseRect.Width >> 1) - (imageRect.Width >> 1);
		imageRect.Y = baseRect.Y + (baseRect.Height >> 1) - (imageRect.Height >> 1);
		return imageRect;
	}

	public static Size GetInscribedSize(Size maxSize, Size baseSize)
	{
		float num = CalcReductionFactor(maxSize.Width, maxSize.Height, baseSize.Width, baseSize.Height);
		return new Size((int)((float)baseSize.Width * num), (int)((float)baseSize.Height * num));
	}

	private static float CalcReductionFactor(int w1, int h1, int w2, int h2)
	{
		float num = (float)w1 / (float)w2;
		float num2 = (float)h1 / (float)h2;
		if (!(num < num2))
		{
			return num2;
		}
		return num;
	}
}
