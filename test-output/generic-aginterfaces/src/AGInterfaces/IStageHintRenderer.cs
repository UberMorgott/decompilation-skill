using System;

namespace AGInterfaces;

public interface IStageHintRenderer
{
	void Init(DataArrays arr, string[] arr_columnNameAdd, string trackStatusesInfo);

	string Render(IAutoGRAPHShell shellProvider, IDisposable aga2Obj, Array[] stage, int i, string textTemplate, string incorrectFormat);
}
