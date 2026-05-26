namespace AGInterfaces;

public interface IImageReportAdapter
{
	object GetDeviceIcon(string iconName, int imageHue);

	object GetStatusIcon(string[] iconName);
}
