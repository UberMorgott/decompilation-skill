using System.Drawing;
using Newtonsoft.Json;

namespace AGInterfaces;

public class ImageProperties
{
	public readonly string imageName;

	public readonly bool turn;

	public readonly int opaque;

	public readonly Color hue;

	[JsonConstructor]
	public ImageProperties(string imageName, bool turn, int opaque, Color hue)
	{
		this.imageName = imageName;
		this.turn = turn;
		this.opaque = opaque;
		this.hue = hue;
	}
}
