using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;

namespace AGInterfaces.Classes;

public class ArraysPool
{
	private ArrayPool<bool> BooleanPool;

	private ArrayPool<byte> BytePool;

	private ArrayPool<int> Int32Pool;

	private ArrayPool<long> Int64Pool;

	private ArrayPool<double> DoublePool;

	private ArrayPool<DateTime> DateTimePool;

	private ArrayPool<TimeSpan> TimeSpanPool;

	private ArrayPool<Guid> GuidPool;

	private ArrayPool<Quadro<Guid>> Guid4Pool;

	private ArrayPool<string> StringPool;

	private ArrayPool<Image> ImagePool;

	private ArrayPool<Coordinates> CoordinatesPool;

	private ArrayPool<Location> LocationPool;

	private Dictionary<Array, ReturnType> dict = new Dictionary<Array, ReturnType>();

	public ArraysPool()
	{
		BooleanPool = ArrayPool<bool>.Create();
		BytePool = ArrayPool<byte>.Create();
		Int32Pool = ArrayPool<int>.Create();
		Int64Pool = ArrayPool<long>.Create();
		DoublePool = ArrayPool<double>.Create();
		DateTimePool = ArrayPool<DateTime>.Create();
		TimeSpanPool = ArrayPool<TimeSpan>.Create();
		GuidPool = ArrayPool<Guid>.Create();
		Guid4Pool = ArrayPool<Quadro<Guid>>.Create();
		StringPool = ArrayPool<string>.Create();
		ImagePool = ArrayPool<Image>.Create();
		CoordinatesPool = ArrayPool<Coordinates>.Create();
		LocationPool = ArrayPool<Location>.Create();
	}

	public Array Rent(ReturnType returnType, int minLength)
	{
		Array array = null;
		switch (returnType)
		{
		case ReturnType.Boolean:
			array = BooleanPool.Rent(minLength);
			break;
		case ReturnType.Byte:
			array = BytePool.Rent(minLength);
			break;
		case ReturnType.Int32:
			array = Int32Pool.Rent(minLength);
			break;
		case ReturnType.Int64:
			array = Int64Pool.Rent(minLength);
			break;
		case ReturnType.Double:
			array = DoublePool.Rent(minLength);
			break;
		case ReturnType.DateTime:
			array = DateTimePool.Rent(minLength);
			break;
		case ReturnType.TimeSpan:
			array = TimeSpanPool.Rent(minLength);
			break;
		case ReturnType.Guid:
			array = GuidPool.Rent(minLength);
			break;
		case ReturnType.Guid4:
			array = Guid4Pool.Rent(minLength);
			break;
		case ReturnType.String:
			array = StringPool.Rent(minLength);
			break;
		case ReturnType.Image:
			array = ImagePool.Rent(minLength);
			break;
		case ReturnType.Coordinates:
			array = CoordinatesPool.Rent(minLength);
			break;
		case ReturnType.Location:
			array = LocationPool.Rent(minLength);
			break;
		}
		dict.Add(array, returnType);
		return array;
	}

	public void Return(Array array)
	{
		switch (dict[array])
		{
		case ReturnType.Boolean:
			BooleanPool.Return((bool[])array);
			break;
		case ReturnType.Byte:
			BytePool.Return((byte[])array);
			break;
		case ReturnType.Int32:
			Int32Pool.Return((int[])array);
			break;
		case ReturnType.Int64:
			Int64Pool.Return((long[])array);
			break;
		case ReturnType.Double:
			DoublePool.Return((double[])array);
			break;
		case ReturnType.DateTime:
			DateTimePool.Return((DateTime[])array);
			break;
		case ReturnType.TimeSpan:
			TimeSpanPool.Return((TimeSpan[])array);
			break;
		case ReturnType.Guid:
			GuidPool.Return((Guid[])array);
			break;
		case ReturnType.Guid4:
			Guid4Pool.Return((Quadro<Guid>[])array);
			break;
		case ReturnType.String:
			StringPool.Return((string[])array);
			break;
		case ReturnType.Image:
			ImagePool.Return((Image[])array);
			break;
		case ReturnType.Coordinates:
			CoordinatesPool.Return((Coordinates[])array);
			break;
		case ReturnType.Location:
			LocationPool.Return((Location[])array);
			break;
		}
		dict.Remove(array);
	}
}
