namespace AGInterfaces;

public interface ILocale
{
	string Locale { get; }

	ILocalizationStorage LocalizationStorage { get; }
}
