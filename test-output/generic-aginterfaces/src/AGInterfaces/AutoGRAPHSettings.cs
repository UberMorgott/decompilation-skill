using MsgPack.Serialization;

namespace AGInterfaces;

public class AutoGRAPHSettings
{
	[MessagePackRuntimeType]
	public IAutoGRAPHSettings Settings { get; set; }

	public AutoGRAPHSettings()
	{
	}

	public AutoGRAPHSettings(IAutoGRAPHSettings settings)
	{
		Settings = settings;
	}
}
