using System;

namespace AGInterfaces;

public delegate MainImageProps GetImageProps(ElementType element = ElementType.Module, Guid guid = default(Guid));
