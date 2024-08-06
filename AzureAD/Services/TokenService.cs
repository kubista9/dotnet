using AzureAD.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AzureAD.Services;

public class TokenService
{
	private readonly IMemoryCache _cache;
	private readonly IAuth0Service _authService;

	public TokenService(IMemoryCache cache, IAuth0Service authService)
	{
		_cache = cache;
		_authService = authService;
	}

	private const string CacheKey = "AccessToken";
	private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5); // Adjust as needed

	public async Task<Token> GetTokenAsync()
	{
		if (_cache.TryGetValue(CacheKey, out Token cachedToken))
		{
			if (cachedToken.ExpirationTime > DateTime.UtcNow)
			{
				return cachedToken;
			}
		}

		// Token expired or not found, refresh token
		var newToken = await _authService.GetTokenAsync();
		_cache.Set(CacheKey, new Token
		{
			AccessToken = newToken.AccessToken,
			ExpirationTime = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn)
		}, _cacheDuration);
		return newToken;
	}

}
