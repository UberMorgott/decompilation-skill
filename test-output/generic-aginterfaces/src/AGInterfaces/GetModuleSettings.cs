using System;

namespace AGInterfaces;

public delegate IAutoGRAPHSettings[] GetModuleSettings(SettingsType type, ElementType element = ElementType.Module, Guid guid = default(Guid), InheritType inherit = InheritType.Yes);
