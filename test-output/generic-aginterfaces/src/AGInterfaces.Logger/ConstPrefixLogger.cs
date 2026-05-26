using System;
using NLog;

namespace AGInterfaces.Logger;

public class ConstPrefixLogger : DecoratorLoggerBase, IDisposable
{
	private const string MessageFormat = "{0}: {1}";

	private readonly string _prefix;

	public ConstPrefixLogger(ILogger logger, string prefix)
		: base(logger)
	{
		_prefix = prefix ?? throw new ArgumentNullException("prefix");
	}

	protected override string ModifyMessage(string message)
	{
		return $"{_prefix}: {message}";
	}

	public void Dispose()
	{
	}
}
