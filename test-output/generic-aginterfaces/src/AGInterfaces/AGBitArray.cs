using System;
using System.Runtime.CompilerServices;

namespace AGInterfaces;

public sealed class AGBitArray
{
	private const int BitsPerInt32 = 32;

	private const int BytesPerInt32 = 4;

	private const int BitsPerByte = 8;

	private readonly int[] m_array;

	private readonly int m_length;

	public bool this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return (m_array[index >> 5] & (1 << index)) != 0;
		}
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set
		{
			if (value)
			{
				m_array[index >> 5] |= 1 << index;
			}
			else
			{
				m_array[index >> 5] &= ~(1 << index);
			}
		}
	}

	public int Length => m_length;

	public AGBitArray(int length)
	{
		m_array = new int[GetArrayLength(length, 32)];
		m_length = length;
	}

	public AGBitArray(byte[] bytes)
	{
		m_array = new int[GetArrayLength(bytes.Length, 4)];
		m_length = bytes.Length * 8;
		int num = 0;
		int i;
		for (i = 0; bytes.Length - i >= 4; i += 4)
		{
			m_array[num++] = (bytes[i] & 0xFF) | ((bytes[i + 1] & 0xFF) << 8) | ((bytes[i + 2] & 0xFF) << 16) | ((bytes[i + 3] & 0xFF) << 24);
		}
		switch (bytes.Length - i)
		{
		default:
			return;
		case 3:
			m_array[num] = (bytes[i + 2] & 0xFF) << 16;
			goto case 2;
		case 2:
			m_array[num] |= (bytes[i + 1] & 0xFF) << 8;
			break;
		case 1:
			break;
		}
		m_array[num] |= bytes[i] & 0xFF;
	}

	public AGBitArray(AGBitArray bits)
	{
		m_array = new int[bits.m_array.Length];
		m_length = bits.m_length;
		Array.Copy(bits.m_array, m_array, m_array.Length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int GetArrayLength(int n, int div)
	{
		if (n <= 0)
		{
			return 0;
		}
		return (n - 1) / div + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Get(int index)
	{
		return (m_array[index >> 5] & (1 << index)) != 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Set(int index, bool value)
	{
		if (value)
		{
			m_array[index >> 5] |= 1 << index;
		}
		else
		{
			m_array[index >> 5] &= ~(1 << index);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public AGBitArray Or(AGBitArray value)
	{
		for (int i = 0; i < m_array.Length; i++)
		{
			m_array[i] |= value.m_array[i];
		}
		return this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetFalse()
	{
		m_array.AsSpan(0, m_array.Length).Clear();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetTrue()
	{
		m_array.AsSpan(0, m_array.Length).Fill(-1);
	}
}
