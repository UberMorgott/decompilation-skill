using System.Collections.Generic;

namespace AGInterfaces;

public interface ISettingsValidator
{
	string ValidatorName { get; }

	IEnumerable<SettingsValidatorMessage> Validate(IAutoGRAPHShell shellProvider, ElementType type, GroupOrElementInfo element);
}
