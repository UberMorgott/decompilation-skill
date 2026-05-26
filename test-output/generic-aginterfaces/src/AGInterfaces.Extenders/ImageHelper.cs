using System.Drawing;

namespace AGInterfaces.Extenders;

public static class ImageHelper
{
	public static Image Inscribe(this Image image, Size maxImageSize, bool disposeBase = true)
	{
		if (image == null)
		{
			return null;
		}
		if (image.Size.Width <= maxImageSize.Width && image.Size.Height <= maxImageSize.Height)
		{
			return image;
		}
		Size inscribedSize = RectangleHelper.GetInscribedSize(maxImageSize, image.Size);
		Bitmap result = new Bitmap(image, inscribedSize);
		if (disposeBase)
		{
			image.Dispose();
		}
		return result;
	}
}
