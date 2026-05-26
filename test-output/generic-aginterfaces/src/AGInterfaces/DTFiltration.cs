namespace AGInterfaces;

public enum DTFiltration : byte
{
	Ok,
	RecoverDate,
	DeletedEntry,
	UnknownEntry,
	DuplicatedEntry,
	InvalidCRC,
	InvalidTime,
	ByTimeFlag,
	ByFileTime,
	ByPowerRenewal,
	BackThrow,
	ForwardThrow
}
