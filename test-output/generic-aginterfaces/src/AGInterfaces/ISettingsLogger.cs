using System;
using System.Collections.Generic;

namespace AGInterfaces;

public interface ISettingsLogger
{
	IEnumerable<DateTime> GetSettingsChangesTime(Guid elementGUID, string settingsType, string moduleName);

	object GetSettingsData(Guid elementGUID, string settingsType, string moduleName, DateTime changesTime);
}
