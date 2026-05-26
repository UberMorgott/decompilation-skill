using System;

namespace AGInterfaces;

public interface ITreeDataObject
{
	Guid ID { get; set; }

	Guid ParentID { get; set; }

	bool IsDirectory { get; }

	bool IsReadOnly { get; set; }
}
