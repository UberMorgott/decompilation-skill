using System.Collections.Generic;

namespace AGInterfaces;

public sealed class ImagingSettings
{
	public ElementImageProps[] backgroundImages;

	public ElementImageProps[] foregroundImages;

	public ImagingSettings(ElementImageProps[] backgroundImages, ElementImageProps[] foregroundImages)
	{
		this.backgroundImages = backgroundImages;
		this.foregroundImages = foregroundImages;
	}

	public HashSet<string> GetUsedPrmsName()
	{
		HashSet<string> hashSet = new HashSet<string>();
		if (backgroundImages != null)
		{
			ElementImageProps[] array = backgroundImages;
			foreach (ElementImageProps elementImageProps in array)
			{
				if (!string.IsNullOrEmpty(elementImageProps?.parameter))
				{
					hashSet.Add(elementImageProps.parameter);
				}
			}
		}
		if (foregroundImages != null)
		{
			ElementImageProps[] array = foregroundImages;
			foreach (ElementImageProps elementImageProps2 in array)
			{
				if (!string.IsNullOrEmpty(elementImageProps2?.parameter))
				{
					hashSet.Add(elementImageProps2.parameter);
				}
			}
		}
		return hashSet;
	}
}
