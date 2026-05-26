using System;

namespace AGInterfaces;

public interface ILoggerProvider
{
	void LogDebug(string message, Exception ex, params object[] args);

	void LogDebug(string message, params object[] args);

	void LogError(string message, Exception ex, params object[] args);

	void LogError(string message, params object[] args);

	void LogFatal(string message, Exception ex, params object[] args);

	void LogFatal(string message, params object[] args);

	void LogInfo(string message, Exception ex, params object[] args);

	void LogInfo(string message, params object[] args);

	void LogTrace(string message, Exception ex, params object[] args);

	void LogTrace(string message, params object[] args);

	void LogWarn(string message, Exception ex, params object[] args);

	void LogWarn(string message, params object[] args);
}
