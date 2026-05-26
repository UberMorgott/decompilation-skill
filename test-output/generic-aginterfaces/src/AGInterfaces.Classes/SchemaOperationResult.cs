using System;

namespace AGInterfaces.Classes;

public class SchemaOperationResult
{
	public bool ok { get; set; }

	public string msg { get; set; }

	public int ID { get; set; }

	public Guid UID { get; set; }

	public int CAdded { get; set; }

	public int CUpdated { get; set; }

	public int CDropped { get; set; }

	public int GAdded { get; set; }

	public int GUpdated { get; set; }

	public int GDropped { get; set; }

	public string Name { get; set; }

	public bool OrgCreated { get; set; }

	public string ServerVersion { get; set; }

	public SchemaOperationResult(bool ok, string msg = null)
	{
		this.ok = ok;
		this.msg = msg;
	}

	public SchemaOperationResult()
	{
	}
}
