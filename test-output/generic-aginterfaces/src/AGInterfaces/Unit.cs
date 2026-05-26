using System;
using System.Drawing;

namespace AGInterfaces;

[Serializable]
public class Unit
{
	private Color _color;

	private bool _isTransparent;

	public string Name { get; set; }

	public double Width { get; set; }

	public double Offset { get; set; }

	public string Sensor { get; set; }

	public int TrackColor
	{
		get
		{
			return Color.FromArgb(255, _color.R, _color.G, _color.B).ToArgb();
		}
		set
		{
			Color baseColor = Color.FromArgb(value);
			_color = Color.FromArgb(IsTransparent ? 64 : 255, baseColor);
		}
	}

	public bool IsTransparent
	{
		get
		{
			return _isTransparent;
		}
		set
		{
			if (_isTransparent != value)
			{
				_isTransparent = value;
				TrackColor = TrackColor;
			}
		}
	}

	public Unit()
	{
		_color = Color.FromArgb(64, 64, 32, 128);
		_isTransparent = true;
	}

	public Color GetColor()
	{
		return _color;
	}
}
