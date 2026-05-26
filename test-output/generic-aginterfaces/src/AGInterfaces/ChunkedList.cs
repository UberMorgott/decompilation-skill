using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace AGInterfaces;

public class ChunkedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
{
	private class Enumerator : IEnumerator<T>, IEnumerator, IDisposable
	{
		private readonly ChunkedList<T> _list;

		private readonly int _version;

		private int _index;

		private int _chunkIndex;

		private int _internalIndex;

		private T _current;

		object IEnumerator.Current
		{
			get
			{
				if (_index == 0 || _index == int.MaxValue)
				{
					throw new InvalidOperationException();
				}
				return Current;
			}
		}

		public T Current => _current;

		public Enumerator(ChunkedList<T> list)
		{
			_list = list;
			_version = _list._version;
			_index = 0;
			_chunkIndex = 0;
			_internalIndex = 0;
			_current = default(T);
		}

		public bool MoveNext()
		{
			if (_version == _list._version && (uint)_index < (uint)_list._count)
			{
				_current = _list._chunks[_chunkIndex][_internalIndex];
				_index++;
				_internalIndex++;
				if (_internalIndex >= _list._chunkSize)
				{
					_chunkIndex++;
					_internalIndex = 0;
				}
				return true;
			}
			if (_version != _list._version)
			{
				throw new InvalidOperationException("Invalid version: Collection has been changed.");
			}
			_index = int.MaxValue;
			_current = default(T);
			return false;
		}

		public void Reset()
		{
			if (_version != _list._version)
			{
				throw new InvalidOperationException("Invalid version: Collection has been changed.");
			}
			_index = 0;
			_chunkIndex = 0;
			_internalIndex = 0;
			_current = default(T);
		}

		public void Dispose()
		{
		}
	}

	private const int MaxEmptyChunkCount = 2;

	private readonly int _powerCapacity;

	private readonly int _chunkSize;

	private readonly IList<IList<T>> _chunks = new List<IList<T>>();

	private int _count;

	private int _capacity;

	private int _version;

	public int Count => _count;

	public bool IsReadOnly => false;

	public T this[int index]
	{
		get
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			GetIndexes(index, out var chunkIndex, out var internalIndex);
			return _chunks[chunkIndex][internalIndex];
		}
		set
		{
			if ((uint)index >= (uint)_count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			GetIndexes(index, out var chunkIndex, out var internalIndex);
			_chunks[chunkIndex][internalIndex] = value;
			_version++;
		}
	}

	public ChunkedList()
		: this(GetMaxPowerCapacity())
	{
	}

	public ChunkedList(int powerCapacity)
	{
		if (powerCapacity < 0)
		{
			throw new ArgumentOutOfRangeException("powerCapacity");
		}
		_powerCapacity = powerCapacity;
		_chunkSize = 1 << _powerCapacity;
	}

	public static int GetMaxPowerCapacity()
	{
		int sampleSize = GetSampleSize();
		int num = 84900 / sampleSize;
		int num2 = 0;
		while (num > 1)
		{
			num2++;
			num >>= 1;
		}
		return num2;
	}

	private static int GetSampleSize()
	{
		Type typeFromHandle = typeof(T);
		if (!typeFromHandle.IsValueType)
		{
			return IntPtr.Size;
		}
		if (!typeFromHandle.IsGenericType)
		{
			return Marshal.SizeOf(typeFromHandle);
		}
		return Marshal.SizeOf(default(T));
	}

	private void GetIndexes(int index, out int chunkIndex, out int internalIndex)
	{
		chunkIndex = index >> _powerCapacity;
		internalIndex = index & (_chunkSize - 1);
	}

	private int GetIndex(int chunkIndex, int internalIndex)
	{
		return (chunkIndex << _powerCapacity) | internalIndex;
	}

	private void ShrinkChunks()
	{
		for (int num = _chunks.Count - 1 - 2; num >= 0; num--)
		{
			if (_chunks[num].Count == 0)
			{
				RemoveChunk(_chunks.Count - 1);
			}
		}
	}

	private List<T> AddNewChunk()
	{
		List<T> list = new List<T>(_chunkSize);
		_chunks.Add(list);
		_capacity += _chunkSize;
		return list;
	}

	private void RemoveChunk(int index)
	{
		_chunks.RemoveAt(index);
		_capacity -= _chunkSize;
	}

	public void Add(T item)
	{
		GetIndexes(_count, out var chunkIndex, out var _);
		if (chunkIndex <= _chunks.Count - 1)
		{
			_chunks[chunkIndex].Add(item);
		}
		else
		{
			AddNewChunk().Add(item);
		}
		_count++;
		_version++;
	}

	public void Clear()
	{
		if (_count > 0)
		{
			foreach (IList<T> chunk in _chunks)
			{
				chunk.Clear();
			}
			_chunks.Clear();
		}
		_capacity = 0;
		_version++;
	}

	public bool Contains(T item)
	{
		foreach (IList<T> chunk in _chunks)
		{
			if (chunk.Contains(item))
			{
				return true;
			}
		}
		return false;
	}

	public int IndexOf(T item)
	{
		for (int i = 0; i < _chunks.Count; i++)
		{
			int num = _chunks[i].IndexOf(item);
			if (num >= 0)
			{
				return GetIndex(i, num);
			}
		}
		return -1;
	}

	public void CopyTo(T[] array, int arrayIndex)
	{
		for (int i = 0; i < _chunks.Count; i++)
		{
			int arrayIndex2 = arrayIndex + (i << _powerCapacity);
			_chunks[i].CopyTo(array, arrayIndex2);
		}
	}

	public void Insert(int index, T item)
	{
		if ((uint)index > (uint)_count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		if (index == _count)
		{
			Add(item);
			return;
		}
		GetIndexes(index, out var chunkIndex, out var internalIndex);
		GetIndexes(_count - 1, out var chunkIndex2, out var _);
		if (_chunks[chunkIndex2].Count == _chunkSize)
		{
			if (_count >= _capacity)
			{
				AddNewChunk();
			}
			chunkIndex2++;
		}
		for (int num = chunkIndex2; num > chunkIndex; num--)
		{
			_chunks[num].Insert(0, _chunks[num - 1][_chunkSize - 1]);
			_chunks[num - 1].RemoveAt(_chunkSize - 1);
		}
		_chunks[chunkIndex].Insert(internalIndex, item);
		_count++;
		_version++;
	}

	public bool Remove(T item)
	{
		int num = IndexOf(item);
		if (num >= 0)
		{
			RemoveAt(num);
			return true;
		}
		return false;
	}

	public void RemoveAt(int index)
	{
		if ((uint)index >= (uint)_count)
		{
			throw new ArgumentOutOfRangeException("index");
		}
		GetIndexes(index, out var chunkIndex, out var internalIndex);
		_chunks[chunkIndex].RemoveAt(internalIndex);
		GetIndexes(_count - 1, out var chunkIndex2, out var _);
		for (int i = chunkIndex + 1; i <= chunkIndex2; i++)
		{
			_chunks[i - 1].Add(_chunks[i][0]);
			_chunks[i].RemoveAt(0);
		}
		_count--;
		_version++;
		ShrinkChunks();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerator<T> GetEnumerator()
	{
		return new Enumerator(this);
	}
}
