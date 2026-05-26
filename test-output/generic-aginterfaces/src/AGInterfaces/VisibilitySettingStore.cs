using System.Collections.Generic;

namespace AGInterfaces;

public sealed class VisibilitySettingStore
{
	private VisibilityInfo[] arrayVisibilitySetting;

	private int MaxItem = 15;

	public VisibilitySettingStore(int MaxItem)
	{
		this.MaxItem = MaxItem;
	}

	public void Merge(List<VisibilityInfo> listPointsTrackInfo)
	{
		if (arrayVisibilitySetting != null && listPointsTrackInfo != null)
		{
			VisibilityInfo[] array = listPointsTrackInfo.ToArray();
			for (int i = 0; i < arrayVisibilitySetting.Length; i++)
			{
				VisibilityInfo visibilityInfo = arrayVisibilitySetting[i];
				int j;
				for (j = 0; j < array.Length; j++)
				{
					if (array[j].name == visibilityInfo.name)
					{
						array[j].check = visibilityInfo.check;
						break;
					}
				}
				if (j == array.Length && listPointsTrackInfo.Count < MaxItem)
				{
					if (listPointsTrackInfo.Count == array.Length)
					{
						visibilityInfo.begingroup = true;
					}
					else
					{
						visibilityInfo.begingroup = false;
					}
					visibilityInfo.enabled = false;
					listPointsTrackInfo.Add(visibilityInfo);
				}
			}
		}
		arrayVisibilitySetting = ((listPointsTrackInfo != null && listPointsTrackInfo.Count > 0) ? listPointsTrackInfo.ToArray() : null);
	}

	public VisibilityInfo[] GetVisibilitySettingStore()
	{
		return arrayVisibilitySetting;
	}

	public void SetVisibilitySettingStore(VisibilityInfo[] arrayPointsTrackInfo)
	{
		arrayVisibilitySetting = arrayPointsTrackInfo;
	}
}
