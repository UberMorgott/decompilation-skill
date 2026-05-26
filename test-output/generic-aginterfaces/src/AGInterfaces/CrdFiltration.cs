namespace AGInterfaces;

public enum CrdFiltration : byte
{
	None,
	Ok,
	InvalidTime,
	ZeroTime,
	NoSignal,
	LowSignal,
	NearError,
	SharpTurn,
	Acceleration,
	Teleportation
}
