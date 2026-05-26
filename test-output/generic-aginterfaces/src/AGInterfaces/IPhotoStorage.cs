using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AGInterfaces;

public interface IPhotoStorage
{
	byte[] GetPhoto(int serialNo, DateTime photoUDT, int cameraNum);

	Task<byte[]> GetPhotoAsync(int serialNo, string fileName);

	Task<byte[]> GetPhotoByRelativePathAsync(bool isThumbnail, string fileName);

	List<PhotoInfo> EnumPhotoFiles(int serialNo, DateTime sdUTC, DateTime edUTC, string SerialNoFormat);
}
