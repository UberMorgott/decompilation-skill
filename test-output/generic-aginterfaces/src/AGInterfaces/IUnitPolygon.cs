using System.Collections.Generic;
using System.Drawing;

namespace AGInterfaces;

public interface IUnitPolygon
{
	List<AreaPoint> AreaPoints { get; }

	Coordinates[] PointsCoordinates { get; }

	Color FillColor { get; set; }
}
