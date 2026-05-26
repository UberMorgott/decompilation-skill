using System;
using BINParser;

namespace AGInterfaces;

public sealed class LoaderDraftInfo
{
	[NonSerialized]
	public DraftItems draftItems;

	public DateTime[] udt;

	public DateTime dateTime;

	public DateTime? firstDT;

	public DateTime? lastDT;

	public LoaderDraftInfo(DraftItems draftItems, DateTime dateTime)
	{
		this.draftItems = draftItems;
		this.dateTime = dateTime;
	}
}
