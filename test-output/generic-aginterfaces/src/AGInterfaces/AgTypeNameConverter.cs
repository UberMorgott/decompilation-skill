using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Polenter.Serialization.Advanced.Serializing;

namespace AGInterfaces;

public class AgTypeNameConverter : ITypeNameConverter
{
	private static readonly Regex RegexRemoveAssemblyVersion = new Regex(", Version=\\d+.\\d+.\\d+.\\d+");

	private static readonly Regex RegexRemoveCulture = new Regex(", Culture=\\w+");

	private static readonly Regex RegexRemovePublicKeyToken = new Regex(", PublicKeyToken=\\w+");

	private readonly Dictionary<Type, string> _cache = new Dictionary<Type, string>();

	private readonly Dictionary<string, string> _userPanelReplaces = new Dictionary<string, string>
	{
		{ "AutoGRAPHUserPanel.UserPanelMenu, AutoGRAPHUserPanel", "AutoGRAPHUserPanel.Settings.MenuItemSettings, AutoGRAPHUserPanel" },
		{ "AutoGRAPHUserPanel.UserPanelButton, AutoGRAPHUserPanel", "AutoGRAPHUserPanel.Settings.ButtonItemSettings, AutoGRAPHUserPanel" },
		{ "AutoGRAPHUserPanel.UserPanelActionApplication, AutoGRAPHUserPanel", "AutoGRAPHUserPanel.Settings.ApplicationActionSettings, AutoGRAPHUserPanel" },
		{ "AutoGRAPHUserPanel.UserPanelActionPowerShell, AutoGRAPHUserPanel", "AutoGRAPHUserPanel.Settings.PowerShellActionSettings, AutoGRAPHUserPanel" }
	};

	private readonly Dictionary<string, string> _propertiesRegistryReplaces = new Dictionary<string, string> { { "AutoGRAPHShell.Modules.PropertiesRegistry.PropertiesRegistryTaringForm+ChartTareTableItem, AutoGRAPHShell", "AutoGRAPHShell.UI.Modules.PropertiesRegistry.PropertiesRegistryTaringForm+ChartTareTableItem, AutoGRAPHShell.UI" } };

	public bool IncludeAssemblyVersion { get; private set; }

	public bool IncludeCulture { get; private set; }

	public bool IncludePublicKeyToken { get; private set; }

	public AgTypeNameConverter()
	{
	}

	public AgTypeNameConverter(bool includeAssemblyVersion, bool includeCulture, bool includePublicKeyToken)
	{
		IncludeAssemblyVersion = includeAssemblyVersion;
		IncludeCulture = includeCulture;
		IncludePublicKeyToken = includePublicKeyToken;
	}

	public string ConvertToTypeName(Type type)
	{
		if (type == null)
		{
			return string.Empty;
		}
		if (_cache.ContainsKey(type))
		{
			return _cache[type];
		}
		string text = type.AssemblyQualifiedName;
		if (!IncludeAssemblyVersion)
		{
			text = removeAssemblyVersion(text);
		}
		if (!IncludeCulture)
		{
			text = removeCulture(text);
		}
		if (!IncludePublicKeyToken)
		{
			text = removePublicKeyToken(text);
		}
		_cache.Add(type, text);
		return text;
	}

	public Type ConvertToType(string typeName)
	{
		if (string.IsNullOrEmpty(typeName))
		{
			return null;
		}
		try
		{
			return Type.GetType(typeName, throwOnError: true);
		}
		catch (TypeLoadException)
		{
			if (PropertiesRegistryConvertToType(typeName, out var type))
			{
				return type;
			}
			if (ServerProtocolConvertToType(typeName, out type))
			{
				_ = 1;
			}
			else
				UserPanelConvertToType(typeName, out type);
			if (type != null)
			{
				return type;
			}
			throw;
		}
	}

	private bool ServerProtocolConvertToType(string typeName, out Type type)
	{
		type = null;
		if (typeName.Contains("AutoGRAPHDeviceDataLoader.Protocol."))
		{
			typeName = typeName.Replace("AutoGRAPHDeviceDataLoader.Protocol", "ServerProtocol").Replace("AutoGRAPHDeviceDataLoader", "ServerProtocol");
			try
			{
				type = Type.GetType(typeName, throwOnError: true);
			}
			catch (TypeLoadException)
			{
			}
			return true;
		}
		return false;
	}

	private bool UserPanelConvertToType(string typeName, out Type type)
	{
		return ConvertByDictionary(_userPanelReplaces, typeName, out type);
	}

	private bool PropertiesRegistryConvertToType(string typeName, out Type type)
	{
		type = null;
		if (_propertiesRegistryReplaces.ContainsKey(typeName))
		{
			try
			{
				type = Type.GetType(_propertiesRegistryReplaces[typeName], throwOnError: false);
			}
			catch (TypeLoadException)
			{
			}
			return true;
		}
		return false;
	}

	private bool ConvertByDictionary(Dictionary<string, string> replaces, string typeName, out Type type)
	{
		type = null;
		if (replaces.ContainsKey(typeName))
		{
			try
			{
				type = Type.GetType(replaces[typeName], throwOnError: true);
			}
			catch (TypeLoadException)
			{
			}
			return true;
		}
		return false;
	}

	private static string removePublicKeyToken(string typename)
	{
		return RegexRemovePublicKeyToken.Replace(typename, string.Empty);
	}

	private static string removeCulture(string typename)
	{
		return RegexRemoveCulture.Replace(typename, string.Empty);
	}

	private static string removeAssemblyVersion(string typename)
	{
		return RegexRemoveAssemblyVersion.Replace(typename, string.Empty);
	}
}
