using System;

namespace AGInterfaces;

public sealed class ModuleInfo
{
	public string ModuleName;

	public string GroupName;

	public string Description;

	public string Name;

	public string Caption;

	public Guid GUID;

	public IAutoGRAPHShell shellProvider;

	public IAutoGRAPHModule module;

	public static int Compare(ModuleInfo moduleInfo1, ModuleInfo moduleInfo2)
	{
		int num = string.Compare(moduleInfo1?.Caption, moduleInfo2?.Caption, ignoreCase: true);
		if (num == 0)
		{
			num = string.Compare(moduleInfo1?.Name, moduleInfo2?.Name, ignoreCase: false);
		}
		return num;
	}
}
