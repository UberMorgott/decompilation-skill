using System;

namespace AGInterfaces;

public interface IDataViewerFormatters
{
	string elapse(TimeSpan interval);

	string elapseShort(TimeSpan interval);

	string ToCustomString(object obj, string format = null);

	string getAddressByID(LocationAddr addr);

	string CrdsToString(Coordinates crds);
}
