using AzureAD.Models;

namespace AzureAD.Services;

public interface IAuth0Service
{
	Task<Token> GetTokenFromAzureAd();
}
