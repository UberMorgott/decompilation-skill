using System.Reflection;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public sealed class MainImageProps : ElementImageProps
{
	public int hue { get; set; }

	public MainImageProps()
	{
		base.opaque = 100;
	}

	public MainImageProps(string image)
	{
		base.image = image;
		base.opaque = 100;
	}
}
