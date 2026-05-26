namespace AGInterfaces;

public class SettingsValidateResult
{
	public readonly bool needRecompile;

	public readonly IAutoGRAPHSettings[] settingsArray;

	public SettingsValidateResult(bool needRecompile, object settingsArray)
	{
		this.needRecompile = needRecompile;
		this.settingsArray = settingsArray as IAutoGRAPHSettings[];
	}

	public SettingsValidateResult(bool needRecompile, IAutoGRAPHSettings[] settingsArray)
	{
		this.needRecompile = needRecompile;
		this.settingsArray = settingsArray;
	}
}
