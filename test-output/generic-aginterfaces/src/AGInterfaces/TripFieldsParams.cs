using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using BINParser;

namespace AGInterfaces;

public class TripFieldsParams
{
	private readonly int _index;

	private readonly DataArrays _arrays;

	private readonly Quadro<Guid>[] _gf;

	private readonly Guid[] _routes;

	private readonly LocationAddr _addr;

	private readonly Guid _area;

	private readonly int _id;

	private readonly int _count;

	private readonly DeviceTrack[] _deviceTracks;

	private readonly ImageFormat _imageFormat;

	public readonly SortedDictionary<string, StageArrays> _tripStages;

	public readonly DevPrmsGroupInfo[] _tripPrmsGroupInfoArray;

	public Image image
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.image;
		}
	}

	public DateTime udt
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.udt[_index];
		}
	}

	public InputFlags flags
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _arrays.flags[_index];
		}
	}

	public Guid driver
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_arrays.driver == null)
			{
				return Guid.Empty;
			}
			return _arrays.driver[_index];
		}
	}

	public Guid implement
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_arrays.implement == null)
			{
				return Guid.Empty;
			}
			return _arrays.implement[_index];
		}
	}

	public Coordinates crd
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_arrays.crds == null)
			{
				return Coordinates.Empty;
			}
			return _arrays.crds[_index];
		}
	}

	public double distance
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_arrays.run == null)
			{
				return 0.0;
			}
			return _arrays.run[_index];
		}
	}

	public Quadro<Guid> mchp
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_arrays.mobileChP == null)
			{
				return Quadro<Guid>.Empty;
			}
			return _arrays.mobileChP[_index];
		}
	}

	public LocationAddr addr
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _addr;
		}
	}

	public int id
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _id;
		}
	}

	public int count
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _count;
		}
	}

	public DeviceTrack[] deviceTracks
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _deviceTracks;
		}
	}

	public ImageFormat imageFormat
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _imageFormat;
		}
	}

	public Quadro<Guid>[] gf
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _gf;
		}
	}

	public Guid[] routes
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return _routes;
		}
	}

	public Guid area
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			if (_deviceTracks == null || !_deviceTracks.Any())
			{
				return Guid.Empty;
			}
			return _area;
		}
	}

	public TripFieldsParams(int index, DataArrays arrays, Quadro<Guid>[] gf, Guid[] routes, LocationAddr addr, Guid area, int id, int count, DeviceTrack[] deviceTracks, ImageFormat imageFormat, SortedDictionary<string, StageArrays> tripStages, DevPrmsGroupInfo[] tripPrmsGroupInfoArray)
	{
		_index = index;
		_arrays = arrays;
		_gf = gf;
		_routes = routes;
		_addr = addr;
		_area = area;
		_id = id;
		_count = count;
		_deviceTracks = deviceTracks;
		_imageFormat = imageFormat;
		_tripStages = tripStages;
		_tripPrmsGroupInfoArray = tripPrmsGroupInfoArray;
	}
}
