using System;
using System.Reflection;

namespace AGInterfaces;

[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
public abstract class BaseOperator
{
	public abstract void set(object val, int index);

	public abstract void setFrom(Array arraySrc, int indexSrc, int indexDst);

	public abstract object get(int index);
}
