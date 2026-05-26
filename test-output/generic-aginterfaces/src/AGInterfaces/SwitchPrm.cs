using System;

namespace AGInterfaces;

[Serializable]
public struct SwitchPrm(object value, int count, int status)
{
	public object value = value;

	public int count = count;

	public int status = status;
}
