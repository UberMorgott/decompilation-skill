using System;

namespace AGInterfaces;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class AGInfoAttribute : Attribute
{
	private readonly string title;

	private readonly string description;

	private readonly string units;

	private readonly DeviceParameterKind kind;

	private readonly AGInfoGroupType type;

	private readonly bool prepared;

	private readonly int[] entriesID;

	public string Title => title;

	public string Description => description;

	public string Units => units;

	public DeviceParameterKind Kind => kind;

	public AGInfoGroupType Type => type;

	public bool Prepared => prepared;

	public int[] EntriesID => entriesID;

	public AGInfoAttribute(string title, string description, string units, DeviceParameterKind kind, AGInfoGroupType type, bool prepared, int[] entriesID = null)
	{
		this.title = title;
		this.description = description;
		this.units = units;
		this.kind = kind;
		this.type = type;
		this.prepared = prepared;
		this.entriesID = entriesID;
	}

	public AGInfoAttribute(string title, string description, DeviceParameterKind kind, AGInfoGroupType type, bool prepared, int[] entriesID = null)
		: this(title, description, null, kind, type, prepared, entriesID)
	{
	}

	[Obsolete]
	public AGInfoAttribute(string title, string description, DeviceParameterKind kind, AGInfoGroupType type)
		: this(title, description, null, kind, type, prepared: false)
	{
	}
}
