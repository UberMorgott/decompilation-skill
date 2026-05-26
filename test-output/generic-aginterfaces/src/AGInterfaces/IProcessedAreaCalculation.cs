using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace AGInterfaces;

public interface IProcessedAreaCalculation
{
	AreaInfo CalculateArea(Guid areaId, DeviceTrack[] deviceTracks, bool areaMode, ImageFormat imageFormat = null, CancellationToken cancellationToken = default(CancellationToken), ReportProgress progress = null);

	Unit[] GetUnits(Guid id);

	void SetUnits(Guid id, Unit[] units);

	string CreateBindingFileText(IList<BindingPoint> bidingPoints, string polygonName, Size size, string filePath);

	bool CheckOutOfMemory();
}
