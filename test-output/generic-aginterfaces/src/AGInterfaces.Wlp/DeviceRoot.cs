using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace AGInterfaces.Wlp;

public class DeviceRoot
{
	public AdvProps advProps { get; set; }

	public List<JObject> afields { get; set; }

	public List<Alias> aliases { get; set; }

	public Counters counters { get; set; }

	public Driving driving { get; set; }

	public List<Field> fields { get; set; }

	public Fuel fuel { get; set; }

	public General general { get; set; }

	public HwConfig hwConfig { get; set; }

	public Icon icon { get; set; }

	public string imgRot { get; set; }

	public List<object> intervals { get; set; }

	public double mu { get; set; }

	public List<Profile> profile { get; set; }

	public ReportProps reportProps { get; set; }

	public List<Sensor> sensors { get; set; }

	public Trip trip { get; set; }

	public string type { get; set; }

	public string version { get; set; }
}
