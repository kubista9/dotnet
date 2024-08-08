using AzureAD.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureAD.Controllers;

public class TokenController : ControllerBase
{
	private readonly IAuth0Service _auth0Service;
	private readonly ITokenService _tokenService;
	private readonly IExternalService<string> _externalService;

	public TokenController(IAuth0Service auth0Service, ITokenService tokenService, IExternalService<string> externalService)
	{
		_auth0Service = auth0Service;
		_tokenService = tokenService;
		_externalService = externalService;
	}

	[HttpGet("tokenFromAzure")]
	public async Task<IActionResult> GetTokenFromAzureAd()
	{
		var token = await _auth0Service.GetTokenFromAzureAd();
		return Ok(token);
	}

	[HttpGet("tokenFromCache")]
	public async Task<IActionResult> GetTokenFromCache()
	{
		var token = await _tokenService.GetTokenFromCacheAsync();
		return Ok(token);
	}

	[HttpGet("data/{url}")]
	public async Task<IActionResult> GetDataFromUrl(string url, Dictionary<string, string>? queryParameters)
	{
		var data = await _externalService.GetAsync(url, queryParameters);
		return Ok(data);
	}
}
