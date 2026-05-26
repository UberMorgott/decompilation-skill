using System.ComponentModel;
using System.Drawing;

namespace AGInterfaces.Extenders;

public class ImageCollection
{
	public readonly Images Images = new Images();

	public Size ImageSize = new Size(16, 16);

	public ImageCollection(bool fake)
	{
	}

	public ImageCollection()
	{
	}

	public ImageCollection(IContainer container)
	{
	}

	public void AddImage(Image img, string key)
	{
		Images.Add(key, img);
	}

	public void AddImage(Image img)
	{
		Images.Add(img);
	}
}
