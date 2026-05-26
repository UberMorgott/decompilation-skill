using System;

namespace AGInterfaces;

public delegate GroupOrElementInfo[] GetElementNodes(ElementType elementType, Guid guid = default(Guid));
