using System.Linq;

namespace AGInterfaces.Extenders;

public static class Helpers
{
	public static T To<T>(this object settings) where T : IAutoGRAPHSettings
	{
		if (settings == null)
		{
			return default(T);
		}
		if (settings is T)
		{
			return (T)settings;
		}
		if (settings is IAutoGRAPHSettings[])
		{
			return ((IAutoGRAPHSettings[])settings).OfType<T>().FirstOrDefault();
		}
		return default(T);
	}

	public static object To(this object settings, string fullClassName)
	{
		if (settings == null)
		{
			return null;
		}
		if (settings.GetType().FullName == fullClassName)
		{
			return settings;
		}
		if (settings is IAutoGRAPHSettings[])
		{
			return ((IAutoGRAPHSettings[])settings).FirstOrDefault((IAutoGRAPHSettings p) => p != null && p.GetType().FullName == fullClassName);
		}
		return null;
	}
}
