using System;
using System.Reflection;
using MsgPack.Serialization;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public class LookUpEditItemEx : LookUpEditItem
{
	public Guid ID { get; set; }

	public string Description { get; set; }

	public bool IsBase { get; set; }

	public bool SupportCompare { get; set; }

	public Guid ParentID { get; set; }

	[MessagePackRuntimeType]
	public object data { get; set; }

	public LookUpEditItemEx(Guid id, object value, string display, string description = null)
		: base(value, display)
	{
		ID = id;
		Description = description;
	}
}
