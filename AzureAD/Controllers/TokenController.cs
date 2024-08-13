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
		try
		{
			var token = await _auth0Service.GetTokenFromAzureAd();
			return Ok(token);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Error retrieving token from Azure AD: {ex.Message}");
		}
	}

	[HttpGet("tokenFromCache")]
	public async Task<IActionResult> GetTokenFromCache()
	{
		try
		{
			var token = await _tokenService.GetTokenFromCacheAsync();
			return Ok(token);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Error retrieving token from cache: {ex.Message}");
		}
	}

	[HttpGet("data/{url}")]
	public async Task<IActionResult> GetDataFromUrl(string url, Dictionary<string, string> queryParameters)
	{
		if (string.IsNullOrEmpty(url))
		{
			return BadRequest("Please provide some URL.");
		}

		try
		{
			var data = await _externalService.GetAsync(url, queryParameters);
			return Ok(data);
		}
		catch (Exception ex)
		{
			return StatusCode(500, $"Error retrieving data from URL: {ex.Message}");
		}
	}
}
