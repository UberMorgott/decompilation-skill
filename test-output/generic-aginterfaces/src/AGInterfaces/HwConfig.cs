using System.Collections.Generic;
using Newtonsoft.Json;

namespace AGInterfaces;

public class HwConfig
{
	public int fullData { get; set; }

	public string hw { get; set; }

	[JsonConverter(typeof(WlpJsonConverter))]
	public List<Param> @params { get; set; }
}
