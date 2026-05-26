using System;
using System.Collections.Generic;
using System.IO;
using BINParser;
using KML;

namespace AGInterfaces;

public interface IStorage
{
	T Load<T>(string schemaName, Stream stm, bool ixXmlContent);

	T Load<T>(string schemaName, string fileName, bool isXmlContent);

	[Obsolete]
	T AGDLoad<T>(string schemaName, Stream stm, IProgressStatus progressStatus, ref bool recompiling, List<string> recompLog);

	[Obsolete]
	KMLFile KMLLoad(string schemaName, string fileName);

	[Obsolete]
	KMLFile KMLLoad(Stream stm);

	void Save(string fileName, object obj, bool isXmlContent, bool tryUAC = true);

	void Save(Stream stm, object obj);

	void Move(string fileFrom, string fileTo, bool tryUAC = true);

	[Obsolete]
	void AGDSave(GroupOrElementInfo[] elementNodes, Stream fs, IProgressStatus progress);

	[Obsolete]
	void KMLSave(string schemaName, string fileName, KMLFile file, bool toKMZ = false, bool tryUAC = true);

	[Obsolete]
	void KMLSave(Stream stm, KMLFile file, bool toKMZ = false);

	void Delete(string fileName, bool tryUAC = true);

	IEnumerable<string> EnumSchemes();

	Dictionary<int, SourceInfo[]> GetSourcesInfo(IAutoGRAPHShell shellProvider, int[] serialNo, string dataFolder, AddSourceInfoDelegate addSourceInfo);

	Dictionary<int, SourceInfo[]> GetRoutesInfo(IAutoGRAPHShell shellProvider, int[] serialNo, string dataFolder, AddSourceInfoDelegate addSourceInfo);

	Dictionary<ReadRecordRequest, List<DeviceRecord>> ReadRecords(IAutoGRAPHShell shellProvider, ReadRecordRequest[] requests, string dataFolder);

	DraftItems GetDraftItems(IAutoGRAPHShell shellProvider, ReadRecordRequest request, string dataFolder);
}
