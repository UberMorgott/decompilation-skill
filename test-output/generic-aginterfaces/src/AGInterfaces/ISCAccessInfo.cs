namespace AGInterfaces;

public interface ISCAccessInfo
{
	bool IsAuthenticated { get; }

	int Authenticate(string host, int port, string userLogin, string userPass);
}
