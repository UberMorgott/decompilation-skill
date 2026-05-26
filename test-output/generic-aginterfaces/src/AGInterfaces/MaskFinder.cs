using System.Collections.Generic;

namespace AGInterfaces;

public class MaskFinder
{
	private enum TypeOperation
	{
		OneSymbol,
		ManySymbol,
		Wait
	}

	private class OperationParam
	{
		internal int index;

		internal TypeOperation typeOperation;

		internal string waitStr;

		internal OperationParam(int index, TypeOperation typeOperation)
		{
			this.index = index;
			this.typeOperation = typeOperation;
			waitStr = string.Empty;
		}

		internal OperationParam(int index, TypeOperation typeOperation, string waitStr)
		{
			this.index = index;
			this.typeOperation = typeOperation;
			this.waitStr = waitStr;
		}
	}

	public string Mask { get; }

	public MaskFinder(string mask)
	{
		Mask = mask;
	}

	public bool FindMask(string inputstr)
	{
		if (string.IsNullOrEmpty(Mask) || string.IsNullOrEmpty(inputstr))
		{
			return false;
		}
		List<OperationParam> list = new List<OperationParam>();
		for (int num = Mask.IndexOf("?"); num != -1; num = Mask.IndexOf("?", num + 1))
		{
			list.Add(new OperationParam(num, TypeOperation.OneSymbol));
		}
		for (int num = Mask.IndexOf("*"); num != -1; num = Mask.IndexOf("*", num + 1))
		{
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (num < list[i].index)
					{
						list.Insert(i, new OperationParam(num, TypeOperation.ManySymbol));
						break;
					}
					if (i == list.Count - 1)
					{
						list.Add(new OperationParam(num, TypeOperation.ManySymbol));
						break;
					}
				}
			}
			else
			{
				list.Add(new OperationParam(num, TypeOperation.ManySymbol));
			}
		}
		if (list.Count > 0)
		{
			if (list[0].index > 0)
			{
				int index = list[0].index;
				string waitStr = Mask.Substring(0, index);
				list.Insert(0, new OperationParam(0, TypeOperation.Wait, waitStr));
			}
		}
		else
		{
			list.Add(new OperationParam(0, TypeOperation.Wait, Mask));
		}
		for (int num = 0; num < list.Count; num++)
		{
			OperationParam operationParam = list[num];
			if (operationParam.typeOperation != TypeOperation.Wait)
			{
				int num2 = operationParam.index + 1;
				int num3 = ((num < list.Count - 1) ? list[num + 1].index : Mask.Length);
				string waitStr2 = Mask.Substring(num2, num3 - num2);
				operationParam.waitStr = waitStr2;
			}
		}
		int num4 = 0;
		bool flag = false;
		bool flag2 = false;
		for (int num = 0; num < list.Count; num++)
		{
			OperationParam operationParam2 = list[num];
			if (operationParam2.typeOperation == TypeOperation.Wait)
			{
				if (inputstr.IndexOf(operationParam2.waitStr, num4) != num4)
				{
					num4 = -1;
					break;
				}
				num4 += operationParam2.waitStr.Length;
				flag2 = false;
			}
			else if (operationParam2.typeOperation == TypeOperation.OneSymbol)
			{
				if (num4 + 1 > inputstr.Length)
				{
					num4 = -1;
					break;
				}
				if (operationParam2.waitStr.Length > 0)
				{
					int num5 = inputstr.IndexOf(operationParam2.waitStr, num4 + 1);
					if (flag2 && num5 > num4)
					{
						num4 = num5 - 1;
					}
					if (num5 - num4 != 1)
					{
						num4 = -1;
						break;
					}
					num4 += operationParam2.waitStr.Length + 1;
					flag2 = false;
				}
				else
				{
					num4++;
				}
			}
			else
			{
				if (operationParam2.typeOperation != TypeOperation.ManySymbol)
				{
					continue;
				}
				if (operationParam2.waitStr.Length > 0)
				{
					int num6 = inputstr.IndexOf(operationParam2.waitStr, num4);
					if (num6 == -1)
					{
						num4 = -1;
						break;
					}
					num4 = num6 + operationParam2.waitStr.Length;
					flag2 = false;
				}
				else
				{
					flag = true;
					flag2 = true;
				}
			}
		}
		if (num4 != -1 && num4 < inputstr.Length && flag)
		{
			num4 = inputstr.Length;
		}
		return num4 == inputstr.Length;
	}
}
