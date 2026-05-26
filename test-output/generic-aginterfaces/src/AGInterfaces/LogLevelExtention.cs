namespace AGInterfaces;

public static class LogLevelExtention
{
	public static AGLogLevel GetLogLevelFromString(string logLevel)
	{
		return logLevel switch
		{
			"Trace" => AGLogLevel.Trace, 
			"Debug" => AGLogLevel.Debug, 
			"Info" => AGLogLevel.Info, 
			"Warn" => AGLogLevel.Warn, 
			"Error" => AGLogLevel.Error, 
			"Fatal" => AGLogLevel.Fatal, 
			_ => AGLogLevel.Off, 
		};
	}
}
