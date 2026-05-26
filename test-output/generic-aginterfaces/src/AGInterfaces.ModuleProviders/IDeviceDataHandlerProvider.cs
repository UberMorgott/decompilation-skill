using System;
using System.Collections.Generic;
using System.Threading;

namespace AGInterfaces.ModuleProviders;

public interface IDeviceDataHandlerProvider
{
	Tuple<Dictionary<Guid, DataArrays>, Dictionary<Guid, SlaveSourcesInfo>> GetMCHPDataArrays(ImagesSize mainImageSize, ImagesSize statusImagesSize, CancellationToken cancellationToken);

	Dictionary<Guid, List<OnlineInfo>> GetOnlineInfo(GetOnlineInfoRequest[] requests, bool fullUpdate, ref bool updated, TimeZoneInfo alternateTimeZoneInfo = null, CancellationToken cancellationToken = default(CancellationToken), bool preloadCasheFiles = false);

	Dictionary<Guid, List<OnlineInfo>> GetOnlineInfoOptimized(GetOnlineInfoRequest[] requests, bool fullUpdate, ref bool updated, TimeZoneInfo alternateTimeZoneInfo = null, CancellationToken cancellationToken = default(CancellationToken), bool preloadCasheFiles = false);

	string[] GetPhotoFolders();

	SourceInfo[] GetSourcesInfo(Guid guid);

	void InvalidSources(SourceChangeType changeType, Guid senderGuid);
}
