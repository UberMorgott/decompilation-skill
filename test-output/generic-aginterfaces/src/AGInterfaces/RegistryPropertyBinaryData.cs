using System.Linq;

namespace AGInterfaces;

public class RegistryPropertyBinaryData
{
	public string name { get; set; }

	public byte[] content { get; set; }

	public string args { get; set; }

	public override bool Equals(object? obj)
	{
		if (!(obj is RegistryPropertyBinaryData registryPropertyBinaryData))
		{
			return false;
		}
		if (!object.Equals(name, registryPropertyBinaryData.name) || !object.Equals(args, registryPropertyBinaryData.args))
		{
			return false;
		}
		if (content == registryPropertyBinaryData.content)
		{
			return true;
		}
		if (content != null)
		{
			return content.SequenceEqual(registryPropertyBinaryData.content);
		}
		return false;
	}
}
