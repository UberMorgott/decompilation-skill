using System;

namespace AGInterfaces;

public delegate void FileWriteDelegate(Guid deviceId, int serial, int fileSize, string fileName);
