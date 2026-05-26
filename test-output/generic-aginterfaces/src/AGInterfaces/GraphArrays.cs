using System;

namespace AGInterfaces;

public sealed class GraphArrays
{
	public TimeZoneInfo timeZoneInfo;

	public DateTime[] udt;

	public Array ordinate;

	public Array absciss;

	public int[] statuses;

	public ValueUpdateType[] updates;

	public string[] captions;

	public GraphArrays(TimeZoneInfo timeZoneInfo, DateTime[] udt, Array ordinate, Array absciss, int[] statuses = null, ValueUpdateType[] updates = null, string[] captions = null)
	{
		this.timeZoneInfo = timeZoneInfo;
		this.udt = udt;
		this.ordinate = ordinate;
		this.absciss = absciss;
		this.statuses = statuses;
		this.updates = updates;
		this.captions = captions;
	}
}
