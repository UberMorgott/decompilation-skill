using System;
using System.Reflection;

namespace AGInterfaces;

[Obfuscation(Exclude = true, ApplyToMembers = true, Feature = "control flow")]
public class ArrayOperator<T> : BaseOperator
{
	private readonly T[] arr;

	public ArrayOperator(Array arr)
	{
		this.arr = (T[])arr;
	}

	public override void set(object val, int index)
	{
		arr[index] = (T)val;
	}

	public override void setFrom(Array arraySrc, int indexSrc, int indexDst)
	{
		arr[indexDst] = ((T[])arraySrc)[indexSrc];
	}

	public override object get(int index)
	{
		return arr[index];
	}
}
