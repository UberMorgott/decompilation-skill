using System.Collections.Generic;
using System.Linq;

namespace AGInterfaces;

public sealed class AGBandInfo
{
	public string name;

	public string caption;

	public List<AGBandInfo> children = new List<AGBandInfo>();

	public List<AGColumnInfo> columns = new List<AGColumnInfo>();

	public float width
	{
		get
		{
			if (!columns.Any())
			{
				return children.Sum((AGBandInfo b) => b.width);
			}
			return columns.Max((AGColumnInfo c) => c.width);
		}
	}

	public int rowCount
	{
		get
		{
			if (!columns.Any())
			{
				if (!children.Any())
				{
					return 1;
				}
				return children.Max((AGBandInfo c) => c.rowCount);
			}
			return columns.Max((AGColumnInfo c) => c.rowCount);
		}
	}

	public AGBandInfo(string name, string caption)
	{
		this.name = name;
		this.caption = caption;
	}
}
