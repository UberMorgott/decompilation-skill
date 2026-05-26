namespace AGInterfaces;

public interface IMergeSettings
{
	IAutoGRAPHSettings Merge(IAutoGRAPHSettings serverSettings, IAutoGRAPHSettings syncSettings, IAutoGRAPHSettings localSettings);
}
