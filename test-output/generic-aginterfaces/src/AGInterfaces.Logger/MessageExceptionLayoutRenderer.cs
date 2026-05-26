using System;
using System.Text;
using NLog;
using NLog.LayoutRenderers;

namespace AGInterfaces.Logger;

[LayoutRenderer("messageEx")]
public class MessageExceptionLayoutRenderer : LayoutRenderer
{
	protected override void Append(StringBuilder builder, LogEventInfo logEvent)
	{
		builder.Append(logEvent.FormattedMessage);
		if (logEvent.Exception != null)
		{
			builder.Append(Environment.NewLine);
			builder.Append(logEvent.Exception.ToString());
		}
	}
}
