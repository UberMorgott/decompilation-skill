using System;

namespace AGInterfaces.Extenders;

public static class ShellExtensionMethods
{
	public static string GetRegistrationNumber(this IAutoGRAPHShell shell, Guid deviceGuid)
	{
		RegistryProperty registryProperty = new RegistryProperty("@Number");
		shell.PropertiesRegistry.GetProps(ElementType.Device, deviceGuid, allInheritedProps: false, new RegistryProperty[1] { registryProperty });
		return registryProperty.value?.ToString();
	}
}
