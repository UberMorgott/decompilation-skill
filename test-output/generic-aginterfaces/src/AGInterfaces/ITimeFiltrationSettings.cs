namespace AGInterfaces;

public interface ITimeFiltrationSettings
{
	bool timeFiltrationByFCT { get; set; }

	bool skipTimeUntilCD { get; set; }

	bool experimentalFiltration { get; set; }

	bool Equals(ITimeFiltrationSettings settings);
}
