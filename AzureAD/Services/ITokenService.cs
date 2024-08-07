using AzureAD.Models;

namespace AzureAD.Services;

public interface ITokenService
{
	Task<Token> GetTokenAsync();
	Task<Token> RefreshTokenAsync();
}
