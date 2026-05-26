using System.Reflection;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public class ElementImageProps
{
	public string image { get; set; }

	public bool turn { get; set; }

	public int opaque { get; set; }

	public string parameter { get; set; }

	public Operator oper { get; set; }

	public double value { get; set; }

	public ElementImageProps()
	{
		opaque = 100;
	}

	public ElementImageProps(string image)
	{
		this.image = image;
		opaque = 100;
	}
}
