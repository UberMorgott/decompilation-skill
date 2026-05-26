using System;

namespace AGInterfaces;

public interface IMonitorMessageReceiver
{
	void RuleExecuted(GroupOrElementInfo device, Guid ruleID, MonitorMessageInfo messageInfo);
}
