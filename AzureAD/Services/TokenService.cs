using AzureAD.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AzureAD.Services;

public class TokenService : ITokenService
{
	private readonly IMemoryCache _memoryCache;
	private readonly IAuth0Service _auth0Service;
	private const string CacheKey = "AccessToken";

	public TokenService(IMemoryCache memoryCache, IAuth0Service auth0Service)
	{
		_memoryCache = memoryCache;
		_auth0Service = auth0Service;
	}

	public async Task<Token> GetTokenFromCacheAsync()
	{
		if (_memoryCache.TryGetValue(CacheKey, out Token? cachedToken) && cachedToken?.ExpirationTime > DateTime.UtcNow)
		{
			return cachedToken;
		}

		var refreshedToken = await RefreshTokenAsync();
		return refreshedToken;
		/*
		var newToken = await _auth0Service.GetTokenFromAzureAd();
		_memoryCache.Set(CacheKey, new Token
		{
			AccessToken = newToken.AccessToken,
			ExpirationTime = DateTime.UtcNow.AddSeconds(newToken.ExpiresIn)
		}, _cacheDuration);
		return newToken;
		*/
	}

	private async Task<Token> RefreshTokenAsync()
	{
		var token = await _auth0Service.GetTokenFromAzureAd();
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
