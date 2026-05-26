using System;
using System.Collections.Generic;
using AGInterfaces.Classes.Properties;

namespace AGInterfaces.ModuleProviders;

public interface IPropertiesRegistryProvider
{
	void GetProps(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty property, DateTime dt = default(DateTime));

	void GetProps(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty[] properties, DateTime dt = default(DateTime));

	void GetPropsTable(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty property, bool getSinglePropsNotAsTable = false);

	void GetPropsTable(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty[] properties, bool getSinglePropsNotAsTable = false);

	Dictionary<string, PropType> GetPropTypes(ElementType element, Guid guid, bool allInheritedProps);

	Dictionary<string, object> GetPropValues(ElementType element, Guid guid, bool allInheritedProps, DateTime dt = default(DateTime));

	Dictionary<string, object> GetPropValueTables(ElementType element, Guid guid, bool allInheritedProps);

	void MovePropsTableToDirectory(ElementType element, bool allInheritedProps, Dictionary<Guid, RegistryProperty[]> elemPropDict, string directoryName);

	void MovePropsTableToDirectory(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty[] properties, string directoryName);

	void MovePropsToDirectory(ElementType element, bool allInheritedProps, Dictionary<Guid, RegistryProperty[]> elemPropDict, string directoryName);

	void MovePropsToDirectory(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty[] properties, string directoryName);

	void MovePropTableToDirectory(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty property, string directoryName);

	void MovePropToDirectory(ElementType element, Guid guid, bool allInheritedProps, RegistryProperty property, string directoryName);

	void SetProps(ElementType element, Dictionary<Guid, RegistryProperty[]> elemPropDict, bool replaceTableToItem = false, DateTime dt = default(DateTime));

	void SetProps(ElementType element, Guid guid, RegistryProperty property, bool replaceTableToItem = false, DateTime dt = default(DateTime));

	void SetProps(ElementType element, Guid guid, RegistryProperty[] properties, bool replaceTableToItem = false, DateTime dt = default(DateTime));

	void SetPropsTable(ElementType element, Dictionary<Guid, RegistryProperty[]> elemPropDict);

	void SetPropsTable(ElementType element, Guid guid, RegistryProperty property);

	void SetPropsTable(ElementType element, Guid guid, RegistryProperty[] properties);

	Dictionary<string, PropertyRegistrySettings> GetAllPropertiesNamesList();

	List<PropertyRegistrySettings> GetAllowedToChangeListByElementType(IAutoGRAPHShell baseShellProvider, ElementType elementType);

	List<PropertyRegistrySettings> GetAllowedToChangeList(IAutoGRAPHShell baseShellProvider);

	string GetCaptionByName(string propertyName);
}
