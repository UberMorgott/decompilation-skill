using System;
using System.Collections.Generic;

namespace AGInterfaces.Classes;

public interface IElements
{
	Dictionary<Guid, GroupOrElementInfo> ElementGuids { get; }

	Dictionary<Guid, PropertyTable> ByGuid { get; }

	Dictionary<string, PropertyTable> ByString { get; }

	Dictionary<long, PropertyTable> ById { get; }

	bool TryGetElement(Guid id, out GroupOrElementInfo element);

	bool TryGetTable(Guid id, out PropertyTable propertyTable);

	bool TryGetTable(string id, out PropertyTable propertyTable);

	bool TryGetTable(long id, out PropertyTable propertyTable);
}
