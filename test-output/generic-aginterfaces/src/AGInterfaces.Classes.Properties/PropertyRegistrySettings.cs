using System.Collections.Generic;

namespace AGInterfaces.Classes.Properties;

public class PropertyRegistrySettings
{
	public string PropertyName { get; set; }

	public StoreMultiCaption Description { get; set; }

	public bool AllowChangingFromMainForm { get; set; }

	public HashSet<PropType> PropertyTypes { get; set; }

	public HashSet<ElementType> ElementTypes { get; set; }

	public PropertyRegistrySettings()
	{
		PropertyTypes = new HashSet<PropType>();
		ElementTypes = new HashSet<ElementType>();
	}

	public PropertyRegistrySettings(string name)
		: this()
	{
		PropertyName = name;
	}

	public PropertyRegistrySettings(string propertyName, StoreMultiCaption description, bool allowChangingFromMainForm, HashSet<PropType> propertyTypes, HashSet<ElementType> elementTypes)
		: this()
	{
		PropertyName = propertyName;
		Description = description;
		AllowChangingFromMainForm = allowChangingFromMainForm;
		PropertyTypes = propertyTypes;
		ElementTypes = elementTypes;
	}

	public string GetCaptionOrPropertName(string currentLanguage)
	{
		return Description?.get(PropertyName, currentLanguage) ?? PropertyName;
	}

	public string GetCaption(string currentLanguage)
	{
		return Description?.get(PropertyName, currentLanguage) ?? "";
	}

	public void SetCaption(string value, string currentLanguage)
	{
		if (Description == null)
		{
			Description = new StoreMultiCaption();
		}
		Description.set(PropertyName, value, currentLanguage);
	}
}
