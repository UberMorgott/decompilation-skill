using System;
using MsgPack.Serialization;
using Newtonsoft.Json;

namespace AGInterfaces;

public class RegistryProperty : IComparable, IEquatable<RegistryProperty>, ITreeDataObject
{
	private string _comment;

	private bool _fixDeserialize;

	private Guid _id = Guid.NewGuid();

	private RegistryNodeType _nodeType = RegistryNodeType.Property;

	public bool inherited;

	public string name { get; set; }

	public PropType type { get; set; }

	[MessagePackRuntimeType]
	public object value { get; set; }

	[JsonIgnore]
	public string comment
	{
		get
		{
			if (value is PropertyTable)
			{
				return ((PropertyTable)value).GetPrevOrCurrentEntry()?.comment;
			}
			return _comment;
		}
		set
		{
			if (this.value is PropertyTable)
			{
				PropertyTableItem prevOrCurrentEntry = ((PropertyTable)this.value).GetPrevOrCurrentEntry();
				if (prevOrCurrentEntry != null)
				{
					prevOrCurrentEntry.comment = value;
				}
				_comment = null;
			}
			else
			{
				_comment = value;
			}
		}
	}

	public bool IsReadOnly { get; set; }

	public Guid ID
	{
		get
		{
			return _id;
		}
		set
		{
			if (value != Guid.Empty)
			{
				_id = value;
				return;
			}
			_id = Guid.NewGuid();
			NodeType = RegistryNodeType.Property;
			_fixDeserialize = true;
		}
	}

	public Guid ParentID { get; set; }

	public bool IsDirectory => NodeType == RegistryNodeType.Directory;

	public bool IsEmpty => ID == Guid.Empty;

	public bool IsNullOrEmpty
	{
		get
		{
			object obj = value;
			if (obj == null || obj is PropertyTable { IsNullOrEmpty: not false })
			{
				return true;
			}
			return false;
		}
	}

	public RegistryNodeType NodeType
	{
		get
		{
			return _nodeType;
		}
		set
		{
			if (!_fixDeserialize)
			{
				_nodeType = value;
				return;
			}
			_nodeType = RegistryNodeType.Property;
			_fixDeserialize = false;
		}
	}

	public static bool checkPropertyName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			return false;
		}
		if (!char.IsLetter(name[0]) && name[0] != '_')
		{
			return false;
		}
		for (int i = 1; i < name.Length; i++)
		{
			if (!char.IsLetterOrDigit(name[i]) && name[i] != '_')
			{
				return false;
			}
		}
		return true;
	}

	public bool correctByType()
	{
		if (value is PropertyTable propertyTable && propertyTable.type == type)
		{
			return propertyTable.correctByType();
		}
		object obj = value;
		value = type.correctByType(value);
		return obj != value;
	}

	public RegistryProperty()
	{
	}

	public RegistryProperty(string name)
	{
		this.name = name;
	}

	public static RegistryProperty Copy(RegistryProperty property)
	{
		RegistryProperty registryProperty = new RegistryProperty
		{
			name = property.name,
			type = property.type,
			value = property.value,
			comment = property.comment,
			ID = property.ID,
			ParentID = property.ParentID,
			NodeType = property.NodeType,
			IsReadOnly = property.IsReadOnly,
			inherited = property.inherited
		};
		if (registryProperty.value != null)
		{
			registryProperty.correctByType();
		}
		return registryProperty;
	}

	public int CompareTo(object obj)
	{
		if (!(obj is RegistryProperty registryProperty) || string.IsNullOrWhiteSpace(registryProperty.name))
		{
			return 1;
		}
		return string.Compare(name, registryProperty.name, StringComparison.Ordinal);
	}

	public bool Equals(RegistryProperty other)
	{
		if (other == null)
		{
			return false;
		}
		return name == other.name;
	}

	public static RegistryProperty CreateProperty(string name, PropType type = PropType.String, Guid parentId = default(Guid))
	{
		return new RegistryProperty(name)
		{
			ParentID = parentId,
			type = type
		};
	}

	public static RegistryProperty CreateDirectory(string name, Guid parentId = default(Guid))
	{
		return new RegistryProperty(name)
		{
			ParentID = parentId,
			NodeType = RegistryNodeType.Directory
		};
	}
}
