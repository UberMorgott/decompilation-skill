using System;

namespace AGInterfaces;

public interface IServerLoader
{
	bool InProgress { get; }

	string Key { get; }

	event FileWriteDelegate OnFileWrite;

	void GetDataImmediately();

	void Start();

	void Stop(Action<IServerLoader> stopCompleted = null);

	HostSetting GetServerInfo();

	void SetElements(GroupOrElementInfo[] _elements);
}
