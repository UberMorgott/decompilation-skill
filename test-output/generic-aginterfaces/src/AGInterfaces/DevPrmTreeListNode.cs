namespace AGInterfaces;

public sealed class DevPrmTreeListNode
{
	public readonly AddValueType addValueType;

	private readonly string nameAdd;

	public DevPrmsGroupInfo grpOrPrmInfo;

	public string caption => grpOrPrmInfo.caption + nameAdd;

	public string simpleCaption => grpOrPrmInfo?.caption;

	public bool IsChecked { get; set; }

	public bool IsGroup => !(grpOrPrmInfo is DevPrmInfo);

	public bool ByAlias
	{
		get
		{
			if (grpOrPrmInfo is DevPrmInfo)
			{
				return !string.IsNullOrEmpty((grpOrPrmInfo as DevPrmInfo).alias);
			}
			return false;
		}
	}

	public string name
	{
		get
		{
			if (!(grpOrPrmInfo is DevPrmInfo))
			{
				return null;
			}
			DevPrmInfo devPrmInfo = grpOrPrmInfo as DevPrmInfo;
			if (!ByAlias)
			{
				return devPrmInfo.name + nameAdd;
			}
			return "[" + devPrmInfo.alias + "]" + nameAdd;
		}
	}

	public string simpleName
	{
		get
		{
			if (!(grpOrPrmInfo is DevPrmInfo))
			{
				return null;
			}
			DevPrmInfo devPrmInfo = grpOrPrmInfo as DevPrmInfo;
			if (!ByAlias)
			{
				return devPrmInfo.name;
			}
			return "[" + devPrmInfo.alias + "]";
		}
	}

	public int ID { get; set; }

	public int ParentID { get; set; }

	public DevPrmTreeListNode()
	{
	}

	public DevPrmTreeListNode(DevPrmsGroupInfo grpOrPrmInfo, AddValueType addValueType, string nameAdd, int ID, int ParentID)
	{
		this.grpOrPrmInfo = grpOrPrmInfo;
		this.addValueType = addValueType;
		this.nameAdd = nameAdd;
		this.ID = ID;
		this.ParentID = ParentID;
	}
}
