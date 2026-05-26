using System;
using System.Drawing;

namespace AGInterfaces;

public class AreaPoint : IEquatable<AreaPoint>
{
	private double _x;

	private double _y;

	private Coordinates _coordinates;

	public double X
	{
		get
		{
			return _x;
		}
		set
		{
			_x = value;
		}
	}

	public double Y
	{
		get
		{
			return _y;
		}
		set
		{
			_y = value;
		}
	}

	public Coordinates Coordinates
	{
		get
		{
			return _coordinates;
		}
		set
		{
			_coordinates.latitude = value.latitude;
			_coordinates.longitude = value.longitude;
		}
	}

	public AreaPoint()
	{
	}

	public AreaPoint(double x, double y)
	{
		_x = x;
		_y = y;
	}

	public AreaPoint CalculatePointCoordinates(AreaPoint trackPoint, double ky, double kx)
	{
		double num = trackPoint.Y - _y;
		double num2 = trackPoint.X - _x;
		_coordinates.latitude = trackPoint.Coordinates.latitude + ky * num;
		_coordinates.longitude = trackPoint.Coordinates.longitude - kx * num2;
		return this;
	}

	public Point ToPoint()
	{
		return new Point(Round(_x), Round(_y));
	}

	private static int Round(double value)
	{
		if (value >= 0.0)
		{
			return (int)(value + 0.5);
		}
		return (int)(value - 0.5);
	}

	public PointF ToPointF()
	{
		return new PointF((float)_x, (float)_y);
	}

	public void Move(AreaPoint p)
	{
		_x += p.X;
		_y = p.Y - _y;
	}

	public void SetScale(double scaleFactor)
	{
		if (!scaleFactor.Equals(0.0))
		{
			_x *= scaleFactor;
			_y *= scaleFactor;
		}
	}

	public double GetDistance(AreaPoint p)
	{
		return Math.Sqrt((p.X - _x) * (p.X - _x) + (p.Y - _y) * (p.Y - _y));
	}

	public bool Equals(AreaPoint other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (_x.Equals(other._x) && _y.Equals(other._y))
		{
			return _coordinates.Equals(other._coordinates);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((AreaPoint)obj);
	}

	public override int GetHashCode()
	{
		return (((_x.GetHashCode() * 397) ^ _y.GetHashCode()) * 397) ^ _coordinates.GetHashCode();
	}

	public static explicit operator Coordinates(AreaPoint areaPoint)
	{
		return areaPoint.Coordinates;
	}

	public override string ToString()
	{
		return "x: " + X + ", y: " + Y;
	}
}
