using System.Collections.Generic;

namespace AGInterfaces;

public interface ISettingsValidators
{
	IEnumerable<ISettingsValidator> GetValidators(ElementType elementType, SettingsValidatorParams validatorPrm);
}
