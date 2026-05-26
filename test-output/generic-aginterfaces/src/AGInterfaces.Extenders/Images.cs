using System.Collections.Generic;
using System.Drawing;

namespace AGInterfaces.Extenders;

public class Images
{
	private readonly List<Image> _ImagesList = new List<Image>();

	private readonly Dictionary<string, int> _ImagesDict = new Dictionary<string, int>();

	public int Count => _ImagesList.Count;

	public Image this[string key]
	{
		get
		{
			if (key == null || !TryGetValue(key, out var image))
			{
				return null;
			}
			return image;
		}
		set
		{
			lock (_ImagesDict)
			{
				int num = IndexOf(key);
				if (num >= 0)
				{
					_ImagesList[num] = value;
				}
				else
				{
					Add(key, value);
				}
			}
		}
	}

	public Image this[int index] => _ImagesList[index];

	public bool TryGetValue(string key, out Image image)
	{
		int num = -1;
		if (key != null)
		{
			lock (_ImagesDict)
			{
				if (_ImagesDict.TryGetValue(key, out var value))
				{
					num = value;
				}
			}
		}
		if (num >= 0)
		{
			image = _ImagesList[num];
			return true;
		}
		image = null;
		return false;
	}

	public void Add(string key, Image img)
	{
		lock (_ImagesDict)
		{
			if (key != null)
			{
				_ImagesDict.Add(key, _ImagesList.Count);
			}
			_ImagesList.Add(img);
		}
	}

	public void Add(Image img)
	{
		lock (_ImagesDict)
		{
			_ImagesList.Add(img);
		}
	}

	public string KeyOf(int index)
	{
		foreach (KeyValuePair<string, int> item in _ImagesDict)
		{
			if (item.Value == index)
			{
				return item.Key;
			}
		}
		return null;
	}

	public int IndexOf(string key)
	{
		int value = -1;
		if (key != null)
		{
			lock (_ImagesDict)
			{
				_ImagesDict.TryGetValue(key, out value);
			}
		}
		return value;
	}
}
