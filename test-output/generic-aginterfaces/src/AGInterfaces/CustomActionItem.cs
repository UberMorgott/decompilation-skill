using System;
using System.Drawing;

namespace AGInterfaces;

public class CustomActionItem
{
	public Guid ID { get; private set; }

	public string Caption { get; private set; }

	public Image Image { get; private set; }

	public CustomActionItem[] Items { get; private set; }

	public CustomActionItem(Guid id, string caption, Image image, params CustomActionItem[] subitems)
	{
		ID = id;
		Caption = caption;
		Image = image;
		Items = subitems;
	}
}
