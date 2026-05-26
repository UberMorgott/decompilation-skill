using System;
using System.Collections.Generic;
using System.Reflection;
using BINParser;

namespace AGInterfaces;

[Obfuscation(Exclude = true)]
public interface IDeviceDataBase : IAutoGRAPHModule
{
	string GetDataFolder(IAutoGRAPHShell shellProvider);

	string GetPhotoFolder(IAutoGRAPHShell shellProvider);

	void SetDataFolder(string absPath);

	bool CheckDeviceAccess(int serialNo);

	Dictionary<Guid, SourceInfo[]> GetSourcesInfo(IAutoGRAPHShell shellProvider, Guid[] guid);

	Dictionary<Guid, SourceInfo[]> GetRoutesInfo(IAutoGRAPHShell shellProvider, Guid[] guid);

	Dictionary<ReadRecordRequest, List<DeviceRecord>> LoadDeviceData(IAutoGRAPHShell shellProvider, ReadRecordRequest[] requests);

	List<PhotoInfo> GetPhotoFilesList(IAutoGRAPHShell shellProvider, int serialNo, ReportTimeSpan timeSpan);

	DraftItems GetDraftItems(IAutoGRAPHShell shellProvider, ReadRecordRequest request);
}
