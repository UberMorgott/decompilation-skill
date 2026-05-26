namespace AGInterfaces;

public class SettingsRouteVectorMap
{
	public TypeModeRouteVectorMap typeModeRouteVectorMap;

	public bool IsEnabledOneNode;

	public static SettingsRouteVectorMap GetSettingsForNavigationMines()
	{
		return new SettingsRouteVectorMap
		{
			typeModeRouteVectorMap = TypeModeRouteVectorMap.WithoutStartEnd,
			IsEnabledOneNode = true
		};
	}

	public static SettingsRouteVectorMap GetSettingsNormal()
	{
		return new SettingsRouteVectorMap();
	}
}
