using System;
using System.Collections.Generic;

namespace AGInterfaces;

public delegate void AddSourceInfoDelegate(List<SourceInfo> sourcesInfo, string fullFileName, long Length, DateTime LastWriteUTC, int serialNo, BinFormatType binFormatType, SourceType forceType, IAutoGRAPHShell shellProvider);
