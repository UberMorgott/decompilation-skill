namespace AGInterfaces;

public enum SetSettingsResult
{
	OK,
	InvalidSettingsType,
	SettingsArrayIsNullOrEmpty,
	ElementNodeNotFound,
	SettingsEquals,
	InvalidNodeType,
	CantDeleteRootSettings,
	ModuleSettingsIsNullOrEmpty,
	ModuleNotFound
}
