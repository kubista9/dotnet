using AzureAD.Models;
using AzureAD.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureAD.Controllers;

public class TokenController : ControllerBase
{
	private readonly IAuth0Service _authService;

	public TokenController(IAuth0Service auth0Service)
	{
		_authService = auth0Service;
	}

	[HttpGet]
	public async Task<Token> GetToken()
	{
		var token = await _authService.GetTokenAsync();
		return token;
	}
}
