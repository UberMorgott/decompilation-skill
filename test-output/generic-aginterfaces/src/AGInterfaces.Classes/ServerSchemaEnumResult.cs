namespace AGInterfaces.Classes;

public sealed class ServerSchemaEnumResult : SchemaOperationResult
{
	public ServerSchemaItem[] Schemas { get; set; }

	public ServerMapItem[] Maps { get; set; }

	public ServerReportItem[] Reports { get; set; }
}
