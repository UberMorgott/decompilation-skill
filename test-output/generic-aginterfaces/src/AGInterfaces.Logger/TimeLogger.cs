using System.Diagnostics;
using NLog;

namespace AGInterfaces.Logger;

public class TimeLogger : DecoratorLoggerBase
{
	private const string MessageFormat = "{0}, +{1} ms";

	private readonly Stopwatch _stopwatch;

	public TimeLogger(ILogger logger)
		: base(logger)
	{
		_stopwatch = Stopwatch.StartNew();
	}

	protected override string ModifyMessage(string message)
	{
		return $"{message}, +{_stopwatch.ElapsedMilliseconds} ms";
	}

	public void Restart()
	{
		_stopwatch.Restart();
	}
}
