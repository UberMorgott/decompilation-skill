using System;

namespace AGInterfaces;

public delegate void GetElementProps(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty[] props, DateTime dt = default(DateTime));
