using System;

namespace AGInterfaces;

public delegate GroupOrElementInfo GetElementNodeInfo(ElementType element = ElementType.Module, Guid guid = default(Guid));
