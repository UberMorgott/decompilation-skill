using System.Collections.Generic;

namespace AGInterfaces.Wlp;

public class DrvAndImplRoot
{
	public string type { get; set; }

	public string version { get; set; }

	public double mu { get; set; }

	public List<Driver> drivers { get; set; }

	public List<Trailer> trailers { get; set; }
}
