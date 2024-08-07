using AzureAD.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AzureAD.Services;

public class TokenService
{
	private readonly IMemoryCache _memoryCache;
	private readonly IAuth0Service _authService;
	private const string CacheKey = "AccessToken";
	private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5); // Adjust as needed

	public TokenService(IMemoryCache memoryCache, IAuth0Service authService)
	{
		_memoryCache = memoryCache;
		_authService = authService;
	}

	public async Task<Token> GetTokenAsync()
	{
		if (_memoryCache.TryGetValue(CacheKey, out Token? cachedToken) && cachedToken?.ExpirationTime > DateTime.UtcNow)
		{
			return cachedToken;
		}

		var newToken = await _authService.GetTokenAsync();
		_memoryCache.Set(CacheKey, new Token
		{
			AccessToken = newToken.AccessToken,
			ExpirationTime = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn)
		}, _cacheDuration);
		return newToken;
	}

	public async Task<Token> RefreshTokenAsync()
	{
		var token = await _authService.GetTokenAsync();
		if (token != Token.Empty)
		{
			var expiresIn = token.ExpiresIn > 0 ? token.ExpiresIn - 10 : token.ExpiresIn;
			_memoryCache.Set(CacheKey, token,
				new MemoryCacheEntryOptions()
					.SetSlidingExpiration(TimeSpan.FromMinutes(5))
					.SetAbsoluteExpiration(TimeSpan.FromSeconds(expiresIn)));
		}
		return token;
	}
}
