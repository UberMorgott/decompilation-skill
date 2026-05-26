using AddressBaseAga2;

namespace AGInterfaces;

public class AddressBaseCollectionAdapter : IAddressBaseCollectionAdapter
{
	public IAddressBaseCollection _addressBaseCollection;

	public AddressBaseCollectionAdapter(IAddressBaseCollection addressBaseCollection)
	{
		_addressBaseCollection = addressBaseCollection;
	}

	public IAddressBaseCollection GetIAddressBaseCollection()
	{
		return _addressBaseCollection;
	}
}
