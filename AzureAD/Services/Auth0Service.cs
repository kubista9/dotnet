using AzureAD.Models;

namespace AzureAD.Services;

public class Auth0Service : IAuth0Service
{
	public Task<Token> GetTokenAsync()
	{
		throw new NotImplementedException();
	}
}
