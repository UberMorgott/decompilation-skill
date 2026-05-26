using System.Collections.Generic;

namespace AGInterfaces;

public sealed class ServerAuthenticateResult
{
	public readonly HashSet<int> Items;

	public readonly bool IsOffline;

	public readonly string ErrorMessage;

	public readonly int ErrorCode;

	public readonly bool AuthRequired;

	public bool ShowSecurityTab;

	public string RoleName;

	public bool SupportRoles;

	public ServerAuthenticateResult(bool AuthRequired, HashSet<int> items, bool isOfflineMode)
	{
		this.AuthRequired = AuthRequired;
		Items = items;
		IsOffline = isOfflineMode;
	}

	public ServerAuthenticateResult(bool AuthRequired, int errorCode, string errorMessage)
	{
		this.AuthRequired = AuthRequired;
		ErrorMessage = errorMessage;
		ErrorCode = errorCode;
	}
}
