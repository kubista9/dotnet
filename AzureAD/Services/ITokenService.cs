using AzureAD.Models;

namespace AzureAD.Services.TokenService;

public interface ITokenService
{
	Task<Token> GetTokenFromCacheAsync();
}
