using System;
using System.Collections.Generic;

namespace AGInterfaces;

public interface IAutoGRAPHModule
{
	void Initialize(IAutoGRAPHShell shellProvider);

	Tuple<string, string> RecommendedClassName(Type iType);

	IAutoGRAPHSettings[] GetSettingsObject(IAutoGRAPHShell shellProvider, SettingsType type, ElementType element = ElementType.Module);

	SettingsValidateResult SettingsObjectValidate(IAutoGRAPHShell shellProvider, object settingsObject, List<string> validatingLog, Guid guid);

	void FireChanges(IAutoGRAPHShell shellProvider, SettingsType types, ElementType element, PrevSettingsDelegates previousSettings, Guid senderGuid);

	void ImportSettings(ElementType element, string fileType, Dictionary<string, string> dict, object modComSettingsObject, object modIndSettingsObject, ref object grpComSettingsObject, ref object grpIndSettingsObject, ref object elmComSettingsObject, ref object elmIndSettingsObject, int serialNo);

	void ImportSettings(ElementType element, string fileType, Dictionary<string, Dictionary<string, ElementImportData>> dict, object modComSettingsObject, object modIndSettingsObject, ref object grpComSettingsObject, ref object grpIndSettingsObject, ref object elmComSettingsObject, ref object elmIndSettingsObject, int serialNo);

	void Close(IAutoGRAPHShell shellProvider);

	IEnumerable<IAutoGRAPHSettings> GetAffectCalculationSettings(IAutoGRAPHShell shellProvider, Guid guid);
}
