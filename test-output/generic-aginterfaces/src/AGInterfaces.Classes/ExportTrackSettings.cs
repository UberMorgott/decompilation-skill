namespace AGInterfaces.Classes;

public class ExportTrackSettings
{
	public static int[] levels = new int[20]
	{
		14800000, 5700000, 2200000, 1100000, 500000, 250000, 125000, 62500, 30720, 15360,
		7680, 3840, 1920, 960, 480, 240, 120, 60, 30, 15
	};

	public bool NeedMaps { get; set; }

	public string MapFileName { get; set; }

	public bool ShowDirectionTrack { get; set; }

	public bool DrawGeoFences { get; set; }

	public int TrackPictureWidth { get; set; }

	public int TrackPictureHeight { get; set; }

	public int StatusPictureWidth { get; set; }

	public int StatusPictureHeight { get; set; }

	public int StatusLevelNum { get; set; }

	public double StatusWidthMeter => (double)(StatusPictureWidth * levels[StatusLevelNum]) / 100.0;

	public ExportTrackSettings()
	{
		ShowDirectionTrack = true;
		TrackPictureWidth = 200;
		TrackPictureHeight = 200;
		StatusPictureWidth = 200;
		StatusPictureHeight = 200;
		StatusLevelNum = 14;
	}
}
